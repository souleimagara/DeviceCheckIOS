using Beamable.Api.Autogenerated.Models;
using Beamable.Api.Autogenerated.Scheduler;
using Beamable.Common.Api;
using Beamable.Common.Content;
using Beamable.Common.Scheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Beamable.Common.Scheduler
{

	public interface IBeamSchedulerContext
	{
		string Cid { get; }
		string Pid { get; }
		string Prefix { get; }
		string ServiceName { get; }
	}

	public class BeamScheduler
	{
		private readonly IBeamSchedulerApi _api;
		public IBeamSchedulerContext SchedulerContext { get; }

		public BeamScheduler(IBeamSchedulerApi api, IBeamSchedulerContext schedulerContext)
		{
			_api = api;
			SchedulerContext = schedulerContext;
		}

		/// <summary>
		/// After a <see cref="Job"/> is created, it will execute some time later.
		/// This method finds the information about the executions that have already happened.
		/// </summary>
		/// <param name="jobId">The <see cref="Job.id"/> of a <see cref="Job"/> that has been scheduled. See <see cref="CreateJob(string,string,Beamable.Common.Scheduler.ISchedulableAction,Beamable.Common.Scheduler.ISchedulerTrigger[],Beamable.Common.Scheduler.RetryPolicy)"/> to create a job.</param>
		/// <param name="limit">The number of events to fetch. By default, this is 1000.</param>
		/// <returns>A set of <see cref="JobExecution"/>s describing the recent activity.</returns>
		public async Promise<List<JobExecution>> GetJobActivity(string jobId, OptionalInt limit = null)
		{
			var res = await _api.GetJobActivity(jobId, limit);
			var events = res.Select(Utility.Convert).ToList();
			var executions = events.GroupBy(e => e.executionId).Select(g => new JobExecution
			{
				events = g.ToList(),
				executionId = g.Key,
				jobId = g.FirstOrDefault().jobId,
			}).ToList();
			return executions;
		}

		/// <summary>
		/// After a <see cref="Job"/> is scheduled, it will execute some time later.
		/// This method returns the upcoming times the job will be executed.
		/// If the job has many upcoming executions due to a CRON trigger, or multiple exactTimes,
		/// this method will include as many upcoming executions up to the <see cref="limit"/>
		/// </summary>
		/// <param name="jobId">The <see cref="Job.id"/> of a scheduled <see cref="Job"/></param>
		/// <param name="from">A time to look for executions afterwards.</param>
		/// <param name="limit">The maximum number of upcoming executions that will be returned.</param>
		/// <returns>A set of <see cref="UpcomingExecution"/></returns>
		public async Promise<List<UpcomingExecution>> GetJobUpcomingExecutions(string jobId, OptionalDateTime from = null, OptionalInt limit = null)
		{
			var res = await _api.GetJobNextExecutions(jobId, from, limit);
			var executions = res.Select(dt => new UpcomingExecution { executeAt = dt }).ToList();
			return executions;
		}


		/// <inheritdoc cref="DeleteJob(string)"/>
		public async Promise DeleteJob(Job job) => await DeleteJob(job?.id);

		/// <summary>
		/// After a <see cref="Job"/> has been scheduled using the <see cref="CreateJob(string,string,Beamable.Common.Scheduler.ISchedulableAction,Beamable.Common.Scheduler.ISchedulerTrigger[],Beamable.Common.Scheduler.RetryPolicy)"/>
		/// method, it will exist forever. This method will remove the job and cancel all upcoming executions.
		///
		/// After a job is deleted, its job activity cannot be seen.
		/// Consider using the <see cref="CancelJob"/> function instead. 
		/// <para>
		/// <b> Be aware </b> that this will not stop any currently executing jobs. 
		/// </para>
		/// </summary>
		/// <param name="jobId">The <see cref="Job.id"/> of a scheduled <see cref="Job"/></param>
		public async Promise DeleteJob(string jobId)
		{
			await _api.DeleteJob(jobId);
		}

		/// <summary>
		/// After a <see cref="Job"/> has been scheduled using the <see cref="CreateJob(string,string,Beamable.Common.Scheduler.ISchedulableAction,Beamable.Common.Scheduler.ISchedulerTrigger[],Beamable.Common.Scheduler.RetryPolicy)"/>
		/// method, it will exist forever. This method will remove all the job triggers, like <see cref="CronEvent"/> or <see cref="ExactTimeEvent"/>s.
		/// Without any events, the job will never be executed again.
		/// However, unlike, <see cref="DeleteJob(Beamable.Common.Scheduler.Job)"/>, the job still exists,
		/// and therefor, methods like, <see cref="GetJobActivity"/> will still be available.
		///
		/// <para>
		/// <b> Be aware </b> that this will not stop any currently executing jobs. 
		/// </para>
		/// </summary>
		/// <param name="jobId">The <see cref="Job.id"/> of a scheduled <see cref="Job"/></param>
		public async Promise CancelJob(string jobId)
		{
			await _api.PutJobCancel(jobId);
		}

		/// <summary>
		/// Fetch a list of scheduled <see cref="Job"/>s
		/// </summary>
		/// <param name="limit">The maximum number of jobs to find. The default value is 1000.</param>
		/// <param name="source">Filter <see cref="Job"/>s by the <see cref="Job.source"/> field.</param>
		/// <param name="name">Filter <see cref="Job"/>s by the <see cref="Job.name"/> field.</param>
		/// <returns>A list of <see cref="Job"/></returns>
		public async Promise<List<Job>> GetJobs(
			OptionalInt limit = null,
			OptionalString source = null,
			OptionalString name = null)
		{
			var res = await _api.GetJobs(limit, name, source);
			var jobs = res.Select(Utility.Convert).ToList();
			return jobs;
		}

		/// <inheritdoc cref="GetJob(String)"/>
		public async Promise<Job> GetJob(Job job) => await GetJob(job?.id);

		/// <summary>
		/// Get a specific scheduled <see cref="Job"/>.
		/// </summary>
		/// <param name="jobId">The <see cref="Job.id"/> of a scheduled <see cref="Job"/></param>
		/// <returns>The requested <see cref="Job"/></returns>
		public async Promise<Job> GetJob(string jobId)
		{
			var res = await _api.GetJob(jobId);
			var job = Utility.Convert(res);
			return job;
		}

		/// <summary>
		/// After a <see cref="Job"/> has been created with the
		/// <see cref="CreateJob(string,string,Beamable.Common.Scheduler.ISchedulableAction,Beamable.Common.Scheduler.ISchedulerTrigger[],Beamable.Common.Scheduler.RetryPolicy)"/>
		/// function, it can be modified using this function.
		/// This will update the <see cref="Job.lastUpdate"/> property
		/// </summary>
		/// <param name="job">The modified <see cref="Job"/>. This job must have a <see cref="Job.id"/> that
		/// already exists. Otherwise, a new job will be created and the given <see cref="Job.id"/> field will
		/// be overridden. Consider specifying the <see cref="Job.name"/> field instead.</param>
		/// <returns>The updated <see cref="Job"/></returns>
		public async Promise<Job> SaveJob(Job job)
		{
			// TODO: Is this even correct at the API level?
			var req = Utility.CreateSaveRequest(job.name, job.source, job.action, job.triggers.ToArray(), job.retryPolicy);
			req.id = new OptionalString(job.id);
			var res = await _api.PostJob(req);
			job = Utility.Convert(res);
			return job;
		}

		/// <summary>
		/// Create and schedule a <see cref="Job"/>.
		/// </summary>
		/// <param name="name">
		/// The <see cref="Job.name"/> value of the <see cref="Job"/>.
		/// The name can be used to filter using the <see cref="GetJobs"/> function.
		/// </param>
		/// <param name="source">
		/// The <see cref="Job.source"/> value of the <see cref="Job"/>.
		/// The name can be used to filter using the <see cref="GetJobs"/> function.
		/// </param>
		/// <param name="action">A <see cref="ISchedulableAction"/> that the <see cref="Job"/>
		/// will execute when any of the <see cref="triggers"/> execute.</param>
		/// <param name="triggers">A set of <see cref="ISchedulerTrigger"/>s that
		/// will cause the <see cref="action"/> to execute.</param>
		/// <param name="retryPolicy">A <see cref="RetryPolicy"/> for the action</param>
		/// <returns>The created <see cref="Job"/></returns>
		public async Promise<Job> CreateJob(
			string name,
			string source,
			ISchedulableAction action,
			ISchedulerTrigger[] triggers,
			RetryPolicy retryPolicy = null)
		{
			// TODO: Is it possible to specify the jobID ahead of time? 
			var req = Utility.CreateSaveRequest(name, source, action, triggers, retryPolicy);
			var res = await _api.PostJob(req);
			var job = Utility.Convert(res);
			return job;
		}

		/// <inheritdoc cref="CreateJob(string,string,Beamable.Common.Scheduler.ISchedulableAction,Beamable.Common.Scheduler.ISchedulerTrigger[],Beamable.Common.Scheduler.RetryPolicy)"/>
		public async Promise<Job> CreateJob(
			string name,
			string source,
			ISchedulableAction action,
			ISchedulerTrigger trigger,
			RetryPolicy retryPolicy = null)
		{
			return await CreateJob(name, source, action, new ISchedulerTrigger[] { trigger }, retryPolicy);
		}


		public static class Utility
		{
			public static JobExecutionEvent Convert(JobActivity activity)
			{
				return new JobExecutionEvent
				{
					id = activity.id.GetOrThrow(() => new Exception("Job activity needs id")),
					jobId = activity.jobId.GetOrThrow(() => new Exception("Job activity needs jobId")),
					executionId =
						activity.executionId.GetOrThrow(() => new Exception("Job activity needs executionId")),
					state = activity.state.GetOrThrow(() => new Exception("Job activity needs state")),
					message = activity.message.Value,
					timestamp = activity.timestamp.GetOrThrow(() => new Exception("JobActivity needs timestamp")).ToString("O")
				};
			}
			public static Job Convert(JobDefinition job)
			{
				var retry = job.retryPolicy.GetOrThrow(() => new Exception("Job definition has no retry policy"));
				var j = new Job()
				{
					id = job.id.GetOrThrow(() => new Exception("Job definition has no id.")),
					action = job.jobAction.Convert(),
					triggers = job.triggers.Select(t => t.Convert()).ToList(),
					source = job.source.GetOrThrow(() => new Exception("Job definition has no source")),
					name = job.name.GetOrThrow(() => new Exception("Job definition has no name")),
					owner = job.owner.GetOrThrow(() => new Exception("Job definition has no owner")),
					retryPolicy = new RetryPolicy
					{
						maxRetryCount = retry.maxRetryCount.GetOrThrow(() => new Exception("Retry policy has no maxRetryCount")),
						retryDelayMs = retry.retryDelayMs.GetOrThrow(() => new Exception("Retry policy has no retryDelayMs")),
						useExponentialBackoff = retry.useExponentialBackoff.GetOrThrow(() => new Exception("Retry policy has no useExponentialBackoff")),
					}
				};

				return j;
			}

			public static JobDefinitionSaveRequest CreateSaveRequest(
				string name,
				string source,
				ISchedulableAction action,
				ISchedulerTrigger[] triggers,
				RetryPolicy retryPolicy = null)
			{
				if (retryPolicy == null)
				{
					retryPolicy = new RetryPolicy();
				}
				return new JobDefinitionSaveRequest
				{
					name = new OptionalString(name),
					source = new OptionalString(source),
					jobAction = action.Convert(),
					triggers = triggers.Select(t => t.Convert()).ToArray(),
					retryPolicy = new OptionalJobRetryPolicy(new JobRetryPolicy
					{
						retryDelayMs = new OptionalInt(retryPolicy.retryDelayMs),
						maxRetryCount = new OptionalInt(retryPolicy.maxRetryCount),
						useExponentialBackoff = new OptionalBool(retryPolicy.useExponentialBackoff)
					})
				};
			}

			public static string GetServiceUrl(string cid, string pid, string serviceName, string path, string prefix = null)
			{
				return $"basic/{cid}.{pid}.{prefix}micro_{serviceName}/{path}";
			}
		}
	}

	/// <summary>
	/// An action that will be executed by Beamable when the <see cref="Job.triggers"/> execute.
	/// <list type="bullet">
	/// <item> <see cref="HttpAction"/> will execute a standard HTTP action </item>
	/// <item> <see cref="ServiceAction"/> will execute a C#MS method </item>
	/// </list>
	/// </summary>
	public interface ISchedulableAction
	{
		IOneOf_HttpCallOrPublishMessageOrServiceCall Convert();
	}


	/// <summary>
	/// A trigger that will cause the <see cref="Job.action"/> to execute
	/// <list type="bullet">
	/// <item> <see cref="CronEvent"/> is an NCronTab expression </item>
	/// <item> <see cref="ExactTimeEvent"/> is an exact time </item>
	/// </list>
	/// </summary>
	public interface ISchedulerTrigger
	{
		IOneOf_CronTriggerOrExactTrigger Convert();
	}

	/// <summary>
	/// A <see cref="Job"/> will eventually execute. When it does, the <see cref="JobExecution"/>
	/// is a record of the execution. The execution has several <see cref="JobExecutionEvent"/>s
	/// that describe the flow of the execution.
	/// </summary>
	[Serializable]
	public class JobExecution
	{
		/// <summary>
		/// The id of the <see cref="Job"/> that executed
		/// </summary>
		public string jobId;

		/// <summary>
		/// The id of the specific execution. A single <see cref="Job"/> may have many executions
		/// if it was scheduled with a <see cref="CronEvent"/>.
		/// </summary>
		public string executionId;

		/// <summary>
		/// A set of <see cref="JobExecutionEvent"/>s that describe the flow of the execution.
		/// </summary>
		public List<JobExecutionEvent> events;
	}


	/// <summary>
	/// A single event that happened during the execution of a <see cref="Job"/>
	/// </summary>
	[Serializable]
	public class JobExecutionEvent : ISerializationCallbackReceiver
	{
		/// <inheritdoc cref="JobExecution.executionId"/>
		public string executionId;

		/// <summary>
		/// The id of the specific event in the whole <see cref="JobExecution"/>
		/// </summary>
		public string id;

		/// <inheritdoc cref="JobExecution.jobId"/>
		public string jobId;

		/// <summary>
		/// A message describing any extra information for this state.
		/// Often, this is an empty string unless the <see cref="state"/> is <see cref="JobState.ERROR"/>
		/// </summary>
		public string message;

		/// <summary>
		/// The <see cref="JobState"/> of the step in the job execution.
		/// <list type="bullet">
		/// <item> <see cref="JobState.ENQUEUED"/> The job has been accepted into the Beamable system. </item>
		/// <item> <see cref="JobState.DISPATCHED"/> The job will attempt to execute at the next scheduled execution time. </item>
		/// <item> <see cref="JobState.RUNNING"/> The job has started running. </item>
		/// <item> <see cref="JobState.DONE"/> The job has completed without error. </item>
		/// <item> <see cref="JobState.CANCELED"/> The job has been stopped. </item>
		/// <item> <see cref="JobState.ERROR"/> The job has failed. Check <see cref="message"/> for details. </item>
		/// </list> 
		/// </summary>
		[NonSerialized]
		public JobState state;

		/// <summary>
		/// A datetime string in ISO 8601
		/// </summary>
		public string timestamp;

		[SerializeField]
		private string jobState;

		public void OnBeforeSerialize()
		{
			jobState = JobStateExtensions.ToEnumString(state);
		}

		public void OnAfterDeserialize()
		{
			state = JobStateExtensions.FromEnumString(jobState);
		}
	}

	/// <summary>
	/// A <see cref="Job"/> will execute at some time in the future.
	/// </summary>
	[Serializable]
	public class UpcomingExecution
	{
		/// <summary>
		/// The time when the <see cref="Job"/> is expected to execute.
		/// </summary>
		public DateTime executeAt;
	}

	/// <summary>
	/// A <see cref="Job"/> is an action that can be scheduled to run later based on various trigger events.
	/// </summary>
	[Serializable]
	public class Job : ISerializationCallbackReceiver
	{
		/// <summary>
		/// The unique id of the Job
		/// </summary>
		public string id;

		/// <summary>
		/// An optional ISO-8601 date string representing the last time the <see cref="Job"/> was modified.
		/// </summary>
		public OptionalString lastUpdate;

		/// <summary>
		/// The name of the <see cref="Job"/>.
		/// This is not a unique value.
		/// </summary>
		public string name;

		/// <summary>
		/// The source of the <see cref="Job"/>.
		/// Jobs can be filtered by this field.
		/// </summary>
		public string source;

		/// <summary>
		/// The owner of the <see cref="Job"/> is the realm that maintains the job. 
		/// </summary>
		public string owner;

		/// <summary>
		/// The <see cref="RetryPolicy"/> contains information about how a <see cref="Job"/>
		/// should be rescheduled in the event of a failure.
		/// </summary>
		public RetryPolicy retryPolicy;

		/// <summary>
		/// The <see cref="ISchedulableAction"/> is the thing that will execute when the <see cref="triggers"/>
		/// dictate.
		/// <list type="bullet">
		/// <item> <see cref="HttpAction"/> will make a standard HTTP request </item>
		/// <item> <see cref="ServiceAction"/> will make a call to a C#MS </item>
		/// </list>
		/// </summary>
		[NonSerialized]
		public ISchedulableAction action;

		/// <summary>
		/// A set of <see cref="ISchedulerTrigger"/>s that dictate when the <see cref="action"/>
		/// will execute.
		/// <list type="bullet">
		/// <item> <see cref="CronEvent"/> will run the job at a given NCronTab expression </item>
		/// <item> <see cref="ExactTimeEvent"/> will run the job a given time in UTC </item>
		/// </list>
		/// </summary>
		[NonSerialized]
		public List<ISchedulerTrigger> triggers = new List<ISchedulerTrigger>();

		[SerializeField]
		private HttpAction _httpAction;

		[SerializeField]
		private ServiceAction _serviceAction;

		[SerializeField]
		private PublishAction _publishAction;

		[SerializeField]
		private List<CronEvent> _cronTriggers;

		[SerializeField]
		private List<ExactTimeEvent> _exactTimeTriggers;


		public void OnBeforeSerialize()
		{
			_httpAction = null;
			_serviceAction = null;
			_publishAction = null;
			_exactTimeTriggers = new List<ExactTimeEvent>();
			_cronTriggers = new List<CronEvent>();
			switch (action)
			{
				case HttpAction http:
					_httpAction = http;
					break;
				case ServiceAction service:
					_serviceAction = service;
					break;
				case PublishAction publish:
					_publishAction = publish;
					break;
			}

			if (triggers == null) return;
			foreach (var trigger in triggers)
			{
				switch (trigger)
				{
					case CronEvent cron:
						_cronTriggers.Add(cron);
						break;
					case ExactTimeEvent exact:
						_exactTimeTriggers.Add(exact);
						break;
				}
			}
		}

		public void OnAfterDeserialize()
		{
			if (_httpAction != null)
			{
				action = _httpAction;
			}
			if (_serviceAction != null)
			{
				action = _serviceAction;
			}
			if (_publishAction != null)
			{
				action = _publishAction;
			}

			triggers = new List<ISchedulerTrigger>();
			if (_cronTriggers != null)
			{
				triggers.AddRange(_cronTriggers);
			}

			if (_exactTimeTriggers != null)
			{
				triggers.AddRange(_exactTimeTriggers);
			}
		}
	}

	/// <summary>
	/// The retry policy is used when a <see cref="Job"/> fails and needs to be rescheduled.
	/// </summary>
	[Serializable]
	public class RetryPolicy
	{
		/// <summary>
		/// The maximum number of times a <see cref="Job"/> may be retried.
		/// The retry limit is per execution, not per job.
		///
		/// For example, if there is a job that runs on a CRON schedule, then
		/// each time the CRON schedule dictates an invocation, the max retry count is in effect.
		/// </summary>
		public int maxRetryCount = 1;

		/// <summary>
		/// How long should Beamable wait before attempting to retry the failed job.
		/// </summary>
		public int retryDelayMs = 10 * 1000;
		public bool useExponentialBackoff = true;
	}

	/// <summary>
	/// An <see cref="ExactTimeEvent"/> will run a <see cref="Job"/> at the given <see cref="executeAt"/> time.
	/// </summary>
	[Serializable]
	public class ExactTimeEvent : ISchedulerTrigger
	{
		/// <summary>
		/// The time at which a <see cref="Job"/> will run. This should be in UTC.
		/// </summary>
		public DateTime executeAt = DateTime.UtcNow;

		public ExactTimeEvent() { }

		public ExactTimeEvent(DateTime executeAt)
		{
			this.executeAt = executeAt;
		}

		IOneOf_CronTriggerOrExactTrigger ISchedulerTrigger.Convert()
		{
			return new ExactTrigger
			{
				type = new OptionalString(nameof(ExactTrigger)),
				executeAt = new OptionalDateTime(executeAt)
			};
		}
	}

	/// <summary>
	/// A <see cref="CronEvent"/> will run a <see cref="Job"/> at the given <see cref="cronExpression"/> is true.
	/// </summary>
	[Serializable]
	public class CronEvent : ISchedulerTrigger
	{
		/// <summary>
		/// an NCronTab expression. This must be a 6 component cron string.
		/// <list type="bullet">
		/// <item> SECONDS - [0-59] </item>
		/// <item> MINUTES - [0-59] </item>
		/// <item> HOURS - [0-59] </item>
		/// <item> DAYS_OF_MONTH - [1-31] </item>
		/// <item> MONTHS - [1-12] </item>
		/// <item> DAYS_OF_WEEK - [0-6] where 0 is Sunday </item>
		/// </list>
		///
		/// Consider using the <see cref="ICronInitial"/> interface from <see cref="CronBuilder"/>.
		/// </summary>
		public string cronExpression = "* * * * * *";

		public CronEvent() { }

		public CronEvent(string cronExpression)
		{
			this.cronExpression = cronExpression;
		}

		IOneOf_CronTriggerOrExactTrigger ISchedulerTrigger.Convert()
		{
			return new Beamable.Api.Autogenerated.Models.CronTrigger
			{
				type = new OptionalString(nameof(Beamable.Api.Autogenerated.Models.CronTrigger)),
				expression = new OptionalString(cronExpression)
			};
		}
	}

	/// <summary>
	/// The <see cref="HttpAction"/> will trigger an HTTP request when the <see cref="Job"/> executes.
	/// </summary>
	[Serializable]
	public class HttpAction : ISchedulableAction
	{
		/// <summary>
		/// The HTTP method to use.
		/// </summary>
		public Method method = Method.GET;

		/// <summary>
		/// The fully qualified uri of the request. Must include the "https://" segment.
		/// </summary>
		public string uri;

		/// <summary>
		/// The content type of the request.
		/// </summary>
		public string contentType = "application/json";

		/// <summary>
		/// The body of the request.
		/// </summary>
		public string body;

		/// <summary>
		/// A set of <see cref="HttpCallHeader"/>s for the request.
		/// </summary>
		public List<HttpCallHeader> headers = new List<HttpCallHeader>();

		IOneOf_HttpCallOrPublishMessageOrServiceCall ISchedulableAction.Convert()
		{
			return new HttpCall
			{
				uri = new OptionalString(uri),
				method = new OptionalString(method.ToReadableString()),
				body = new OptionalString(body),
				type = new OptionalString(nameof(HttpCall)),
				contentType = new OptionalString(contentType),
				headers = new OptionalArrayOfStringStringKeyValuePair(headers.Select(h => new StringStringKeyValuePair
				{
					key = new OptionalString(h.key),
					value = new OptionalString(h.value)
				}).ToArray())
			};
		}
	}

	/// <summary>
	/// The <see cref="ServiceAction"/> will trigger a C#MS ServerCallable when the <see cref="Job"/> executes.
	/// </summary>
	[Serializable]
	public class ServiceAction : ISchedulableAction
	{
		/// <summary>
		/// The JSON body of the request.
		/// </summary>
		public string body;

		/// <summary>
		/// The method of the request.
		/// </summary>
		public Method method = Method.POST;

		/// <summary>
		/// The fully qualified Uri of the request. This must in the format of
		/// <code>
		/// basic/{CID}.{PID}.{ROUTING_PREFIX}micro_{SERVICE_NAME}/{METHOD_NAME}
		/// </code>
		/// </summary>
		public string uri;

		public ServiceAction()
		{
			// empty cons
		}

		IOneOf_HttpCallOrPublishMessageOrServiceCall ISchedulableAction.Convert()
		{
			return new ServiceCall
			{
				body = new OptionalString(body),
				method = new OptionalString(method.ToReadableString()),
				uri = new OptionalString(uri),
				type = new OptionalString(nameof(ServiceCall))
			};
		}
	}

	[Serializable]
	public class PublishAction : ISchedulableAction
	{
		public string topic;
		public string message;
		public OptionalMapOfString headers;
		public bool persist;


		IOneOf_HttpCallOrPublishMessageOrServiceCall ISchedulableAction.Convert()
		{
			return new PublishMessage
			{
				topic = new OptionalString(topic),
				message = new OptionalString(message),
				headers = headers,
				persist = new OptionalBool(persist),
				type = new OptionalString(nameof(PublishMessage))
			};
		}
	}

	[Serializable]
	public class HttpCallHeader
	{
		public string key;
		public string value;
	}

}


namespace Beamable.Api.Autogenerated.Models
{
	public partial interface IOneOf_CronTriggerOrExactTrigger
	{
		ISchedulerTrigger Convert();
	}

	public partial class CronTrigger
	{
		public ISchedulerTrigger Convert()
		{
			return new CronEvent
			{
				cronExpression = expression.GetOrThrow(() => new Exception("CronEvent needs cron expression"))
			};
		}
	}

	public partial class ExactTrigger
	{
		public ISchedulerTrigger Convert()
		{

			return new ExactTimeEvent { executeAt = executeAt.GetOrThrow(() => new Exception("ExactTime needs exactAt")) };
		}
	}

	public partial interface IOneOf_HttpCallOrPublishMessageOrServiceCall
	{
		ISchedulableAction Convert();
	}

	public partial class HttpCall
	{
		public ISchedulableAction Convert()
		{
			if (!MethodUtil.TryParseMethod(method.GetOrThrow(() => new Exception("HttpAction must have a method")),
					out var parsedMethod))
			{
				throw new Exception("HttpAction method was not able to parse");
			}

			return new HttpAction
			{
				body = body.Value,
				uri = uri.GetOrThrow(() => new Exception("HttpAction must have uri")),
				contentType = contentType.GetOrThrow(() => new Exception("HttpAction must have contentType")),
				method = parsedMethod,
				headers = headers.GetOrElse(Array.Empty<StringStringKeyValuePair>()).Select(v => new HttpCallHeader
				{
					key = v.key.GetOrThrow(() => new Exception("Header must have a key")),
					value = v.value.GetOrThrow(() => new Exception("Header must have a value")),
				}).ToList()
			};
		}
	}
	public partial class PublishMessage
	{
		public ISchedulableAction Convert()
		{
			return new PublishAction
			{
				headers = headers,
				message = message.GetOrThrow(() => new Exception("PublishMessage must have message")),
				topic = topic.GetOrThrow(() => new Exception("PublishMessage must have topic")),
				persist = persist.GetOrThrow(() => new Exception("PublishMessage must have persist")),
			};
		}
	}
	public partial class ServiceCall
	{
		public ISchedulableAction Convert()
		{
			if (!MethodUtil.TryParseMethod(method.GetOrThrow(() => new Exception("ServiceCall must have a method")),
					out var parsedMethod))
			{
				throw new Exception("ServiceCall method was not able to parse");
			}
			return new ServiceAction
			{
				body = body.GetOrThrow(() => new Exception("ServiceCall must have a body")),
				method = parsedMethod,
				uri = uri.GetOrThrow(() => new Exception("ServiceCall must have a uri")),
			};
		}
	}
}
