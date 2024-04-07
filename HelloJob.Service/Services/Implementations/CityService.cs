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
    public class CityService : ICityService
    {
        readonly ICityDAL _CityRepository;
        readonly IMapper _mapper;

        public CityService(ICityDAL CityRepository, IMapper mapper)
        {
            _CityRepository = CityRepository;
            _mapper = mapper;
        }
        public async Task<IResult> CreateAsync(CityPostDto dto)
        {
            City City = _mapper.Map<City>(dto);
            if (City == null)
            {
                return new ErrorResult("City is null");
            }

            await _CityRepository.AddAsync(City);

            return new SuccessResult("Create City successfully");

        }

        public async Task<PagginatedResponse<CityGetDto>> GetAllAsync(int pageNumber = 1, int pageSize = 6)
        {
            var query = _CityRepository.GetQuery(x => !x.IsDeleted);
            var totalCount = await query.CountAsync();

            var paginatedCitys = await query.ToPagedListAsync(pageNumber, pageSize);
            var CityGetDtos = paginatedCitys.Datas.Select(x =>
               new CityGetDto
               {
                   Id = x.Id,
                   Name = x.Name,
                   CreateAt = DateTime.Now,
               }).ToList();
            var pagginatedResponse = new PagginatedResponse<CityGetDto>(CityGetDtos, paginatedCitys.PageNumber,
              paginatedCitys.PageSize,
              totalCount);
            return pagginatedResponse;
        }

        public async Task<IDataResult<CityGetDto>> GetAsync(int id)
        {
            var City = _CityRepository.GetAsync(x => !x.IsDeleted && x.Id == id).Result;
            if (City == null)
            {
                return new ErrorDataResult<CityGetDto>("City Not Found");
            }
            CityGetDto dto = new CityGetDto
            {
                Id = City.Id,
                Name = City.Name,
                CreateAt = DateTime.Now,
            };


            return new SuccessDataResult<CityGetDto>(dto, "Get City");
        }

        public async Task<IResult> RemoveAsync(int id)
        {
            City? City = await _CityRepository.GetAsync(x => !x.IsDeleted && x.Id == id);

            if (City == null)
            {
                return new ErrorResult("City Not Found");
            }
            City.IsDeleted = true;
            await _CityRepository.UpdateAsync(City);
            return new SuccessResult("City removed");
        }

        public async Task<IResult> UpdateAsync(int id, CityPostDto dto)
        {
            City? City = await _CityRepository.GetAsync(x => !x.IsDeleted && x.Id == id);

            if (City == null)
            {
                return new ErrorResult("City is null");
            }
            City.Name = dto.Name;
            

            await _CityRepository.UpdateAsync(City);

            return new SuccessResult("Update City successfully");
        }
        
     
    }
}
