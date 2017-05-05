using System.ComponentModel.DataAnnotations;
using Acom.Forms;
using Acom.Forms.Models;

namespace PowerBI.Web.Features.Forms.Models
{
    public class SignupFormViewModel : IHaveFormOptions
    {
        public FormOptions FormOptions { get; set; }

        [Required(ErrorMessageResourceType = typeof(PowerBI.Resources.Shared.Forms.Form), ErrorMessageResourceName = "Email_RequiredMessage")]
        [EmailAddress(ErrorMessageResourceType = typeof(PowerBI.Resources.Shared.Forms.Form), ErrorMessageResourceName = "Email_ErrorMessage")]
        [NoConsumerEmail(ErrorMessageResourceType = typeof(PowerBI.Resources.Shared.Forms.Form), ErrorMessageResourceName = "Email_ConsumerMessage")]
        [DisallowedTldEmail(".gov", ".mil", ErrorMessageResourceType = typeof(PowerBI.Resources.Shared.Forms.Form), ErrorMessageResourceName = "Email_DisallowedTldMessage")]
        public string Email { get; set; }
    }
}