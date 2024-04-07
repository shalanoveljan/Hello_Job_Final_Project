using AutoMapper;
using HelloJob.Core.Helper;
using HelloJob.Core.Utilities.Results.Abstract;
using HelloJob.Core.Utilities.Results.Concrete;
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
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Service.Services.Implementations
{
    public class ResumeService : IResumeService
    {
        readonly IWebHostEnvironment _env;
        readonly IResumeDAL _ResumeRepository;
        readonly ICategoryDAL _categoryRepository;
        readonly IEducationDAL _educationRepository;
        readonly ILanguageDAL _languageRepository;


        public ResumeService(IWebHostEnvironment env, IResumeDAL ResumeRepository,  ICategoryDAL categoryRepository, IEducationDAL educationRepository, ILanguageDAL languageRepository)
        {
            _env = env;
            _ResumeRepository = ResumeRepository;
            _categoryRepository = categoryRepository;
            _educationRepository = educationRepository;
            _languageRepository = languageRepository;
        }
        private void AddEducationsFromDto(ResumePostDto dto, Resume resume)
        {
            for (int i = 0; i < dto.Univerties.Count; i++)
            {
                Employee_Special_Education education = new Employee_Special_Education
                {
                    University = dto.Univerties[i],
                    Degree = dto.Degrees[i], 
                    StartDate = dto.EducationStartDates[i],
                    EndDate = dto.EducationEndDates[i],
                    Resume=resume
                };
                resume.educations.Add(education);
            }
        }

        private void AddExperiencesFromDto(ResumePostDto dto, Resume resume)
        {
            for (int i = 0; i < dto.Workplaces.Count; i++)
            {
                Employee_Special_Experience experience = new Employee_Special_Experience
                {
                    Workplace = dto.Workplaces[i],
                    Position = dto.Positions[i],
                    StartDate = dto.ExperienceStartDates[i],
                    EndDate = dto.ExperienceEndDates[i],
                    Resume=resume
                };
                resume.experiences.Add(experience);
            }
        }
        private void AddSkillsFromDto(ResumePostDto dto, Resume resume)
        {
            foreach (var skillName in dto.SkillNames)
            {
                Skill skill = new Skill
                {
                    Name = skillName,
                    Resume=resume
                };
                resume.Skills.Add(skill);
            }
        }
        public async Task<IResult> CreateAsync(ResumePostDto dto)
        {
            Order orderStatus = Order.None;

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
            if (dto.EducationStartDates.Count != dto.EducationEndDates.Count ||
       dto.ExperienceStartDates.Count != dto.ExperienceEndDates.Count)
            {
                return new ErrorResult("Invalid date range for education or experience");
            }

            Resume resume = new Resume
            {
                AppUserId = dto.AppUserId,
                Name = dto.Name,
                Surname = dto.Surname,
                Email = dto.Email,
                PhoneNumber = dto.PhoneNumber,
                Gender = dto.Gender,
                Mode = dto.Mode,
                Status = dto.Status,
                Salary = dto.Salary,
                IsDriverLicense = dto.IsDriverLicense,
                IsPremium = dto.IsPremium,
                CategoryId = dto.CategoryId,
                EducationId = dto.EducationId,
                LanguageId = dto.LanguageId,
                CityId = dto.CityId,
                Experience = dto.Experience,
                Birthday = dto.Birthday,
                EndDate = dto.EndDate,
                educations = new List<Employee_Special_Education>(),
                experiences = new List<Employee_Special_Experience>(),
                Skills = new List<Skill>()
            };
            resume.order = orderStatus;

            resume.Image = dto.ImageFile.SaveFile(_env.WebRootPath, "assets/images/Resumes");

            AddEducationsFromDto(dto, resume);
            AddExperiencesFromDto(dto, resume);
            AddSkillsFromDto(dto, resume);
            await _ResumeRepository.AddAsync(resume);
            return new SuccessResult("Create Resume successfully");
        
    }

        public async Task<PagginatedResponse<ResumeGetDto>> GetAllAsync(string userId, bool isAdmin, int pageNumber = 1, int pageSize = 8)
        {
            IQueryable<Resume> query;

            if (isAdmin)
            {
                query = _ResumeRepository.GetQuery(x => !x.IsDeleted);
            }
            else
            {
                query = _ResumeRepository.GetQuery(x => x.AppUserId == userId && !x.IsDeleted && x.order != Order.Reject);
            }
            var totalCount = await query.CountAsync();

            var paginatedResumes = await query
                .AsNoTrackingWithIdentityResolution()
                .Include(x => x.AppUser)
                .Include(x => x.City)
                .Include(x => x.Education)
                .Include(x => x.Language)
                .Include(x => x.experiences)
                 .Include(x => x.Skills)
                 .Include(x => x.educations)
                .Include(x => x.Category)
                .Include(x => x.WishListItems)
                  .ThenInclude(y => y.Wishlist)
                 .Include(x => x.WishListItems)
                  .ThenInclude(y => y.Wishlist.AppUser)
                .ToPagedListAsync(pageNumber, pageSize);

            var ResumeGetDtos = paginatedResumes.Datas.Select(x =>
                new ResumeGetDto
                {
                    Id = x.Id,
                    Image = x.Image,
                    Name = x.Name,
                    Surname = x.Surname,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    Gender = x.Gender,
                    Mode = x.Mode,
                    Status = x.Status,
                    Salary = x.Salary,
                    IsDriverLicense = x.IsDriverLicense,
                    Birthday=x.Birthday,
                    IsPremium = x.IsPremium,
                    Experience = x.Experience,
                    CreatedAt = x.CreatedAt,
                    EndDate = x.EndDate,
                    order=x.order,
                    ViewCount = x.ViewCount,
                    educations = x.educations,
                    experiences = x.experiences,
                    Skills = x.Skills,
                    Category = new CategoryGetDto { Id = x.Category.Id, Name = x.Category.Name, Image = x.Category.Image, ParentId = x.Category.ParentId },
                    Education = new EducationGetDto { Id = x.Education.Id, Name = x.Education.Name, CreateAt = x.Education.CreatedAt },
                    Language = x.Language,
                    City = new CityGetDto { Id = x.City.Id, Name = x.City.Name, CreateAt = x.City.CreatedAt },
                    AppUser = x.AppUser,
                }).ToList();

            var pagginatedResponse = new PagginatedResponse<ResumeGetDto>(
                ResumeGetDtos, paginatedResumes.PageNumber,
                paginatedResumes.PageSize,
                totalCount);

            return pagginatedResponse;
        }

        public async Task<IDataResult<ResumeGetDto>> GetAsync(int id)
        {
            var resume = await _ResumeRepository.GetQuery(x => x.Id == id && !x.IsDeleted)
                                        .AsNoTrackingWithIdentityResolution()
                                        .Include(x => x.AppUser)
                                        .Include(x => x.City)
                                        .Include(x => x.Education)
                                        .Include(x => x.Language)
                                        .Include(x => x.experiences)
                                        .Include(x => x.Skills)
                                        .Include(x => x.educations)
                                        .Include(x => x.Category)
                                        .Include(x => x.WishListItems)
                                        .ThenInclude(y => y.Wishlist)
                                       .Include(x => x.WishListItems)
                                        .ThenInclude(y => y.Wishlist.AppUser)
                                        .FirstOrDefaultAsync();
            if (resume == null)
            {
                return new ErrorDataResult<ResumeGetDto>("Resume Not Found");
            }
            var resumeGetDto = new ResumeGetDto
            {
                Id = resume.Id,
                Image = resume.Image,
                Name = resume.Name,
                Surname = resume.Surname,
                Email = resume.Email,
                PhoneNumber = resume.PhoneNumber,
                Birthday=resume.Birthday,
                Gender = resume.Gender,
                Mode = resume.Mode,
                Status = resume.Status,
                Salary = resume.Salary,
                order=resume.order,
                IsDriverLicense = resume.IsDriverLicense,
                IsPremium = resume.IsPremium,
                Category = new CategoryGetDto { Id = resume.Category.Id, Name = resume.Category.Name, Image = resume.Category.Image, ParentId = resume.Category.ParentId },
                Education = new EducationGetDto { Id = resume.Education.Id, Name = resume.Education.Name, CreateAt = resume.Education.CreatedAt },
                Language = resume.Language,
                City = new CityGetDto { Id = resume.City.Id, Name = resume.City.Name, CreateAt = resume.City.CreatedAt },
                Experience = resume.Experience,
                CreatedAt = resume.CreatedAt,
                EndDate = resume.EndDate,
                ViewCount = resume.ViewCount,
                educations = resume.educations,
                experiences = resume.experiences,
                Skills = resume.Skills,
                AppUser = resume.AppUser
            };
             return new SuccessDataResult<ResumeGetDto>(resumeGetDto,"Get Resume"); 
        }

        public async Task<IResult> RemoveAsync(int id)
        {
            Resume? Resume = await _ResumeRepository.GetAsync(x => !x.IsDeleted && x.Id == id, "Category", "City", "Education", "Language", "AppUser");

            if (Resume == null)
            {
                return new ErrorResult("Resume Not Found");
            }
            Resume.IsDeleted = true;
            await _ResumeRepository.UpdateAsync(Resume);
            return new SuccessResult("Resume removed");
        }

        public async Task<IResult> UpdateAsync(int id, ResumePostDto dto)
        {
            Order orderStatus = Order.None;

            Resume? resume = await _ResumeRepository.GetAsync(x => !x.IsDeleted && x.Id == id, "Category", "City", "Education", "Language", "AppUser", "Skills", "educations","experiences");
            if (resume == null)
            {
                return new ErrorResult("The Resume not found");
            }
            if (dto.EducationStartDates.Count != dto.EducationEndDates.Count ||
              dto.ExperienceStartDates.Count != dto.ExperienceEndDates.Count)
            {
                return new ErrorResult("Invalid date range for education or experience");
            }
            resume.order = Order.None;
            resume.Name = dto.Name;
            resume.Surname = dto.Surname;
            resume.Email = dto.Email;
            resume.PhoneNumber = dto.PhoneNumber;
            resume.Gender = dto.Gender;
            resume.Mode = dto.Mode;
            resume.Status = dto.Status;
            resume.Salary = dto.Salary;
            resume.IsDriverLicense = dto.IsDriverLicense;
            resume.IsPremium = dto.IsPremium;
            resume.CategoryId = dto.CategoryId;
            resume.EducationId = dto.EducationId;
            resume.LanguageId = dto.LanguageId;
            resume.CityId = dto.CityId;
            resume.Experience = dto.Experience;
            resume.Birthday = dto.Birthday;
            resume.EndDate = dto.EndDate;
            resume.educations.Clear();
            resume.experiences.Clear();
            resume.Skills.Clear();

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

                resume.Image = dto.ImageFile.SaveFile(_env.WebRootPath, "assets/images/Resumes");
            }

            AddEducationsFromDto(dto, resume);
            AddExperiencesFromDto(dto, resume);
            AddSkillsFromDto(dto, resume);

            await _ResumeRepository.UpdateAsync(resume);
            return new SuccessResult("Update Resume successfully");
        }


        public async Task  IncreaseCount(int id)
        {
            Resume Resume = await _ResumeRepository.GetAsync(x => !x.IsDeleted && x.Id == id, "Category", "City", "Education", "Language", "AppUser");
            
                Resume.ViewCount++;
                await _ResumeRepository.UpdateAsync(Resume);
        }

        private IQueryable<Resume> GetBaseQuery(int pageNumber=1,int pageSize=2)
        {
            return _ResumeRepository.GetQuery(x => !x.IsDeleted && x.order == Order.Accept)
                .AsNoTrackingWithIdentityResolution()
                  .Include(x => x.AppUser)
                  .Include(x => x.City)
                  .Include(x => x.Education)
                  .Include(x => x.Language)
                  .Include(x => x.experiences)
                  .Include(x => x.Skills)
                   .Include(x => x.educations)
                   .Include(x => x.Category)
                   .Include(x=>x.WishListItems)
                    .ThenInclude(y=>y.Wishlist)
                   .Include(x => x.WishListItems)
                    .ThenInclude(y => y.Wishlist.AppUser);
        }

        private async Task<List<ResumeGetDto>> GetResumeGetDtoListAsync(IQueryable<Resume> query)
        {
            return await query.Select(resume => new ResumeGetDto
            {
                Id = resume.Id,
                Image = resume.Image,
                Name = resume.Name,
                Surname = resume.Surname,
                Email = resume.Email,
                PhoneNumber = resume.PhoneNumber,
                Gender = resume.Gender,
                Mode = resume.Mode,
                Status = resume.Status,
                Salary = resume.Salary,
                order=resume.order,
                Birthday=resume.Birthday,
                IsDriverLicense = resume.IsDriverLicense,
                IsPremium = resume.IsPremium,
                Category = new CategoryGetDto { Id = resume.Category.Id, Name = resume.Category.Name, Image = resume.Category.Image, ParentId = resume.Category.ParentId },
                Education = new EducationGetDto { Id = resume.Education.Id, Name = resume.Education.Name, CreateAt = resume.Education.CreatedAt },
                Language = resume.Language,
                City = new CityGetDto { Id = resume.City.Id, Name = resume.City.Name, CreateAt = resume.City.CreatedAt },
                Experience = resume.Experience,
                CreatedAt = resume.CreatedAt,
                EndDate = resume.EndDate,
                ViewCount = resume.ViewCount,
                educations = resume.educations,
                experiences = resume.experiences,
                Skills = resume.Skills,
                AppUser = resume.AppUser
            }).ToListAsync();
        }
        public async Task<IDataResult<List<ResumeGetDto>>> GetAllForResumePageInWebSiteAsync()
        {
            var query = GetBaseQuery();

            var ResumeGetDtos = await GetResumeGetDtoListAsync(query);

            if (ResumeGetDtos == null)
            {
                return new ErrorDataResult<List<ResumeGetDto>>("Resumes Not Found");
            }

            return new SuccessDataResult<List<ResumeGetDto>>(ResumeGetDtos, "Get Resumes for SITE PAGE");
        }
        public async Task<IDataResult<List<ResumeGetDto>>> SortResumes(int id, ResumeFilterDto dto)
        {

            dto.IsSort = true;
            var filteredResumeGetDtos = await FilterResumes(dto);
            var data = filteredResumeGetDtos.Data;
            switch (id)
            {
                case 1:
                    data = data.OrderByDescending(Resume => Resume.CreatedAt).ToList();
                    break;

                case 2:
                    data = data.OrderBy(Resume => Resume.CreatedAt).ToList();
                    break;
                case 3:
                    data = data.OrderByDescending(Resume => Resume.Salary).ToList();
                    break;
                case 4:
                    data = data.OrderBy(Resume => Resume.Salary).ToList();
                    break;
                default:
                    break;
            }
            return new SuccessDataResult<List<ResumeGetDto>>(data, "Halaldi");
        }
        public async Task<IDataResult<List<ResumeGetDto>>> FilterResumes(ResumeFilterDto dto)
        {
            var query = GetBaseQuery();

            var ResumeGetDtos = await GetResumeGetDtoListAsync(query);

            // Filter by category IDs
            if (dto.CategoriesIds != null && dto.CategoriesIds.Count > 0)
            {
                var activeCategories = _categoryRepository.GetQuery(x => !x.IsDeleted).ToList();
                var categories = activeCategories.Where(cat =>
                    dto.CategoriesIds.Any(categoryId => cat.Id == categoryId)
                );

                ResumeGetDtos = ResumeGetDtos.Where(c => categories.Any(cat => cat.Id == c.Category.Id)).ToList();

            }

            // Filter by education IDs
            if (dto.EducationsIds != null && dto.EducationsIds.Count > 0)
            {
                var activeEducations = _educationRepository.GetQuery(x => !x.IsDeleted).ToList();
                var educations = activeEducations.Where(edu =>
                    dto.EducationsIds.Any(eduId => edu.Id == eduId)
                );

                ResumeGetDtos = ResumeGetDtos.Where(c => educations.Any(edu => edu.Id == c.Education.Id)).ToList();

            }

            // Filter by language IDs
            if (dto.LanguagesIds != null && dto.LanguagesIds.Count > 0)
            {
                var activeLanguages = _languageRepository.GetQuery(x => !x.IsDeleted).ToList();
                var languages = activeLanguages.Where(lan =>
                    dto.LanguagesIds.Any(lanId => lan.Id == lanId)
                );

                ResumeGetDtos = ResumeGetDtos.Where(c => languages.Any(lan => lan.Id == c.Language.Id)).ToList();
            }
            // Filter by minimum salary
            if (dto.MinSalary > 100)
            {
                ResumeGetDtos = ResumeGetDtos.Where(Resume => Resume.Salary >= dto.MinSalary).ToList();
            }

            // Filter by maximum salary
            if (dto.MaxSalary < 10001)
            {
                ResumeGetDtos = ResumeGetDtos.Where(Resume => Resume.Salary <= dto.MaxSalary).ToList();
            }

            // Filter by minimum experience
            if (dto.MinExperience > 0)
            {
                ResumeGetDtos = ResumeGetDtos.Where(Resume => Resume.Experience >= dto.MinExperience).ToList();
            }
            // Filter by maximum experience
            if (dto.MaxExperience < 7)
            {
                ResumeGetDtos = ResumeGetDtos.Where(Resume => Resume.Experience <= dto.MaxExperience).ToList();
            }
           
            // Filter by certificate availability
            if (dto.IsDriverLicense)
            {
                ResumeGetDtos = ResumeGetDtos.Where(Resume => Resume.IsDriverLicense).ToList();
            }
            // Filter by selected job mode
            if (dto.Mode != JobMode.None)
            {
                ResumeGetDtos = ResumeGetDtos.Where(Resume => Resume.Mode == dto.Mode).ToList();
            }

            // Filter by selected marital status
            if (dto.Status != MaritalStatus.None)
            {
                ResumeGetDtos = ResumeGetDtos.Where(Resume => Resume.Status == dto.Status).ToList();
            }

            // Filter by selected gender
            if (dto.Gender != Gender.None)
            {
                ResumeGetDtos = ResumeGetDtos.Where(Resume => Resume.Gender == dto.Gender).ToList();
            }
            return new SuccessDataResult<List<ResumeGetDto>>(ResumeGetDtos, "Halaldi sene ");

        }
        public async Task<IDataResult<List<ResumeGetDto>>> LoadMoreResumesAsync(int id, ResumeFilterDto dto,int pageNumber, int pageSize)
        {
            var sortedResumesResult = await SortResumes(id, dto);
            if (!sortedResumesResult.Success)
            {
                return new ErrorDataResult<List<ResumeGetDto>>("Error occurred while sorting resumes");
            }
            var sortedResumes = sortedResumesResult.Data;

            if (pageNumber == 1)
            {
                var firstPageResumes = sortedResumes.Take(2).ToList();
                return new SuccessDataResult<List<ResumeGetDto>>(firstPageResumes, "Loaded initial resumes");
            }

            var resumesToReturn = sortedResumes
                                    .Skip((pageNumber - 1) * pageSize) 
                                    .Take(pageSize) 
                                    .ToList();

            if (!resumesToReturn.Any())
            {
                return new ErrorDataResult<List<ResumeGetDto>>("No more resumes available");
            }

            return new SuccessDataResult<List<ResumeGetDto>>(resumesToReturn, $"Loaded resumes for page {pageNumber}");
        }

        public async Task<IResult> SetOrderStatus(int resumeId, Order orderStatus)
        {
            var resume = await _ResumeRepository.GetAsync(x => !x.IsDeleted && x.Id == resumeId, "Category", "City", "Education", "Language", "AppUser");
            if (resume == null)
            {
                return new ErrorResult("Resume not found");
            }

            resume.order = orderStatus;
            await _ResumeRepository.UpdateAsync(resume);

            return new SuccessResult("Order status updated successfully");
        }

    }
}
