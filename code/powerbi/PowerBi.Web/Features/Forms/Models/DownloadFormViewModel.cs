using Acom.Forms.Models;

namespace PowerBI.Web.Features.Forms.Models
{
    public class DownloadFormViewModel : IHaveFormOptions
    {
        public FormOptions FormOptions { get; set; }
        public string Email { get; set; }
        public string MarketoResourceId { get; set; }
    }
}