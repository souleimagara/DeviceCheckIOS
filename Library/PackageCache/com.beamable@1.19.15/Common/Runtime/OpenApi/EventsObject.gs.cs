
namespace Beamable.Api.Autogenerated.Events
{
    using Beamable.Api.Autogenerated.Models;
    using Beamable.Common.Content;
    using Beamable.Common;
    using IBeamableRequester = Beamable.Common.Api.IBeamableRequester;
    using Method = Beamable.Common.Api.Method;
    using Beamable.Common.Dependencies;
    
    public partial interface IEventsApi
    {
        /// <param name="objectId">Format: events.event_content_id.event_started_timestamp</param>
        /// <param name="gsReq">The <see cref="EventPhaseEndRequest"/> instance to use for the request</param>
        /// <returns>A promise containing the <see cref="CommonResponse"/></returns>
        Promise<CommonResponse> ObjectPutEndPhase(string objectId, EventPhaseEndRequest gsReq);
        /// <param name="objectId">Format: events.event_content_id.event_started_timestamp</param>
        /// <returns>A promise containing the <see cref="EventObjectData"/></returns>
        Promise<EventObjectData> ObjectGet(string objectId);
        /// <param name="objectId">Format: events.event_content_id.event_started_timestamp</param>
        /// <returns>A promise containing the <see cref="PingRsp"/></returns>
        Promise<PingRsp> ObjectGetPing(string objectId);
        /// <param name="objectId">Format: events.event_content_id.event_started_timestamp</param>
        /// <param name="gsReq">The <see cref="SetContentRequest"/> instance to use for the request</param>
        /// <returns>A promise containing the <see cref="CommonResponse"/></returns>
        Promise<CommonResponse> ObjectPutContent(string objectId, SetContentRequest gsReq);
        /// <param name="objectId">Format: events.event_content_id.event_started_timestamp</param>
        /// <returns>A promise containing the <see cref="CommonResponse"/></returns>
        Promise<CommonResponse> ObjectDeleteContent(string objectId);
        /// <param name="objectId">Format: events.event_content_id.event_started_timestamp</param>
        /// <returns>A promise containing the <see cref="CommonResponse"/></returns>
        Promise<CommonResponse> ObjectPutRefresh(string objectId);
    }
    public partial class EventsApi : IEventsApi
    {
        /// <param name="objectId">Format: events.event_content_id.event_started_timestamp</param>
        /// <param name="gsReq">The <see cref="EventPhaseEndRequest"/> instance to use for the request</param>
        /// <returns>A promise containing the <see cref="CommonResponse"/></returns>
        public virtual Promise<CommonResponse> ObjectPutEndPhase(string objectId, EventPhaseEndRequest gsReq)
        {
            string gsUrl = "/object/events/{objectId}/endPhase";
            gsUrl = gsUrl.Replace("{objectId}", _requester.EscapeURL(objectId.ToString()));
            // make the request and return the result
            return _requester.Request<CommonResponse>(Method.PUT, gsUrl, Beamable.Serialization.JsonSerializable.ToJson(gsReq), true, this.Serialize<CommonResponse>);
        }
        /// <param name="objectId">Format: events.event_content_id.event_started_timestamp</param>
        /// <returns>A promise containing the <see cref="EventObjectData"/></returns>
        public virtual Promise<EventObjectData> ObjectGet(string objectId)
        {
            string gsUrl = "/object/events/{objectId}/";
            gsUrl = gsUrl.Replace("{objectId}", _requester.EscapeURL(objectId.ToString()));
            // make the request and return the result
            return _requester.Request<EventObjectData>(Method.GET, gsUrl, default(object), true, this.Serialize<EventObjectData>);
        }
        /// <param name="objectId">Format: events.event_content_id.event_started_timestamp</param>
        /// <returns>A promise containing the <see cref="PingRsp"/></returns>
        public virtual Promise<PingRsp> ObjectGetPing(string objectId)
        {
            string gsUrl = "/object/events/{objectId}/ping";
            gsUrl = gsUrl.Replace("{objectId}", _requester.EscapeURL(objectId.ToString()));
            // make the request and return the result
            return _requester.Request<PingRsp>(Method.GET, gsUrl, default(object), true, this.Serialize<PingRsp>);
        }
        /// <param name="objectId">Format: events.event_content_id.event_started_timestamp</param>
        /// <param name="gsReq">The <see cref="SetContentRequest"/> instance to use for the request</param>
        /// <returns>A promise containing the <see cref="CommonResponse"/></returns>
        public virtual Promise<CommonResponse> ObjectPutContent(string objectId, SetContentRequest gsReq)
        {
            string gsUrl = "/object/events/{objectId}/content";
            gsUrl = gsUrl.Replace("{objectId}", _requester.EscapeURL(objectId.ToString()));
            // make the request and return the result
            return _requester.Request<CommonResponse>(Method.PUT, gsUrl, Beamable.Serialization.JsonSerializable.ToJson(gsReq), true, this.Serialize<CommonResponse>);
        }
        /// <param name="objectId">Format: events.event_content_id.event_started_timestamp</param>
        /// <returns>A promise containing the <see cref="CommonResponse"/></returns>
        public virtual Promise<CommonResponse> ObjectDeleteContent(string objectId)
        {
            string gsUrl = "/object/events/{objectId}/content";
            gsUrl = gsUrl.Replace("{objectId}", _requester.EscapeURL(objectId.ToString()));
            // make the request and return the result
            return _requester.Request<CommonResponse>(Method.DELETE, gsUrl, default(object), true, this.Serialize<CommonResponse>);
        }
        /// <param name="objectId">Format: events.event_content_id.event_started_timestamp</param>
        /// <returns>A promise containing the <see cref="CommonResponse"/></returns>
        public virtual Promise<CommonResponse> ObjectPutRefresh(string objectId)
        {
            string gsUrl = "/object/events/{objectId}/refresh";
            gsUrl = gsUrl.Replace("{objectId}", _requester.EscapeURL(objectId.ToString()));
            // make the request and return the result
            return _requester.Request<CommonResponse>(Method.PUT, gsUrl, default(object), true, this.Serialize<CommonResponse>);
        }
    }
}
