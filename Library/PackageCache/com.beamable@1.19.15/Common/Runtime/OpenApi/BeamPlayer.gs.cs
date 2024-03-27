
namespace Beamable.Api.Autogenerated.Player
{
    using Beamable.Api.Autogenerated.Models;
    using Beamable.Common.Content;
    using Beamable.Common;
    using IBeamableRequester = Beamable.Common.Api.IBeamableRequester;
    using Method = Beamable.Common.Api.Method;
    using Beamable.Common.Dependencies;
    
    public partial interface IBeamPlayerApi
    {
        /// <param name="playerId">PlayerId</param>
        /// <param name="includeAuthHeader">By default, every request will include an authorization header so that the request acts on behalf of the current user. When the includeAuthHeader argument is false, the request will not include the authorization header for the current user.</param>
        /// <returns>A promise containing the <see cref="PartyInvitesForPlayerResponse"/></returns>
        Promise<PartyInvitesForPlayerResponse> GetPartiesInvites(string playerId, [System.Runtime.InteropServices.DefaultParameterValueAttribute(true)] [System.Runtime.InteropServices.OptionalAttribute()] bool includeAuthHeader);
        /// <param name="playerId"></param>
        /// <param name="includeAuthHeader">By default, every request will include an authorization header so that the request acts on behalf of the current user. When the includeAuthHeader argument is false, the request will not include the authorization header for the current user.</param>
        /// <returns>A promise containing the <see cref="ApiPlayersPresencePutPlayerPresenceResponse"/></returns>
        Promise<ApiPlayersPresencePutPlayerPresenceResponse> PutPresence(string playerId, [System.Runtime.InteropServices.DefaultParameterValueAttribute(true)] [System.Runtime.InteropServices.OptionalAttribute()] bool includeAuthHeader);
        /// <param name="playerId"></param>
        /// <param name="includeAuthHeader">By default, every request will include an authorization header so that the request acts on behalf of the current user. When the includeAuthHeader argument is false, the request will not include the authorization header for the current user.</param>
        /// <returns>A promise containing the <see cref="OnlineStatus"/></returns>
        Promise<OnlineStatus> GetPresence(string playerId, [System.Runtime.InteropServices.DefaultParameterValueAttribute(true)] [System.Runtime.InteropServices.OptionalAttribute()] bool includeAuthHeader);
        /// <param name="playerId"></param>
        /// <param name="gsReq">The <see cref="SetPresenceStatusRequest"/> instance to use for the request</param>
        /// <param name="includeAuthHeader">By default, every request will include an authorization header so that the request acts on behalf of the current user. When the includeAuthHeader argument is false, the request will not include the authorization header for the current user.</param>
        /// <returns>A promise containing the <see cref="OnlineStatus"/></returns>
        Promise<OnlineStatus> PutPresenceStatus(string playerId, SetPresenceStatusRequest gsReq, [System.Runtime.InteropServices.DefaultParameterValueAttribute(true)] [System.Runtime.InteropServices.OptionalAttribute()] bool includeAuthHeader);
    }
    public partial class BeamPlayerApi : IBeamPlayerApi
    {
        /// <param name="playerId">PlayerId</param>
        /// <param name="includeAuthHeader">By default, every request will include an authorization header so that the request acts on behalf of the current user. When the includeAuthHeader argument is false, the request will not include the authorization header for the current user.</param>
        /// <returns>A promise containing the <see cref="PartyInvitesForPlayerResponse"/></returns>
        public virtual Promise<PartyInvitesForPlayerResponse> GetPartiesInvites(string playerId, [System.Runtime.InteropServices.DefaultParameterValueAttribute(true)] [System.Runtime.InteropServices.OptionalAttribute()] bool includeAuthHeader)
        {
            string gsUrl = "/api/players/{playerId}/parties/invites";
            gsUrl = gsUrl.Replace("{playerId}", _requester.EscapeURL(playerId.ToString()));
            // make the request and return the result
            return _requester.Request<PartyInvitesForPlayerResponse>(Method.GET, gsUrl, default(object), includeAuthHeader, this.Serialize<PartyInvitesForPlayerResponse>);
        }
        /// <param name="playerId"></param>
        /// <param name="includeAuthHeader">By default, every request will include an authorization header so that the request acts on behalf of the current user. When the includeAuthHeader argument is false, the request will not include the authorization header for the current user.</param>
        /// <returns>A promise containing the <see cref="ApiPlayersPresencePutPlayerPresenceResponse"/></returns>
        public virtual Promise<ApiPlayersPresencePutPlayerPresenceResponse> PutPresence(string playerId, [System.Runtime.InteropServices.DefaultParameterValueAttribute(true)] [System.Runtime.InteropServices.OptionalAttribute()] bool includeAuthHeader)
        {
            string gsUrl = "/api/players/{playerId}/presence";
            gsUrl = gsUrl.Replace("{playerId}", _requester.EscapeURL(playerId.ToString()));
            // make the request and return the result
            return _requester.Request<ApiPlayersPresencePutPlayerPresenceResponse>(Method.PUT, gsUrl, default(object), includeAuthHeader, this.Serialize<ApiPlayersPresencePutPlayerPresenceResponse>);
        }
        /// <param name="playerId"></param>
        /// <param name="includeAuthHeader">By default, every request will include an authorization header so that the request acts on behalf of the current user. When the includeAuthHeader argument is false, the request will not include the authorization header for the current user.</param>
        /// <returns>A promise containing the <see cref="OnlineStatus"/></returns>
        public virtual Promise<OnlineStatus> GetPresence(string playerId, [System.Runtime.InteropServices.DefaultParameterValueAttribute(true)] [System.Runtime.InteropServices.OptionalAttribute()] bool includeAuthHeader)
        {
            string gsUrl = "/api/players/{playerId}/presence";
            gsUrl = gsUrl.Replace("{playerId}", _requester.EscapeURL(playerId.ToString()));
            // make the request and return the result
            return _requester.Request<OnlineStatus>(Method.GET, gsUrl, default(object), includeAuthHeader, this.Serialize<OnlineStatus>);
        }
        /// <param name="playerId"></param>
        /// <param name="gsReq">The <see cref="SetPresenceStatusRequest"/> instance to use for the request</param>
        /// <param name="includeAuthHeader">By default, every request will include an authorization header so that the request acts on behalf of the current user. When the includeAuthHeader argument is false, the request will not include the authorization header for the current user.</param>
        /// <returns>A promise containing the <see cref="OnlineStatus"/></returns>
        public virtual Promise<OnlineStatus> PutPresenceStatus(string playerId, SetPresenceStatusRequest gsReq, [System.Runtime.InteropServices.DefaultParameterValueAttribute(true)] [System.Runtime.InteropServices.OptionalAttribute()] bool includeAuthHeader)
        {
            string gsUrl = "/api/players/{playerId}/presence/status";
            gsUrl = gsUrl.Replace("{playerId}", _requester.EscapeURL(playerId.ToString()));
            // make the request and return the result
            return _requester.Request<OnlineStatus>(Method.PUT, gsUrl, Beamable.Serialization.JsonSerializable.ToJson(gsReq), includeAuthHeader, this.Serialize<OnlineStatus>);
        }
    }
}
