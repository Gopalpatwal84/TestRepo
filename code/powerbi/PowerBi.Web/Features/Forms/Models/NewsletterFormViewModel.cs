using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;
using Acom.Forms.Models;
using Acom.IO.Json;
using ChameleonForms.Attributes;
using PowerBI.Entities;

namespace PowerBI.Web.Features.Forms.Models
{
    public class NewsletterFormViewModel : IHaveFormOptions
    {
        public FormOptions FormOptions { get; set; }

        [Required(ErrorMessageResourceType = typeof(PowerBI.Resources.Shared.Forms.Form), ErrorMessageResourceName = "Email_RequiredMessage")]
        [EmailAddress(ErrorMessageResourceType = typeof(PowerBI.Resources.Shared.Forms.Form), ErrorMessageResourceName = "Email_ErrorMessage")]
        public string Email { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        [ExistsIn(nameof(JobRolesList), "Key", "Value")]
        public string JobRole { get; set; }

        [Required(ErrorMessageResourceType = typeof(PowerBI.Resources.Shared.Forms.Form), ErrorMessageResourceName = "Country_RequiredMessage")]
        [ExistsIn(nameof(CountriesList), "Key", "Value")]
        public string Country { get; set; }

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