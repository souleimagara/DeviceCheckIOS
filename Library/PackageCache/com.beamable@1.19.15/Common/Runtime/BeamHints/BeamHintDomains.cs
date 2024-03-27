namespace Beamable.Common.Assistant
{
	/// <summary>
	/// <see cref="BeamHint"/> domains are a large-scale contextual grouping for hints. We use these to organize and display them in a logical and easy to navigate way.
	/// <para/>
	/// Domains cannot have <see cref="SUB_DOMAIN_SEPARATOR"/>, <see cref="BeamHintHeader.AS_KEY_SEPARATOR"/> or <see cref="BeamHintSharedConstants.BEAM_HINT_PREFERENCES_SEPARATOR"/> as they are reserved characters
	/// </summary>
	// ReSharper disable once ClassNeverInstantiated.Global
	public class BeamHintDomains : BeamHintDomainProvider
	{
		/// <summary>
		/// Call this method to generate user domain names.
		/// We use these so we can make optimizations when querying hints and storages that are created by users versus create by Beamable.
		/// </summary>
		/// <param name="userDomainName">The "_"-separated all-caps name of your <see cref="BeamHint"/> domain.</param>
		/// <returns>A prefixed <see cref="userDomainName"/> that you can use to generate your own <see cref="BeamHint"/>s with.</returns>
		public static string GenerateUserDomain(string userDomainName) => $"{USER_DOMAIN_PREFIX}_{userDomainName}";

		/// <summary>
		/// Checks if a domain is a User-created domain.
		/// </summary>
		public static bool IsUserDomain(string domain) => domain.StartsWith(USER_DOMAIN_PREFIX);

		/// <summary>
		/// Domains generated by <see cref="GenerateUserDomain"/> start with this prefix.
		/// </summary>
		public const string USER_DOMAIN_PREFIX = "USER";

		/// <summary>
		/// Generates a Beamable-owned domain name.
		/// We use these so we can make optimizations when querying hints and storages that are created by users versus create by Beamable. 
		/// </summary>
		/// <param name="domainName">The "_"-separated all-caps name of your <see cref="BeamHint"/> domain.</param>
		/// <returns>A prefixed <see cref="domainName"/> that we can us to generate our <see cref="BeamHint"/>s.</returns>
		internal static string GenerateBeamableDomain(string domainName) => $"{BEAM_DOMAIN_PREFIX}_{domainName}";

		/// <summary>
		/// Checks if a domain is a Beamable-created domain.
		/// </summary>
		public static bool IsBeamableDomain(string domain) => domain.StartsWith(BEAM_DOMAIN_PREFIX);

		/// <summary>
		/// Domains generated by <see cref="GenerateBeamableDomain"/> start with this prefix.
		/// </summary>
		public const string BEAM_DOMAIN_PREFIX = "BEAM";

		/// <summary>
		/// Generate a sub-domain. These are used by the UI to group <see cref="BeamHint"/>s hierarchically and display them in a more organized way.
		/// Sub-domains are simply domain strings separated by <see cref="SUB_DOMAIN_SEPARATOR"/>.
		/// </summary>
		/// <param name="ownerDomain">
		/// A string generated via one of these <see cref="GenerateBeamableDomain"/>, <see cref="GenerateUserDomain"/> or <see cref="GenerateSubDomain"/>.
		/// </param>
		/// <param name="subDomainName">
		/// The name of the sub-domain to append.
		/// </param>
		/// <returns>A string defining the path to the sub-domain.</returns>
		public static string GenerateSubDomain(string ownerDomain, string subDomainName) => $"{ownerDomain}{SUB_DOMAIN_SEPARATOR}{subDomainName}";

		/// <summary>
		/// Character used to textually split domains. 
		/// </summary>
		public const string SUB_DOMAIN_SEPARATOR = "¬";

		[BeamHintDomain] public static readonly string BEAM_INIT = GenerateBeamableDomain("INITIALIZATION");
		public static bool IsInitializationDomain(string domain) => IsBeamableDomain(domain) && domain.Contains(BEAM_INIT);

		[BeamHintDomain] public static readonly string BEAM_REFLECTION_CACHE = GenerateBeamableDomain("REFLECTION_CACHE");
		public static bool IsReflectionCacheDomain(string domain) => IsBeamableDomain(domain) && domain.Contains(BEAM_REFLECTION_CACHE);


		[BeamHintDomain] public static readonly string BEAM_CSHARP_MICROSERVICES = GenerateBeamableDomain("C#MS");
		[BeamHintDomain] public static readonly string BEAM_CSHARP_MICROSERVICES_CODE_MISUSE = GenerateSubDomain(BEAM_CSHARP_MICROSERVICES, "CODE_MISUSE");
		[BeamHintDomain] public static readonly string BEAM_CSHARP_MICROSERVICES_DOCKER = GenerateSubDomain(BEAM_CSHARP_MICROSERVICES, "DOCKER");
		public static bool IsCSharpMSDomain(string domain) => IsBeamableDomain(domain) && domain.Contains(BEAM_CSHARP_MICROSERVICES);

		[BeamHintDomain] public static readonly string BEAM_CONTENT = GenerateBeamableDomain("CONTENT");
		[BeamHintDomain] public static readonly string BEAM_CONTENT_CODE_MISUSE = GenerateSubDomain(BEAM_CONTENT, "CODE_MISUSE");
		public static bool IsContentDomain(string domain) => IsBeamableDomain(domain) && domain.Contains(BEAM_CONTENT);

		[BeamHintDomain] public static readonly string BEAM_ASSISTANT = GenerateBeamableDomain("BEAMABLE_ASSISTANT");
		[BeamHintDomain] public static readonly string BEAM_ASSISTANT_CODE_MISUSE = GenerateSubDomain(BEAM_ASSISTANT, "CODE_MISUSE");
		public static bool IsAssistantDomain(string domain) => IsBeamableDomain(domain) && domain.Contains(BEAM_ASSISTANT);

		/// <summary>
		/// Attempts to get a sub-domain at depth <paramref name="currDepth"/> from the given <paramref name="domain"/> string.
		///
		/// At 0, returns the higher-level domain.
		/// At 1, the first sub-domain.
		/// Etc...
		/// </summary>
		public static bool TryGetDomainAtDepth(string domain, int currDepth, out string subDomain)
		{
			var splitDomain = domain.Split(SUB_DOMAIN_SEPARATOR[0]);
			if (currDepth >= domain.Length || currDepth < 0)
			{
				subDomain = "";
				return false;
			}

			subDomain = splitDomain[currDepth];
			return true;
		}

		/// <summary>
		/// Gets how many sub-domains are baked into the given <paramref name="domain"/> string. 
		/// </summary>
		/// <param name="domain">A string generated by <see cref="GenerateUserDomain"/>, <see cref="GenerateUserDomain"/> and/or <see cref="GenerateSubDomain"/>.</param>
		public static int GetDomainDepth(string domain) => domain.Split(SUB_DOMAIN_SEPARATOR[0]).Length - 1;
	}
}
