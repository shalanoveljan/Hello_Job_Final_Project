using HelloJob.Entities.DTOS;
using HelloJob.Service.Responses;

namespace HelloJob.App.ViewModels
{
    public class HomeVM
    {
        public PagginatedResponse<BlogGetDto> blogs { get; set; }
        public PagginatedResponse<VacancyGetDto> vacancies { get; set; }
        public PagginatedResponse<CategoryGetDto> categories { get; set; }
        public PagginatedResponse<CompanyGetDto> companies { get; set; }
    }
}
