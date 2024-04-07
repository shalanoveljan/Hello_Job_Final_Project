using HelloJob.Core.Utilities.Results.Abstract;
using HelloJob.Entities.DTOS;
using HelloJob.Entities.Enums;
using HelloJob.Service.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Service.Services.Interfaces
{
    public  interface IVacancyService
    {
        public Task<PagginatedResponse<VacancyGetDto>> GetAllAsync(string userId, bool isAdmin, int pageNumber = 1, int pageSize = 6);
        public Task<IResult> CreateAsync(VacancyPostDto dto);
        public Task<IResult> RemoveAsync(int id);
        public Task<IResult> UpdateAsync(int id, VacancyPostDto dto);
        public Task<IDataResult<VacancyGetDto>> GetAsync(int id);
        public Task<IDataResult<List<VacancyGetDto>>> GetAllForVacancyPageInWebSiteAsync();
        public Task<PagginatedResponse<VacancyGetDto>> GetVacancysBySearchTextAsync(string searchText, int pageNumber = 1, int pageSize = 6);
        public Task IncreaseCount(int id);
        public Task<IResult> SetOrderStatus(int VacancyId, Order orderStatus);
    }
}
