using AutoMapper;
using HelloJob.Core.Helper;
using HelloJob.Core.Utilities.Results.Abstract;
using HelloJob.Core.Utilities.Results.Concrete.ErrorResults;
using HelloJob.Core.Utilities.Results.Concrete.SuccessResults;
using HelloJob.Data.DAL.Interfaces;
using HelloJob.Entities.DTOS;
using HelloJob.Entities.Models;
using HelloJob.Service.Extensions;
using HelloJob.Service.Responses;
using HelloJob.Service.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Service.Services.Implementations
{
    public class CategoryService : ICategoryService
    {

        readonly IWebHostEnvironment _env;
        readonly ICategoryDAL _categoryRepository;
        readonly IMapper _mapper;


        public CategoryService(IWebHostEnvironment env, ICategoryDAL categoryRepository, IMapper mapper)
        {
            _env = env;
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }
        public async Task<IResult> CreateAsync(CategoryPostDto dto)
        {
            if (string.IsNullOrEmpty(dto.Name))
            {
                return new ErrorResult("Category name is required");
            }
            
            if (dto.ParentId!=null)
            {
                var parentCategory = await _categoryRepository.GetAsync(x=>x.Id==dto.ParentId);
                var subcategory = _mapper.Map<Category>(dto);
                subcategory.Storage = "wwwroot";
                subcategory.ParentId = parentCategory.Id;
                parentCategory.Children.Add(subcategory);
                await _categoryRepository.UpdateAsync(parentCategory);
            }
            else
            {
                var category = _mapper.Map<Category>(dto);
                category.ParentId = dto.ParentId;
                category.Storage = "wwwroot";
                if (dto.ImageFile != null)
                {
                    if (!dto.ImageFile.IsImage())
                    {
                        return new ErrorResult("Image is not valid");

                    }

                    if (dto.ImageFile.IsSizeOk(2))
                    {
                        return new ErrorResult("Image size is not valid");

                    }
                    category.Image = dto.ImageFile.SaveFile(_env.WebRootPath, "assets/images/categories");

                }
            await _categoryRepository.AddAsync(category);
            }
            

            return new SuccessResult("Create Category successfully");
        }

        public async Task<PagginatedResponse<CategoryGetDto>> GetAllAsync(int pageNumber = 1, int pageSize = 6)
        {
            var query = _categoryRepository.GetQuery(x => !x.IsDeleted)
             .AsNoTrackingWithIdentityResolution()
             .Include(x => x.Parent).ThenInclude(x => x.Children);

            var totalCount = await query.CountAsync();


            var paginatedCategorys = await query.ToPagedListAsync(pageNumber, pageSize);

            var getdto=query.Select(x =>
                new CategoryGetDto
                {
                    Name = x.Name,
                    Id = x.Id,
                    Image = x.Image,
                    ParentId = x.ParentId,
                    Children = x.Children
                }).ToList();

            var CategoryGetDtos = paginatedCategorys.Datas.Select(x =>
                new CategoryGetDto
                {
                    Name = x.Name,
                    Id = x.Id,
                    Image = x.Image,
                    ParentId = x.ParentId,
                    Children=x.Children
                }).ToList();

            var pagginatedResponse = new PagginatedResponse<CategoryGetDto>(
                CategoryGetDtos, paginatedCategorys.PageNumber,
                paginatedCategorys.PageSize,
                totalCount, getdto);

            return pagginatedResponse;
        }

        public async Task<IDataResult<CategoryGetDto>> GetAsync(int id)
        {
            Category category = await _categoryRepository.GetAsync(x => !x.IsDeleted && x.Id == id,  "Parent");

            if (category == null)
            {
                return new ErrorDataResult<CategoryGetDto>("Category Not Found");
            }

            CategoryGetDto categoryGetDto = new CategoryGetDto
            {
                Name = category.Name,
                Id = category.Id,
                ParentId =category.ParentId,
                Image = category.ParentId == 0 ? category.Image : null 
            };

            return new SuccessDataResult<CategoryGetDto>(categoryGetDto, "Get Category successfully");
        }


        public async Task<IResult> RemoveAsync(int id)
        {
            Category? Category = await _categoryRepository.GetAsync(x => !x.IsDeleted && x.Id == id, "Parent");

            if (Category == null)
            {
                return new ErrorResult("Category Not Found");
            }
            Category.IsDeleted = true;
            await _categoryRepository.UpdateAsync(Category);
            return new SuccessResult("Category removed");
        }

        public async Task<IResult> UpdateAsync(int id, CategoryPostDto dto)
        {
            var categoryToUpdate = await _categoryRepository.GetAsync(x=>x.Id==id);
            if (categoryToUpdate == null)
            {
                return new ErrorResult("Category not found");
            }

            
            categoryToUpdate.Name = dto.Name;
            categoryToUpdate.ParentId = dto.ParentId;


            if (dto.ParentId != null) 
            {
                categoryToUpdate.ParentId = dto.ParentId;

                var parentCategory = await _categoryRepository.GetAsync(x=>x.ParentId==dto.ParentId);
                if (parentCategory != null)
                {
                    parentCategory.Children.Clear();
                    parentCategory.Children.Add(categoryToUpdate); 
                    await _categoryRepository.UpdateAsync(parentCategory);
                    await _categoryRepository.UpdateAsync(categoryToUpdate);
                }
            }

            else 
            {
                if (dto.ImageFile != null)
                {
                    if (!dto.ImageFile.IsImage())
                    {
                        return new ErrorResult("Image is not valid");
                    }

                    if (dto.ImageFile.IsSizeOk(2))
                    {
                        return new ErrorResult("Image size is not valid");
                    }
                    categoryToUpdate.Image = dto.ImageFile.SaveFile(_env.WebRootPath, "assets/images/categories");
                }
            }

            await _categoryRepository.UpdateAsync(categoryToUpdate);

            return new SuccessResult("Update Category successfully");
        }

    }
}
