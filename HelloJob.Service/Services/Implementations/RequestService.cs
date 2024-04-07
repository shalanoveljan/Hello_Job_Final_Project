using AutoMapper;
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
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Service.Services.Implementations
{
    public class RequestService : IRequestService
    {
        readonly IRequestDAL _RequestRepository;

        public RequestService(IRequestDAL RequestRepository)
        {
            _RequestRepository = RequestRepository;
        }
   


        public async Task<HelloJob.Core.Utilities.Results.Abstract.IResult> CreateAsync(RequestPostDto dto)
        {
            Request Request = new Request
            {
                VacancyId = dto.VacancyId,
                ResumeId = dto.SelectedResumeId,
                AppUserId = dto.AppUserId,
            };

            if (Request == null)
            {
                return new ErrorResult("Request is null");
            }

            {
                var existingRequest = await _RequestRepository.GetByVacancyIdAndRequestId(dto.VacancyId, dto.AppUserId);

                if (existingRequest != null)   return new ErrorResult("A request for this vacancy by this resume already exists.");
            }

            await _RequestRepository.AddAsync(Request);

            return new SuccessResult("Create Request successfully");

        }


        public async Task<PagginatedResponse<RequestGetDto>> GetAllAsync(string userid, int pageNumber = 1, int pageSize = 6)
        {
            var query = _RequestRepository.GetQuery(x => !x.IsDeleted && x.Vacancy.Company.AppUserId == userid);
            var totalCount = await query.CountAsync();

            var paginatedRequests = await query
                 .AsNoTrackingWithIdentityResolution()
                 .Include(x=>x.AppUser)
                 .Include(x => x.Resume.AppUser)
                .Include(x => x.Resume.City)
                .Include(x => x.Resume.Category)
                .Include(x => x.Vacancy.Company)
                .ThenInclude(x => x.AppUser)
                .Include(x => x.Vacancy.City)
                 .Include(x => x.Vacancy.abouts)
                .Include(x => x.Vacancy.Category)
                .Include(x => x.Vacancy.WishListItems)
                    .ThenInclude(y => y.Wishlist)
                   .Include(x => x.Vacancy.WishListItems)
                    .ThenInclude(y => y.Wishlist.AppUser)
                .ToPagedListAsync(pageNumber, pageSize);

            var RequestGetDtos = paginatedRequests.Datas.Select(x =>
      new RequestGetDto
      {
          Id = x.Id,
          CreatedAt = x.CreatedAt,
          Resume = new ResumeGetDto
          {
              Id = x.Resume.Id,
              Image = x.Resume.Image,
              Name = x.Resume.Name,
              Surname = x.Resume.Surname,
              Email = x.Resume.Email,
              PhoneNumber = x.Resume.PhoneNumber,
              Gender = x.Resume.Gender,
              Mode = x.Resume.Mode,
              Status = x.Resume.Status,
              Salary = x.Resume.Salary,
              IsPremium = x.Resume.IsPremium,
              Experience = x.Resume.Experience,
              CreatedAt = x.CreatedAt,
              EndDate = x.Resume.EndDate,
              order = x.Resume.order,
              ViewCount = x.Resume.ViewCount,
              Category = new CategoryGetDto
              {
                  Id = x.Resume.Category.Id,
                  Name = x.Resume.Category.Name,
                  Image = x.Resume.Category.Image,
                  ParentId = x.Resume.Category.ParentId
              },
              AppUser = x.Resume.AppUser,
          },

          Vacancy = new VacancyGetDto
          {
              Id = x.Vacancy.Id,
              Mode = x.Vacancy.Mode,
              Salary = x.Vacancy.Salary,
              Position = x.Vacancy.Position,
              Required_Experience = x.Vacancy.Required_Experience,
              IsPremium = x.Vacancy.IsPremium,
              CreatedAt = x.Vacancy.CreatedAt,
              EndDate = x.Vacancy.EndDate,
              order = x.Vacancy.order,
              ViewCount = x.Vacancy.ViewCount,
              abouts = x.Vacancy.abouts,
              Company = new CompanyGetDto
              {
                  Id = x.Vacancy.Company.Id,
                  About = x.Vacancy.Company.About,
                  Email = x.Vacancy.Company.Email,
                  Image = x.Vacancy.Company.Image,
                  Name = x.Vacancy.Company.Name,
                  WebsiteUrlLink = x.Vacancy.Company.WebsiteUrlLink,
                  order = x.Vacancy.Company.order,
                  AppUser = x.Vacancy.Company.AppUser
              },
              Category = new CategoryGetDto
              {
                  Id = x.Vacancy.Category.Id,
                  Name = x.Vacancy.Category.Name,
                  Image = x.Vacancy.Category.Image,
                  ParentId = x.Vacancy.Category.ParentId
              },
              City = new CityGetDto
              {
                  Id = x.Vacancy.City.Id,
                  Name = x.Vacancy.City.Name,
                  CreateAt = x.Vacancy.City.CreatedAt
              },
          }
      }).ToList();
            var pagginatedResponse = new PagginatedResponse<RequestGetDto>(RequestGetDtos, paginatedRequests.PageNumber,
              paginatedRequests.PageSize,
              totalCount);
            return pagginatedResponse;
        }


        public async Task<Core.Utilities.Results.Abstract.IResult> RemoveAsync(int id)
        {
            Request? Request = await _RequestRepository.GetAsync(x => !x.IsDeleted && x.Id == id, "Resume.AppUser", "Resume.City", "Resume.Category", "Vacancy.Company.AppUser", "Vacancy.City", "Vacancy.Category", "Vacancy.abouts", "Vacancy.WishListItems.Wishlist.AppUser","AppUser");

            if (Request == null)
            {
                return new ErrorResult("Request Not Found");
            }
            Request.IsDeleted = true;
            await _RequestRepository.UpdateAsync(Request);
            return new SuccessResult("Request removed");
        }
    }
}
