namespace BusinessPlatform.Web.Features.Forms.Models
{
    using System.ComponentModel.DataAnnotations;
    using Acom.Forms;
    using Acom.Forms.Models;

    public class SignupFormViewModel : IHaveFormOptions
    {
        public FormOptions FormOptions { get; set; }

        [Required(ErrorMessageResourceType = typeof(BusinessPlatform.Resources.Shared.Forms.Form), ErrorMessageResourceName = "Email_ErrorMessage")]
        [EmailAddress(ErrorMessageResourceType = typeof(BusinessPlatform.Resources.Shared.Forms.Form), ErrorMessageResourceName = "Email_ErrorMessage")]
        [NoConsumerEmail(ErrorMessageResourceType = typeof(BusinessPlatform.Resources.Shared.Forms.Form), ErrorMessageResourceName = "Email_ConsumerMessage")]
        public string Email { get; set; }
    }
}