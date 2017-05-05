namespace PowerBI.Entities
{
    using System;
    using Acom.IO.Entities;

    [Serializable]
    public class TicketingDisabledDate : IHaveSlug
    {
        public string Slug { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public string Message { get; set; }
    }
}
