﻿using AutoMapper;
using HelloJob.Core.Helper.MailHelper;
using HelloJob.Core.Utilities.Results.Abstract;
using HelloJob.Core.Utilities.Results.Concrete;
using HelloJob.Core.Utilities.Results.Concrete.ErrorResults;
using HelloJob.Core.Utilities.Results.Concrete.SuccessResults;
using HelloJob.Entities.DTOS;
using HelloJob.Entities.Enums;
using HelloJob.Entities.Models;
using HelloJob.Service.Responses;
using HelloJob.Service.Services.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Org.BouncyCastle.Asn1.Ocsp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using myResult = HelloJob.Core.Utilities.Results.Abstract.IResult;

namespace HelloJob.Service.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IEmailHelper _emailService;
        private readonly IHttpContextAccessor _http;
        private readonly IUrlHelper _helper;



        public AccountService(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager,
            IEmailHelper emailService, IHttpContextAccessor http, IUrlHelper helper, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailService = emailService;
            _http = http;
            _helper = helper;
            _roleManager = roleManager;
        }
        public async Task<Core.Utilities.Results.Abstract.IDataResult<string>> SignUp(RegisterDto dto, string role)
        {

            var isValidEmail = _emailService.IsValidEmail(dto.Email);
            if (!isValidEmail) return new ErrorDataResult<string>(message: "Invalid Email!");
            var checkUser = await _userManager.FindByEmailAsync(dto.Email);
            if (checkUser != null) return new ErrorDataResult<string>(message: "Email is already used!");
            AppUser appUser = new AppUser()
            {
                UserName = dto.Username,
                Email = dto.Email,
                IsActivate = true
            };
            var result = await _userManager.CreateAsync(appUser, dto.Password);
            if (!result.Succeeded) return new ErrorDataResult<string>(message: string.Join('\n', result.Errors.Select(x => x.Description)));

            var hasUserRole = await _userManager.IsInRoleAsync(appUser, role);
            if (!hasUserRole) await _userManager.AddToRoleAsync(appUser, role);
            string token = await _userManager.GenerateEmailConfirmationTokenAsync(appUser);
            var urlHelperFactory = _http.HttpContext.RequestServices.GetService<IUrlHelperFactory>();

            if (urlHelperFactory != null)
            {
                var endpoint = _http.HttpContext.GetEndpoint();
                if (endpoint != null)
                {
                    var actionDescriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();
                    if (actionDescriptor != null)
                    {
                        var actionContext = new ActionContext(_http.HttpContext, _http.HttpContext.GetRouteData(), actionDescriptor);

                        var urlHelper = urlHelperFactory.GetUrlHelper(actionContext);

                        var url = urlHelper.Action("VerifyEmail", "Account", new { email = appUser.Email, token = token }, protocol: _http.HttpContext.Request.Scheme);

                        //var url = $"{_http.HttpContext.Request.Scheme}://{_http.HttpContext.Request.Host}{_helper.Action("VerifyEmail", "Identity", new { email = appUser.Email, token = token })}";

                        await _emailService.SendEmailAsync(appUser.Email, url, "Verify Email", token);
                    }
                }

            }
            return new SuccessDataResult<string>(
                message: "RegisterDto olundu"
                );
        }

        private async Task<bool> IsUserInRole(AppUser user, string roleName)
        {
            return await _userManager.IsInRoleAsync(user, roleName);
        }

        public async Task<myResult> Login(LoginDto dto, bool isAdminPanelLogin)
        {
            try
            {
                AppUser checkUser = await _userManager.FindByNameAsync(dto.UserNameOrEmail);

                if (checkUser == null)
                {
                    checkUser = await _userManager.FindByEmailAsync(dto.UserNameOrEmail);
                }

                if (checkUser == null)
                    return new ErrorResult("User not Found!");

                if (!checkUser.EmailConfirmed)
                    return new ErrorResult("Please verify this email before sign in!");

                if (!checkUser.IsActivate)
                    return new ErrorResult("Your account is blocked! Please contact the administrator.");

                if ((isAdminPanelLogin &&  (await IsUserInRole(checkUser, "Owner") || await IsUserInRole(checkUser, "Employee"))))
                {
                    return new ErrorResult(isAdminPanelLogin ? "You are not authorized to access the admin panel." : "You are not authorized to access the user panel. Please create a user for yourself.");
                }

                var result = await _signInManager.PasswordSignInAsync(checkUser, dto.Password, dto.RememberMe, true);

                if (!result.Succeeded)
                    return new ErrorResult("Email or Password is incorrect!");

                if (result.IsLockedOut)
                    return new ErrorResult("User is locked out!");

                if (result.IsNotAllowed)
                    return new ErrorResult("User is not allowed to sign in!");

                return new SuccessResult("Login Successfully");
            }
            catch (Exception ex)
            {
                return new ErrorResult(ex.Message);
            }
        }

        public async Task<myResult> VerifyEmail(string token, string email)
        {
            AppUser appUser = await _userManager.FindByEmailAsync(email);
            if (appUser == null) return new ErrorResult("User tapilmadi");
            var res = await _userManager.ConfirmEmailAsync(appUser, token);
            if (!res.Succeeded)
            {
                var errors = string.Join(", ", res.Errors.Select(e => e.Description));
                return new ErrorResult($"Confirm Email is invalid : {errors}");
            }
            appUser.EmailConfirmed = true;
            await _signInManager.SignInAsync(appUser, true);
            return new SuccessResult("Email tesdiq olundu ve signin olundu");
        }

        public async Task<myResult> LogOut()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return new SuccessResult();
            }

            catch (Exception ex)
            {
                return new ErrorResult(ex.Message);
            }
        }

        public async Task<myResult> ForgetPassword(string email)
        {
            if (email is null)
            {
                return new ErrorResult("please enter email!");
            }
            var isValidEmail = _emailService.IsValidEmail(email);
            if (!isValidEmail) return new ErrorResult("Invalid Email!");
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return new ErrorResult("Email is not registered!");
            string token = await _userManager.GeneratePasswordResetTokenAsync(user);
            if (token is null) return new ErrorResult("token is not generated");
            var urlHelperFactory = _http.HttpContext.RequestServices.GetService<IUrlHelperFactory>();

            if (urlHelperFactory != null)
            {
                var endpoint = _http.HttpContext.GetEndpoint();
                if (endpoint != null)
                {
                    var actionDescriptor = endpoint.Metadata.GetMetadata<ControllerActionDescriptor>();
                    if (actionDescriptor != null)
                    {
                        var actionContext = new ActionContext(_http.HttpContext, _http.HttpContext.GetRouteData(), actionDescriptor);

                        var urlHelper = urlHelperFactory.GetUrlHelper(actionContext);

                        var url = urlHelper.Action("ResetPassword", "Account", new { email = user.Email, token = token }, protocol: _http.HttpContext.Request.Scheme);

                        await _emailService.SendEmailAsync(user.Email, url, "Verify Email for resetpassword", token);
                    }
                }
            }
            return new SuccessResult();
        }

        public async Task<Core.Utilities.Results.Abstract.IDataResult<ResetPasswordDto>> ResetPasswordGet(string email, string token)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user is null)
            {
                return new ErrorDataResult<ResetPasswordDto>("User not found");
            }
            ResetPasswordDto dto = new ResetPasswordDto()
            {
                Email = email,
                Token = token
            };

            return new SuccessDataResult<ResetPasswordDto>(dto, "get resetPassword");
        }

        public async Task<myResult> ResetPasswordPost(ResetPasswordDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if (user == null)
            {
                return new ErrorResult("User not Found!");
            }

            var result = await _userManager.ResetPasswordAsync(user, dto.Token, dto.Password);
            if (!result.Succeeded)
            {
                string errors = string.Join("\n", result.Errors.Select(error => error.Description));
                return new ErrorResult("Reset password failed: " + errors);
            }

            return new SuccessResult("Reset password success");
        }

        public async Task<myResult> Update(UpdateDto dto)
        {
            var user = await _userManager.FindByNameAsync(_http.HttpContext.User.Identity.Name);
            if (user == null)
            {
                return new ErrorResult("User not found!");
            }

            if (!string.IsNullOrEmpty(dto.UserName))
            {
                user.UserName = dto.UserName;
            }

            if (!string.IsNullOrEmpty(dto.OldPassword) && !string.IsNullOrEmpty(dto.NewPassword))
            {
                var changePasswordResult = await _userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);
                if (!changePasswordResult.Succeeded)
                {
                    string errors = string.Join("\n", changePasswordResult.Errors.Select(error => error.Description));
                    return new ErrorResult(errors);
                }
            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                string errors = string.Join("\n", result.Errors.Select(error => error.Description));
                return new ErrorResult(errors);
            }

            await _signInManager.RefreshSignInAsync(user);

            return new SuccessResult("User updated successfully");
        }

        public async Task<myResult> ChangeUserActivationStatus(string email, bool activate)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new ErrorResult("User not found!");
            }

            if (activate)
            {
                user.IsActivate = true;
            }
            else
            {
                user.IsActivate = false;
            }

            var result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                string errors = string.Join("\n", result.Errors.Select(error => error.Description));
                return new ErrorResult(errors);
            }

            return new SuccessResult("User activation status changed successfully");
        }

        public async Task<PagginatedResponse<AppUser>> GetAllUsers(int page,int count)
        {
            try
            {
                IQueryable<AppUser> query = _userManager.Users;

                if (count > 0 && page > 0)
                {
                    var usersInRoles = new List<AppUser>();

                    foreach (var user in query)
                    {
                        var roles = await _userManager.GetRolesAsync(user);

                        foreach (var role in roles)
                        {
                            if (role == "Employee" || role=="Owner")
                            {
                                usersInRoles.Add(user);
                            }
                        }

                    }

                    query = usersInRoles.AsQueryable();


                }

                int totalCount = query.Count();

                List<AppUser> users =  query.Skip((page - 1) * count).Take(count).ToList();

                var response = new PagginatedResponse<AppUser>(
                    datas: users,
                    pageNumber: page,
                    pageSize: count,
                    totalCount: totalCount,
                    otherdatas: null
                );

                return response;
            }
            catch (Exception ex)
            {
                return new PagginatedResponse<AppUser>(
                    datas: null,
                    pageNumber: 0,
                    pageSize: 0,
                    totalCount: 0,
                    otherdatas: null
                );
            }
        }

        public async Task<PagginatedResponse<AppUser>> GetAllAdmin(int page,int count)
        {
            try
            {
                IQueryable<AppUser> query = _userManager.Users;

                if (count > 0 && page > 0)
                {
                    var usersInRoles = new List<AppUser>();

                    foreach (var user in query)
                    {
                        var roles = await _userManager.GetRolesAsync(user);

                        foreach (var role in roles)
                        {
                            if (role == "Admin")
                            {
                                usersInRoles.Add(user);
                            }
                        }

                    }

                    query = usersInRoles.AsQueryable();


                }
                int totalCount =  query.Count();
                List<AppUser> users =  query.Skip((page - 1) * count).Take(count).ToList();

                var response = new PagginatedResponse<AppUser>(
                    datas: users,
                    pageNumber: page,
                    pageSize: count,
                    totalCount: totalCount,
                    otherdatas: null
                );

                return response;
            }
            catch (Exception ex)
            {
                return new PagginatedResponse<AppUser>(
                    datas: null,
                    pageNumber: 0,
                    pageSize: 0,
                    totalCount: 0,
                    otherdatas: null
                );
            }
        }

        public async Task<Core.Utilities.Results.Abstract.IResult> RegisterWithGoogle(string returnUrl = null)
        {
            try
            {
                var authenticationProperties = _signInManager.ConfigureExternalAuthenticationProperties("Google", _helper.Action("GoogleCallback", "Account", new { returnUrl }));
                return new SuccessResult("Google authentication initiated successfully.");
            }
            catch (Exception ex)
            {
                return new ErrorResult($"Error initiating Google authentication: {ex.Message}");
            }
        }


        public async Task<Core.Utilities.Results.Abstract.IResult> GoogleCallback(string returnUrl = null)
        {
            try
            {
                var externalLoginInfo = await _signInManager.GetExternalLoginInfoAsync();
                if (externalLoginInfo == null)
                {
                    return new ErrorResult("External login information not found.");
                }

                var email = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Email);
                var userName = externalLoginInfo.Principal.FindFirstValue(ClaimTypes.Name);

                var existingUser = await _userManager.FindByEmailAsync(email);
                if (existingUser != null)
                {
                    await _signInManager.SignInAsync(existingUser, isPersistent: false);
                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return new SuccessResult("User signed in successfully.");
                    }
                    return new SuccessResult("User signed in successfully.");
                }

                var userNameWithoutSpaces = userName.Replace(" ", string.Empty);
                var newUser = new AppUser
                {
                    UserName = userNameWithoutSpaces.ToLower(),
                    Email = email,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(newUser);
                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(newUser, UserRoles.Employee.ToString());

                    await _signInManager.SignInAsync(newUser, isPersistent: false);

                    if (!string.IsNullOrEmpty(returnUrl))
                    {
                        return new SuccessResult("New user registered and signed in successfully.");
                    }

                    return new SuccessResult("New user registered and signed in successfully.");
                }
                else
                {
                    return new ErrorResult("Error registering new user.");
                }
            }
            catch (Exception ex)
            {
                return new ErrorResult($"Error during Google callback: {ex.Message}");
            }
        }

        public async Task<AppUser> GetUser(string id)
        {
            return _signInManager.UserManager.Users.FirstOrDefault(u => u.Id == id);
        }

        public async Task<bool> ChangeRole(string userId, string newRoleId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return false; 
            }

            var currentRoles = await _userManager.GetRolesAsync(user);
            if (currentRoles == null || currentRoles.Count == 0)
            {
                return false; 
            }

            var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
            if (!removeResult.Succeeded)
            {
                return false; 
            }

            
            var addResult = await _userManager.AddToRoleAsync(user, newRoleId);
            return addResult.Succeeded;
        }
    }

}

