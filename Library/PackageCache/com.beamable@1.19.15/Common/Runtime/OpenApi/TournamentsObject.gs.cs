
namespace Beamable.Api.Autogenerated.Tournaments
{
    using Beamable.Api.Autogenerated.Models;
    using Beamable.Common.Content;
    using Beamable.Common;
    using IBeamableRequester = Beamable.Common.Api.IBeamableRequester;
    using Method = Beamable.Common.Api.Method;
    using Beamable.Common.Dependencies;
    
    public partial interface ITournamentsApi
    {
        /// <param name="objectId">Gamertag of the player.Underlying objectId type is integer in format int64.</param>
        /// <param name="includeAuthHeader">By default, every request will include an authorization header so that the request acts on behalf of the current user. When the includeAuthHeader argument is false, the request will not include the authorization header for the current user.</param>
        /// <returns>A promise containing the <see cref="TournamentClientView"/></returns>
        Promise<TournamentClientView> ObjectGet(long objectId, [System.Runtime.InteropServices.DefaultParameterValueAttribute(true)] [System.Runtime.InteropServices.OptionalAttribute()] bool includeAuthHeader);
    }
    public partial class TournamentsApi : ITournamentsApi
    {
        /// <param name="objectId">Gamertag of the player.Underlying objectId type is integer in format int64.</param>
        /// <param name="includeAuthHeader">By default, every request will include an authorization header so that the request acts on behalf of the current user. When the includeAuthHeader argument is false, the request will not include the authorization header for the current user.</param>
        /// <returns>A promise containing the <see cref="TournamentClientView"/></returns>
        public virtual Promise<TournamentClientView> ObjectGet(long objectId, [System.Runtime.InteropServices.DefaultParameterValueAttribute(true)] [System.Runtime.InteropServices.OptionalAttribute()] bool includeAuthHeader)
        {
            string gsUrl = "/object/tournaments/{objectId}/";
            gsUrl = gsUrl.Replace("{objectId}", _requester.EscapeURL(objectId.ToString()));
            // make the request and return the result
            return _requester.Request<TournamentClientView>(Method.GET, gsUrl, default(object), includeAuthHeader, this.Serialize<TournamentClientView>);
        }
    }
}
