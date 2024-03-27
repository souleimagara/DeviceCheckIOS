
namespace Beamable.Api.Autogenerated.Notification
{
    using Beamable.Api.Autogenerated.Models;
    using Beamable.Common.Content;
    using Beamable.Common;
    using IBeamableRequester = Beamable.Common.Api.IBeamableRequester;
    using Method = Beamable.Common.Api.Method;
    using Beamable.Common.Dependencies;
    
    public partial interface INotificationApi
    {
        /// <param name="gsReq">The <see cref="NotificationRequest"/> instance to use for the request</param>
        /// <returns>A promise containing the <see cref="CommonResponse"/></returns>
        Promise<CommonResponse> PostChannel(NotificationRequest gsReq);
        /// <param name="gsReq">The <see cref="NotificationRequest"/> instance to use for the request</param>
        /// <returns>A promise containing the <see cref="CommonResponse"/></returns>
        Promise<CommonResponse> PostPlayer(NotificationRequest gsReq);
        /// <param name="gsReq">The <see cref="NotificationRequest"/> instance to use for the request</param>
        /// <returns>A promise containing the <see cref="CommonResponse"/></returns>
        Promise<CommonResponse> PostCustom(NotificationRequest gsReq);
        /// <param name="gsReq">The <see cref="ServerEvent"/> instance to use for the request</param>
        /// <returns>A promise containing the <see cref="CommonResponse"/></returns>
        Promise<CommonResponse> PostServer(ServerEvent gsReq);
        /// <param name="gsReq">The <see cref="NotificationRequest"/> instance to use for the request</param>
        /// <returns>A promise containing the <see cref="CommonResponse"/></returns>
        Promise<CommonResponse> PostGeneric(NotificationRequest gsReq);
        /// <returns>A promise containing the <see cref="SubscriberDetailsResponse"/></returns>
        Promise<SubscriberDetailsResponse> Get();
        /// <param name="gsReq">The <see cref="NotificationRequest"/> instance to use for the request</param>
        /// <returns>A promise containing the <see cref="CommonResponse"/></returns>
        Promise<CommonResponse> PostGame(NotificationRequest gsReq);
    }
    public partial class NotificationApi : INotificationApi
    {
        /// <param name="gsReq">The <see cref="NotificationRequest"/> instance to use for the request</param>
        /// <returns>A promise containing the <see cref="CommonResponse"/></returns>
        public virtual Promise<CommonResponse> PostChannel(NotificationRequest gsReq)
        {
            string gsUrl = "/basic/notification/channel";
            // make the request and return the result
            return _requester.Request<CommonResponse>(Method.POST, gsUrl, Beamable.Serialization.JsonSerializable.ToJson(gsReq), true, this.Serialize<CommonResponse>);
        }
        /// <param name="gsReq">The <see cref="NotificationRequest"/> instance to use for the request</param>
        /// <returns>A promise containing the <see cref="CommonResponse"/></returns>
        public virtual Promise<CommonResponse> PostPlayer(NotificationRequest gsReq)
        {
            string gsUrl = "/basic/notification/player";
            // make the request and return the result
            return _requester.Request<CommonResponse>(Method.POST, gsUrl, Beamable.Serialization.JsonSerializable.ToJson(gsReq), true, this.Serialize<CommonResponse>);
        }
        /// <param name="gsReq">The <see cref="NotificationRequest"/> instance to use for the request</param>
        /// <returns>A promise containing the <see cref="CommonResponse"/></returns>
        public virtual Promise<CommonResponse> PostCustom(NotificationRequest gsReq)
        {
            string gsUrl = "/basic/notification/custom";
            // make the request and return the result
            return _requester.Request<CommonResponse>(Method.POST, gsUrl, Beamable.Serialization.JsonSerializable.ToJson(gsReq), true, this.Serialize<CommonResponse>);
        }
        /// <param name="gsReq">The <see cref="ServerEvent"/> instance to use for the request</param>
        /// <returns>A promise containing the <see cref="CommonResponse"/></returns>
        public virtual Promise<CommonResponse> PostServer(ServerEvent gsReq)
        {
            string gsUrl = "/basic/notification/server";
            // make the request and return the result
            return _requester.Request<CommonResponse>(Method.POST, gsUrl, Beamable.Serialization.JsonSerializable.ToJson(gsReq), true, this.Serialize<CommonResponse>);
        }
        /// <param name="gsReq">The <see cref="NotificationRequest"/> instance to use for the request</param>
        /// <returns>A promise containing the <see cref="CommonResponse"/></returns>
        public virtual Promise<CommonResponse> PostGeneric(NotificationRequest gsReq)
        {
            string gsUrl = "/basic/notification/generic";
            // make the request and return the result
            return _requester.Request<CommonResponse>(Method.POST, gsUrl, Beamable.Serialization.JsonSerializable.ToJson(gsReq), true, this.Serialize<CommonResponse>);
        }
        /// <returns>A promise containing the <see cref="SubscriberDetailsResponse"/></returns>
        public virtual Promise<SubscriberDetailsResponse> Get()
        {
            string gsUrl = "/basic/notification/";
            // make the request and return the result
            return _requester.Request<SubscriberDetailsResponse>(Method.GET, gsUrl, default(object), true, this.Serialize<SubscriberDetailsResponse>);
        }
        /// <param name="gsReq">The <see cref="NotificationRequest"/> instance to use for the request</param>
        /// <returns>A promise containing the <see cref="CommonResponse"/></returns>
        public virtual Promise<CommonResponse> PostGame(NotificationRequest gsReq)
        {
            string gsUrl = "/basic/notification/game";
            // make the request and return the result
            return _requester.Request<CommonResponse>(Method.POST, gsUrl, Beamable.Serialization.JsonSerializable.ToJson(gsReq), true, this.Serialize<CommonResponse>);
        }
    }
}
