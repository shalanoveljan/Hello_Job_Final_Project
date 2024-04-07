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
    public interface ILanguageService
    {
        public Task<PagginatedResponse<LanguageGetDto>> GetAllAsync(int pageNumber=1, int pageSize=6);

        public Task<IResult> CreateAsync(LanguagePostDto dto);

        public Task<IResult> RemoveAsync(int id);

        public Task<IResult> UpdateAsync(int id, LanguagePostDto dto);
        public Task<IDataResult<LanguageGetDto>> GetAsync(int id);
    }
}
