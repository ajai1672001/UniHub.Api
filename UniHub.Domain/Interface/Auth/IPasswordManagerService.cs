using System;
using System.Collections.Generic;
using System.Text;
using UniHub.Dto;

namespace UniHub.Domain.Interface;

public interface IPasswordManagerService
{
    Task<BaseResponse<string>> ChangePasswordAsync(ChangePasswordDto dto, Guid userId);
    Task<BaseResponse<string>> SendForgotPasswordOtpAsync(string email);
    Task<BaseResponse<string>> VerifyOtpAsync(Guid userId, string otp);
    Task<BaseResponse<string>> ResetPasswordAsync(Guid userId, string newPassword);
    Task<BaseResponse<string>> SendEmailVerificationAsync(Guid userId);
    Task<BaseResponse<string>> VerifyEmailAsync(Guid userId, string token);
}