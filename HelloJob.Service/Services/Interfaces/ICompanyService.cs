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
    public interface ICompanyService
    {
        public Task<PagginatedResponse<CompanyGetDto>> GetAllAsync(string userId, bool isAdmin, int pageNumber = 1, int pageSize = 6);
        public Task<PagginatedResponse<CompanyGetDto>> GetAllCheckingOrderAsync(int pageNumber = 1, int pageSize = 6);
        public Task<IResult> CreateAsync(CompanyPostDto dto);
        public Task<IResult> RemoveAsync(int id);
        public Task<IResult> UpdateAsync(int id, CompanyPostDto dto);
        public Task<IDataResult<CompanyGetDto>> GetAsync(int id);
        public Task<IDataResult<List<CompanyGetDto>>> GetAllForCompanyPageInWebSiteAsync();
        public Task<IResult> SetOrderStatus(int CompanyId, Order orderStatus);
    }
}
