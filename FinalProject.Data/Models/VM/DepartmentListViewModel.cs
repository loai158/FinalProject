using FinalProject.Data.Models.AppModels;

namespace FinalProject.Data.Models.VM
{

    public class DepartmentListViewModel
    {
        public List<Department> Departments { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public string SearchQuery { get; set; }

    }

}

