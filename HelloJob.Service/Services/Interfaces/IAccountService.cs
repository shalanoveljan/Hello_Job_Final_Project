using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HelloJob.Entities.DTOS;
using HelloJob.Entities.Models;
using HelloJob.Core.Utilities.Results.Abstract;
using HelloJob.Service.Responses;

namespace HelloJob.Service.Services.Interfaces
{
    public interface IAccountService
    {
        public Task<IDataResult<string>> SignUp(RegisterDto dto,string role);
        public Task<IResult> VerifyEmail(string token, string email);
        public Task<IResult> Login(LoginDto dto,bool IsAdminPanel);
        public Task<IResult> LogOut();
        public Task<IResult> ForgetPassword(string email);
        public Task<IDataResult<ResetPasswordDto>> ResetPasswordGet(string email, string token);
        public Task<IResult> ResetPasswordPost(ResetPasswordDto dto);
        public Task<IResult> ChangeUserActivationStatus(string email, bool activate);
        public Task<IResult> Update(UpdateDto dto);
        public Task<PagginatedResponse<AppUser>> GetAllUsers(int count, int page);
        public Task<PagginatedResponse<AppUser>> GetAllAdmin(int count, int page);
        public Task<IResult> RegisterWithGoogle(string returnUrl = null);
        public Task<IResult> GoogleCallback(string returnUrl = null);
        public Task<AppUser> GetUser(string id);
        public Task<bool> ChangeRole(string userId, string newRoleId);


    }
}
