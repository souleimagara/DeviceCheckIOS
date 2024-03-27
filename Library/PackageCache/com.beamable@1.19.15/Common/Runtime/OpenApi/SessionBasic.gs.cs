
namespace Beamable.Api.Autogenerated.Session
{
    using Beamable.Api.Autogenerated.Models;
    using Beamable.Common.Content;
    using Beamable.Common;
    using IBeamableRequester = Beamable.Common.Api.IBeamableRequester;
    using Method = Beamable.Common.Api.Method;
    using Beamable.Common.Dependencies;
    
    public partial interface ISessionApi
    {
        /// <returns>A promise containing the <see cref="SessionHeartbeat"/></returns>
        Promise<SessionHeartbeat> PostHeartbeat();
        /// <param name="dbid"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns>A promise containing the <see cref="SessionHistoryResponse"/></returns>
        Promise<SessionHistoryResponse> GetHistory(long dbid, [System.Runtime.InteropServices.DefaultParameterValueAttribute(null)] [System.Runtime.InteropServices.OptionalAttribute()] Beamable.Common.Content.Optional<int> month, [System.Runtime.InteropServices.DefaultParameterValueAttribute(null)] [System.Runtime.InteropServices.OptionalAttribute()] Beamable.Common.Content.Optional<int> year);
        /// <param name="intervalSecs"></param>
        /// <param name="playerIds"></param>
        /// <returns>A promise containing the <see cref="OnlineStatusResponses"/></returns>
        Promise<OnlineStatusResponses> GetStatus(long intervalSecs, string playerIds);
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns>A promise containing the <see cref="SessionClientHistoryResponse"/></returns>
        Promise<SessionClientHistoryResponse> GetClientHistory([System.Runtime.InteropServices.DefaultParameterValueAttribute(null)] [System.Runtime.InteropServices.OptionalAttribute()] Beamable.Common.Content.Optional<int> month, [System.Runtime.InteropServices.DefaultParameterValueAttribute(null)] [System.Runtime.InteropServices.OptionalAttribute()] Beamable.Common.Content.Optional<int> year);
        /// <param name="gsReq">The <see cref="StartSessionRequest"/> instance to use for the request</param>
        /// <returns>A promise containing the <see cref="StartSessionResponse"/></returns>
        Promise<StartSessionResponse> Post(StartSessionRequest gsReq);
    }
    public partial class SessionApi : ISessionApi
    {
        /// <returns>A promise containing the <see cref="SessionHeartbeat"/></returns>
        public virtual Promise<SessionHeartbeat> PostHeartbeat()
        {
            string gsUrl = "/basic/session/heartbeat";
            // make the request and return the result
            return _requester.Request<SessionHeartbeat>(Method.POST, gsUrl, default(object), true, this.Serialize<SessionHeartbeat>);
        }
        /// <param name="dbid"></param>
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns>A promise containing the <see cref="SessionHistoryResponse"/></returns>
        public virtual Promise<SessionHistoryResponse> GetHistory(long dbid, [System.Runtime.InteropServices.DefaultParameterValueAttribute(null)] [System.Runtime.InteropServices.OptionalAttribute()] Beamable.Common.Content.Optional<int> month, [System.Runtime.InteropServices.DefaultParameterValueAttribute(null)] [System.Runtime.InteropServices.OptionalAttribute()] Beamable.Common.Content.Optional<int> year)
        {
            string gsUrl = "/basic/session/history";
            string gsQuery = "?";
            System.Collections.Generic.List<string> gsQueries = new System.Collections.Generic.List<string>();
            gsQueries.Add(string.Concat("dbid=", _requester.EscapeURL(dbid.ToString())));
            if (((month != default(OptionalInt)) 
                        && month.HasValue))
            {
                gsQueries.Add(string.Concat("month=", month.Value.ToString()));
            }
            if (((year != default(OptionalInt)) 
                        && year.HasValue))
            {
                gsQueries.Add(string.Concat("year=", year.Value.ToString()));
            }
            if ((gsQueries.Count > 0))
            {
                gsQuery = string.Concat(gsQuery, string.Join("&", gsQueries));
                gsUrl = string.Concat(gsUrl, gsQuery);
            }
            // make the request and return the result
            return _requester.Request<SessionHistoryResponse>(Method.GET, gsUrl, default(object), true, this.Serialize<SessionHistoryResponse>);
        }
        /// <param name="intervalSecs"></param>
        /// <param name="playerIds"></param>
        /// <returns>A promise containing the <see cref="OnlineStatusResponses"/></returns>
        public virtual Promise<OnlineStatusResponses> GetStatus(long intervalSecs, string playerIds)
        {
            string gsUrl = "/basic/session/status";
            string gsQuery = "?";
            System.Collections.Generic.List<string> gsQueries = new System.Collections.Generic.List<string>();
            gsQueries.Add(string.Concat("playerIds=", _requester.EscapeURL(playerIds.ToString())));
            gsQueries.Add(string.Concat("intervalSecs=", _requester.EscapeURL(intervalSecs.ToString())));
            if ((gsQueries.Count > 0))
            {
                gsQuery = string.Concat(gsQuery, string.Join("&", gsQueries));
                gsUrl = string.Concat(gsUrl, gsQuery);
            }
            // make the request and return the result
            return _requester.Request<OnlineStatusResponses>(Method.GET, gsUrl, default(object), true, this.Serialize<OnlineStatusResponses>);
        }
        /// <param name="month"></param>
        /// <param name="year"></param>
        /// <returns>A promise containing the <see cref="SessionClientHistoryResponse"/></returns>
        public virtual Promise<SessionClientHistoryResponse> GetClientHistory([System.Runtime.InteropServices.DefaultParameterValueAttribute(null)] [System.Runtime.InteropServices.OptionalAttribute()] Beamable.Common.Content.Optional<int> month, [System.Runtime.InteropServices.DefaultParameterValueAttribute(null)] [System.Runtime.InteropServices.OptionalAttribute()] Beamable.Common.Content.Optional<int> year)
        {
            string gsUrl = "/basic/session/client/history";
            string gsQuery = "?";
            System.Collections.Generic.List<string> gsQueries = new System.Collections.Generic.List<string>();
            if (((month != default(OptionalInt)) 
                        && month.HasValue))
            {
                gsQueries.Add(string.Concat("month=", month.Value.ToString()));
            }
            if (((year != default(OptionalInt)) 
                        && year.HasValue))
            {
                gsQueries.Add(string.Concat("year=", year.Value.ToString()));
            }
            if ((gsQueries.Count > 0))
            {
                gsQuery = string.Concat(gsQuery, string.Join("&", gsQueries));
                gsUrl = string.Concat(gsUrl, gsQuery);
            }
            // make the request and return the result
            return _requester.Request<SessionClientHistoryResponse>(Method.GET, gsUrl, default(object), true, this.Serialize<SessionClientHistoryResponse>);
        }
        /// <param name="gsReq">The <see cref="StartSessionRequest"/> instance to use for the request</param>
        /// <returns>A promise containing the <see cref="StartSessionResponse"/></returns>
        public virtual Promise<StartSessionResponse> Post(StartSessionRequest gsReq)
        {
            string gsUrl = "/basic/session/";
            // make the request and return the result
            return _requester.Request<StartSessionResponse>(Method.POST, gsUrl, Beamable.Serialization.JsonSerializable.ToJson(gsReq), true, this.Serialize<StartSessionResponse>);
        }
    }
}
