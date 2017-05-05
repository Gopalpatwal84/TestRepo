using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Acom.Forms;
using Acom.Forms.Models;
using Acom.IO.Json;
using ChameleonForms.Attributes;
using PowerBI.Entities;

namespace PowerBI.Web.Features.Forms.Models
{
    public class PartnersInvitationRequestViewModel : IHaveFormOptions
    {

        [Required(ErrorMessageResourceType = typeof(PowerBI.Resources.Shared.Forms.PartnersInvitationRequest), ErrorMessageResourceName = "Company_ErrorMessage")]
        public string Company { get; set; }

        [BooleanMustBeTrue(ErrorMessageResourceType = typeof(PowerBI.Resources.Shared.Forms.PartnersInvitationRequest), ErrorMessageResourceName = "Disclaimer_ErrorMessage")]
        public bool Consent { get; set; }

        [Required(ErrorMessageResourceType = typeof(PowerBI.Resources.Shared.Forms.PartnersInvitationRequest), ErrorMessageResourceName = "Country_ErrorMessage")]
        [ExistsIn(nameof(CountriesList), "Key", "Value")]
        public string Country { get; set; }

        [Required(ErrorMessageResourceType = typeof(PowerBI.Resources.Shared.Forms.Form), ErrorMessageResourceName = "Email_RequiredMessage")]
        [EmailAddress(ErrorMessageResourceType = typeof(PowerBI.Resources.Shared.Forms.Form), ErrorMessageResourceName = "Email_ErrorMessage")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(PowerBI.Resources.Shared.Forms.PartnersInvitationRequest), ErrorMessageResourceName = "FirstName_ErrorMessage")]
        public string FirstName { get; set; }

        public FormOptions FormOptions { get; set; }

        [Required(ErrorMessageResourceType = typeof(PowerBI.Resources.Shared.Forms.PartnersInvitationRequest), ErrorMessageResourceName = "Title_ErrorMessage")]
        [ExistsIn(nameof(JobRolesList), "Key", "Value")]
        public string JobRole { get; set; }

        [Required(ErrorMessageResourceType = typeof(PowerBI.Resources.Shared.Forms.PartnersInvitationRequest), ErrorMessageResourceName = "LastName_ErrorMessage")]
        public string LastName { get; set; }

        [Required(ErrorMessageResourceType = typeof(PowerBI.Resources.Shared.Forms.PartnersInvitationRequest), ErrorMessageResourceName = "MpnId_Error")]
        [StringLength(8, MinimumLength = 5, ErrorMessageResourceType = typeof(PowerBI.Resources.Shared.Forms.PartnersInvitationRequest), ErrorMessageResourceName = "MpnId_Length_Error")]
        [RegularExpression("^[0-9]*$", ErrorMessageResourceType = typeof(PowerBI.Resources.Shared.Forms.PartnersInvitationRequest), ErrorMessageResourceName = "MpnId_Type_Error")]
        public string MpnId { get; set; }

        public List<KeyValuePair<string, string>> CountriesList
        {
            get
            {
                return DependencyResolver.Current.GetService<JsonLoader<Country>>()
                    .Data.Value
                    .Select(x => new KeyValuePair<string, string>(
                        x.Slug,
                        Resources.Shared.Countries.ResourceManager.GetString(x.LocKey)))
                    .ToList();
            }
        }

        public List<Country> CountriesRequiringContactConsent
        {
            get
            {
                return DependencyResolver.Current.GetService<JsonLoader<Country>>()
                    .Data.Value
                    .Where(c => c.ContactInfo.RequiresContactConsent)
                    .ToList();
            }
        }

        public List<KeyValuePair<string, string>> JobRolesList
        {
            get
            {
                return DependencyResolver.Current.GetService<JsonLoader<Jobrole>>()
                    .Data.Value
                    .Select(x => new KeyValuePair<string, string>(
                        x.Slug,
                        Resources.Shared.Jobroles.ResourceManager.GetString(x.LocKey)))
                    .ToList();
            }
        }
    }
}