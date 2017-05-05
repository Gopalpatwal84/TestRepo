using PowerBI.Entities;

namespace PowerBI.Web.Models.Pages.Departments
{
    public class DepartmentDetailsViewModel : ViewMetadataModel
    {
        public Department Department { get; set; }

        public Department[] Departments { get; set; }
    }
}