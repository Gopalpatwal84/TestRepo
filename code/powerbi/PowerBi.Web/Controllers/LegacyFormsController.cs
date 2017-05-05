using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Web.Mvc;
using Mediation;
using PowerBI.Messages.Commands;
using PowerBI.Web.Models.LegacyForms;

namespace PowerBI.Web.Controllers
{
    public class LegacyFormsController : Controller
    {
        private readonly IMediator _mediator;

        public LegacyFormsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [AcceptVerbs(HttpVerbs.Post | HttpVerbs.Options)]
        [Route("submit-newsletter-email")]
        public async Task<ActionResult> SubmitNewsletterEmail(SubmitEmailViewModel model)
        {
            if (!string.IsNullOrEmpty(model.Email) && new EmailAddressAttribute().IsValid(model.Email))
            {
                var command = new RegistrationRequestFormMessage
                {
                    MarketoResourceId = "PowerBINewsletter",
                    Email = model.Email,
                    FormSlug = "submit-newsletter-email",
                };

                await _mediator.Send(command);
                return Json("Success");
            }

            return Json("Invalid Email");
        }

        [AcceptVerbs(HttpVerbs.Post | HttpVerbs.Options)]
        [Route("submitemail")]
        public async Task<ActionResult> SubmitEmail(SubmitEmailViewModel model)
        {
            if (!string.IsNullOrEmpty(model.Email) && new EmailAddressAttribute().IsValid(model.Email))
            {
                var command = new RegistrationRequestFormMessage
                {
                    MarketoResourceId = "PowerBIMobile",
                    Email = model.Email,
                    FormSlug = "submitemail",
                };

                await _mediator.Send(command);
                return Json("Success");
            }

            return Json("Invalid Email");
        }
    }
}