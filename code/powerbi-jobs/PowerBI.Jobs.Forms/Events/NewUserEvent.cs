using Mediation.MessageContracts;
using PowerBI.Jobs.Forms.Data;

namespace PowerBI.Jobs.Forms.Events
{
    public class NewUserEvent : IMediatorEvent
    {
        public NewUserEvent(User user, string formSlug)
        {
            this.User = user;
            this.FormSlug = formSlug;
        }

        public string FormSlug { get; set; }
        public User User { get; set; }
    }
}
