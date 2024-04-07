using AutoMapper;
using HelloJob.Core.Helper;
using HelloJob.Core.Utilities.Results.Abstract;
using HelloJob.Core.Utilities.Results.Concrete.ErrorResults;
using HelloJob.Core.Utilities.Results.Concrete.SuccessResults;
using HelloJob.Data.DAL.Interfaces;
using HelloJob.Data.DBContexts.SQLSERVER;
using HelloJob.Entities.DTOS;
using HelloJob.Entities.Enums;
using HelloJob.Entities.Models;
using HelloJob.Service.Extensions;
using HelloJob.Service.Responses;
using HelloJob.Service.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Service.Services.Implementations
{
    public class CourseService : ICourseService
    {
        readonly IWebHostEnvironment _env;
        readonly ICourseDAL _CourseRepository;
        readonly IMapper _mapper;
        readonly ICategoryDAL _categoryRepository;
        private readonly HelloJobDbContext _context;

        public CourseService(IWebHostEnvironment env, ICourseDAL CourseRepository, IMapper mapper, ITagDAL tag, ICategoryDAL categoryRepository)
        {
            _env = env;
            _CourseRepository = CourseRepository;
            _mapper = mapper;
            _categoryRepository = categoryRepository;

        }
        public async Task<IResult> CreateAsync(CoursePostDto dto)
        {
            Course Course = _mapper.Map<Course>(dto);

            if (dto.ImageFile == null)
            {
                return new ErrorResult("The field image is required");
            }

            if (!dto.ImageFile.IsImage())
            {
                return new ErrorResult("Image is not valid");

            }

            if (dto.ImageFile.IsSizeOk(2))
            {
                return new ErrorResult("Image size is not valid");

            }


            Course.Image = dto.ImageFile.SaveFile(_env.WebRootPath, "assets/images/courses");
            Course.TagsCourse = new List<TagCourse>();
            foreach (var item in dto.TagsIds)
            {
                Course.TagsCourse.Add(new TagCourse
                {
                    TagId = item
                });
            }
            //await _context.Courses.AddAsync(Course);
            //await _context.SaveChangesAsync();
            await _CourseRepository.AddAsync(Course);
            var a = _CourseRepository.GetQuery(x => !x.IsDeleted && x.Id == Course.Id);
            return new SuccessResult("Create Course successfully");

        }

        public async Task<PagginatedResponse<CourseGetDto>> GetAllAsync(int pageNumber = 1, int pageSize = 6)
        {
            var query = GetBaseQuery();

            var totalCount = await query.CountAsync();


            var paginatedCourses = await query.ToPagedListAsync<Course>(pageNumber, pageSize);

            var CourseGetDtos = paginatedCourses.Datas.Select(x =>
                   new CourseGetDto
                   {
                       Title = x.Title,
                       Id = x.Id,
                       Description = x.Description,
                       Agency = x.Agency,
                       Image = x.Image,
                       Price = x.Price,
                       IsPremium = x.IsPremium,
                       IsRetirement = x.IsRetirement,
                       IsSertificate = x.IsSertificate,
                       Level = x.Level,
                       Mode = x.Mode,
                       Period = x.Period,
                       Tags = x.TagsCourse.Select(x => new TagGetDto { Name = x.Tag.Name, Id = x.Tag.Id }),
                       CategoryId = x.CategoryId,
                       Category = new CategoryGetDto { Name = x.Category.Name, Id = x.Category.Id },
                   }

                   ).ToList();

            var pagginatedResponse = new PagginatedResponse<CourseGetDto>(CourseGetDtos, paginatedCourses.PageNumber,
                paginatedCourses.PageSize,
                totalCount);


            return pagginatedResponse;
        }

        public async Task<IDataResult<CourseGetDto>> GetAsync(int id)
        {
            var query = _CourseRepository.GetQuery(x => !x.IsDeleted)
                         .AsNoTrackingWithIdentityResolution()
                         .Include(x => x.Category)
                         .Include(x => x.TagsCourse)
                         .ThenInclude(x => x.Tag);
            var Courses = await query.Select(x =>
              new CourseGetDto
              {
                  Title = x.Title,
                  Id = x.Id,
                  Description = x.Description,
                  Agency = x.Agency,
                  Image = x.Image,
                  Price = x.Price,
                  IsPremium = x.IsPremium,
                  IsRetirement = x.IsRetirement,
                  IsSertificate = x.IsSertificate,
                  Level = x.Level,
                  Mode = x.Mode,
                  maxAge= x.maxAge,
                  minAge= x.minAge,
                  Period = x.Period,
                  Tags = x.TagsCourse.Select(x => new TagGetDto { Name = x.Tag.Name, Id = x.Tag.Id }),
                  CategoryId = x.CategoryId,
                  Category = new CategoryGetDto { Name = x.Category.Name, Id = x.Category.Id },
              }).ToListAsync();


            CourseGetDto? Course = Courses.FirstOrDefault(x => x.Id == id);

            if (Course == null)
            {
                return new ErrorDataResult<CourseGetDto>("Course Not Found");
            }

            return new SuccessDataResult<CourseGetDto>(Course, "Get Course");
        }

        public async Task<IResult> RemoveAsync(int id)
        {
            Course? Course = await _CourseRepository.GetAsync(x => !x.IsDeleted && x.Id == id, "TagsCourse.Tag");

            if (Course == null)
            {
                return new ErrorResult("Course Not Found");
            }
            Course.IsDeleted = true;
            await _CourseRepository.UpdateAsync(Course);
            return new SuccessResult("Course removed");
        }

        public async Task<IResult> UpdateAsync(int id, CoursePostDto dto)
        {
            Course? Course = await _CourseRepository.GetAsync(x => !x.IsDeleted && x.Id == id, "TagsCourse.Tag");
            if (Course == null)
            {
                return new ErrorResult("The Course not found");

            }
            Course.CategoryId = dto.CategoryId;
            Course.Agency = dto.Agency;
            Course.maxAge = dto.maxAge;
            Course.minAge = dto.minAge;
            Course.Description = dto.Description;
            Course.Mode = dto.Mode;
            Course.Level = dto.Level;
            Course.IsPremium = dto.IsPremium;
            Course.IsRetirement = dto.IsRetirement;
            Course.IsSertificate = dto.IsSertificate;
            Course.Price = dto.Price;
            Course.Period = dto.Period;
            Course.Title = dto.Title;



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

                Course.Image = dto.ImageFile.SaveFile(_env.WebRootPath, "assets/images/courses");
            }


            Course.TagsCourse.Clear();

            foreach (var item in dto.TagsIds)
            {
                Course.TagsCourse.Add(new TagCourse
                {
                    Course = Course,
                    TagId = item,
                });
            }

            await _CourseRepository.UpdateAsync(Course);
            return new SuccessResult("Update Course successfully");

        }

        private IQueryable<Course> GetBaseQuery()
        {
            return _CourseRepository.GetQuery(x => !x.IsDeleted)
                .AsNoTrackingWithIdentityResolution()
                .Include(x => x.Category)
                .Include(x => x.TagsCourse)
                .ThenInclude(x => x.Tag);
        }

        private async Task<List<CourseGetDto>> GetCourseGetDtoListAsync(IQueryable<Course> query)
        {
            return await query.Select(x => new CourseGetDto
            {
                Title = x.Title,
                Id = x.Id,
                Description = x.Description,
                Agency = x.Agency,
                Image = x.Image,
                Price = x.Price,
                IsPremium = x.IsPremium,
                IsRetirement = x.IsRetirement,
                IsSertificate = x.IsSertificate,
                maxAge = x.maxAge,
                minAge = x.minAge,
                Level = x.Level,
                Mode = x.Mode,
                Period = x.Period,
                Tags = x.TagsCourse.Select(x => new TagGetDto { Name = x.Tag.Name, Id = x.Tag.Id }),
                CategoryId = x.CategoryId,
                Category = new CategoryGetDto { Name = x.Category.Name, Id = x.Category.Id },
            }).ToListAsync();
        }
        public async Task<IDataResult<List<CourseGetDto>>> GetAllForCoursePageInWebSiteAsync()
        {
            var query = GetBaseQuery();

            var CourseGetDtos = await GetCourseGetDtoListAsync(query);

            if (CourseGetDtos == null)
            {
                return new ErrorDataResult<List<CourseGetDto>>("Courses Not Found");
            }


            return new SuccessDataResult<List<CourseGetDto>>(CourseGetDtos, "Get Courses for SITE PAGE");
        }
        public async Task<IDataResult<List<CourseGetDto>>> SortCourses(int id,CourseFilterDto dto)
            {
               dto.IsSort = true;
                var filteredCourseGetDtos = await FilterCourses(dto);
                
                var data = filteredCourseGetDtos.Data;
                switch (id)
                {
                    case 1:
                        data = data.OrderBy(course => course.Period).ToList();
                        break;
                    case 2:
                        data = data.OrderByDescending(course => course.Period).ToList();
                        break;
                    case 3:
                        data = data.OrderByDescending(course => course.Level).ToList();
                        break;
                    case 4:
                        data = data.OrderBy(course => course.Level).ToList();
                        break;
                    default:
                        break;
                }
                return new SuccessDataResult<List<CourseGetDto>>(data, "Halaldi");
            }
        public async Task<IDataResult<List<CourseGetDto>>> FilterCourses(CourseFilterDto dto)
        {
            var query = GetBaseQuery();

            var CourseGetDtos = await GetCourseGetDtoListAsync(query);

            // Filter by category IDs
            if (dto.CategoriesIds != null && dto.CategoriesIds.Count > 0)
            {
                var activeCategories = _categoryRepository.GetQuery(x => !x.IsDeleted && x.ParentId == null).ToList();

                //CourseGetDtos = CourseGetDtos.Where(course =>
                //    dto.CategoriesIds.Any(categoryId => activeCategories.Any(category => category.Id == categoryId))
                //).ToList();

                var categories = activeCategories.Where(cat =>
                    dto.CategoriesIds.Any(categoryId => cat.Id == categoryId)
                );

                CourseGetDtos = CourseGetDtos.Where(c => categories.Any(cat => cat.Id == c.CategoryId)).ToList();

            }

            // Filter by minimum time
            if (dto.MinTime > 0)
            {
                CourseGetDtos = CourseGetDtos.Where(course => course.Period >= dto.MinTime).ToList();
            }

            // Filter by maximum time
            if (dto.MaxTime < 25)
            {
                CourseGetDtos = CourseGetDtos.Where(course => course.Period <= dto.MaxTime).ToList();
            }

            // Filter by agencies
            if (dto.Agencies != null && dto.Agencies.Count > 0)
            {
                CourseGetDtos = CourseGetDtos.Where(course => dto.Agencies.Contains(course.Agency)).ToList();
            }
            // Filter by free courses
            if (dto.IsFree)
            {
                CourseGetDtos = CourseGetDtos.Where(course => course.Price == 0).ToList();
            }

            // Filter by certificate availability
            if (dto.IsSertificate)
            {
                CourseGetDtos = CourseGetDtos.Where(course => course.IsSertificate).ToList();
            }

            // Filter by retirement status
            if (dto.IsRetirement)
            {
                CourseGetDtos = CourseGetDtos.Where(course => course.IsRetirement).ToList();
            }

            // Filter by selected lesson mode
            if (dto.Selected_Lesson_Mode != LessonMode.None)
            {
                CourseGetDtos = CourseGetDtos.Where(course => course.Mode == dto.Selected_Lesson_Mode).ToList();

            }

            // Filter by selected job mode
            if (dto.Level != Level.None)
            {
                CourseGetDtos = CourseGetDtos.Where(course => course.Level == dto.Level).ToList();
            }

            return new SuccessDataResult<List<CourseGetDto>>(CourseGetDtos, "Halaldi sene ");

        }
    }
}

