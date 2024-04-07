using HelloJob.Entities.DTOS;

namespace HelloJob.App.ViewModels
{
    public class LikeVM
    {
        public List<WishlistItemDto> resumeDtos { get; set; }
        public List<WishlistVacancyDto> vacancyDtos { get; set; }
    }
}
