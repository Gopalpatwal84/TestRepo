using System.Threading.Tasks;
using Acom.DocumentDb;
using Mediation.Handlers;
using PowerBI.Jobs.Forms.Data;
using PowerBI.Jobs.Forms.Events;

namespace PowerBI.Jobs.Forms.Handlers
{
    public class NewUserHandler : IHandleCompetingEvent<NewUserEvent>
    {
        private readonly IDocumentDbClient documentDbClient;

        public NewUserHandler(IDocumentDbClient documentDbClient)
        {
            this.documentDbClient = documentDbClient;
        }

        public async Task Handle(NewUserEvent mediatorEvent)
        {
            var slug = mediatorEvent.User.Email.ToLowerInvariant();
            var user = (await this.documentDbClient.GetAsync<User>(slug)) ?? new User();

            user.Slug = slug;
            user.Email = mediatorEvent.User.Email ?? user.Email;
            user.FirstName = mediatorEvent.User.FirstName ?? user.FirstName;
            user.LastName = mediatorEvent.User.LastName ?? user.LastName;
            user.Country = mediatorEvent.User.Country ?? user.Country;
            user.JobTitle = mediatorEvent.User.JobTitle ?? user.JobTitle;
            user.IpAddress = mediatorEvent.User.IpAddress ?? user.IpAddress;
            user.Phone = mediatorEvent.User.Phone ?? user.Phone;

            if (!string.IsNullOrEmpty(mediatorEvent.FormSlug) && !user.Forms.Contains(mediatorEvent.FormSlug))
            {
                user.Forms.Add(mediatorEvent.FormSlug);
            }

            await this.documentDbClient.SetAsync(user, fullReplace: true); // full replace until we get better patching support in documentDbClient
        }
    }
}
