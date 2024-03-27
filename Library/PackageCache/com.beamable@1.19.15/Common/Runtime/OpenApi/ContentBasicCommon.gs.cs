
namespace Beamable.Api.Autogenerated.Content
{
    using Beamable.Api.Autogenerated.Models;
    using Beamable.Common.Content;
    using Beamable.Common;
    using IBeamableRequester = Beamable.Common.Api.IBeamableRequester;
    using Method = Beamable.Common.Api.Method;
    using Beamable.Common.Dependencies;
    
    public partial interface IContentApi
    {
    }
    public partial class ContentApi : IContentApi
    {
        private IBeamableRequester _requester;
        private IDependencyProvider _provider;
        private System.Collections.Generic.List<Beamable.Serialization.JsonSerializable.ISerializableFactory> _serializationFactories;
        public ContentApi(IBeamableRequester requester, [System.Runtime.InteropServices.DefaultParameterValueAttribute(default(Beamable.Common.Dependencies.IDependencyProvider))] [System.Runtime.InteropServices.OptionalAttribute()] IDependencyProvider provider)
        {
            this._requester = requester;
            _provider = provider;
            _serializationFactories = new System.Collections.Generic.List<Beamable.Serialization.JsonSerializable.ISerializableFactory>();
            _serializationFactories.Add(new IOneOf_HttpCallOrPublishMessageOrServiceCallFactory());
            _serializationFactories.Add(new IOneOf_CronTriggerOrExactTriggerFactory());
            _serializationFactories.Add(new IOneOf_ContentOrTextOrBinaryFactory());
        }
        private T Serialize<T>(string json)
            where T : Beamable.Serialization.JsonSerializable.ISerializable, new ()
        {
            if ((_provider != default(Beamable.Common.Dependencies.IDependencyProvider)))
            {
                if (_provider.CanBuildService<ICustomSerializer<T>>())
                {
                    ICustomSerializer<T> serializer = _provider.GetService<ICustomSerializer<T>>();
                    return serializer.Deserialize(json);
                }
            }
            return Beamable.Serialization.JsonSerializable.FromJson<T>(json, _serializationFactories);
        }
    }
}
