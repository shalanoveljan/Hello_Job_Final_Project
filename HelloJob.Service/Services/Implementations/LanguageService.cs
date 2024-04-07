using AutoMapper;
using HelloJob.Core.Utilities.Results.Abstract;
using HelloJob.Core.Utilities.Results.Concrete.ErrorResults;
using HelloJob.Core.Utilities.Results.Concrete.SuccessResults;
using HelloJob.Data.DAL.Interfaces;
using HelloJob.Entities.DTOS;
using HelloJob.Entities.Models;
using HelloJob.Service.Extensions;
using HelloJob.Service.Responses;
using HelloJob.Service.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Service.Services.Implementations
{
    public class LanguageService : ILanguageService
    {
        readonly ILanguageDAL _LanguageRepository;
        readonly IMapper _mapper;

        public LanguageService(ILanguageDAL LanguageRepository, IMapper mapper)
        {
            _LanguageRepository = LanguageRepository;
            _mapper = mapper;
        }
        public async Task<IResult> CreateAsync(LanguagePostDto dto)
        {
            Language Language = _mapper.Map<Language>(dto);
            if (Language == null)
            {
                return new ErrorResult("Language is null");
            }

            await _LanguageRepository.AddAsync(Language);

            return new SuccessResult("Create Language successfully");

        }

        public async Task<PagginatedResponse<LanguageGetDto>> GetAllAsync(int pageNumber = 1, int pageSize = 6)
        {
            var query = _LanguageRepository.GetQuery(x => !x.IsDeleted);
            var totalCount = await query.CountAsync();

            var paginatedLanguages = await query.ToPagedListAsync(pageNumber, pageSize);
            var LanguageGetDtos = paginatedLanguages.Datas.Select(x =>
               new LanguageGetDto
               {
                   Id = x.Id,
               Name= x.Name,
               CreateAt= DateTime.Now,
               }).ToList();
            var pagginatedResponse = new PagginatedResponse<LanguageGetDto>(LanguageGetDtos, paginatedLanguages.PageNumber,
             paginatedLanguages.PageSize,
             totalCount);
            return pagginatedResponse;
        }

        public async Task<IDataResult<LanguageGetDto>> GetAsync(int id)
        {
            var Language = _LanguageRepository.GetAsync(x => !x.IsDeleted && x.Id == id).Result;
            if (Language == null)
            {
                return new ErrorDataResult<LanguageGetDto>("Language Not Found");
            }
            LanguageGetDto dto = new LanguageGetDto
            {
                Id = Language.Id,
                Name = Language.Name,
                CreateAt = DateTime.Now,
            };


            return new SuccessDataResult<LanguageGetDto>(dto, "Get Language");
        }

        public async Task<IResult> RemoveAsync(int id)
        {
            Language? Language = await _LanguageRepository.GetAsync(x => !x.IsDeleted && x.Id == id);

            if (Language == null)
            {
                return new ErrorResult("Language Not Found");
            }
            Language.IsDeleted = true;
            await _LanguageRepository.UpdateAsync(Language);
            return new SuccessResult("Language removed");
        }

        public async Task<IResult> UpdateAsync(int id, LanguagePostDto dto)
        {
            Language? Language = await _LanguageRepository.GetAsync(x => !x.IsDeleted && x.Id == id);

            if (Language == null)
            {
                return new ErrorResult("Language is null");
            }

            Language.Name= dto.Name;


            await _LanguageRepository.UpdateAsync(Language);

            return new SuccessResult("Update Language successfully");
        }
        
     
    }
}
