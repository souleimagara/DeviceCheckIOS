
namespace Beamable.Api.Autogenerated.Cloudsaving
{
    using Beamable.Api.Autogenerated.Models;
    using Beamable.Common.Content;
    using Beamable.Common;
    using IBeamableRequester = Beamable.Common.Api.IBeamableRequester;
    using Method = Beamable.Common.Api.Method;
    using Beamable.Common.Dependencies;
    
    public partial interface ICloudsavingApi
    {
        /// <param name="gsReq">The <see cref="ReplaceObjectsRequest"/> instance to use for the request</param>
        /// <returns>A promise containing the <see cref="CloudsavingBasicManifest"/></returns>
        Promise<CloudsavingBasicManifest> PostDataReplace(ReplaceObjectsRequest gsReq);
        /// <param name="gsReq">The <see cref="ObjectRequests"/> instance to use for the request</param>
        /// <returns>A promise containing the <see cref="EmptyResponse"/></returns>
        Promise<EmptyResponse> DeleteData(ObjectRequests gsReq);
        /// <param name="gsReq">The <see cref="ObjectRequests"/> instance to use for the request</param>
        /// <returns>A promise containing the <see cref="URLSResponse"/></returns>
        Promise<URLSResponse> PostDataDownloadURL(ObjectRequests gsReq);
        /// <param name="playerId"></param>
        /// <param name="request"></param>
        /// <returns>A promise containing the <see cref="ObjectsMetadataResponse"/></returns>
        Promise<ObjectsMetadataResponse> GetDataMetadata([System.Runtime.InteropServices.DefaultParameterValueAttribute(null)] [System.Runtime.InteropServices.OptionalAttribute()] Beamable.Common.Content.Optional<long> playerId, [System.Runtime.InteropServices.DefaultParameterValueAttribute(null)] [System.Runtime.InteropServices.OptionalAttribute()] Beamable.Common.Content.Optional<ObjectRequest[]> request);
        /// <param name="gsReq">The <see cref="ObjectRequests"/> instance to use for the request</param>
        /// <returns>A promise containing the <see cref="URLSResponse"/></returns>
        Promise<URLSResponse> PostDataDownloadURLFromPortal(ObjectRequests gsReq);
        /// <param name="gsReq">The <see cref="PlayerBasicCloudDataRequest"/> instance to use for the request</param>
        /// <returns>A promise containing the <see cref="CloudsavingBasicManifest"/></returns>
        Promise<CloudsavingBasicManifest> PutDataMove(PlayerBasicCloudDataRequest gsReq);
        /// <param name="gsReq">The <see cref="PlayerBasicCloudDataRequest"/> instance to use for the request</param>
        /// <returns>A promise containing the <see cref="CloudsavingBasicManifest"/></returns>
        Promise<CloudsavingBasicManifest> PutDataMoveFromPortal(PlayerBasicCloudDataRequest gsReq);
        /// <param name="gsReq">The <see cref="UploadRequestsFromPortal"/> instance to use for the request</param>
        /// <returns>A promise containing the <see cref="URLSResponse"/></returns>
        Promise<URLSResponse> PostDataUploadURLFromPortal(UploadRequestsFromPortal gsReq);
        /// <param name="gsReq">The <see cref="UploadRequests"/> instance to use for the request</param>
        /// <returns>A promise containing the <see cref="CloudsavingBasicManifest"/></returns>
        Promise<CloudsavingBasicManifest> PutDataCommitManifest(UploadRequests gsReq);
        /// <param name="gsReq">The <see cref="UploadRequests"/> instance to use for the request</param>
        /// <returns>A promise containing the <see cref="URLSResponse"/></returns>
        Promise<URLSResponse> PostDataUploadURL(UploadRequests gsReq);
        /// <param name="playerId"></param>
        /// <param name="includeAuthHeader">By default, every request will include an authorization header so that the request acts on behalf of the current user. When the includeAuthHeader argument is false, the request will not include the authorization header for the current user.</param>
        /// <returns>A promise containing the <see cref="CloudsavingBasicManifest"/></returns>
        Promise<CloudsavingBasicManifest> Get([System.Runtime.InteropServices.DefaultParameterValueAttribute(null)] [System.Runtime.InteropServices.OptionalAttribute()] Beamable.Common.Content.Optional<long> playerId, [System.Runtime.InteropServices.DefaultParameterValueAttribute(true)] [System.Runtime.InteropServices.OptionalAttribute()] bool includeAuthHeader);
    }
    public partial class CloudsavingApi : ICloudsavingApi
    {
        /// <param name="gsReq">The <see cref="ReplaceObjectsRequest"/> instance to use for the request</param>
        /// <returns>A promise containing the <see cref="CloudsavingBasicManifest"/></returns>
        public virtual Promise<CloudsavingBasicManifest> PostDataReplace(ReplaceObjectsRequest gsReq)
        {
            string gsUrl = "/basic/cloudsaving/data/replace";
            // make the request and return the result
            return _requester.Request<CloudsavingBasicManifest>(Method.POST, gsUrl, Beamable.Serialization.JsonSerializable.ToJson(gsReq), true, this.Serialize<CloudsavingBasicManifest>);
        }
        /// <param name="gsReq">The <see cref="ObjectRequests"/> instance to use for the request</param>
        /// <returns>A promise containing the <see cref="EmptyResponse"/></returns>
        public virtual Promise<EmptyResponse> DeleteData(ObjectRequests gsReq)
        {
            string gsUrl = "/basic/cloudsaving/data";
            // make the request and return the result
            return _requester.Request<EmptyResponse>(Method.DELETE, gsUrl, Beamable.Serialization.JsonSerializable.ToJson(gsReq), true, this.Serialize<EmptyResponse>);
        }
        /// <param name="gsReq">The <see cref="ObjectRequests"/> instance to use for the request</param>
        /// <returns>A promise containing the <see cref="URLSResponse"/></returns>
        public virtual Promise<URLSResponse> PostDataDownloadURL(ObjectRequests gsReq)
        {
            string gsUrl = "/basic/cloudsaving/data/downloadURL";
            // make the request and return the result
            return _requester.Request<URLSResponse>(Method.POST, gsUrl, Beamable.Serialization.JsonSerializable.ToJson(gsReq), true, this.Serialize<URLSResponse>);
        }
        /// <param name="playerId"></param>
        /// <param name="request"></param>
        /// <returns>A promise containing the <see cref="ObjectsMetadataResponse"/></returns>
        public virtual Promise<ObjectsMetadataResponse> GetDataMetadata([System.Runtime.InteropServices.DefaultParameterValueAttribute(null)] [System.Runtime.InteropServices.OptionalAttribute()] Beamable.Common.Content.Optional<long> playerId, [System.Runtime.InteropServices.DefaultParameterValueAttribute(null)] [System.Runtime.InteropServices.OptionalAttribute()] Beamable.Common.Content.Optional<ObjectRequest[]> request)
        {
            string gsUrl = "/basic/cloudsaving/data/metadata";
            string gsQuery = "?";
            System.Collections.Generic.List<string> gsQueries = new System.Collections.Generic.List<string>();
            if (((request != default(OptionalArrayOfObjectRequest)) 
                        && request.HasValue))
            {
                gsQueries.Add(string.Concat("request=", request.Value.ToString()));
            }
            if (((playerId != default(OptionalLong)) 
                        && playerId.HasValue))
            {
                gsQueries.Add(string.Concat("playerId=", playerId.Value.ToString()));
            }
            if ((gsQueries.Count > 0))
            {
                gsQuery = string.Concat(gsQuery, string.Join("&", gsQueries));
                gsUrl = string.Concat(gsUrl, gsQuery);
            }
            // make the request and return the result
            return _requester.Request<ObjectsMetadataResponse>(Method.GET, gsUrl, default(object), true, this.Serialize<ObjectsMetadataResponse>);
        }
        /// <param name="gsReq">The <see cref="ObjectRequests"/> instance to use for the request</param>
        /// <returns>A promise containing the <see cref="URLSResponse"/></returns>
        public virtual Promise<URLSResponse> PostDataDownloadURLFromPortal(ObjectRequests gsReq)
        {
            string gsUrl = "/basic/cloudsaving/data/downloadURLFromPortal";
            // make the request and return the result
            return _requester.Request<URLSResponse>(Method.POST, gsUrl, Beamable.Serialization.JsonSerializable.ToJson(gsReq), true, this.Serialize<URLSResponse>);
        }
        /// <param name="gsReq">The <see cref="PlayerBasicCloudDataRequest"/> instance to use for the request</param>
        /// <returns>A promise containing the <see cref="CloudsavingBasicManifest"/></returns>
        public virtual Promise<CloudsavingBasicManifest> PutDataMove(PlayerBasicCloudDataRequest gsReq)
        {
            string gsUrl = "/basic/cloudsaving/data/move";
            // make the request and return the result
            return _requester.Request<CloudsavingBasicManifest>(Method.PUT, gsUrl, Beamable.Serialization.JsonSerializable.ToJson(gsReq), true, this.Serialize<CloudsavingBasicManifest>);
        }
        /// <param name="gsReq">The <see cref="PlayerBasicCloudDataRequest"/> instance to use for the request</param>
        /// <returns>A promise containing the <see cref="CloudsavingBasicManifest"/></returns>
        public virtual Promise<CloudsavingBasicManifest> PutDataMoveFromPortal(PlayerBasicCloudDataRequest gsReq)
        {
            string gsUrl = "/basic/cloudsaving/data/moveFromPortal";
            // make the request and return the result
            return _requester.Request<CloudsavingBasicManifest>(Method.PUT, gsUrl, Beamable.Serialization.JsonSerializable.ToJson(gsReq), true, this.Serialize<CloudsavingBasicManifest>);
        }
        /// <param name="gsReq">The <see cref="UploadRequestsFromPortal"/> instance to use for the request</param>
        /// <returns>A promise containing the <see cref="URLSResponse"/></returns>
        public virtual Promise<URLSResponse> PostDataUploadURLFromPortal(UploadRequestsFromPortal gsReq)
        {
            string gsUrl = "/basic/cloudsaving/data/uploadURLFromPortal";
            // make the request and return the result
            return _requester.Request<URLSResponse>(Method.POST, gsUrl, Beamable.Serialization.JsonSerializable.ToJson(gsReq), true, this.Serialize<URLSResponse>);
        }
        /// <param name="gsReq">The <see cref="UploadRequests"/> instance to use for the request</param>
        /// <returns>A promise containing the <see cref="CloudsavingBasicManifest"/></returns>
        public virtual Promise<CloudsavingBasicManifest> PutDataCommitManifest(UploadRequests gsReq)
        {
            string gsUrl = "/basic/cloudsaving/data/commitManifest";
            // make the request and return the result
            return _requester.Request<CloudsavingBasicManifest>(Method.PUT, gsUrl, Beamable.Serialization.JsonSerializable.ToJson(gsReq), true, this.Serialize<CloudsavingBasicManifest>);
        }
        /// <param name="gsReq">The <see cref="UploadRequests"/> instance to use for the request</param>
        /// <returns>A promise containing the <see cref="URLSResponse"/></returns>
        public virtual Promise<URLSResponse> PostDataUploadURL(UploadRequests gsReq)
        {
            string gsUrl = "/basic/cloudsaving/data/uploadURL";
            // make the request and return the result
            return _requester.Request<URLSResponse>(Method.POST, gsUrl, Beamable.Serialization.JsonSerializable.ToJson(gsReq), true, this.Serialize<URLSResponse>);
        }
        /// <param name="playerId"></param>
        /// <param name="includeAuthHeader">By default, every request will include an authorization header so that the request acts on behalf of the current user. When the includeAuthHeader argument is false, the request will not include the authorization header for the current user.</param>
        /// <returns>A promise containing the <see cref="CloudsavingBasicManifest"/></returns>
        public virtual Promise<CloudsavingBasicManifest> Get([System.Runtime.InteropServices.DefaultParameterValueAttribute(null)] [System.Runtime.InteropServices.OptionalAttribute()] Beamable.Common.Content.Optional<long> playerId, [System.Runtime.InteropServices.DefaultParameterValueAttribute(true)] [System.Runtime.InteropServices.OptionalAttribute()] bool includeAuthHeader)
        {
            string gsUrl = "/basic/cloudsaving/";
            string gsQuery = "?";
            System.Collections.Generic.List<string> gsQueries = new System.Collections.Generic.List<string>();
            if (((playerId != default(OptionalLong)) 
                        && playerId.HasValue))
            {
                gsQueries.Add(string.Concat("playerId=", playerId.Value.ToString()));
            }
            if ((gsQueries.Count > 0))
            {
                gsQuery = string.Concat(gsQuery, string.Join("&", gsQueries));
                gsUrl = string.Concat(gsUrl, gsQuery);
            }
            // make the request and return the result
            return _requester.Request<CloudsavingBasicManifest>(Method.GET, gsUrl, default(object), includeAuthHeader, this.Serialize<CloudsavingBasicManifest>);
        }
    }
}
