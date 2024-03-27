
namespace Beamable.Api.Autogenerated.GroupUsers
{
    using Beamable.Api.Autogenerated.Models;
    using Beamable.Common.Content;
    using Beamable.Common;
    using IBeamableRequester = Beamable.Common.Api.IBeamableRequester;
    using Method = Beamable.Common.Api.Method;
    using Beamable.Common.Dependencies;
    
    public partial interface IGroupUsersApi
    {
        /// <param name="name"></param>
        /// <param name="objectId">Gamertag of the player.Underlying objectId type is integer in format int64.</param>
        /// <param name="subGroup"></param>
        /// <param name="tag"></param>
        /// <param name="type"></param>
        /// <param name="includeAuthHeader">By default, every request will include an authorization header so that the request acts on behalf of the current user. When the includeAuthHeader argument is false, the request will not include the authorization header for the current user.</param>
        /// <returns>A promise containing the <see cref="AvailabilityResponse"/></returns>
        Promise<AvailabilityResponse> ObjectGetAvailability(long objectId, GroupType type, [System.Runtime.InteropServices.DefaultParameterValueAttribute(null)] [System.Runtime.InteropServices.OptionalAttribute()] Beamable.Common.Content.Optional<string> name, [System.Runtime.InteropServices.DefaultParameterValueAttribute(null)] [System.Runtime.InteropServices.OptionalAttribute()] Beamable.Common.Content.Optional<bool> subGroup, [System.Runtime.InteropServices.DefaultParameterValueAttribute(null)] [System.Runtime.InteropServices.OptionalAttribute()] Beamable.Common.Content.Optional<string> tag, [System.Runtime.InteropServices.DefaultParameterValueAttribute(true)] [System.Runtime.InteropServices.OptionalAttribute()] bool includeAuthHeader);
        /// <param name="objectId">Gamertag of the player.Underlying objectId type is integer in format int64.</param>
        /// <param name="includeAuthHeader">By default, every request will include an authorization header so that the request acts on behalf of the current user. When the includeAuthHeader argument is false, the request will not include the authorization header for the current user.</param>
        /// <returns>A promise containing the <see cref="GroupSearchResponse"/></returns>
        Promise<GroupSearchResponse> ObjectGetRecommended(long objectId, [System.Runtime.InteropServices.DefaultParameterValueAttribute(true)] [System.Runtime.InteropServices.OptionalAttribute()] bool includeAuthHeader);
        /// <param name="objectId">Gamertag of the player.Underlying objectId type is integer in format int64.</param>
        /// <param name="gsReq">The <see cref="GroupMembershipRequest"/> instance to use for the request</param>
        /// <param name="includeAuthHeader">By default, every request will include an authorization header so that the request acts on behalf of the current user. When the includeAuthHeader argument is false, the request will not include the authorization header for the current user.</param>
        /// <returns>A promise containing the <see cref="GroupMembershipResponse"/></returns>
        Promise<GroupMembershipResponse> ObjectPostJoin(long objectId, GroupMembershipRequest gsReq, [System.Runtime.InteropServices.DefaultParameterValueAttribute(true)] [System.Runtime.InteropServices.OptionalAttribute()] bool includeAuthHeader);
        /// <param name="objectId">Gamertag of the player.Underlying objectId type is integer in format int64.</param>
        /// <param name="gsReq">The <see cref="GroupMembershipRequest"/> instance to use for the request</param>
        /// <param name="includeAuthHeader">By default, every request will include an authorization header so that the request acts on behalf of the current user. When the includeAuthHeader argument is false, the request will not include the authorization header for the current user.</param>
        /// <returns>A promise containing the <see cref="GroupMembershipResponse"/></returns>
        Promise<GroupMembershipResponse> ObjectDeleteJoin(long objectId, GroupMembershipRequest gsReq, [System.Runtime.InteropServices.DefaultParameterValueAttribute(true)] [System.Runtime.InteropServices.OptionalAttribute()] bool includeAuthHeader);
        /// <param name="objectId">Gamertag of the player.Underlying objectId type is integer in format int64.</param>
        /// <param name="gsReq">The <see cref="GroupCreate"/> instance to use for the request</param>
        /// <param name="includeAuthHeader">By default, every request will include an authorization header so that the request acts on behalf of the current user. When the includeAuthHeader argument is false, the request will not include the authorization header for the current user.</param>
        /// <returns>A promise containing the <see cref="GroupCreateResponse"/></returns>
        Promise<GroupCreateResponse> ObjectPostGroup(long objectId, GroupCreate gsReq, [System.Runtime.InteropServices.DefaultParameterValueAttribute(true)] [System.Runtime.InteropServices.OptionalAttribute()] bool includeAuthHeader);
        /// <param name="enrollmentTypes"></param>
        /// <param name="hasSlots"></param>
        /// <param name="limit"></param>
        /// <param name="name"></param>
        /// <param name="objectId">Gamertag of the player.Underlying objectId type is integer in format int64.</param>
        /// <param name="offset"></param>
        /// <param name="scoreMax"></param>
        /// <param name="scoreMin"></param>
        /// <param name="sortField"></param>
        /// <param name="sortValue"></param>
        /// <param name="subGroup"></param>
        /// <param name="type"></param>
        /// <param name="userScore"></param>
        /// <param name="includeAuthHeader">By default, every request will include an authorization header so that the request acts on behalf of the current user. When the includeAuthHeader argument is false, the request will not include the authorization header for the current user.</param>
        /// <returns>A promise containing the <see cref="GroupSearchResponse"/></returns>
        Promise<GroupSearchResponse> ObjectGetSearch(long objectId, GroupType type, [System.Runtime.InteropServices.DefaultParameterValueAttribute(null)] [System.Runtime.InteropServices.OptionalAttribute()] Beamable.Common.Content.Optional<string> enrollmentTypes, [System.Runtime.InteropServices.DefaultParameterValueAttribute(null)] [System.Runtime.InteropServices.OptionalAttribute()] Beamable.Common.Content.Optional<bool> hasSlots, [System.Runtime.InteropServices.DefaultParameterValueAttribute(null)] [System.Runtime.InteropServices.OptionalAttribute()] Beamable.Common.Content.Optional<int> limit, [System.Runtime.InteropServices.DefaultParameterValueAttribute(null)] [System.Runtime.InteropServices.OptionalAttribute()] Beamable.Common.Content.Optional<string> name, [System.Runtime.InteropServices.DefaultParameterValueAttribute(null)] [System.Runtime.InteropServices.OptionalAttribute()] Beamable.Common.Content.Optional<int> offset, [System.Runtime.InteropServices.DefaultParameterValueAttribute(null)] [System.Runtime.InteropServices.OptionalAttribute()] Beamable.Common.Content.Optional<long> scoreMax, [System.Runtime.InteropServices.DefaultParameterValueAttribute(null)] [System.Runtime.InteropServices.OptionalAttribute()] Beamable.Common.Content.Optional<long> scoreMin, [System.Runtime.InteropServices.DefaultParameterValueAttribute(null)] [System.Runtime.InteropServices.OptionalAttribute()] Beamable.Common.Content.Optional<string> sortField, [System.Runtime.InteropServices.DefaultParameterValueAttribute(null)] [System.Runtime.InteropServices.OptionalAttribute()] Beamable.Common.Content.Optional<int> sortValue, [System.Runtime.InteropServices.DefaultParameterValueAttribute(null)] [System.Runtime.InteropServices.OptionalAttribute()] Beamable.Common.Content.Optional<bool> subGroup, [System.Runtime.InteropServices.DefaultParameterValueAttribute(null)] [System.Runtime.InteropServices.OptionalAttribute()] Beamable.Common.Content.Optional<long> userScore, [System.Runtime.InteropServices.DefaultParameterValueAttribute(true)] [System.Runtime.InteropServices.OptionalAttribute()] bool includeAuthHeader);
        /// <param name="objectId">Gamertag of the player.Underlying objectId type is integer in format int64.</param>
        /// <param name="includeAuthHeader">By default, every request will include an authorization header so that the request acts on behalf of the current user. When the includeAuthHeader argument is false, the request will not include the authorization header for the current user.</param>
        /// <returns>A promise containing the <see cref="GroupUser"/></returns>
        Promise<GroupUser> ObjectGet(long objectId, [System.Runtime.InteropServices.DefaultParameterValueAttribute(true)] [System.Runtime.InteropServices.OptionalAttribute()] bool includeAuthHeader);
    }
    public partial class GroupUsersApi : IGroupUsersApi
    {
        /// <param name="name"></param>
        /// <param name="objectId">Gamertag of the player.Underlying objectId type is integer in format int64.</param>
        /// <param name="subGroup"></param>
        /// <param name="tag"></param>
        /// <param name="type"></param>
        /// <param name="includeAuthHeader">By default, every request will include an authorization header so that the request acts on behalf of the current user. When the includeAuthHeader argument is false, the request will not include the authorization header for the current user.</param>
        /// <returns>A promise containing the <see cref="AvailabilityResponse"/></returns>
        public virtual Promise<AvailabilityResponse> ObjectGetAvailability(long objectId, GroupType type, [System.Runtime.InteropServices.DefaultParameterValueAttribute(null)] [System.Runtime.InteropServices.OptionalAttribute()] Beamable.Common.Content.Optional<string> name, [System.Runtime.InteropServices.DefaultParameterValueAttribute(null)] [System.Runtime.InteropServices.OptionalAttribute()] Beamable.Common.Content.Optional<bool> subGroup, [System.Runtime.InteropServices.DefaultParameterValueAttribute(null)] [System.Runtime.InteropServices.OptionalAttribute()] Beamable.Common.Content.Optional<string> tag, [System.Runtime.InteropServices.DefaultParameterValueAttribute(true)] [System.Runtime.InteropServices.OptionalAttribute()] bool includeAuthHeader)
        {
            string gsUrl = "/object/group-users/{objectId}/availability";
            gsUrl = gsUrl.Replace("{objectId}", _requester.EscapeURL(objectId.ToString()));
            string gsQuery = "?";
            System.Collections.Generic.List<string> gsQueries = new System.Collections.Generic.List<string>();
            if (((name != default(OptionalString)) 
                        && name.HasValue))
            {
                gsQueries.Add(string.Concat("name=", name.Value.ToString()));
            }
            if (((tag != default(OptionalString)) 
                        && tag.HasValue))
            {
                gsQueries.Add(string.Concat("tag=", tag.Value.ToString()));
            }
            gsQueries.Add(string.Concat("type=", _requester.EscapeURL(type.ToString())));
            if (((subGroup != default(OptionalBool)) 
                        && subGroup.HasValue))
            {
                gsQueries.Add(string.Concat("subGroup=", subGroup.Value.ToString()));
            }
            if ((gsQueries.Count > 0))
            {
                gsQuery = string.Concat(gsQuery, string.Join("&", gsQueries));
                gsUrl = string.Concat(gsUrl, gsQuery);
            }
            // make the request and return the result
            return _requester.Request<AvailabilityResponse>(Method.GET, gsUrl, default(object), includeAuthHeader, this.Serialize<AvailabilityResponse>);
        }
        /// <param name="objectId">Gamertag of the player.Underlying objectId type is integer in format int64.</param>
        /// <param name="includeAuthHeader">By default, every request will include an authorization header so that the request acts on behalf of the current user. When the includeAuthHeader argument is false, the request will not include the authorization header for the current user.</param>
        /// <returns>A promise containing the <see cref="GroupSearchResponse"/></returns>
        public virtual Promise<GroupSearchResponse> ObjectGetRecommended(long objectId, [System.Runtime.InteropServices.DefaultParameterValueAttribute(true)] [System.Runtime.InteropServices.OptionalAttribute()] bool includeAuthHeader)
        {
            string gsUrl = "/object/group-users/{objectId}/recommended";
            gsUrl = gsUrl.Replace("{objectId}", _requester.EscapeURL(objectId.ToString()));
            // make the request and return the result
            return _requester.Request<GroupSearchResponse>(Method.GET, gsUrl, default(object), includeAuthHeader, this.Serialize<GroupSearchResponse>);
        }
        /// <param name="objectId">Gamertag of the player.Underlying objectId type is integer in format int64.</param>
        /// <param name="gsReq">The <see cref="GroupMembershipRequest"/> instance to use for the request</param>
        /// <param name="includeAuthHeader">By default, every request will include an authorization header so that the request acts on behalf of the current user. When the includeAuthHeader argument is false, the request will not include the authorization header for the current user.</param>
        /// <returns>A promise containing the <see cref="GroupMembershipResponse"/></returns>
        public virtual Promise<GroupMembershipResponse> ObjectPostJoin(long objectId, GroupMembershipRequest gsReq, [System.Runtime.InteropServices.DefaultParameterValueAttribute(true)] [System.Runtime.InteropServices.OptionalAttribute()] bool includeAuthHeader)
        {
            string gsUrl = "/object/group-users/{objectId}/join";
            gsUrl = gsUrl.Replace("{objectId}", _requester.EscapeURL(objectId.ToString()));
            // make the request and return the result
            return _requester.Request<GroupMembershipResponse>(Method.POST, gsUrl, Beamable.Serialization.JsonSerializable.ToJson(gsReq), includeAuthHeader, this.Serialize<GroupMembershipResponse>);
        }
        /// <param name="objectId">Gamertag of the player.Underlying objectId type is integer in format int64.</param>
        /// <param name="gsReq">The <see cref="GroupMembershipRequest"/> instance to use for the request</param>
        /// <param name="includeAuthHeader">By default, every request will include an authorization header so that the request acts on behalf of the current user. When the includeAuthHeader argument is false, the request will not include the authorization header for the current user.</param>
        /// <returns>A promise containing the <see cref="GroupMembershipResponse"/></returns>
        public virtual Promise<GroupMembershipResponse> ObjectDeleteJoin(long objectId, GroupMembershipRequest gsReq, [System.Runtime.InteropServices.DefaultParameterValueAttribute(true)] [System.Runtime.InteropServices.OptionalAttribute()] bool includeAuthHeader)
        {
            string gsUrl = "/object/group-users/{objectId}/join";
            gsUrl = gsUrl.Replace("{objectId}", _requester.EscapeURL(objectId.ToString()));
            // make the request and return the result
            return _requester.Request<GroupMembershipResponse>(Method.DELETE, gsUrl, Beamable.Serialization.JsonSerializable.ToJson(gsReq), includeAuthHeader, this.Serialize<GroupMembershipResponse>);
        }
        /// <param name="objectId">Gamertag of the player.Underlying objectId type is integer in format int64.</param>
        /// <param name="gsReq">The <see cref="GroupCreate"/> instance to use for the request</param>
        /// <param name="includeAuthHeader">By default, every request will include an authorization header so that the request acts on behalf of the current user. When the includeAuthHeader argument is false, the request will not include the authorization header for the current user.</param>
        /// <returns>A promise containing the <see cref="GroupCreateResponse"/></returns>
        public virtual Promise<GroupCreateResponse> ObjectPostGroup(long objectId, GroupCreate gsReq, [System.Runtime.InteropServices.DefaultParameterValueAttribute(true)] [System.Runtime.InteropServices.OptionalAttribute()] bool includeAuthHeader)
        {
            string gsUrl = "/object/group-users/{objectId}/group";
            gsUrl = gsUrl.Replace("{objectId}", _requester.EscapeURL(objectId.ToString()));
            // make the request and return the result
            return _requester.Request<GroupCreateResponse>(Method.POST, gsUrl, Beamable.Serialization.JsonSerializable.ToJson(gsReq), includeAuthHeader, this.Serialize<GroupCreateResponse>);
        }
        /// <param name="enrollmentTypes"></param>
        /// <param name="hasSlots"></param>
        /// <param name="limit"></param>
        /// <param name="name"></param>
        /// <param name="objectId">Gamertag of the player.Underlying objectId type is integer in format int64.</param>
        /// <param name="offset"></param>
        /// <param name="scoreMax"></param>
        /// <param name="scoreMin"></param>
        /// <param name="sortField"></param>
        /// <param name="sortValue"></param>
        /// <param name="subGroup"></param>
        /// <param name="type"></param>
        /// <param name="userScore"></param>
        /// <param name="includeAuthHeader">By default, every request will include an authorization header so that the request acts on behalf of the current user. When the includeAuthHeader argument is false, the request will not include the authorization header for the current user.</param>
        /// <returns>A promise containing the <see cref="GroupSearchResponse"/></returns>
        public virtual Promise<GroupSearchResponse> ObjectGetSearch(long objectId, GroupType type, [System.Runtime.InteropServices.DefaultParameterValueAttribute(null)] [System.Runtime.InteropServices.OptionalAttribute()] Beamable.Common.Content.Optional<string> enrollmentTypes, [System.Runtime.InteropServices.DefaultParameterValueAttribute(null)] [System.Runtime.InteropServices.OptionalAttribute()] Beamable.Common.Content.Optional<bool> hasSlots, [System.Runtime.InteropServices.DefaultParameterValueAttribute(null)] [System.Runtime.InteropServices.OptionalAttribute()] Beamable.Common.Content.Optional<int> limit, [System.Runtime.InteropServices.DefaultParameterValueAttribute(null)] [System.Runtime.InteropServices.OptionalAttribute()] Beamable.Common.Content.Optional<string> name, [System.Runtime.InteropServices.DefaultParameterValueAttribute(null)] [System.Runtime.InteropServices.OptionalAttribute()] Beamable.Common.Content.Optional<int> offset, [System.Runtime.InteropServices.DefaultParameterValueAttribute(null)] [System.Runtime.InteropServices.OptionalAttribute()] Beamable.Common.Content.Optional<long> scoreMax, [System.Runtime.InteropServices.DefaultParameterValueAttribute(null)] [System.Runtime.InteropServices.OptionalAttribute()] Beamable.Common.Content.Optional<long> scoreMin, [System.Runtime.InteropServices.DefaultParameterValueAttribute(null)] [System.Runtime.InteropServices.OptionalAttribute()] Beamable.Common.Content.Optional<string> sortField, [System.Runtime.InteropServices.DefaultParameterValueAttribute(null)] [System.Runtime.InteropServices.OptionalAttribute()] Beamable.Common.Content.Optional<int> sortValue, [System.Runtime.InteropServices.DefaultParameterValueAttribute(null)] [System.Runtime.InteropServices.OptionalAttribute()] Beamable.Common.Content.Optional<bool> subGroup, [System.Runtime.InteropServices.DefaultParameterValueAttribute(null)] [System.Runtime.InteropServices.OptionalAttribute()] Beamable.Common.Content.Optional<long> userScore, [System.Runtime.InteropServices.DefaultParameterValueAttribute(true)] [System.Runtime.InteropServices.OptionalAttribute()] bool includeAuthHeader)
        {
            string gsUrl = "/object/group-users/{objectId}/search";
            gsUrl = gsUrl.Replace("{objectId}", _requester.EscapeURL(objectId.ToString()));
            string gsQuery = "?";
            System.Collections.Generic.List<string> gsQueries = new System.Collections.Generic.List<string>();
            if (((name != default(OptionalString)) 
                        && name.HasValue))
            {
                gsQueries.Add(string.Concat("name=", name.Value.ToString()));
            }
            if (((scoreMin != default(OptionalLong)) 
                        && scoreMin.HasValue))
            {
                gsQueries.Add(string.Concat("scoreMin=", scoreMin.Value.ToString()));
            }
            if (((sortField != default(OptionalString)) 
                        && sortField.HasValue))
            {
                gsQueries.Add(string.Concat("sortField=", sortField.Value.ToString()));
            }
            if (((userScore != default(OptionalLong)) 
                        && userScore.HasValue))
            {
                gsQueries.Add(string.Concat("userScore=", userScore.Value.ToString()));
            }
            if (((hasSlots != default(OptionalBool)) 
                        && hasSlots.HasValue))
            {
                gsQueries.Add(string.Concat("hasSlots=", hasSlots.Value.ToString()));
            }
            if (((enrollmentTypes != default(OptionalString)) 
                        && enrollmentTypes.HasValue))
            {
                gsQueries.Add(string.Concat("enrollmentTypes=", enrollmentTypes.Value.ToString()));
            }
            if (((offset != default(OptionalInt)) 
                        && offset.HasValue))
            {
                gsQueries.Add(string.Concat("offset=", offset.Value.ToString()));
            }
            if (((scoreMax != default(OptionalLong)) 
                        && scoreMax.HasValue))
            {
                gsQueries.Add(string.Concat("scoreMax=", scoreMax.Value.ToString()));
            }
            if (((subGroup != default(OptionalBool)) 
                        && subGroup.HasValue))
            {
                gsQueries.Add(string.Concat("subGroup=", subGroup.Value.ToString()));
            }
            if (((sortValue != default(OptionalInt)) 
                        && sortValue.HasValue))
            {
                gsQueries.Add(string.Concat("sortValue=", sortValue.Value.ToString()));
            }
            gsQueries.Add(string.Concat("type=", _requester.EscapeURL(type.ToString())));
            if (((limit != default(OptionalInt)) 
                        && limit.HasValue))
            {
                gsQueries.Add(string.Concat("limit=", limit.Value.ToString()));
            }
            if ((gsQueries.Count > 0))
            {
                gsQuery = string.Concat(gsQuery, string.Join("&", gsQueries));
                gsUrl = string.Concat(gsUrl, gsQuery);
            }
            // make the request and return the result
            return _requester.Request<GroupSearchResponse>(Method.GET, gsUrl, default(object), includeAuthHeader, this.Serialize<GroupSearchResponse>);
        }
        /// <param name="objectId">Gamertag of the player.Underlying objectId type is integer in format int64.</param>
        /// <param name="includeAuthHeader">By default, every request will include an authorization header so that the request acts on behalf of the current user. When the includeAuthHeader argument is false, the request will not include the authorization header for the current user.</param>
        /// <returns>A promise containing the <see cref="GroupUser"/></returns>
        public virtual Promise<GroupUser> ObjectGet(long objectId, [System.Runtime.InteropServices.DefaultParameterValueAttribute(true)] [System.Runtime.InteropServices.OptionalAttribute()] bool includeAuthHeader)
        {
            string gsUrl = "/object/group-users/{objectId}/";
            gsUrl = gsUrl.Replace("{objectId}", _requester.EscapeURL(objectId.ToString()));
            // make the request and return the result
            return _requester.Request<GroupUser>(Method.GET, gsUrl, default(object), includeAuthHeader, this.Serialize<GroupUser>);
        }
    }
}
