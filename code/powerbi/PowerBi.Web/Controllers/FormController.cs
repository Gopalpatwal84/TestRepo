using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
using System.Web.Mvc;
using Acom.Forms.Models;
using Acom.IO;
using PowerBI.Entities;
using PowerBI.Web.Authorization;
using PowerBI.Web.Features.Forms.Models;

namespace PowerBI.Web.Controllers
{
    public class FormController : Controller
    {
        private readonly IRepository<SupportProblemQuestions> _questionsRepository;

        public FormController(IRepository<SupportProblemQuestions> questionsRepository)
        {
            _questionsRepository = questionsRepository;
        }

        [ChildActionOnly]
        [Route("Form/Signup")]
        public ActionResult Signup(FormOptions formOptions)
        {
            var viewModel = new SignupFormViewModel
            {
                FormOptions = formOptions
            };

            return PartialView("form/_signup", viewModel);
        }

        [ChildActionOnly]
        [Route("Form/Mobile")]
        public ActionResult Mobile(FormOptions formOptions)
        {
            var viewModel = new MobileFormViewModel
            {
                FormOptions = formOptions
            };

            return PartialView("form/_mobile", viewModel);
        }

        [ChildActionOnly]
        [Route("Form/Newsletter")]
        public ActionResult Newsletter(FormOptions formOptions)
        {
            var viewModel = new NewsletterFormViewModel
            {
                FormOptions = formOptions
            };

            return PartialView("form/_newsletter", viewModel);
        }

        [ChildActionOnly]
        [Route("Form/Feedback")]
        public ActionResult Feedback(FeedbackFormOptions formOptions)
        {
            var viewModel = new FeedbackFormViewModel
            {
                ArticleModified = formOptions.ArticleModified,
                FormOptions = formOptions
            };

            return PartialView("form/_feedback", viewModel);
        }

        [ChildActionOnly]
        [Route("Form/NewsletterFull")]
        public ActionResult NewsletterFull(FormOptions formOptions)
        {
            var viewModel = new NewsletterFormViewModel
            {
                FormOptions = formOptions,
            };

            return PartialView("form/_newsletter-full", viewModel);
        }

        [ChildActionOnly]
        [Route("Form/TrackProgress")]
        public ActionResult TrackProgress(FormOptions formOptions)
        {
            var viewModel = new SimpleRegistrationFormViewModel
            {
                FormOptions = formOptions,
            };

            return PartialView("form/_track-progress", viewModel);
        }

        [ChildActionOnly]
        [Route("Form/DownloadDesktop")]
        public ActionResult DownloadDesktop(FormOptions formOptions)
        {
            var viewModel = new DownloadFormViewModel
            {
                FormOptions = formOptions,
                MarketoResourceId = "PowerBIDesktop"
            };

            return PartialView("form/_download", viewModel);
        }

        [ChildActionOnly]
        [Route("Form/DownloadDesktopExcelPlugin")]
        public ActionResult DownloadDesktopExcelPlugin(FormOptions formOptions)
        {
            var viewModel = new DownloadFormViewModel
            {
                FormOptions = formOptions,
                MarketoResourceId = "PowerBIExceldashpub"
            };

            return PartialView("form/_download", viewModel);
        }

        [ChildActionOnly]
        [Route("Form/DownloadGated")]
        public ActionResult DownloadGated(FormOptions formOptions)
        {
            var viewModel = new DownloadGatedFormViewModel
            {
                FormOptions = formOptions,
            };

            return PartialView("form/_download-gated", viewModel);
        }

        [ChildActionOnly]
        [Route("Form/ContentRegistration")]

        public ActionResult ContentRegistration (FormOptions formOptions)
        {
            var viewModel = new ContentRegistrationFormViewModel
            {
                FormOptions = formOptions,
            };

            return PartialView("form/_content-registration", viewModel);
        }

        [ChildActionOnly]
        [Route("Form/PartnersInvitationRequest")]
        public ActionResult PartnersInvitationRequest(FormOptions options)
        {
            var viewModel = new PartnersInvitationRequestViewModel()
            {
                FormOptions = options
            };

            return PartialView("form/_partnersInvitationRequest", viewModel);
        }

        [Route("Form/SupportProTicket")]
        public ActionResult SupportProTicket(FormOptions options)
        {
            var viewModel = new SupportProTicketFormViewModel()
            {
                FormOptions = options
            };

            return PartialView("form/_support-pro-ticket", viewModel);
        }

        private object Parse(Type dataType, string ValueToConvert)
        {
            TypeConverter obj = TypeDescriptor.GetConverter(dataType);
            object value = obj.ConvertFromString(null, CultureInfo.InvariantCulture, ValueToConvert);
            return value;
        }

        [ChildActionOnly]
        [Route("Form/ContentRegistrationFull")]

        public ActionResult ContentRegistrationFull(FormOptions formOptions)
        {
            var viewModel = new ContentRegistrationFormViewModel
            {
                FormOptions = formOptions,
            };

            return PartialView("form/_content-registration-full", viewModel);
        }

        [ChildActionOnly]
        [Route("Form/DatajournalismRegistration")]

        public ActionResult DatajournalismRegistration(FormOptions formOptions)
        {
            var viewModel = new DatajournalismRegistrationFormViewModel
            {
                FormOptions = formOptions,
            };

            return PartialView("form/_datajournalism-registration", viewModel);
        }

        [Authorize(Roles = KnownRoles.SupportPro)]
        [Route("Form/Support/Questions")]
        public ActionResult Questions(string problemType)
        {
            var questions = _questionsRepository.Get().FirstOrDefault(q => !string.IsNullOrWhiteSpace(problemType) && q.ProblemTypeId.Equals(problemType));

            if (questions == null)
            {
                questions = _questionsRepository.Get("default");
            }

            return PartialView("form/_support-questions", questions);
        }
    }
}