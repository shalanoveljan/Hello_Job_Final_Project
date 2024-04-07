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
    public class TagService : ITagService
    {
        readonly ITagDAL _TagRepository;
        readonly IMapper _mapper;

        public TagService(ITagDAL TagRepository, IMapper mapper)
        {
            _TagRepository = TagRepository;
            _mapper = mapper;
        }
        public async Task<IResult> CreateAsync(TagPostDto dto)
        {
            Tag Tag = _mapper.Map<Tag>(dto);
            if (Tag == null)
            {
                return new ErrorResult("Tag is null");
            }

            await _TagRepository.AddAsync(Tag);

            return new SuccessResult("Create Tag successfully");

        }

        public async Task<PagginatedResponse<TagGetDto>> GetAllAsync(int pageNumber = 1, int pageSize = 6)
        {
            var query = _TagRepository.GetQuery(x => !x.IsDeleted);
            var totalCount = await query.CountAsync();

            var paginatedTags = await query.ToPagedListAsync(pageNumber, pageSize);
            var TagGetDtos = paginatedTags.Datas.Select(x =>
               new TagGetDto
               {
                   Id = x.Id,
                   Name = x.Name,
                   CreateAt = DateTime.Now,
               }).ToList();
            var pagginatedResponse = new PagginatedResponse<TagGetDto>(TagGetDtos, paginatedTags.PageNumber,
              paginatedTags.PageSize,
              totalCount);
            return pagginatedResponse;
        }

        public async Task<IDataResult<TagGetDto>> GetAsync(int id)
        {
            var Tag = _TagRepository.GetAsync(x => !x.IsDeleted && x.Id == id).Result;
            if (Tag == null)
            {
                return new ErrorDataResult<TagGetDto>("Tag Not Found");
            }
            TagGetDto dto = new TagGetDto
            {
                Id = Tag.Id,
                Name = Tag.Name,
                CreateAt = DateTime.Now,
            };


            return new SuccessDataResult<TagGetDto>(dto, "Get Tag");
        }

        public async Task<IResult> RemoveAsync(int id)
        {
            Tag? Tag = await _TagRepository.GetAsync(x => !x.IsDeleted && x.Id == id);

            if (Tag == null)
            {
                return new ErrorResult("Tag Not Found");
            }
            Tag.IsDeleted = true;
            await _TagRepository.UpdateAsync(Tag);
            return new SuccessResult("Tag removed");
        }

        public async Task<IResult> UpdateAsync(int id, TagPostDto dto)
        {
            Tag? Tag = await _TagRepository.GetAsync(x => !x.IsDeleted && x.Id == id);

            if (Tag == null)
            {
                return new ErrorResult("Tag is null");
            }
            Tag.Name = dto.Name;
            

            await _TagRepository.UpdateAsync(Tag);

            return new SuccessResult("Update Tag successfully");
        }
        
     
    }
}
