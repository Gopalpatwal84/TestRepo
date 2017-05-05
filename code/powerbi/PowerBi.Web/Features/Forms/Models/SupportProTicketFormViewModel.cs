using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Acom.Forms;
using Acom.Forms.Models;
using Acom.IO;
using Acom.IO.Entities;
using Acom.IO.Json;
using ChameleonForms.Attributes;
using PowerBI.Entities;
using Countries = PowerBI.Resources.Shared.Countries;
using SupportProTicket = PowerBI.Resources.Shared.Forms.SupportProTicket;
using TimeZones = PowerBI.Resources.Shared.TimeZones;

namespace PowerBI.Web.Features.Forms.Models
{
    public class SupportProTicketFormViewModel : IHaveFormOptions
    {
        public FormOptions FormOptions { get; set; }

        public string TicketId { get; set; }

        [Required(ErrorMessageResourceType = typeof(PowerBI.Resources.Shared.Forms.Form), ErrorMessageResourceName = "Email_RequiredMessage")]
        [EmailAddress(ErrorMessageResourceType = typeof(PowerBI.Resources.Shared.Forms.Form), ErrorMessageResourceName = "Email_ErrorMessage")]
        public string Email { get; set; }

        [Required(ErrorMessageResourceType = typeof(SupportProTicket), ErrorMessageResourceName = "FirstName_ErrorMessage")]
        public string FirstName { get; set; }

        [Required(ErrorMessageResourceType = typeof(SupportProTicket), ErrorMessageResourceName = "LastName_ErrorMessage")]
        public string LastName { get; set; }

        [Required(ErrorMessageResourceType = typeof(SupportProTicket), ErrorMessageResourceName = "Phone_ErrorMessage")]
        public string Phone { get; set; }

        [Required(ErrorMessageResourceType = typeof(SupportProTicket), ErrorMessageResourceName = "TimeZone_ErrorMessage")]
        [ExistsIn(nameof(TimeZoneList), "Key", "Value")]
        public string TimeZone { get; set; }

        [Required(ErrorMessageResourceType = typeof(SupportProTicket), ErrorMessageResourceName = "Country_ErrorMessage")]
        [ExistsIn(nameof(CountriesList), "Key", "Value")]
        public string Country { get; set; }

        [Required(ErrorMessageResourceType = typeof(SupportProTicket), ErrorMessageResourceName = "ProblemType_ErrorMessage")]
        [ExistsIn(nameof(ProblemTypeList), "Key", "Value")]
        public string ProblemType { get; set; }

        [Required(ErrorMessageResourceType = typeof(SupportProTicket), ErrorMessageResourceName = "Category_ErrorMessage")]
        [ExistsIn(nameof(CategoryList), "Key", "Value")]
        public string Category { get; set; }

        [Required(ErrorMessageResourceType = typeof(SupportProTicket), ErrorMessageResourceName = "Subject_ErrorMessage")]
        public string Subject { get; set; }

        [BooleanMustBeTrue(ErrorMessageResourceType = typeof(SupportProTicket), ErrorMessageResourceName = "Disclaimer_ErrorMessage")]
        public bool Consent { get; set; }

        public Dictionary<string, string[]> Questions { get; set; }

        public string[] ParsedQuestions { get; set; }

        public List<KeyValuePair<string, string>> CountriesList
        {
            get
            {
                return DependencyResolver.Current.GetService<JsonLoader<Country>>()
                    .Data.Value
                    .Select(x => new KeyValuePair<string, string>(
                        x.CodeAlpha3,
                        Countries.ResourceManager.GetString(x.LocKey)))
                    .OrderBy(x => x.Value)
                    .ToList();
            }
        }

        public List<KeyValuePair<string, string>> ProblemTypeList
        {
            get
            {
                return DependencyResolver.Current.GetService<IRepository<ProblemType>>().Get()
                    .Select(x => new KeyValuePair<string, string>(
                        x.Id,
                        x.AlternateCultures.ContainsKey(KnownCultures.Current) ? x.AlternateCultures[KnownCultures.Current] : x.Title))
                    .OrderBy(x => x.Value)
                    .ToList();
            }
        }

        public List<KeyValuePair<string, string>> CategoryList
        {
            get
            {
                return DependencyResolver.Current.GetService<IRepository<ProblemType>>().Get()
                    .SelectMany(problem => problem.Categories.Select(category => new KeyValuePair<string, string>(
                        string.Format("{0}.{1}", problem.Id, category.Id),
                        category.AlternateCultures.ContainsKey(KnownCultures.Current) ? category.AlternateCultures[KnownCultures.Current] : category.Title)))
                    .OrderBy(x => x.Value)
                    .ToList();
            }
        }

        public List<KeyValuePair<string, string>> TimeZoneList
        {
            get
            {
                return DependencyResolver.Current.GetService<IRepository<Curation<TimeZoneInfo>>>().Get("metis-timezones")
                    .Items
                    .Select(x => new KeyValuePair<string, string>(
                        x.Id,
                        GetTimeZoneDisplayName(x)
                    )).ToList();
            }
        }

        private string GetTimeZoneDisplayName(TimeZoneInfo timeZone)
        {
            var resourceKey = Regex.Replace(timeZone.Id, "[^0-9a-zA-Z]+", "");
            return TimeZones.ResourceManager.GetString(resourceKey) ?? timeZone.DisplayName;
        }
    }
}