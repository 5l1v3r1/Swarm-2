using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Quartz;
using Swarm.Core.Common;

namespace Swarm.Core.Controllers
{
    [Route("swarm/v1.0/trigger")]
    public class TriggerController : Controller
    {
        private readonly IScheduler _scheduler;
        private readonly SwarmOptions _options;
        private readonly ILogger _logger;
        private readonly ISwarmStore _store;

        public TriggerController(IScheduler scheduler, ILoggerFactory loggerFactory, ISwarmStore store,
            IOptions<SwarmOptions> options)
        {
            _options = options.Value;
            _scheduler = scheduler;
            _logger = loggerFactory.CreateLogger<JobController>();
            _store = store;
        }

        [HttpPost("{jobId}")]
        public async Task<IActionResult> Trigger(string jobId)
        {
            if (string.IsNullOrWhiteSpace(jobId))
            {
                return new JsonResult(new ApiResult {Code = ApiResult.Error, Msg = "Id is empty/null."});
            }

            if (!await _store.CheckJobExists(jobId))
            {
                return new JsonResult(new ApiResult {Code = ApiResult.Error, Msg = $"Job {jobId} not exists."});
            }

            await _scheduler.TriggerJob(new JobKey(jobId));
            _logger.LogInformation($"Trigger job {jobId} success");
            return new JsonResult(new ApiResult {Code = ApiResult.SuccessCode, Msg = "success"});
        }
    }
}