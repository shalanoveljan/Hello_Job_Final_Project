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
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HelloJob.Service.Services.Implementations
{
    public class CompanyService : ICompanyService
    {
        readonly IWebHostEnvironment _env;
        readonly ICompanyDAL _CompanyRepository;

        public CompanyService(IWebHostEnvironment env, ICompanyDAL companyRepository)
        {
            _env = env;
            _CompanyRepository = companyRepository;
        }

        public async Task<IResult> CreateAsync(CompanyPostDto dto)
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
            Company Company = new Company
            {
                AppUserId = dto.AppUserId,
                Name = dto.Name,
                Email = dto.Email,
                WebsiteUrlLink= dto.WebsiteUrlLink,
                About=dto.About,
            };
            Company.order = orderStatus;

            Company.Image = dto.ImageFile.SaveFile(_env.WebRootPath, "assets/images/Companies");

            await _CompanyRepository.AddAsync(Company);
            return new SuccessResult("Create Company successfully");
        }

        public async Task<PagginatedResponse<CompanyGetDto>> GetAllAsync(string userId, bool isAdmin, int pageNumber = 1, int pageSize = 6)
        {
            IQueryable<Company> query;

            if (isAdmin)
            {
                query = _CompanyRepository.GetQuery(x => !x.IsDeleted);
            }
            else
            {
                query = _CompanyRepository.GetQuery(x => x.AppUserId == userId && !x.IsDeleted && x.order != Order.Reject);
            }
            var totalCount = await query.CountAsync();

            var paginatedCompanys = await query
                .AsNoTrackingWithIdentityResolution()
                .Include(x => x.AppUser)
                .ToPagedListAsync(pageNumber, pageSize);

            var CompanyGetDtos = paginatedCompanys.Datas.Select(x =>
                new CompanyGetDto
                {
                    Id = x.Id,
                    Image = x.Image,
                    Name = x.Name,
                    Email = x.Email,
                    order = x.order,
                    AppUser = x.AppUser,
                    WebsiteUrlLink= x.WebsiteUrlLink,
                    About= x.About,
                }).ToList();

            var pagginatedResponse = new PagginatedResponse<CompanyGetDto>(
                CompanyGetDtos, paginatedCompanys.PageNumber,
                paginatedCompanys.PageSize,
                totalCount);

            return pagginatedResponse;

    }

        public async Task<PagginatedResponse<CompanyGetDto>> GetAllCheckingOrderAsync(int pageNumber = 1, int pageSize = 6)
        {
            IQueryable<Company> query;

                query = _CompanyRepository.GetQuery(x => !x.IsDeleted && x.order == Order.Accept);
            var totalCount = await query.CountAsync();

            var paginatedCompanys = await query
                .AsNoTrackingWithIdentityResolution()
                .Include(x => x.AppUser)
                .ToPagedListAsync(pageNumber, pageSize);

            var CompanyGetDtos = paginatedCompanys.Datas.Select(x =>
                new CompanyGetDto
                {
                    Id = x.Id,
                    Image = x.Image,
                    Name = x.Name,
                    Email = x.Email,
                    order = x.order,
                    AppUser = x.AppUser,
                    WebsiteUrlLink = x.WebsiteUrlLink,
                    About = x.About,
                }).ToList();

            var pagginatedResponse = new PagginatedResponse<CompanyGetDto>(
                CompanyGetDtos, paginatedCompanys.PageNumber,
                paginatedCompanys.PageSize,
                totalCount);

            return pagginatedResponse;

        }

        private IQueryable<Company> GetBaseQuery()
        {
            return _CompanyRepository.GetQuery(x => !x.IsDeleted && x.order == Order.Accept)/**/
                .AsNoTrackingWithIdentityResolution()
                  .Include(x => x.AppUser);
        }

        private async Task<List<CompanyGetDto>> GetCompanyGetDtoListAsync(IQueryable<Company> query)
        {
            return await query.Select(Company => new CompanyGetDto
            {
                Id = Company.Id,
                Image = Company.Image,
                Name = Company.Name,
                Email = Company.Email,
                AppUser = Company.AppUser,
                WebsiteUrlLink= Company.WebsiteUrlLink,
                About = Company.About,
                order=Company.order
            }).ToListAsync();
        }
        public async Task<IDataResult<List<CompanyGetDto>>> GetAllForCompanyPageInWebSiteAsync()
        {
            var query = GetBaseQuery();

            var CompanyGetDtos = await GetCompanyGetDtoListAsync(query);

            if (CompanyGetDtos == null)
            {
                return new ErrorDataResult<List<CompanyGetDto>>("Companys Not Found");
            }

            return new SuccessDataResult<List<CompanyGetDto>>(CompanyGetDtos, "Get Companys for SITE PAGE");
        }

        public async Task<IDataResult<CompanyGetDto>> GetAsync(int id)
        {
            var Company = await _CompanyRepository.GetQuery(x => x.Id == id && !x.IsDeleted)
                                       .AsNoTrackingWithIdentityResolution()
                                       .Include(x => x.AppUser)
                                       .FirstOrDefaultAsync();
            if (Company == null)
            {
                return new ErrorDataResult<CompanyGetDto>("Company Not Found");
            }
            var CompanyGetDto = new CompanyGetDto
            {
                Id = Company.Id,
                Image = Company.Image,
                Name = Company.Name,
                Email = Company.Email,
                order = Company.order,
                WebsiteUrlLink= Company.WebsiteUrlLink,
                About= Company.About,
                AppUser = Company.AppUser
            };
            return new SuccessDataResult<CompanyGetDto>(CompanyGetDto, "Get Company");
        }

        public async Task<IResult> RemoveAsync(int id)
        {
            Company? Company = await _CompanyRepository.GetAsync(x => !x.IsDeleted && x.Id == id, "AppUser");

            if (Company == null)
            {
                return new ErrorResult("Company Not Found");
            }
            Company.IsDeleted = true;
            await _CompanyRepository.UpdateAsync(Company);
            return new SuccessResult("Company removed");
        }

        public async Task<IResult> SetOrderStatus(int CompanyId, Order orderStatus)
        {
            var Company = await _CompanyRepository.GetAsync(x => !x.IsDeleted && x.Id == CompanyId, "AppUser");
            if (Company == null)
            {
                return new ErrorResult("Company not found");
            }

            Company.order = orderStatus;
            await _CompanyRepository.UpdateAsync(Company);

            return new SuccessResult("Order status updated successfully");
        }

        public async Task<IResult> UpdateAsync(int id, CompanyPostDto dto)
        {
            Order orderStatus = Order.None;

            Company? Company = await _CompanyRepository.GetAsync(x => !x.IsDeleted && x.Id == id, "AppUser");
            if (Company == null)
            {
                return new ErrorResult("The Company not found");
            }
           
            Company.order = Order.None;
            Company.Name = dto.Name;
            Company.Email = dto.Email;
            Company.WebsiteUrlLink= dto.WebsiteUrlLink;
            Company.About= dto.About;

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

                Company.Image = dto.ImageFile.SaveFile(_env.WebRootPath, "assets/images/Companies");
            }
            await _CompanyRepository.UpdateAsync(Company);
            return new SuccessResult("Update Company successfully");
        }
    }
}
