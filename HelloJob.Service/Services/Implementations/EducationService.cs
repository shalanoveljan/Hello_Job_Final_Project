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
    public class EducationService : IEducationService
    {
        readonly IEducationDAL _EducationRepository;
        readonly IMapper _mapper;

        public EducationService(IEducationDAL EducationRepository, IMapper mapper)
        {
            _EducationRepository = EducationRepository;
            _mapper = mapper;
        }
        public async Task<IResult> CreateAsync(EducationPostDto dto)
        {
            Education Education = _mapper.Map<Education>(dto);
            if (Education == null)
            {
                return new ErrorResult("Education is null");
            }

            await _EducationRepository.AddAsync(Education);

            return new SuccessResult("Create Education successfully");

        }

        public async Task<PagginatedResponse<EducationGetDto>> GetAllAsync(int pageNumber = 1, int pageSize = 6)
        {
            var query = _EducationRepository.GetQuery(x => !x.IsDeleted);
            var totalCount = await query.CountAsync();

            var paginatedEducations = await query.ToPagedListAsync(pageNumber, pageSize);
            var EducationGetDtos = paginatedEducations.Datas.Select(x =>
               new EducationGetDto
               {
                   Id = x.Id,
               Name= x.Name,
               CreateAt= DateTime.Now,
               }).ToList();
            var pagginatedResponse = new PagginatedResponse<EducationGetDto>(EducationGetDtos, paginatedEducations.PageNumber,
             paginatedEducations.PageSize,
             totalCount);
            return pagginatedResponse;
        }

        public async Task<IDataResult<EducationGetDto>> GetAsync(int id)
        {
            var Education = _EducationRepository.GetAsync(x => !x.IsDeleted && x.Id == id).Result;
            if (Education == null)
            {
                return new ErrorDataResult<EducationGetDto>("Education Not Found");
            }
            EducationGetDto dto = new EducationGetDto
            {
                Id = Education.Id,
                Name = Education.Name,
                CreateAt = DateTime.Now,
            };


            return new SuccessDataResult<EducationGetDto>(dto, "Get Education");
        }

        public async Task<IResult> RemoveAsync(int id)
        {
            Education? Education = await _EducationRepository.GetAsync(x => !x.IsDeleted && x.Id == id);

            if (Education == null)
            {
                return new ErrorResult("Education Not Found");
            }
            Education.IsDeleted = true;
            await _EducationRepository.UpdateAsync(Education);
            return new SuccessResult("Education removed");
        }

        public async Task<IResult> UpdateAsync(int id, EducationPostDto dto)
        {
            Education? Education = await _EducationRepository.GetAsync(x => !x.IsDeleted && x.Id == id);

            if (Education == null)
            {
                return new ErrorResult("Education is null");
            }
            Education.Name = dto.Name;

            await _EducationRepository.UpdateAsync(Education);

            return new SuccessResult("Update Education successfully");
        }
        
     
    }
}
