using HelloJob.Core.Helper;
using HelloJob.Core.Utilities.Results.Abstract;
using HelloJob.Core.Utilities.Results.Concrete.ErrorResults;
using HelloJob.Core.Utilities.Results.Concrete.SuccessResults;
using HelloJob.Data.DAL.Interfaces;
using HelloJob.Entities.DTOS;
using HelloJob.Entities.Enums;
using HelloJob.Entities.Models;
using HelloJob.Service.Extensions;
using HelloJob.Service.Responses;
using HelloJob.Service.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Service.Services.Implementations
{

    public class VacancyService : IVacancyService
    {
      readonly IWebHostEnvironment _env;
        readonly IVacancyDAL _VacancyRepository;

        public VacancyService(IWebHostEnvironment env, IVacancyDAL vacancyRepository)
        {
            _env = env;
            _VacancyRepository = vacancyRepository;
        }
        private void AddAboutsFromDto(VacancyPostDto dto, Vacancy Vacancy)
        {
            foreach (var con in dto.Conditions)
            {
                About_Vacancy av = new About_Vacancy
                {
                    Condition = con,
                    Vacancy = Vacancy
                };
                Vacancy.abouts.Add(av);
            }
        }
        public async Task<IResult> CreateAsync(VacancyPostDto dto)
        {
            Order orderStatus = Order.None;

           

            Vacancy Vacancy = new Vacancy
            {
               CompanyId= dto.CompanyId,
                Position = dto.Position,
                CityId = dto.CityId,
                Mode= dto.Mode,
                IsPremium= dto.IsPremium,
                Required_Experience= dto.Required_Experience,
                Salary= dto.Salary,
                CategoryId= dto.CategoryId,
                //RequestId= dto.RequestId,
                EndDate = dto.EndDate,
                abouts = new List<About_Vacancy>()
            };
            Vacancy.order = orderStatus;


            AddAboutsFromDto(dto, Vacancy);
            await _VacancyRepository.AddAsync(Vacancy);
            return new SuccessResult("Create Vacancy successfully");
        }
        public async Task<PagginatedResponse<VacancyGetDto>> GetAllAsync(string userId, bool isAdmin, int pageNumber = 1, int pageSize = 6)
        { 
            IQueryable<Vacancy> query;

            if (isAdmin)
            {
                query = _VacancyRepository.GetQuery(x => !x.IsDeleted);
            }
            else
            {
                query = _VacancyRepository.GetQuery(x => x.Company.AppUserId == userId && !x.IsDeleted && x.order != Order.Reject);
            }
            var totalCount = await query.CountAsync();

            var paginatedVacancys = await query
                .AsNoTrackingWithIdentityResolution()
                .Include(x => x.Company)
                .ThenInclude(x => x.AppUser)
                .Include(x => x.City)
                //.Include(x => x.Request)
                 .Include(x => x.abouts)
                .Include(x => x.Category)
                .Include(x => x.WishListItems)
                    .ThenInclude(y => y.Wishlist)
                   .Include(x => x.WishListItems)
                    .ThenInclude(y => y.Wishlist.AppUser)
                .ToPagedListAsync(pageNumber, pageSize);

            var VacancyGetDtos = paginatedVacancys.Datas.Select(x =>
                new VacancyGetDto
                {
                    Id = x.Id,
                    Mode = x.Mode,
                    Salary = x.Salary,
                    Position = x.Position,
                    Required_Experience = x.Required_Experience,
                    IsPremium = x.IsPremium,
                    CreatedAt = x.CreatedAt,
                    EndDate = x.EndDate,
                    order = x.order,
                    ViewCount = x.ViewCount,
                    abouts = x.abouts,
                    Company = new CompanyGetDto { Id = x.Company.Id, About = x.Company.About, Email = x.Company.Email, Image = x.Company.Image, Name = x.Company.Name, WebsiteUrlLink = x.Company.WebsiteUrlLink, order = x.Company.order, AppUser = x.Company.AppUser },
                    Category = new CategoryGetDto { Id = x.Category.Id, Name = x.Category.Name, Image = x.Category.Image, ParentId = x.Category.ParentId },
                    City = new CityGetDto { Id = x.City.Id, Name = x.City.Name, CreateAt = x.City.CreatedAt },
                }).ToList();

            var pagginatedResponse = new PagginatedResponse<VacancyGetDto>(
                VacancyGetDtos, paginatedVacancys.PageNumber,
                paginatedVacancys.PageSize,
                totalCount);

            return pagginatedResponse;
        }
        public async Task<PagginatedResponse<VacancyGetDto>> GetAllDefaultSearchAsync(int pageNumber = 1, int pageSize = 6)
        {
            IQueryable<Vacancy> query;

                query = _VacancyRepository.GetQuery(x =>!x.IsDeleted && x.order == Order.Accept && x.EndDate>DateTime.Now);
          
            var totalCount = await query.CountAsync();

            var paginatedVacancys = await query
                .AsNoTrackingWithIdentityResolution()
                .Include(x => x.Company)
                .ThenInclude(x => x.AppUser)
                .Include(x => x.City)
                 .Include(x => x.abouts)
                .Include(x => x.Category)
                .Include(x => x.WishListItems)
                    .ThenInclude(y => y.Wishlist)
                   .Include(x => x.WishListItems)
                    .ThenInclude(y => y.Wishlist.AppUser)
                .ToPagedListAsync(pageNumber, pageSize);

            var VacancyGetDtos = paginatedVacancys.Datas.Select(x =>
                new VacancyGetDto
                {
                    Id = x.Id,
                    Mode = x.Mode,
                    Salary = x.Salary,
                    Position = x.Position,
                    Required_Experience = x.Required_Experience,
                    IsPremium = x.IsPremium,
                    CreatedAt = x.CreatedAt,
                    EndDate = x.EndDate,
                    order = x.order,
                    ViewCount = x.ViewCount,
                    abouts = x.abouts,
                    Company = new CompanyGetDto { Id = x.Company.Id, About = x.Company.About, Email = x.Company.Email, Image = x.Company.Image, Name = x.Company.Name, WebsiteUrlLink = x.Company.WebsiteUrlLink, order = x.Company.order, AppUser = x.Company.AppUser },
                    Category = new CategoryGetDto { Id = x.Category.Id, Name = x.Category.Name, Image = x.Category.Image, ParentId = x.Category.ParentId },
                    City = new CityGetDto { Id = x.City.Id, Name = x.City.Name, CreateAt = x.City.CreatedAt },
                }).ToList();

            var pagginatedResponse = new PagginatedResponse<VacancyGetDto>(
                VacancyGetDtos, paginatedVacancys.PageNumber,
                paginatedVacancys.PageSize,
                totalCount);

            return pagginatedResponse;
        }
        public async Task<PagginatedResponse<VacancyGetDto>> GetVacancysBySearchTextAsync(string searchText, int pageNumber=1, int pageSize=6)
        {
            if (string.IsNullOrEmpty(searchText))
            {
                return await GetAllDefaultSearchAsync(pageNumber,pageSize);
            }
            IQueryable<Vacancy> query;

            query = _VacancyRepository.GetQuery(x => !x.IsDeleted && x.order == Order.Accept && x.EndDate > DateTime.Now)
                                     .Where(m => m.Position.ToLower().Contains(searchText.ToLower()));

            var totalCount = await query.CountAsync();

            var paginatedVacancys = await query
                .AsNoTrackingWithIdentityResolution()
                .Include(x => x.Company)
                .ThenInclude(x => x.AppUser)
                .Include(x => x.City)
                 .Include(x => x.abouts)
                .Include(x => x.Category)
                .Include(x => x.WishListItems)
                    .ThenInclude(y => y.Wishlist)
                   .Include(x => x.WishListItems)
                    .ThenInclude(y => y.Wishlist.AppUser)
                .ToPagedListAsync(pageNumber, pageSize);

            var VacancyGetDtos = paginatedVacancys.Datas.Select(x =>
              new VacancyGetDto
              {
                  Id = x.Id,
                  Mode = x.Mode,
                  Salary = x.Salary,
                  Position = x.Position,
                  Required_Experience = x.Required_Experience,
                  IsPremium = x.IsPremium,
                  CreatedAt = x.CreatedAt,
                  EndDate = x.EndDate,
                  order = x.order,
                  ViewCount = x.ViewCount,
                  abouts = x.abouts,
                  Company = new CompanyGetDto { Id = x.Company.Id, About = x.Company.About, Email = x.Company.Email, Image = x.Company.Image, Name = x.Company.Name, WebsiteUrlLink = x.Company.WebsiteUrlLink, order = x.Company.order, AppUser = x.Company.AppUser },
                  Category = new CategoryGetDto { Id = x.Category.Id, Name = x.Category.Name, Image = x.Category.Image, ParentId = x.Category.ParentId },
                  City = new CityGetDto { Id = x.City.Id, Name = x.City.Name, CreateAt = x.City.CreatedAt },
              }).ToList();

            var pagginatedResponse = new PagginatedResponse<VacancyGetDto>(
                VacancyGetDtos, paginatedVacancys.PageNumber,
                paginatedVacancys.PageSize,
                totalCount);

            return pagginatedResponse;
        }
        public async Task<IDataResult<VacancyGetDto>> GetAsync(int id)
        {
            var Vacancy = await _VacancyRepository.GetQuery(x => x.Id == id && !x.IsDeleted)
                                           .AsNoTrackingWithIdentityResolution()
                                            .Include(x => x.Company)
                                            .ThenInclude(x => x.AppUser)
                                            .Include(x => x.City)
                                             .Include(x => x.abouts)
                                            .Include(x => x.Category)
                                         .Include(x => x.WishListItems)
                                        .ThenInclude(y => y.Wishlist)
                                       .Include(x => x.WishListItems)
                                        .ThenInclude(y => y.Wishlist.AppUser)
                                          .FirstOrDefaultAsync();
            if (Vacancy == null)
            {
                return new ErrorDataResult<VacancyGetDto>("Vacancy Not Found");
            }
            var VacancyGetDto = new VacancyGetDto
            {
                Id = Vacancy.Id,
                Position=Vacancy.Position,
                Required_Experience=Vacancy.Required_Experience,
                Mode = Vacancy.Mode,
                Salary = Vacancy.Salary,
                IsPremium = Vacancy.IsPremium,
                CreatedAt = Vacancy.CreatedAt,
                EndDate = Vacancy.EndDate,
                order = Vacancy.order,
                ViewCount = Vacancy.ViewCount,
                abouts = Vacancy.abouts,
                Company = new CompanyGetDto { Id = Vacancy.Company.Id, About = Vacancy.Company.About, Email = Vacancy.Company.Email, Image = Vacancy.Company.Image, Name = Vacancy.Company.Name, WebsiteUrlLink = Vacancy.Company.WebsiteUrlLink, order = Vacancy.Company.order, AppUser = Vacancy.Company.AppUser },
                Category = new CategoryGetDto { Id = Vacancy.Category.Id, Name = Vacancy.Category.Name, Image = Vacancy.Category.Image, ParentId = Vacancy.Category.ParentId },
                City = new CityGetDto { Id = Vacancy.City.Id, Name = Vacancy.City.Name, CreateAt = Vacancy.City.CreatedAt },
                
            };
            return new SuccessDataResult<VacancyGetDto>(VacancyGetDto, "Get Vacancy");
        }
        private IQueryable<Vacancy> GetBaseQuery()
        {
            return _VacancyRepository.GetQuery(x => !x.IsDeleted && x.order == Order.Accept)
                             .AsNoTrackingWithIdentityResolution()
                                           .Include(x => x.Company)
                                            .ThenInclude(x => x.AppUser)
                                            .Include(x => x.City)
                                             .Include(x => x.abouts)
                                            .Include(x => x.Category)
                                            .Include(x => x.WishListItems)
                                                .ThenInclude(y => y.Wishlist)
                                               .Include(x => x.WishListItems)
                                                .ThenInclude(y => y.Wishlist.AppUser);
        }

        private async Task<List<VacancyGetDto>> GetCompanyGetDtoListAsync(IQueryable<Vacancy> query)
        {
            return await query.Select(Vacancy => new VacancyGetDto
            {
                Id = Vacancy.Id,
                Position = Vacancy.Position,
                Required_Experience = Vacancy.Required_Experience,
                Mode = Vacancy.Mode,
                Salary = Vacancy.Salary,
                IsPremium = Vacancy.IsPremium,
                CreatedAt = Vacancy.CreatedAt,
                EndDate = Vacancy.EndDate,
                order = Vacancy.order,
                ViewCount = Vacancy.ViewCount,
                abouts = Vacancy.abouts,
                Company = new CompanyGetDto { Id = Vacancy.Company.Id, About = Vacancy.Company.About, Email = Vacancy.Company.Email, Image = Vacancy.Company.Image, Name = Vacancy.Company.Name, WebsiteUrlLink = Vacancy.Company.WebsiteUrlLink, order = Vacancy.Company.order, AppUser = Vacancy.Company.AppUser },
                Category = new CategoryGetDto { Id = Vacancy.Category.Id, Name = Vacancy.Category.Name, Image = Vacancy.Category.Image, ParentId = Vacancy.Category.ParentId },
                City = new CityGetDto { Id = Vacancy.City.Id, Name = Vacancy.City.Name, CreateAt = Vacancy.City.CreatedAt },

            }).ToListAsync();
        }
        public async Task<IDataResult<List<VacancyGetDto>>> GetAllForVacancyPageInWebSiteAsync()
        {
            var query = GetBaseQuery();

            var VacanciesGetDtos = await GetCompanyGetDtoListAsync(query);

            if (VacanciesGetDtos == null)
            {
                return new ErrorDataResult<List<VacancyGetDto>>("Companys Not Found");
            }

            return new SuccessDataResult<List<VacancyGetDto>>(VacanciesGetDtos, "Get Companys for SITE PAGE");
        }


        public async Task IncreaseCount(int id)
        {
            Vacancy Vacancy = await _VacancyRepository.GetAsync(x => !x.IsDeleted && x.Id == id, "Category", "City", "Company.AppUser", "abouts");

            Vacancy.ViewCount++;
            await _VacancyRepository.UpdateAsync(Vacancy);
        }

        public async Task<IResult> RemoveAsync(int id)
        {
            Vacancy? Vacancy = await _VacancyRepository.GetAsync(x => !x.IsDeleted && x.Id == id, "Category", "City", "Company.AppUser", "abouts");

            if (Vacancy == null)
            {
                return new ErrorResult("Vacancy Not Found");
            }
            Vacancy.IsDeleted = true;
            await _VacancyRepository.UpdateAsync(Vacancy);
            return new SuccessResult("Vacancy removed");
        }

        public async Task<IResult> SetOrderStatus(int VacancyId, Order orderStatus)
        {
            var Vacancy = await _VacancyRepository.GetAsync(x => !x.IsDeleted && x.Id == VacancyId, "Category", "City", "Company.AppUser");
            if (Vacancy == null)
            {
                return new ErrorResult("Vacancy not found");
            }

            Vacancy.order = orderStatus;
            await _VacancyRepository.UpdateAsync(Vacancy);

            return new SuccessResult("Order status updated successfully");
        }

        public async Task<IResult> UpdateAsync(int id, VacancyPostDto dto)
        {
            Order orderStatus = Order.None;

            Vacancy? Vacancy = await _VacancyRepository.GetAsync(x => !x.IsDeleted && x.Id == id, "Category", "City", "Company.AppUser", "abouts");
            if (Vacancy == null)
            {
                return new ErrorResult("The Vacancy not found");
            }
          
            Vacancy.order = Order.None;
            Vacancy.CompanyId = dto.CompanyId;
            Vacancy.Position = dto.Position;
            Vacancy.CityId = dto.CityId;
            Vacancy.Mode = dto.Mode;
            Vacancy.IsPremium = dto.IsPremium;
            Vacancy.Required_Experience = dto.Required_Experience;
            Vacancy.Salary = dto.Salary;
            Vacancy.CategoryId = dto.CategoryId;
            Vacancy.EndDate = dto.EndDate;
            Vacancy.abouts.Clear();

            AddAboutsFromDto(dto, Vacancy);

            await _VacancyRepository.UpdateAsync(Vacancy);
            return new SuccessResult("Update Vacancy successfully");
        }
    }
}
