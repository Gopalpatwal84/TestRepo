using Marketo.API.Leads;
using Mediation.MessageContracts;
using System;

namespace PowerBI.Jobs.Forms.Events
{
    public class NewMarketoLeadEvent : IMediatorEvent
    {
        public NewMarketoLeadEvent(Lead lead, bool contactConsent)
        {
            if (lead.MarketoTimeStamp == default(DateTime)) lead.MarketoTimeStamp =  DateTime.UtcNow;
            Lead = lead;
            Lead.ExtensionProperties["ce_powerbiemailpreferencepremiumcontent"] = contactConsent;
        }

        public Lead Lead { get; set; }
    }
}
