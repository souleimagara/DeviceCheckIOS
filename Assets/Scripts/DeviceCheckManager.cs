
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.UI;
using System;
using Beamable;
using Beamable.Server.Clients;


public class DeviceCheckManager : MonoBehaviour
{


    public Text Token;
    public Text checkingdevice;
    // Define the method to call the native code
    [DllImport("__Internal")]

    private static extern void _FetchDeviceCheckToken();
    // Use this method to trigger  native token fetch

    private void Start()
    {
       FetchDeviceCheckToken();
    }
    public void FetchDeviceCheckToken()
    {

        #if UNITY_IOS && !UNITY_EDITOR
        _FetchDeviceCheckToken();
        #endif
    }

    void ReceiveDeviceCheckToken( string token)
    {
        if( !string.IsNullOrEmpty(token))
        {
            Token.text = token;
            Debug.Log("Received Device Check Token : " + token);
            SendToServer(token);
        }
        else
        {
            Token.text = "Failed to receive Device check Token";
            Debug.LogError("Failed to receive Device check Token");
        }
    }



    async void SendToServer( string token)
     {
         var ctx = BeamContext.Default;
         await ctx.OnReady;
         try
         {


             var result = await ctx.Microservices().DeviceCheckMicroservice().SendToServer(token);
             Debug.Log("this is the result " + result);
             checkingdevice.text = " Result is : " + result;
         }
         catch (Exception e)
         {

             Debug.LogError(e);
             

         }
       
     }

  

   

   

}
