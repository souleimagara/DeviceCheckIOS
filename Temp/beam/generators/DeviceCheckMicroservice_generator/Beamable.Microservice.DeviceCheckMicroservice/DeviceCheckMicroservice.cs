
using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Beamable.Common;
using Beamable.Server;
using Beamable.Server.Api.RealmConfig;
using Newtonsoft.Json;
using UnityEngine;

namespace Beamable.Microservices
{
    [Microservice("DeviceCheckMicroservice")]
    public class DeviceCheckMicroservice : Microservice
    {
        

       


        [ClientCallable]
        public async Task<string> SendToServer(string deviceCheckToken)
        {
            string missingConfigs = "";

            RealmConfig config = await Services.RealmConfig.GetRealmConfigSettings();
            string P8filecontent = config.GetSetting("accounts", "fileP8", "");
            string Teamidapple = config.GetSetting("accounts", "TEAM_ID_APPLE", "");
            string keyID = config.GetSetting("accounts", "KEYID", "");

            if (P8filecontent == "")
            {

                missingConfigs = "Please add the following to your beamable Config :  FileP8";
                return missingConfigs;
            }

            if (Teamidapple == "")
            {

                missingConfigs = "Please add the following to your beamable Config:   Team ID APPLE";
                return missingConfigs;
            }
            if (keyID == "")
            {

                missingConfigs = "Please add the following to your beamable Config:   KEY ID";
                return missingConfigs;
            }
            string iosToken = GenerateJwt(P8filecontent, Teamidapple, keyID);
            BeamableLogger.Log("iostokenplease " + iosToken);

            string transactionId = "id_" + DateTimeOffset.Now.ToUnixTimeMilliseconds(); // random string

            // Prepare payload for DeviceCheck API
            var payload = new
            {
                device_token = deviceCheckToken,
                transaction_id = transactionId,
                timestamp = DateTimeOffset.Now.ToUnixTimeMilliseconds() // unix timestamp in milliseconds
            };
         


            // HttpClient initialization
            using (HttpClient client = new HttpClient())
            {
                client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", iosToken);

                try
                {
                    // Convert payload to JSON string
                    // string jsonPayload = JsonUtility.ToJson(payload);
                    string jsonPayload = JsonConvert.SerializeObject(payload);
                    BeamableLogger.Log("Payload: " + jsonPayload);

                    // Make a POST request to Apple's DeviceCheck API
                    HttpResponseMessage response = await client.PostAsync("https://api.devicecheck.apple.com/v1/validate_device_token", new StringContent(jsonPayload, Encoding.UTF8, "application/json"));

                    // Log response status code
                    BeamableLogger.Log("status: " + response.StatusCode);
                   
                    // Read and log response data
                    string responseData = await response.Content.ReadAsStringAsync();

                    missingConfigs = "response is :   "+ response.IsSuccessStatusCode + "StatusCode :  " + response.StatusCode + " RequestMessage : " + response.RequestMessage;
                    BeamableLogger.Log("Response Data: " + responseData);
                }
                catch (HttpRequestException e)
                {
                    BeamableLogger.LogError("HTTP Request Exception: " + e.Message);
                }
            }
        

        
            // This code executes on the server.
            return missingConfigs ;
        }

        public string GenerateJwt(string privateKey, string teamId, string keyId)
        {
            using (ECDsa key = ECDsa.Create())
            {
                key.ImportPkcs8PrivateKey(Convert.FromBase64String(privateKey), out _);

                var credentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(
                    new Microsoft.IdentityModel.Tokens.ECDsaSecurityKey(key),
                    Microsoft.IdentityModel.Tokens.SecurityAlgorithms.EcdsaSha256);

                var header = new JwtHeader(credentials);
                header.Add("kid", keyId); // Add 'keyid' to the header
                long unixTimestampSeconds = DateTimeOffset.Now.ToUnixTimeSeconds();
                BeamableLogger.Log("unixTimestampMilliseconds" + unixTimestampSeconds);

                // Convert the Unix timestamp to a DateTime object
                DateTime expiryDate = DateTimeOffset.FromUnixTimeSeconds(unixTimestampSeconds).DateTime;

                // Add 180 days to the expiry date
                expiryDate = expiryDate.AddDays(180);

                // Convert the expiry date back to a Unix timestamp
                long expiryUnixTimestampSeconds = new DateTimeOffset(expiryDate).ToUnixTimeSeconds();

                BeamableLogger.Log("Expiry Unix Timestamp in seconds: " + expiryUnixTimestampSeconds);
                var payload = new JwtPayload
                {   { "iat", unixTimestampSeconds },
                    { "exp", expiryUnixTimestampSeconds }, // Set 'expiresIn' to 180 days
                    { "iss", teamId }
        
                };

                var securityToken = new JwtSecurityToken(header, payload);
                var handler = new JwtSecurityTokenHandler();
                return handler.WriteToken(securityToken);
            }
        }
      

        public bool ValidateJwtToken(string jwtToken, string teamid)
        {
            try
            {
                // Parse the JWT token
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(jwtToken);

                // Check issuer claim
                if (token.Issuer != teamid)
                {
                    // Invalid issuer
                    return false;
                }

                // Check expiration claim
                if (token.ValidTo < DateTime.UtcNow)
                {
                    // Token has expired
                    return false;
                }

                // Additional validation checks for other claims if required

                // Token is valid
                return true;
            }
            catch (Exception ex)
            {
                // Log or handle the exception
                BeamableLogger.LogError($"Error validating JWT token: {ex.Message}");
                return false;
            }
        }
    }
}
