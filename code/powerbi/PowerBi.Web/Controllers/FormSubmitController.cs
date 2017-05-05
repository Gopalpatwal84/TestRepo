using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;
using Acom.Forms;
using Acom.IO;
using Acom.IO.Entities;
using Acom.Mvc.Filters;
using Mediation;
using PowerBI.Entities;
using PowerBI.Messages.Commands;
using PowerBI.Support.Models;
using PowerBI.Support.Services;
using PowerBI.Web.Authorization;
using PowerBI.Web.Configuration;
using PowerBI.Web.Features.Forms.Models;
using PowerBI.Web.Features.UserInfoCookie;
using PowerBI.Web.Infrastructure.Helpers;
using Serilog;
using Questions = PowerBI.Resources.Shared.Forms.SupportQuestions;

namespace PowerBI.Web.Controllers
{
    public class FormSubmitController : BaseController
    {
        private readonly IMediator mediator;
        private readonly PowerBIConfiguration powerbiConfiguration;
        private readonly IRepository<SupportRegionSku> supportRegionSkuRepository;
        private readonly IRepository<SupportProblemQuestions> questionsRepository;
        private readonly MetisService metisService;

        public FormSubmitController(
            IMediator mediator, 
            PowerBIConfiguration powerbiConfiguration, 
            IRepository<SupportRegionSku> supportRegionSkuRepository,
            IRepository<SupportProblemQuestions> questionsRepository,
            MetisService metisService)
        {
            this.mediator = mediator;
            this.powerbiConfiguration = powerbiConfiguration;
            this.supportRegionSkuRepository = supportRegionSkuRepository;
            this.questionsRepository = questionsRepository;
            this.metisService = metisService;
        }

        [HttpPost]
        [Route("{culture=en-us}/forms/submit-signup/")]
        [ValidateModelState("form/_signup")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitSignupForm(SignupFormViewModel model)
        {
            var command = new SignUpRequestFormMessage
            {
                Date = DateTime.UtcNow,
                Email = model.Email,
                FormSlug = model.FormOptions.Slug,
            };

            await this.mediator.Send(command);
            return Json(new { Redirect = string.Format("{0}{1}pbi_source=web&email={2}", 
                model.FormOptions.SubmitRedirectUrl,
                model.FormOptions.SubmitRedirectUrl.Contains("?") ? "&":"?",
                model.Email) });
        }

        [HttpPost]
        [Route("{culture=en-us}/forms/submit-mobile/")]
        [ValidateModelState("form/_mobile")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> MobileRegistrationForm(MobileFormViewModel model)
        {
            var command = new RegistrationRequestFormMessage
            {
                MarketoResourceId = "PowerBIMobile",
                Email = model.Email,
                FormSlug = model.FormOptions.Slug,
            };

            await this.mediator.Send(command);
            return PartialView(model.FormOptions.SubmitView ?? "form/submitviews/_mobilethanks");

        }

        [HttpPost]
        [Route("{culture=en-us}/forms/submit-newsletter/")]
        [ValidateModelState("form/_newsletter")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitNewsletterForm(NewsletterFormViewModel model)
        {
            var command = new RegistrationRequestFormMessage
            {
                MarketoResourceId = "PowerBINewsletter",
                Email = model.Email,
                FormSlug = model.FormOptions.Slug,
            };

            await this.mediator.Send(command);
            return PartialView(model.FormOptions.SubmitView ?? "form/submitviews/_newsletterthanks");
        }

        [HttpPost]
        [ModelHydrator]
        [Route("{culture=en-us}/forms/submit-newsletter-full/")]
        [ValidateModelState("form/_newsletter-full")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitFullNewsletterForm(NewsletterFormViewModel model)
        {
            var command = new RegistrationRequestFormMessage
            {
                MarketoResourceId = "PowerBINewsletter",
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                JobTitle = model.JobRole,
                Country = model.Country,
                FormSlug = model.FormOptions.Slug,
            };

            await this.mediator.Send(command);
            return PartialView(model.FormOptions.SubmitView ?? "form/submitviews/_newsletterfullthanks");
        }

        [HttpPost]
        [Route("{culture=en-us}/forms/submit-track-progress/")]
        [ValidateModelState("form/_track-progress")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitTrackProgressForm(SimpleRegistrationFormViewModel model)
        {
            var command = new RegistrationRequestFormMessage
            {
                MarketoResourceId = "PowerBIProgressTrackingGL",
                Email = model.Email,
                FormSlug = model.FormOptions.Slug,
            };

            await this.mediator.Send(command);
            return PartialView(model.FormOptions.SubmitView ?? "form/submitviews/_trackprogress");
        }

        [HttpPost]
        [Route("{culture=en-us}/forms/submit-download/")]
        [ValidateModelState("form/_download")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitDownloadForm(DownloadFormViewModel model)
        {
            // we intentionally do not have validation on the form model
            // however, we need to only submit clean emails to the backend
            if (!string.IsNullOrEmpty(model.Email) && new EmailAddressAttribute().IsValid(model.Email))
            {
                var command = new RegistrationRequestFormMessage
                {
                    MarketoResourceId = model.MarketoResourceId,
                    Email = model.Email,
                    FormSlug = model.FormOptions.Slug,
                };

                await this.mediator.Send(command);
            }

            return Json(new { Redirect = model.FormOptions.SubmitRedirectUrl });
        }

        [HttpPost]
        [Route("{culture=en-us}/forms/submit-download-gated/")]
        [ValidateModelState("form/_download-gated")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitDownloadGatedForm(DownloadGatedFormViewModel model)
        {

            var command = new RegistrationRequestFormMessage
            {
                MarketoResourceId = "PowerBIDesktop",
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Phone = model.Phone,
                Country = model.Country,
                Consent = model.Consent,
                FormSlug = model.FormOptions.Slug,
            };

            await this.mediator.Send(command);
            return Json(new { Redirect = model.FormOptions.SubmitRedirectUrl });
        }

        [HttpPost]
        [Route("{culture=en-us}/forms/submit-content-registration/")]
        [ValidateModelState("form/_content-registration")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitContentRegistrationForm (ContentRegistrationFormViewModel model)
        {
            var command = new RegistrationRequestFormMessage
            {
                MarketoResourceId = "ContentRegistration",
                Email = model.Email,
                Country = model.Country,
                Consent = model.Consent,
                FormSlug = model.FormOptions.Slug,
            };

            await this.mediator.Send(command);
            return PartialView(model.FormOptions.SubmitView ?? "section/_modal-ebook-registration-thankyou");
        }

        [HttpPost]
        [Route("{culture=en-us}/forms/submit-content-registration-full/")]
        [ValidateModelState("form/_content-registration-full")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitFullContentRegistrationForm(ContentRegistrationFormViewModel model)
        {
            var command = new RegistrationRequestFormMessage
            {
                MarketoResourceId = "ContentRegistration",
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Phone = model.Phone,
                Country = model.Country,
                Consent = model.Consent,
                FormSlug = model.FormOptions.Slug,
            };

            await this.mediator.Send(command);
            return PartialView(model.FormOptions.SubmitView ?? "form/submitviews/_contentregistrationthanks");
        }

        [HttpPost]
        [Route("{culture=en-us}/forms/submit-datajournalism-registration/")]
        [ValidateModelState("form/_datajournalism-registration")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitDatajournalismRegistrationForm(DatajournalismRegistrationFormViewModel model)
        {
            this.HttpContext.SetDataJournalismRegistrationComplete();

            var command = new RegistrationRequestFormMessage
            {
                MarketoResourceId = "CO-PowerBI-Data-Journalism",
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Phone = model.Phone,
                Country = model.Country,
                CompanyName = model.Company,
                Consent = model.Consent,
                FormSlug = model.FormOptions.Slug,
            };

            await this.mediator.Send(command);
            return Json(new { Redirect = model.FormOptions.SubmitRedirectUrl });
        }

        [HttpPost]
        [Route("{culture=en-us}/forms/submit-feedback/")]
        [ValidateModelState("form/_feedback")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitFeedbackForm(FeedbackFormViewModel model)
        {
            await this.SendArticleFeedback(model);

            if (model.Action == "submit-no")
            {
                return PartialView("form/_feedbackComment", model);
            }
            else
            {
                return PartialView(model.FormOptions.SubmitView ?? "form/submitviews/_feedbackthanks");
            }
        }

        [HttpPost]
        [Route("{culture=en-us}/forms/submit-feedback-comments/")]
        [ValidateModelState("form/_feedbackComment")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitFeedbackCommentsForm(FeedbackFormViewModel model)
        {
            if (model.Action != "skip")
            {
                await this.SendArticleFeedback(model);
            }

            return PartialView(model.FormOptions.SubmitView ?? "form/submitviews/_feedbackthanks");
        }

        [HttpPost]
        [Route("{culture=en-us}/forms/submit-partners-invitation-request/")]
        [ValidateModelState("form/_partnersInvitationRequest")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> SubmitPartnersInvitationRequest(PartnersInvitationRequestViewModel form)
        {
            var message = new PartnersInvitationFormMessage
            {
                CompanyName = form.Company,
                Email = form.Email,
                SubmittedOn = DateTime.UtcNow,
                Consent = form.Consent,
                Country = form.Country,
                FirstName = form.FirstName,
                LastName = form.LastName,
                JobTitle = form.JobRole,
                MpnId = form.MpnId
            };

            await this.mediator.Send(message);
            return this.PartialView(form.FormOptions.SubmitView ?? "form/submitviews/_thanksForApplication");
        }

        [HttpPost]
        [Route("{culture=en-us}/forms/submit-support-pro-ticket/")]
        [ValidateModelState("form/_support-pro-ticket")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = KnownRoles.SupportPro)]
        public async Task<ActionResult> SubmitSupportProTicket(SupportProTicketFormViewModel form)
        {
            form.ParsedQuestions = this.ParseQuestions(form).ToArray();

            try
            {
                var metisIncedent = await MapMetisIncident(form);
                form.TicketId = await this.metisService.SubmitSupportTicketAsync(metisIncedent);
            }
            catch(Exception e)
            {
                Log.Error("There was an error creating the support ticket. Message: {0}", e.Message);
            }

            var message = new SupportTicketFormMessage
            {
                FirstName = form.FirstName,
                LastName = form.LastName,
                Email = form.Email,
                Phone = form.Phone,
                Country = form.Country,
                FormSlug = form.FormOptions.Slug,
            };

            await this.mediator.Send(message);

            return this.PartialView(form.FormOptions.SubmitView ?? "form/submitviews/_support-pro-ticket-thanks", form);
        }

        private async Task SendArticleFeedback(FeedbackFormViewModel model)
        {
            var command = new ArticleFeedbackMessage
            {
                ArticleId = model.FormOptions.Slug,
                Culture = CultureHelperBase.GetCurrentCulture(),
                FeedbackTimestamp = DateTime.UtcNow,
                Url = string.Format("{0}{1}/documentation/{2}/", this.powerbiConfiguration.ProductionDomain, CultureHelperBase.GetCurrentCulture(), model.FormOptions.Slug),
                Helpful = model.Action == "submit-yes",
                IpAddress = this.HttpContext.Request.UserIPAddress(),
                Comment = model.Comments,
                CorrelationId = Guid.Parse(this.Request.GetUserInfoCookieGuid()),
                ArticleLastModified = model.ArticleModified
            };

            await this.mediator.Send(command);
        }

        private IEnumerable<string> ParseQuestions(SupportProTicketFormViewModel form)
        {
            var result = new List<string>();
            var answeredQuestions = form.Questions.Where(q => q.Value.Any(a => !string.IsNullOrWhiteSpace(a))).ToDictionary(kvp => kvp.Key, kvp => kvp.Value);

            var problemQuestions = this.questionsRepository.Get().FirstOrDefault(q => !string.IsNullOrWhiteSpace(form.ProblemType) && q.ProblemTypeId.Equals(form.ProblemType));
            if (problemQuestions == null)
            {
                problemQuestions = this.questionsRepository.Get("default");
            }

            foreach (var question in problemQuestions.Questions)
            {
                if (answeredQuestions.ContainsKey(question.QuestionLockey))
                {
                    var questionText = Questions.ResourceManager.GetString(question.QuestionLockey);
                    var answerText = string.Join(", ", answeredQuestions[question.QuestionLockey]);

                    if ((question.QuestionType == SupportQuestionType.MultiselectWithOther)
                        && (answeredQuestions.ContainsKey($"{question.QuestionLockey}_Other")))
                    {
                        answerText += ": " + string.Join(", ", answeredQuestions[$"{question.QuestionLockey}_Other"]);
                    }

                    result.Add($"{questionText}: {answerText}");
                }
            }

            return result;
        }

        private async Task<MetisIncident> MapMetisIncident(SupportProTicketFormViewModel form)
        {
            var supportRegionsList = await this.supportRegionSkuRepository.GetAsync();
            var supportRegion = supportRegionsList.FirstOrDefault(x => x.Countries.Contains(form.Country)) ?? supportRegionsList.First();

            var category = string.Empty;
            if (!string.IsNullOrWhiteSpace(form.Category))
            {
                category = form.Category.Contains('.') ? form.Category.Split('.')[1] : string.Empty;
            }

            var entitlement = new Entitlement();
            entitlement.ExtendedProperties.Add(new KeyValuePair<string, string>(nameof(SupportRegionSku.ProductServicePackageSku), supportRegion?.ProductServicePackageSku ?? string.Empty));

            var orgName = User.Identity.ClaimValue(KnownClaimTypes.CompanyName);
            var tenantId = User.Identity.ClaimValue(KnownClaimTypes.TenantId);

            return new MetisIncident
            {
                CustomerCountry = form.Country,
                Subject = form.Subject,
                Description = string.Join("\r\n", form.ParsedQuestions),
                SupportTopicId = category,
                Organization = new Organization
                {
                    TenantId = tenantId,
                    Name = !string.IsNullOrWhiteSpace(orgName) ? orgName : "Unknown"
                },
                PrimaryContact = new PrimaryContact
                {
                    Puid = User.Identity.ClaimValue(KnownClaimTypes.Puid),
                    FirstName = form.FirstName,
                    LastName = form.LastName,
                    Language = KnownCultures.Current,
                    OrganizationTenantId = tenantId,
                    PrimaryEmailAddress = form.Email,
                    PrimaryPhoneNumber = form.Phone,
                    TimeZoneId = form.TimeZone,
                    AadObjectId = User.Identity.ClaimValue(KnownClaimTypes.ObjectIdentifier)
                },
                Entitlement = entitlement
            };
        }
    }
}