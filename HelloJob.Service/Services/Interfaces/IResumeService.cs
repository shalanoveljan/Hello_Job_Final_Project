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
    public interface IResumeService
    {
        public Task<PagginatedResponse<ResumeGetDto>> GetAllAsync(string userId, bool isAdmin, int pageNumber =1, int pageSize=6);
        public Task<IResult> CreateAsync(ResumePostDto dto);
        public Task<IResult> RemoveAsync(int id);
        public Task<IResult> UpdateAsync(int id, ResumePostDto dto);
        public Task<IDataResult<ResumeGetDto>> GetAsync(int id);
        public  Task IncreaseCount(int id);
        public Task<IDataResult<List<ResumeGetDto>>> GetAllForResumePageInWebSiteAsync();
        public Task<IDataResult<List<ResumeGetDto>>> SortResumes(int id, ResumeFilterDto dto);
        public Task<IDataResult<List<ResumeGetDto>>> FilterResumes(ResumeFilterDto dto);
        public Task<IResult> SetOrderStatus(int resumeId, Order orderStatus);
        public Task<IDataResult<List<ResumeGetDto>>> LoadMoreResumesAsync(int id, ResumeFilterDto dto, int pageNumber, int pageSize);
    }
}
