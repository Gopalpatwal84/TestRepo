using System;

namespace PowerBI.Web.Models
{
    public class Pagination
    {
        public Pagination(int pageNumber, int pageSize, int totalCount)
        {
            this.PageNumber = pageNumber;
            this.PageSize = pageSize;
            this.TotalCount = totalCount;
            if (Math.Ceiling(totalCount / (double)pageSize) > pageNumber)
            {
                this.NextPage = pageNumber + 1;
            }

            this.PrevPage = pageNumber - 1;
        }

        public int PageNumber { get; set; }

        public int PageSize { get; set; }

        public int TotalCount { get; set; }

        public int PrevPage { get; set; }

        public int NextPage { get; set; }
    }
}