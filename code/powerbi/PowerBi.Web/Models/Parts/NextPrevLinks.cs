namespace PowerBI.Web.Models.Parts
{
    using System;

    public class NextPrevLinks
    {
        public NextPrevLinks(int pageNumber, int pageSize, int totalCount)
        {
            if (Math.Ceiling(totalCount / (double)pageSize) > pageNumber) // check for final page
            {
                this.NextPage = pageNumber + 1;
            }

            this.PrevPage = pageNumber - 1;
        }
        public int PrevPage { get; set; }

        public int NextPage { get; set; }
    }
}