using Beamable.Api;
using Beamable.Api.AdvertisingIdentifier;
using Beamable.Api.Auth;
using Beamable.Api.Autogenerated.Models;
using Beamable.Api.Autogenerated.Realms;
using Beamable.Api.Caches;
using Beamable.Api.Connectivity;
using Beamable.Api.Notification;
using Beamable.Api.Payments;
using Beamable.Api.Sessions;
using Beamable.Common;
using Beamable.Common.Api;
using Beamable.Common.Api.Auth;
using Beamable.Common.Api.Content;
using Beamable.Common.Api.Notifications;
using Beamable.Common.Api.Presence;
using Beamable.Common.Content;
using Beamable.Common.Dependencies;
using Beamable.Common.Spew;
using Beamable.Common.Util;
using Beamable.Config;
using Beamable.Connection;
using Beamable.Content.Utility;
using Beamable.Coroutines;
using Beamable.Player;
using Core.Platform.SDK;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Debug = UnityEngine.Debug;
using EmptyResponse = Beamable.Common.Api.EmptyResponse;
using ExternalIdentity = Beamable.Common.Api.Auth.ExternalIdentity;
using Random = UnityEngine.Random;
using TokenResponse = Beamable.Common.Api.Auth.TokenResponse;

namespace Beamable
{

	/// <summary>
	/// The observed player has a limited subset of functionality from the larger <see cref="BeamContext"/>.
	/// </summary>
	public interface IObservedPlayer : IUserContext
	{
		PlayerStats Stats { get; }
		PlayerLobby Lobby { get; }
		PlayerParty Party { get; }
	}

	/// <summary>
	/// <para>
	/// The <see cref="BeamContext"/> represents a player's session and is equipped with all required services.
	/// <see cref="BeamContext"/> instances are not tied to scene memory. Instead, <see cref="BeamContext"/> instances live in a static way.
	/// However, if a <see cref="BeamableBehaviour"/> is connected to a <see cref="BeamContext"/>, then the <see cref="BeamableBehaviour"/>'s lifecycle
	/// will control the `<see cref="BeamContext"/>'s lifecycle.
	/// Additionally, the <see cref="PlayerCode"/> variable is the unique id of the <see cref="BeamContext"/>.
	/// </para>
	///
	/// <para>
	/// From Monobehaviours or Unity components, you should use the <see cref="InParent"/> method to access a <see cref="BeamContext"/>.
	/// The <see cref="InParent"/> will give you the closest context in the Unity object heirarchy from your script's location, or give you
	/// <see cref="Default"/> if no context exists. You can add <see cref="BeamableBehaviour"/> components to add a context to a GameObject.
	///
	/// If you want to access an instance without using a context sensitive approach, you should use the <see cref="Instantiate"/> method.
	/// Finally, you can always use <see cref="Default"/>.
	/// </para>
	///
	/// <para>
	/// Remember, there will only ever be one instance of <see cref="BeamContext"/> per <see cref="PlayerCode"/> value.
	/// The <see cref="Default"/> context has a <see cref="PlayerCode"/> value of an empty string.
	/// </para>
	/// </summary>
	[Serializable]
	public class BeamContext : IPlatformService, IGameObjectContext, IObservedPlayer, IBeamableDisposableOrder, IDependencyNameProvider, IDependencyScopeNameProvider
	{
		#region Internal State
		/// <summary>
		/// The <see cref="PlayerCode"/> is the name of a player's slot on the device. The <see cref="Default"/> context uses an empty string,
		/// but you could use values like "player1" and "player2" to enable a feature like couch-coop.
		/// </summary>
		public string PlayerCode { get; private set; }

		/// <summary>
		/// The User that this context is authenticated with. Any web-calls that are made from this <see cref="BeamContext"/> are made by this User
		/// </summary>
		public ObservableUser AuthorizedUser = new ObservableUser
		{
			Value = new User
			{
				thirdPartyAppAssociations = new List<string>(),
				external = new List<ExternalIdentity>(),
				deviceIds = new List<string>(),
				scopes = new List<string>()
			}
		};

		/// <summary>
		/// The <see cref="IDependencyProvider"/> is a collection of all services required to provide a Beamable SDK full funcitonality
		/// </summary>
		public IDependencyProvider ServiceProvider => _serviceScope;

		/// <summary>
		/// If the <see cref="Stop"/> method has been run, this property will return true. Once a context is disposed, you shouldn't use
		/// it anymore, and the <see cref="ServiceProvider"/> will throw exceptions if you do.
		/// You can re-initialize the context by using <see cref="InParent"/>
		/// </summary>
		public bool IsStopped => _isStopped;

		/// <summary>
		/// Each <see cref="BeamContext"/> has a set of components that need to live on a gameObject in the scene.
		/// </summary>
		public GameObject GameObject => _gob ? _gob : _parent?.GameObject;

		public bool IsInitialized => _initPromise != null;

		public bool IsDefault => IsDefaultPlayerCode(PlayerCode);

		private IDependencyProviderScope _serviceScope;
		private bool _isStopped;
		private GameObject _gob;
		private Promise _initPromise;
		private BeamContext _parent;
		private HashSet<BeamContext> _children = new HashSet<BeamContext>();


		#endregion

		#region Service Accessors

		public long PlayerId
		{
			get
			{
				var userContext = ServiceProvider.GetService<IUserContext>();
				if (userContext == this) return AuthorizedUser?.Value?.id ?? 0;
				return userContext.UserId;
			}
		}

		public string Cid => _requester.Cid;

		public string Pid => _requester.Pid; // TODO: rename to rid

		public AccessToken AccessToken => _requester.Token;

		public IBeamableRequester Requester => ServiceProvider.GetService<IBeamableRequester>();
		public CoroutineService CoroutineService => ServiceProvider.GetService<CoroutineService>();


		[SerializeField]
		private PlayerAnnouncements _announcements;
		// [SerializeField]
		// private PlayerCurrencyGroup _currency;
		[SerializeField]
		private PlayerStats _playerStats;

		[SerializeField] private PlayerLobby _playerLobby;

		public PlayerAnnouncements Announcements =>
			_announcements?.IsInitialized ?? false
				? _announcements
				: (_announcements = _serviceScope.GetService<PlayerAnnouncements>());

		// public PlayerCurrencyGroup Currencies =>
		// 	_currency?.IsInitialized ?? false
		// 		? _currency
		// 		: (_currency = _serviceScope.GetService<PlayerCurrencyGroup>());

		public PlayerStats Stats =>
			_playerStats?.IsInitialized ?? false
				? _playerStats
				: (_playerStats = _serviceScope.GetService<PlayerStats>());

		/// <summary>
		/// Access the <see cref="PlayerLobby"/> for this context.
		/// </summary>
		public PlayerLobby Lobby => _playerLobby = _playerLobby ?? _serviceScope.GetService<PlayerLobby>();

		/// <summary>
		/// Access the <see cref="PlayerParty"/> for this context.
		/// </summary>
		public PlayerParty Party => _serviceScope.GetService<PlayerParty>();

		/// <summary>
		/// Access the <see cref="IPresenceApi"/> for this context.
		/// </summary>
		public IPresenceApi Presence => _serviceScope.GetService<IPresenceApi>();

		/// <summary>
		/// Access the <see cref="PlayerAccounts"/> for this context.
		/// </summary>
		public PlayerAccounts Accounts
		{
			get
			{
				var service = _serviceScope.GetService<PlayerAccounts>();
				_playerAccounts = service;
				return service;
			}
		}

		private bool HasAccountsService => _playerAccounts != null;
		private PlayerAccounts _playerAccounts;

		/// <summary>
		/// <para>
		/// Access the player's inventory
		/// </para>
		/// <para>
		/// <inheritdoc cref="PlayerInventory"/>
		/// </para>
		/// </summary>
		public PlayerInventory Inventory => ServiceProvider.GetService<PlayerInventory>();

		/// <summary>
		/// Access the <see cref="IContentApi"/> for this player.
		/// </summary>
		public IContentApi Content => _contentService = _contentService ?? _serviceScope.GetService<IContentApi>();

		/// <summary>
		/// Access the <see cref="IBeamableAPI"/> for this player.
		/// </summary>
		public ApiServices Api => ServiceProvider.GetService<ApiServices>();

		/// <summary>
		/// Access the player's friends and blocked enemies.
		/// </summary>
		public PlayerSocial Social => _serviceScope.GetService<PlayerSocial>();

		public string TimeOverride
		{
			get => _requester.TimeOverride;
			set
			{
				if (value == null)
				{
					_requester.TimeOverride = null;
					TimeOverrideChanged?.Invoke();
					return;
				}

				var date = DateTime.Parse(value, null, System.Globalization.DateTimeStyles.RoundtripKind);
				var str = date.ToString(DateUtility.ISO_FORMAT);
				_requester.TimeOverride = str;
				TimeOverrideChanged?.Invoke();
			}
		}

		private AccessTokenStorage _tokenStorage;
		private EnvironmentData _environment;
		private IPlatformRequester _requester;
		private IBeamableApiRequester _beamableApiRequester;

		// TODO: Assess each of these as "do we need this as hard field state"
		private IAuthService _authService;
		private IContentApi _contentService;
		private IConnectivityService _connectivityService;
		private IConnectivityChecker _connectivityChecker;
		private NotificationService _notification;
		private ISessionService _sessionService;
		private IHeartbeatService _heartbeatService;
		private BeamableBehaviour _behaviour;
		private OfflineCache _offlineCache;
		private static bool IsDefaultPlayerCode(string code) => DefaultPlayerCode == code;
#if BEAMABLE_ENABLE_BEAM_CONTEXT_DEFAULT_OVERRIDE
		private static string DefaultPlayerCode { get; set; } = string.Empty;
#else
		private static string DefaultPlayerCode => string.Empty;
#endif
		#endregion

		#region events

		public event Action OnShutdown;
		public event Action OnShutdownComplete;
		public event Action OnReloadUser;
		public event Action TimeOverrideChanged; // TODO: What to do with the time override?

		public event Action<User> OnUserLoggingOut;
		public event Action<User> OnUserLoggedIn;

		#endregion

		protected BeamContext()
		{
			AuthorizedUser.OnDataUpdated += user => OnUserLoggedIn?.Invoke(user);
		}

		/// <summary>
		/// A <see cref="BeamContext"/> is configured for one authorized user.
		/// You can get <see cref="Common.Api.Auth.TokenResponse"/> values from the <see cref="IAuthService"/> by calling various log in methods.
		///
		/// This method will <i>create</i> new <see cref="BeamContext"/> instance using <see cref="Common.Api.Auth.TokenResponse"/> values
		/// </summary>
		/// <param name="playerCode">id for the <see cref="BeamContext"/></param>
		/// <param name="token">Authorization token</param>
		/// <returns>New instance of the <see cref="BeamContext"/></returns>
		public static BeamContext CreateAuthorizedContext(string playerCode, TokenResponse token)
		{
			bool isDefault = IsDefaultPlayerCode(playerCode);
			if (isDefault || _playerCodeToContext.ContainsKey(playerCode))
			{
#if UNITY_EDITOR
				const string log =
					@"<b>BeamContext</b> with id <b>{0}</b> already exists. " +
					"In order to update existing BeamContext it is recommended to use <b>" +
					nameof(ChangeAuthorizedPlayer) + "</b> method instead.";
				Debug.LogError(string.Format(log, isDefault ? "Default" : playerCode));
#endif
				throw new BeamContextInitException(_playerCodeToContext[playerCode],
												   new[] { new Exception($"BeamContext with \"{playerCode}\" prefix already exist.") });
			}


			string cid = Beam.RuntimeConfigProvider.Cid;
			string pid = Beam.RuntimeConfigProvider.Pid;

			var accessToken = new AccessToken(new AccessTokenStorage(playerCode), cid, pid, token.access_token,
											  token.refresh_token, token.expires_in);
			accessToken.Save();
			return ForPlayer(playerCode);
		}


		/// <summary>
		/// A <see cref="BeamContext"/> is configured for one authorized user. If you wish to change the user, you need to give it a new token.
		/// You can get <see cref="TokenResponse"/> values from the <see cref="IAuthService"/> by calling various log in methods.
		///
		/// This method will <i>mutate</i> the current <see cref="BeamContext"/> instance itself, and returned the mutated object.
		/// </summary>
		/// <param name="token"></param>
		/// <returns>The same instance, but now mutated for the new authorized user</returns>
		public async Promise<BeamContext> ChangeAuthorizedPlayer(TokenResponse token)
		{
			if (AuthorizedUser != null)
			{
				OnUserLoggingOut?.Invoke(AuthorizedUser);
			}

			await SaveToken(token); // set the token so that it gets picked up on the next initialization
			var ctx = Instantiate(_behaviour, PlayerCode);

			await InitProcedure(silent: true);

			// before we broadcast the event; we'll ask the accounts to update if they exist...
			if (HasAccountsService)
			{
				await Accounts.Refresh();
			}
			OnReloadUser?.Invoke();

			return ctx;
		}


		/// <summary>
		/// Using the authorization associated with the current context, observe the public data of another player
		/// </summary>
		/// <param name="otherPlayerId"></param>
		public IObservedPlayer ObservePlayer(long otherPlayerId)
		{
			return Fork(builder =>
			{
				builder
					.RemoveIfExists<IUserContext>()
					.AddScoped<IUserContext>(new SimpleUserContext(otherPlayerId))
					;
			});
		}

		private BeamContext Fork(Action<IDependencyBuilder> configure)
		{
			var ctx = new BeamContext();
			ctx._parent = this;
			ctx.AuthorizedUser = AuthorizedUser;
			_children.Add(ctx);

			var subScope = _serviceScope.Fork(configure);
			ctx._serviceScope = subScope;
			ctx.InitServices(Cid, Pid);
			return ctx;
		}

		[Obsolete("You do not need to include the cid or pid anymore")]
		protected void Init(string cid,
		                    string pid,
		                    string playerCode,
		                    BeamableBehaviour behaviour,
		                    IDependencyBuilder builder)
		{
			Init(playerCode, behaviour, builder);
		}
		
		protected void Init(string playerCode,
							BeamableBehaviour behaviour,
							IDependencyBuilder builder)
		{
#if UNITY_EDITOR
			if (!UnityEditor.EditorApplication.isPlaying)
			{
				_initPromise = new Promise();
				var exception = new Exception("BeamContext is meant to use only in play mode, not edit mode.");
				_initPromise.CompleteError(exception);
				return;
			}
			if (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode)
			{
				// if the context is inside the editor, and something is trying to 
				// get the context in a tight loop as the editor exits playmode, then 
				// the GameObject will get destroyed soon, and services will start throwing
				// exceptions that the GameObject doesn't exist. Better to curb the init process all
				// together, since the application is exiting playmode anyway.
				_initPromise = new Promise(); // do not complete the promise...
				return;
			}
#endif

			PlayerCode = playerCode;
			_isStopped = false;

			var shouldCreateGob = behaviour == null; // if there is no behaviour, then we definately need one
			if (!shouldCreateGob) // but also, if the object needs to be preserved, then it also needs to be at root level
			{
				shouldCreateGob = behaviour.transform.parent != null && behaviour.DontDestroyContext.IsTruthy();
			}

			if (shouldCreateGob)
			{
				var gob = new GameObject($"Beamable {playerCode}");
				var nextBehaviour = gob.AddComponent<BeamableBehaviour>();

				if (string.IsNullOrEmpty(playerCode) || (behaviour?.DontDestroyContext?.Value ?? false))
				{
					// the default context shouldn't destroy on load, unless again, it has already been specified.
					nextBehaviour.DontDestroyContext = new OptionalBoolean { HasValue = true, Value = true };
				}

				behaviour = nextBehaviour;
			}

			_gob = behaviour.gameObject;
			builder = builder.Clone();

			RegisterServices(builder);

			var oldScope = _serviceScope;
			_serviceScope = builder.Build();
			oldScope?.Hydrate(_serviceScope);

			var config = _serviceScope.GetService<IRuntimeConfigProvider>();
			InitServices(config.Cid, config.Pid);
			_behaviour.Initialize(this);
			_initPromise = new Promise();

			IEnumerator Try()
			{
				var success = false;
				var attempt = 0;
				var coreConfig = ServiceProvider.GetService<CoreConfiguration>();
				var attemptDurations = coreConfig.ContextRetryDelays;
				var errors = new Exception[attemptDurations.Length];
				while (!success)
				{
					var attemptIndex = attempt;
					yield return InitProcedure()
						.Error(err =>
						{
							errors[attemptIndex] = err;
							switch (err)
							{
								case ServiceScopeDisposedException ex:
									PlatformLogger.Log($"Beamable is exiting early, and caught a {ex.GetType().Name} exception. msg=[{ex.Message}] stack=[{ex.StackTrace}]");
									break;
								default:
									Debug.LogException(err);
									break;
							}
						})
						.Then(__ =>
						{
							success = true;
							_initPromise.CompleteSuccess();
						}).ToYielder();

					if (success) break;

					if (attempt >= attemptDurations.Length - 1 && !coreConfig.EnableInfiniteContextRetries)
					{
						// we've failed, and we've been configured to exit early.
						_initPromise.CompleteError(new BeamContextInitException(this, errors));
						yield break;
					}

					var duration =
						attemptDurations[attempt >= attemptDurations.Length ? attemptDurations.Length - 1 : attempt];
					Debug.LogWarning($"Beamable couldn't start. playerCode=[{playerCode}] Will try again in {duration} seconds...");
					yield return new WaitForSecondsRealtime((float)duration);
					attempt++;
				}
			}
			CoroutineService.StartNew("context_init", Try());
		}

		protected virtual void RegisterServices(IDependencyBuilder builder)
		{
			builder.AddSingleton<PlatformRequester>(provider => new PlatformRequester(provider));
			builder.AddSingleton<IBeamableApiRequester>(provider => new BeamableApiRequester(provider));
			builder.AddSingleton<IPlatformRequester>(provider => provider.GetService<PlatformRequester>());
			builder.AddSingleton<IBeamableAPI>(provider => Api);
			builder.AddSingleton<BeamContext>(this);
			builder.AddSingleton<IPlatformService>(this);
			builder.AddSingleton<IGameObjectContext>(this);
			builder.AddScoped<IDependencyScopeNameProvider>(this);
			builder.AddSingleton<IDependencyNameProvider>(this);
			builder.AddSingleton(new AccessTokenStorage(PlayerCode));
		}

		protected virtual void InitServices(string cid, string pid)
		{
			_tokenStorage = ServiceProvider.GetService<AccessTokenStorage>();
			_environment = ServiceProvider.GetService<EnvironmentData>();
			_authService = ServiceProvider.GetService<IAuthService>();

			_requester = ServiceProvider.GetService<IPlatformRequester>();
			_beamableApiRequester = ServiceProvider.GetService<IBeamableApiRequester>();
			_requester.AuthService = _authService;
			_requester.Cid = cid;
			_requester.Pid = pid;

			_connectivityService = ServiceProvider.GetService<IConnectivityService>();
			if (ServiceProvider.CanBuildService<IConnectivityChecker>())
			{
				_connectivityChecker = ServiceProvider.GetService<IConnectivityChecker>();
			}
			_notification = ServiceProvider.GetService<NotificationService>();
			_sessionService = ServiceProvider.GetService<ISessionService>();
			_heartbeatService = ServiceProvider.GetService<IHeartbeatService>();
			_behaviour = ServiceProvider.GetService<BeamableBehaviour>();
			_offlineCache = ServiceProvider.GetService<OfflineCache>();

			var _ = _serviceScope.GetService<SingletonDependencyList<ILoadWithContext>>();
		}


		private async Promise SaveToken(TokenResponse rsp)
		{
			ClearToken();
			var token = new AccessToken(_tokenStorage,
										Cid,
										Pid,
										rsp.access_token,
										rsp.refresh_token,
										rsp.expires_in);

			_requester.Token = token;
			_beamableApiRequester.Token = token;
			await _requester.Token.Save();
		}

		private void ClearToken()
		{
			_requester.DeleteToken();
		}

		private async Promise InitStep_StartPubnub()
		{
			try
			{
				var pubnubSubscriptionManager = ServiceProvider.GetService<IPubnubSubscriptionManager>();
				pubnubSubscriptionManager.UnsubscribeAll();
				await pubnubSubscriptionManager.SubscribeToProvider();
			}
			catch (NoConnectivityException)
			{
				// let it go..
			}
		}

		private async Promise InitStep_StartWebsocket(string socketUri)
		{
			// Need to get this in order to subscribe the message callbacks.
			var _ = _serviceScope.GetService<BeamableSubscriptionManager>();
			var connection = _serviceScope.GetService<IBeamableConnection>();
			await connection.Connect(socketUri, _beamableApiRequester.Token);
		}


		private async Promise InitProcedure(bool silent = false)
		{

			#region load token from storage
			_requester.Token = _tokenStorage.LoadTokenForRealmImmediate(Cid, Pid);
			_beamableApiRequester.Token = _requester.Token;
			#endregion

			#region wait for connectivity...
			var hasInternet = await _connectivityChecker.ForceCheck();
			#endregion

			#region make decisions about the current account
			var hasNoToken = AccessToken == null;
			var hasOfflineToken = AccessToken?.Token == Constants.Commons.OFFLINE;
			var needsToken = hasNoToken || hasOfflineToken;
			#endregion

			if (!hasInternet)
			{
				await SetupWithoutConnection();
				// when internet reconnects, re-run the init. 
				_connectivityService.OnReconnectOnce(() => InitProcedure(), -100);
			}
			else
			{
				try
				{
					await SetupWithConnection();
				}
				catch (NoConnectivityException)
				{
					// we tried so hard to avoid this.
					Debug.LogWarning("Lost internet during Beamable initiation. unpredictable behaviour may occur.");
				}
			}

			async Promise SetupBeamableNotificationChannel()
			{
				RealmConfiguration config = await ServiceProvider.GetService<IRealmsApi>().GetClientDefaults();

				string provider = config.websocketConfig.provider ?? "pubnub";
				if (provider != "pubnub")
				{
					// Let's make sure that we get a fresh new JWT before attempting to connect.
					await _beamableApiRequester.RefreshToken();
					await InitStep_StartWebsocket(config.websocketConfig.uri);
				}
				else
				{
					await InitStep_StartPubnub();
				}
			}

			async Promise SetupGetUser()
			{
				var user = await _authService.GetUser();
				AuthorizedUser.Value = user;
			}

			async Promise SetupNewSession()
			{
				var adId = await AdvertisingIdentifier.GetIdentifier();
				var promise = _sessionService.StartSession(AuthorizedUser.Value, adId);
				await promise.RecoverFromNoConnectivity(_ => new EmptyResponse());
				if (CoreConfiguration.Instance.SendHeartbeat)
				{
					// this breakpoint only hit one time, so the session isn't restarting? But also, the id is bad???
					_heartbeatService.Start();
				}
			}

			async Promise SetupPurchaser()
			{
				if (ServiceProvider.CanBuildService<IBeamablePurchaser>())
				{
					var purchaser = ServiceProvider.GetService<IBeamablePurchaser>();
					Promise<Unit> promise = purchaser.Initialize(ServiceProvider);
					await promise.Recover(err =>
					{
						Debug.LogError(err);
						return PromiseBase.Unit;
					});
					ServiceProvider.GetService<Promise<IBeamablePurchaser>>().CompleteSuccess(purchaser);
				}
			}

			void SetupEmitEvents()
			{
				if (silent) return;
				ContentApi.Instance.CompleteSuccess(Content); // TODO XXX: This is a bad hack. And we really shouldn't do it. But we need to because the regular contentRef can't access a BeamContext, unless we move the entire BeamContext to C#MS land
				OnReloadUser?.Invoke();
			}

			async Promise SetupWithoutConnection()
			{
				if (needsToken)
				{
					var offlineToken = new TokenResponse
					{
						token_type = Constants.Commons.OFFLINE,
						access_token = Constants.Commons.OFFLINE,
						refresh_token = Constants.Commons.OFFLINE,
						expires_in = long.MaxValue - 1
					};
					await SaveToken(offlineToken);

					if (_offlineCache.UseOfflineCache)
					{
						var dbid = Random.Range(int.MinValue, 0);
						_offlineCache.Set<User>(AuthApi.ACCOUNT_URL + "/me",
												new User
												{
													id = dbid,
													scopes = new List<string>(),
													thirdPartyAppAssociations = new List<string>(),
													deviceIds = new List<string>()
												}, Requester.AccessToken, true);
						_offlineCache.Merge("stats", Requester.AccessToken, new Dictionary<long, Dictionary<string, string>>
						{
							[dbid] = new Dictionary<string, string>()
						});
					}
				}

				await SetupGetUser();
				SetupEmitEvents();
			}

			async Promise SetupWithConnection()
			{
				if (needsToken)
				{
					var rsp = await _authService.CreateUser();
					await SaveToken(rsp);
				}
				else if (AccessToken.IsExpired)
				{
					var rsp = await _authService.LoginRefreshToken(AccessToken.RefreshToken);
					await SaveToken(rsp);
				}

				await SetupGetUser();
				var connection = SetupBeamableNotificationChannel();
				var session = SetupNewSession();
				var purchase = SetupPurchaser();
				await Promise.Sequence(connection, session, purchase);

				SetupEmitEvents();
			}
		}



		/// <summary>
		/// Create or retrieve a <see cref="BeamContext"/> for the given <see cref="PlayerCode"/>. There is only one instance of a context per <see cref="PlayerCode"/>.
		/// A <see cref="BeamableBehaviour"/> is required because the context needs to attach specific Unity components to a GameObject, and the given <see cref="BeamableBehaviour"/>'s GameObject will be used.
		/// If no <see cref="BeamableBehaviour"/> is given, then a new GameObject will be instantiated at the root transform level named, "Beamable (playerCode)"
		/// </summary>
		/// <param name="beamable">A component that will invite other Beamable components to exist on its GameObject</param>
		/// <param name="playerCode">A named code that represents a player slot on the device. The <see cref="Default"/> context uses an empty string. </param>
		/// <returns></returns>
		public static BeamContext Instantiate(
			BeamableBehaviour beamable = null,
			string playerCode = null,
			IDependencyBuilder dependencyBuilder = null
			)
		{

			dependencyBuilder = dependencyBuilder ?? Beam.DependencyBuilder;

			playerCode ??= DefaultPlayerCode;

			// there should only be one context per playerCode.
			if (_playerCodeToContext.TryGetValue(playerCode, out var existingContext))
			{
				if (existingContext.IsStopped)
				{
					existingContext.Init(playerCode, beamable, dependencyBuilder);
				}

				return existingContext;
			}

#if BEAMABLE_ENABLE_BEAM_CONTEXT_DEFAULT_OVERRIDE
			if (_playerCodeToContext.Count == 0)
			{
				DefaultPlayerCode = playerCode;
			}
#endif

			var ctx = new BeamContext();
			ctx.Init(playerCode, beamable, dependencyBuilder);
			_playerCodeToContext[playerCode] = ctx;
			return ctx;
		}

		private static Dictionary<string, BeamContext> _playerCodeToContext = new Dictionary<string, BeamContext>();

		/// <summary>
		/// A static <see cref="BeamContext"/> that uses a <see cref="PlayerCode"/> of an empty string.
		/// By default, the default context will persist between scene reloads. If you need to dispose it, you'll
		/// need to manually invoke <see cref="Stop"/>
		/// </summary>
		public static BeamContext Default => Instantiate();


		/// <summary>
		/// Find the first <see cref="BeamableBehaviour.Context"/> in the parent lineage of the current component, or <see cref="BeamContext.Default"/> if no <see cref="BeamableBehaviour"/> components exist
		/// </summary>
		public static BeamContext InParent(Component c) => c.GetBeamable();

		/// <summary>
		/// Finds or creates the first <see cref="BeamContext"/> that matches the given <see cref="BeamContext.PlayerCode"/> value
		/// </summary>
		public static BeamContext ForPlayer(string playerCode = "") => Instantiate(playerCode: playerCode);

		/// <summary>
		/// Finds all <see cref="BeamContext"/>s that have been created. This may include disposed contexts.
		/// </summary>
		public static IEnumerable<BeamContext> All => _playerCodeToContext.Values;

#if UNITY_2019_3_OR_NEWER
		[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
		private static async void HandleDomainReset()
		{
			// tear down all instances, and let them reboot normally.
			await Beam.StopAllContexts();
		}
#endif

		/// <summary>
		/// After a context has been Stopped with the <see cref="Stop"/> method, this method can restart the instance.
		/// <para>
		/// If the context hasn't been stopped, this method won't do anything meaningful.
		/// </para>
		/// </summary>
		/// <returns>The same context instance</returns>
		public BeamContext Start() => Instantiate(playerCode: PlayerCode);

		/// <summary>
		/// This method will tear down a <see cref="BeamContext"/> and notify all internal services that the context should be destroyed.
		/// All coroutines associated with the context will stop.
		/// No notifications will be received internal to the context.
		/// No services will be accessible from the <see cref="ServiceProvider"/> after <see cref="Stop"/> has been called.
		/// <para>
		/// After a context has been disposed, if a call is made to <see cref="Start"/> or <see cref="Instantiate"/> with the disposed context's <see cref="PlayerCode"/>,
		/// then the disposed instance will be reinitialized and rehydrated and returned to the <see cref="Instantiate"/>'s callsite.
		/// </para>
		/// </summary>
		public async Promise Stop()
		{
			if (_isStopped) return;

			_isStopped = true;

			// clear all events...
			OnShutdown?.Invoke();

			OnReloadUser = null;
			OnShutdown = null;
			TimeOverrideChanged = null;
			OnUserLoggedIn = null;
			OnUserLoggingOut = null;

			await _serviceScope.Dispose();

			_contentService = null;
			_announcements = null;
			_playerStats = null;

			OnShutdownComplete?.Invoke();
			OnShutdownComplete = null;
		}


		/// <summary>
		/// Clear the authorization token for the <see cref="PlayerCode"/>, then internally calls <see cref="Stop"/>
		/// </summary>
		public async Promise ClearPlayerAndStop()
		{
			ClearToken();
			await Stop();
		}

		public void ChangeTime()
		{
			TimeOverrideChanged?.Invoke();
		}

		long IUserContext.UserId => PlayerId;

		User IPlatformService.User => AuthorizedUser.Value;
		public Promise<Unit> OnReady => _initPromise;

		/// <summary>
		/// A <see cref="Promise{T}"/> that is completed when the <see cref="BeamContext"/> is ready to be used.
		/// This promise happens immediately after the <see cref="OnReady"/> promise, but returns the current instance
		/// of the context.
		///
		/// Suggested usage.
		/// <code>
		/// var ctx = await BeamContext.Default.Instance;
		/// </code>
		/// </summary>
		public Promise<BeamContext> Instance => OnReady?.Map(_ => this);

		INotificationService IPlatformService.Notification => _notification;

		IPubnubNotificationService IPlatformService.PubnubNotificationService =>
			ServiceProvider.GetService<IPubnubNotificationService>();
		IConnectivityService IPlatformService.ConnectivityService => _connectivityService;
		IHeartbeatService IPlatformService.Heartbeat => _heartbeatService;


		public Promise OnDispose()
		{
			if (GameObject)
			{
				UnityEngine.Object.Destroy(GameObject);
				_behaviour = null;
				_gob = null;
			}


			return Promise.Success;
		}

		string IDependencyNameProvider.DependencyProviderName => BeamUtil.SanitizeStringForPath(PlayerCode);
		string IDependencyScopeNameProvider.DependencyScopeName => PlayerId.ToString();
		int IBeamableDisposableOrder.DisposeOrder => 100;
	}

	[Serializable]
	public class BeamContextInitException : Exception
	{
		/// <summary>
		/// The <see cref="BeamContext"/> that couldn't start.
		/// </summary>
		public BeamContext Ctx { get; }

		/// <summary>
		/// A set of exceptions that map to each failed initialization attempt.
		/// There will be the same count of errors as there is in the <see cref="CoreConfiguration.ContextRetryDelays"/> array
		/// </summary>
		public Exception[] Exceptions { get; }

		public BeamContextInitException(BeamContext ctx, Exception[] exceptions) : base($"Could not initialize " +
																						$"BeamContext=[{ctx?.PlayerCode}]. " +
																						$"Errors=[{string.Join("\n", exceptions.Where(e => e != null).Select(e => e.Message))}]")
		{
			Ctx = ctx;
			Exceptions = exceptions;
		}
	}
}
