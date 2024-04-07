using HelloJob.Core.Utilities.Results.Concrete;
using HelloJob.Data.DAL.Implementations;
using HelloJob.Data.DAL.Interfaces;
using HelloJob.Data.DBContexts.SQLSERVER;
using HelloJob.Entities.DTOS;
using HelloJob.Entities.Models;
using HelloJob.Service.Services.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using MimeKit.Encodings;
using Newtonsoft.Json;
using System;

namespace HelloJob.Service.Services.Implementations
{
    public class LikedService : ILikedService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IWishlistDAL _wishlistRepository;
        private readonly IWishlistItemDAL _wishlistItemRepository;
        private readonly IResumeDAL _resumeRepository;
        readonly IResumeService _resumeService;
        readonly IVacancyService _VacancyService;
        readonly IHttpContextAccessor _http;


        public LikedService(UserManager<AppUser> userManager, IResumeDAL resumeRepository, IHttpContextAccessor http, IWishlistDAL wishlistRepository, IWishlistItemDAL wishlistItemRepository, IResumeService resumeService, IVacancyService vacancyService)
        {
            _userManager = userManager;
            _resumeRepository = resumeRepository;
            _http = http;
            _wishlistRepository = wishlistRepository;
            _wishlistItemRepository = wishlistItemRepository;
            _resumeService = resumeService;
            _VacancyService = vacancyService;
        }
        public async Task AddToWishlist(int itemId, string itemType)
        {
            if (_http.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUser appUser = await _userManager.FindByNameAsync(_http.HttpContext.User.Identity.Name);
                var wishlist = await _wishlistRepository.GetAsync(x => !x.IsDeleted && x.AppUserId == appUser.Id, "WishListItems");

                if (wishlist != null)
                {
                    if (itemType == "resume")
                    {
                        var wishlistItem = wishlist.WishListItems.FirstOrDefault(x => !x.IsDeleted && x.ResumeId == itemId);
                        if (wishlistItem != null)
                        {
                            wishlistItem.IsLiked = false;
                            wishlist.WishListItems.Remove(wishlistItem);
                        }
                        else
                        {
                            wishlistItem = new WishlistItem
                            {
                                ResumeId = itemId,
                                Wishlist = wishlist,
                                IsLiked = true
                            };
                            wishlist.WishListItems.Add(wishlistItem);
                        }
                    }
                    else if (itemType == "vacancy")
                    {
                        var wishlistItem = wishlist.WishListItems.FirstOrDefault(x => !x.IsDeleted && x.VacancyId == itemId);
                        if (wishlistItem != null)
                        {
                            wishlistItem.IsLiked = false;
                            wishlist.WishListItems.Remove(wishlistItem);
                        }
                        else
                        {
                            wishlistItem = new WishlistItem
                            {
                                VacancyId = itemId,
                                Wishlist = wishlist,
                                IsLiked = true
                            };
                            wishlist.WishListItems.Add(wishlistItem);
                        }
                    }
                    await _wishlistRepository.UpdateAsync(wishlist);
                }
                else
                {
                    wishlist = new Wishlist { AppUserId = appUser.Id };
                    await _wishlistRepository.AddAsync(wishlist);

                    var item = new WishlistItem();
                    if (itemType == "resume")
                    {
                        item = new WishlistItem
                        {
                            ResumeId = itemId,
                            Wishlist = wishlist,
                            IsLiked = true
                        };
                    }
                    else if (itemType == "vacancy")
                    {
                        item = new WishlistItem
                        {
                            VacancyId = itemId,
                            Wishlist = wishlist,
                            IsLiked = true
                        };
                    }
                    await _wishlistItemRepository.AddAsync(item);
                    await _wishlistRepository.SaveChangesAsync();
                }
            }
            else
            {
                List<WishlistPostDto> WishlistDtos = new List<WishlistPostDto>();
                var wishlistJson = _http.HttpContext.Request.Cookies["wishlist"];

                if (wishlistJson == null)
                {
                    WishlistPostDto wishlistitem = new WishlistPostDto()
                    {
                        Id = itemId,
                        IsLiked = true,
                    };
                    WishlistDtos.Add(wishlistitem);
                }
                else
                {
                    WishlistDtos = JsonConvert.DeserializeObject<List<WishlistPostDto>>(wishlistJson);
                    var wishlistdto = WishlistDtos.FirstOrDefault(x => x.Id == itemId);

                    if (wishlistdto == null)
                    {
                        wishlistdto = new WishlistPostDto()
                        {
                            Id = itemId,
                            IsLiked = true,
                        };
                        WishlistDtos.Add(wishlistdto);
                    }
                    else
                    {
                        wishlistdto.IsLiked = false;
                        WishlistDtos.Remove(wishlistdto);
                    }
                }

                wishlistJson = JsonConvert.SerializeObject(WishlistDtos);
                _http.HttpContext.Response.Cookies.Append("wishlist", wishlistJson);
            }
        }

        public async Task<WishlistGetDto> GetWishList()
        {
            WishlistGetDto wishlistGetDto = new WishlistGetDto();

            if (_http.HttpContext.User.Identity.IsAuthenticated)
            {
                AppUser appUser = await _userManager.FindByNameAsync(_http.HttpContext.User.Identity.Name);
                var wishlistQuery = await _wishlistRepository.GetQuery(x => !x.IsDeleted && x.AppUserId == appUser.Id)
                    .Include(x => x.WishListItems)
                    .ThenInclude(c => c.Resume)
                     .Include(x => x.WishListItems)
                    .ThenInclude(c => c.Resume)
                        .ThenInclude(v => v.Category)
                .Include(x => x.WishListItems)
                    .ThenInclude(c => c.Resume)
                        .ThenInclude(v => v.Education)
                .Include(x => x.WishListItems)
                    .ThenInclude(c => c.Resume)
                        .ThenInclude(v => v.Skills)
                .Include(x => x.WishListItems)
                    .ThenInclude(c => c.Resume)
                        .ThenInclude(v => v.City)
                .Include(x => x.WishListItems)
                    .ThenInclude(c => c.Resume)
                        .ThenInclude(v => v.Language)
                .Include(x => x.WishListItems)
                    .ThenInclude(c => c.Resume)
                        .ThenInclude(v => v.experiences)
                       .Include(x => x.WishListItems)
                                .ThenInclude(c => c.Vacancy)
                                    .ThenInclude(v => v.Category)
                            .Include(x => x.WishListItems)
                                .ThenInclude(c => c.Vacancy)
                                    .ThenInclude(v => v.Company)
                            .Include(x => x.WishListItems)
                                .ThenInclude(c => c.Vacancy)
                                    .ThenInclude(v => v.Category)
                            .Include(x => x.WishListItems)
                                .ThenInclude(c => c.Vacancy)
                                    .ThenInclude(v => v.City)


                    .FirstOrDefaultAsync();

                if (wishlistQuery != null)
                {
                    wishlistGetDto.wishlistItems = wishlistQuery.WishListItems.Where(x => !x.IsDeleted && x.ResumeId != null)
                        .Select(cv => new WishlistItemDto
                        {
                            Id = cv.Resume.Id,
                            Image = cv.Resume.Image,
                            Name = cv.Resume.Name,
                            Category = new CategoryGetDto { Id = cv.Resume.Category.Id, Name = cv.Resume.Category.Name, Image = cv.Resume.Category.Image, ParentId = cv.Resume.Category.ParentId },
                            Experience = cv.Resume.Experience,
                            Mode = cv.Resume.Mode,
                            Surname = cv.Resume.Surname,
                            ViewCount = cv.Resume.ViewCount,
                            EndDate = cv.Resume.EndDate,
                            Salary = (int)cv.Resume.Salary,
                            IsPremium = cv.Resume.IsPremium,
                            IsLiked = true

                        }).ToList();

                    wishlistGetDto.wishlistVacancies = wishlistQuery.WishListItems.Where(x => !x.IsDeleted && x.VacancyId != null)
                        .Select(cv => new WishlistVacancyDto
                        {
                            Id = cv.Vacancy.Id,
                            Position = cv.Vacancy.Position,
                            Required_Experience = cv.Vacancy.Required_Experience,
                            Category = new CategoryGetDto { Id = cv.Vacancy.Category.Id, Name = cv.Vacancy.Category.Name, Image = cv.Vacancy.Category.Image, ParentId = cv.Vacancy.Category.ParentId },
                            Company = new CompanyGetDto { Id = cv.Vacancy.Company.Id, Name = cv.Vacancy.Company.Name },
                            Mode = cv.Vacancy.Mode,
                            ViewCount = cv.Vacancy.ViewCount,
                            EndDate = cv.Vacancy.EndDate,
                            Salary = (int)cv.Vacancy.Salary,
                            IsPremium = cv.Vacancy.IsPremium,
                            IsLiked = true

                        }).ToList();
                }
            }

            else
            {
                var wishlistJson = _http.HttpContext.Request.Cookies["wishlist"];

                if (wishlistJson != null)
                {
                    List<WishlistItem> wishlistItems = JsonConvert.DeserializeObject<List<WishlistItem>>(wishlistJson);

                    foreach (var item in wishlistItems)
                    {
                        var resume = await _resumeService.GetAsync(item.Id);
                        var Vacancy = await _VacancyService.GetAsync(item.Id);
                        if (resume.Data != null)
                        {
                            var wishlistItem = new WishlistItemDto
                            {
                                Id = resume.Data.Id,
                                Image = resume.Data.Image,
                                Name = resume.Data.Name,
                                Category = new CategoryGetDto { Id = resume.Data.Category.Id, Name = resume.Data.Category.Name, Image = resume.Data.Category.Image, ParentId = resume.Data.Category.ParentId },
                                Experience = resume.Data.Experience,
                                Mode = resume.Data.Mode,
                                Surname = resume.Data.Surname,
                                ViewCount = resume.Data.ViewCount,
                                EndDate = resume.Data.EndDate,
                                Salary = (int)resume.Data.Salary,
                                IsPremium = resume.Data.IsPremium,
                                IsLiked = true
                            };
                            wishlistGetDto.wishlistItems.Add(wishlistItem);
                        }

                        if (Vacancy.Data != null)
                        {
                            var wishlistItem = new WishlistVacancyDto
                            {
                                Id = Vacancy.Data.Id,
                                Position = Vacancy.Data.Position,
                                Required_Experience = Vacancy.Data.Required_Experience,
                                Category = new CategoryGetDto { Id = Vacancy.Data.Category.Id, Name = Vacancy.Data.Category.Name, Image = Vacancy.Data.Category.Image, ParentId = Vacancy.Data.Category.ParentId },
                                Company = new CompanyGetDto { Id = Vacancy.Data.Company.Id, Name = Vacancy.Data.Company.Name },
                                Mode = Vacancy.Data.Mode,
                                ViewCount = Vacancy.Data.ViewCount,
                                EndDate = Vacancy.Data.EndDate,
                                Salary = (int)Vacancy.Data.Salary,
                                IsPremium = Vacancy.Data.IsPremium,
                                IsLiked = true
                            };

                            wishlistGetDto.wishlistVacancies.Add(wishlistItem);

                        }


                    }
                }
            }


            return wishlistGetDto;
        }
    }
}
