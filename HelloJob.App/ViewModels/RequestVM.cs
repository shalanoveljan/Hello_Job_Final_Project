using HelloJob.Entities.DTOS;

namespace HelloJob.App.ViewModels
{
    public class RequestVM
    {
        public VacancyGetDto Vacancy { get; set; }    
        public RequestPostDto dto { get; set; }
        public List<ResumeGetDto> Resumes { get; set; }
        public List<CategoryGetDto> Categories { get; set; }
    }
}
