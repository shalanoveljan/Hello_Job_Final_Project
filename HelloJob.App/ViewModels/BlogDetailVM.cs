using HelloJob.Entities.DTOS;

namespace HelloJob.App.ViewModels
{
    public class BlogDetailVM
    {
        public IList<BlogGetDto> blogs { get; set; }

        public BlogGetDto result { get; set; }
    }
}
