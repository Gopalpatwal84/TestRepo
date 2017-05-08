namespace BusinessPlatform.Messages.Commands
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Mediation.Queues;

    [DestinationQueueName("signup-request")]
    public class SignUpRequestFormMessage : DiagnosticsCommand
    {
        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public DateTime Date { get; set; }

        public string FormSlug { get; set; }
    }
}
