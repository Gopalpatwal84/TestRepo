using System;
using System.ComponentModel.DataAnnotations;
using Mediation.Queues;
using PowerBI.Messages.Models;

namespace PowerBI.Messages.Commands
{
    [DestinationQueueName("registration-request")]
    public class RegistrationRequestFormMessage : DiagnosticsCommand, ITrackingCookie
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string JobTitle { get; set; }
        public string FormSlug { get; set; }
        public string FormType { get; set; }
        public DateTime SubmittedOn { get; set; }
        public string MarketoLeadId { get; set; }
        public string MarketoResourceId { get; set; }
        public string Country { get; set; }
        public string CompanyName { get; set; }
        public bool Consent { get; set; }
        public string TrackingCookie { get; set; }
    }
}
