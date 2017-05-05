namespace PowerBI.Json
{
    using Acom.IO.FileReaders;
    using Acom.IO.Json;
    using PowerBI.Entities;

    public class TicketingDisabledDatesJsonLoader : JsonLoader<TicketingDisabledDate>
    {
        public TicketingDisabledDatesJsonLoader(IFileReader reader)
            : base(reader.ReadAllText("ticketingDisabledDates.json"))
        {

        }
    }
}
