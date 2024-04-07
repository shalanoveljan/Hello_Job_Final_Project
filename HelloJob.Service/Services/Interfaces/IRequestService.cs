using HelloJob.Core.Utilities.Results.Abstract;
using HelloJob.Entities.DTOS;
using HelloJob.Entities.Enums;
using HelloJob.Entities.Models;
using HelloJob.Service.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Service.Services.Interfaces
{
    public interface IRequestService
    {
        public Task<PagginatedResponse<RequestGetDto>> GetAllAsync(string userid,int pageNumber=1, int pageSize=6);
        public Task<IResult> CreateAsync(RequestPostDto dto);
        public Task<IResult> RemoveAsync(int id);

    }
}
