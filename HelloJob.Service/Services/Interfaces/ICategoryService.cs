using HelloJob.Core.Utilities.Results.Abstract;
using HelloJob.Entities.DTOS;
using HelloJob.Service.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Service.Services.Interfaces
{
    public interface ICategoryService
    {
        public Task<PagginatedResponse<CategoryGetDto>> GetAllAsync(int pageNumber = 1, int pageSize=6);

        public Task<IResult> CreateAsync(CategoryPostDto dto);
        public Task<IResult> RemoveAsync(int id);

        public Task<IResult> UpdateAsync(int id, CategoryPostDto dto);
        public Task<IDataResult<CategoryGetDto>> GetAsync(int id);
    }
}
