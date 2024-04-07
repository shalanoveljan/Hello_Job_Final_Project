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
    public class SettingService : ISettingService
    {
         readonly ISettingDAL _settingRepository;
        readonly IMapper _mapper;

        public SettingService(ISettingDAL settingRepository, IMapper mapper)
        {
            _settingRepository = settingRepository;
            _mapper = mapper;
        }
        public async Task<IResult> CreateAsync(SettingPostDto dto)
        {
            Setting setting = _mapper.Map<Setting>(dto);
            if(setting == null)
            {
                return new ErrorResult("Setting is null");
            }

            await _settingRepository.AddAsync(setting);

            return new SuccessResult("Create Setting successfully");

        }

        public async Task<PagginatedResponse<SettingGetDto>> GetAllAsync(int pageNumber = 1, int pageSize = 6)
        {
            var query = _settingRepository.GetQuery(x => !x.IsDeleted);
            var totalCount = await query.CountAsync();

            var paginatedSettings = await query.ToPagedListAsync(pageNumber, pageSize);

            var SettingGetDtos = paginatedSettings.Datas.Select(x =>
               new SettingGetDto
               {
                   Key=x.Key,
                   Value=x.Value,
                   Id = x.Id,
                   CreatedAt = x.CreatedAt,
               }).ToList();
            var pagginatedResponse = new PagginatedResponse<SettingGetDto>(SettingGetDtos, paginatedSettings.PageNumber,
               paginatedSettings.PageSize,
               totalCount);
            return pagginatedResponse;
        }

        public async Task<IDataResult<SettingGetDto>> GetAsync(int id)
        {
            var Setting = _settingRepository.GetAsync(x => !x.IsDeleted && x.Id == id).Result;
            if (Setting == null)
            {
                return new ErrorDataResult<SettingGetDto>("Setting Not Found");
            }
            SettingGetDto dto = new SettingGetDto
            {
                Id = Setting.Id,
                Key = Setting.Key,
                Value = Setting.Value,
                  CreatedAt = DateTime.Now,
            };


            return new SuccessDataResult<SettingGetDto>(dto, "Get Setting");
        }

        public async Task<IResult> RemoveAsync(int id)
        {
            Setting? setting = await _settingRepository.GetAsync(x => !x.IsDeleted && x.Id == id);

            if (setting == null)
            {
                return new ErrorResult("setting Not Found");
            }
            setting.IsDeleted = true;
            await _settingRepository.UpdateAsync(setting);
            return new SuccessResult("setting removed");
        }

        public async Task<IResult> UpdateAsync(int id, SettingPostDto dto)
        {
            Setting? setting = await _settingRepository.GetAsync(x => !x.IsDeleted && x.Id == id);

            if (setting == null)
            {
                return new ErrorResult("Setting is null");
            }
            setting.Key= dto.Key;
            setting.Value= dto.Value;
            await _settingRepository.UpdateAsync(setting);

            return new SuccessResult("Update Setting successfully");
        }
    }
}
