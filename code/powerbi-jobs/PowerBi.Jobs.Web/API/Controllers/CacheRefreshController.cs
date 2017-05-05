namespace PowerBI.Jobs.Web.API.Controllers
{
    using System;
    using System.Linq;
    using System.Threading.Tasks;
    using System.Web.Http;
    using Mediation.MessageContracts;
    using PowerBI.Jobs.Web.API.Models;
    using PowerBI.Jobs.Web.Services;
    using PowerBI.Messages.Refresh;
    using Serilog;

    public class CacheRefreshController : ApiController
    {
        private readonly MessageBroadcastService messageBroadcastService;

        public CacheRefreshController(MessageBroadcastService messageBroadcastService)
        {
            this.messageBroadcastService = messageBroadcastService;
        }

        [Authorize]
        [HttpPost]
        [Route("api/cacherefresh/blog")]
        public async Task<IHttpActionResult> Blog([FromBody] BlogRefreshOptions refreshOptions)
        {
            return await BroadcastRefreshMessage(() => new RefreshFullBlogCommand());
        }

        private async Task<IHttpActionResult> BroadcastRefreshMessage<T>(Func<T> createMessageFunc, bool allRegions = true) where T : IMediatorCommand
        {
            try
            {
                Log.Warning("RefreshAPI called for {CommandType}", typeof(T).Name);

                var queueMessage = createMessageFunc();

                var errors = await messageBroadcastService.Broadcast(queueMessage, x => allRegions || x.Default);

                if (errors.Any())
                {
                    throw new Exception(string.Join(" - ", errors));
                }

                return Ok();
            }
            catch (Exception e)
            {
                return InternalServerError(e);
            }
        }
    }
}