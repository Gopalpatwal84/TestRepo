namespace PowerBI.Web.Features.Forms.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Acom.Forms.Models;

    public class FeedbackFormViewModel : IHaveFormOptions
    {
        public FormOptions FormOptions { get; set; }

        public string Action { get; set; }

        [StringLength(160, ErrorMessageResourceType = typeof(PowerBI.Resources.Shared.Forms.Feedback), ErrorMessageResourceName = "CommentsLength_ErrorMessage")]
        public string Comments { get; set; }

        public DateTime ArticleModified { get; set; }
    }
}