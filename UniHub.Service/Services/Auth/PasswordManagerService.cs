using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using UniHub.Core;
using UniHub.Domain.Entities.Identity;
using UniHub.Domain.Interface;
using UniHub.Dto;
using UniHub.Infrastructure;

public class PasswordManagerService : IPasswordManagerService
{
    private readonly UserManager<AspNetUser> _userManager;
    private readonly ApplicationDbContext _context;
    private readonly IEmailService _emailService;
    private readonly IConfiguration _configuration;

    public PasswordManagerService(
        UserManager<AspNetUser> userManager,
        ApplicationDbContext context,
        IEmailService emailService,
        IConfiguration configuration)
    {
        _userManager = userManager;
        _context = context;
        _emailService = emailService;
        _configuration = configuration;
    }

    #region Change Password

    public async Task<BaseResponse<string>> ChangePasswordAsync(ChangePasswordDto dto, Guid userId)
    {
        if (dto.NewPassword == dto.OldPassword)
            return BaseResponse<string>.Fail("New password cannot be same as old password");

        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return BaseResponse<string>.Fail("User not found");

        var result = await _userManager.ChangePasswordAsync(user, dto.OldPassword, dto.NewPassword);
        if (!result.Succeeded)
            return BaseResponse<string>.Fail(string.Join(", ", result.Errors.Select(e => e.Description)));

        return BaseResponse<string>.Success("Password changed successfully");
    }

    #endregion

    #region Forgot Password - OTP

    public async Task<BaseResponse<string>> SendForgotPasswordOtpAsync(string email)
    {
        if (string.IsNullOrWhiteSpace(email))
            return BaseResponse<string>.Fail("Email is required");

        var user = await _userManager.FindByEmailAsync(email);
        if (user == null)
            return BaseResponse<string>.Fail("User not found");

        // Prevent OTP spam: allow only 1 active OTP
        var activeOtp = await _context.UserOtps
            .Where(x => x.AspNetUserId == user.Id && !x.IsUsed && x.ExpiryTime > DateTime.UtcNow)
            .FirstOrDefaultAsync();

        if (activeOtp != null)
            return BaseResponse<string>.Fail("OTP already sent. Please wait until it expires.");

        var otp = OtpGenerator.GenerateOtp();

        var userOtp = new UserOtp
        {
            Id = Guid.NewGuid(),
            AspNetUserId = user.Id,
            Otp = otp, // 🔐 Replace with hash in production
            ExpiryTime = DateTime.UtcNow.AddMinutes(10),
            IsUsed = false
        };

        _context.UserOtps.Add(userOtp);
        await _context.SaveChangesAsync();

        var emailDto = new SendEmailTemplateDto
        {
            To = new List<string> { user.Email },
            TemplateName = "ResetPassword", // Folder name in EmailTemplates

            SubjectPlaceHolders = new List<PlaceHolderDto>
            {
                new PlaceHolderDto { Key = "AppName", Value = "UniHub" }
            },

                    BodyPlaceHolders = new List<PlaceHolderDto>
            {
                new PlaceHolderDto { Key = "UserName", Value = user.FirstName },
                new PlaceHolderDto { Key = "AppName", Value = "UniHub" },
                new PlaceHolderDto { Key = "Otp", Value = otp },
                new PlaceHolderDto { Key = "OtpExpiryMinutes", Value = "10" },
                new PlaceHolderDto { Key = "CopyRightYear", Value = DateTime.UtcNow.Year.ToString() },
                new PlaceHolderDto { Key = "SupportEmail", Value = "support@unihub.com" }
            },

                    TextPlaceHolders = new List<PlaceHolderDto>
            {
                new PlaceHolderDto { Key = "UserName", Value = user.FirstName },
                new PlaceHolderDto { Key = "Otp", Value = otp }
            },

            Cc = new List<string>(),   // optional
            BCC = new List<string>(),  // optional
            Attachement = new List<string>() // optional
        };


        await _emailService.SendEmailFromTemplateAsync(emailDto);

        return BaseResponse<string>.Success("OTP sent successfully");
    }

    public async Task<BaseResponse<string>> VerifyOtpAsync(Guid userId, string otp)
    {
        if (string.IsNullOrWhiteSpace(otp))
            return BaseResponse<string>.Fail("OTP is required");

        var otpRecord = await _context.UserOtps
            .Where(x => x.AspNetUserId == userId && !x.IsUsed)
            .OrderByDescending(x => x.ExpiryTime)
            .FirstOrDefaultAsync();

        if (otpRecord == null)
            return BaseResponse<string>.Fail("OTP not found");

        if (otpRecord.ExpiryTime < DateTime.UtcNow)
            return BaseResponse<string>.Fail("OTP expired");

        if (otpRecord.Otp != otp) // 🔐 Use hash compare in prod
            return BaseResponse<string>.Fail("Invalid OTP");
            
        otpRecord.IsUsed = true;
        await _context.SaveChangesAsync();

        return BaseResponse<string>.Success("OTP verified successfully");
    }

    #endregion

    #region Reset Password

    public async Task<BaseResponse<string>> ResetPasswordAsync(Guid userId, string newPassword)
    {
        if (string.IsNullOrWhiteSpace(newPassword))
            return BaseResponse<string>.Fail("New password is required");

        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return BaseResponse<string>.Fail("User not found");

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, newPassword);

        if (!result.Succeeded)
            return BaseResponse<string>.Fail(string.Join(", ", result.Errors.Select(e => e.Description)));

        // Invalidate all OTPs after successful reset
        var userOtps = await _context.UserOtps
            .Where(x => x.AspNetUserId == userId)
            .ToListAsync();

        _context.UserOtps.RemoveRange(userOtps);
        await _context.SaveChangesAsync();

        return BaseResponse<string>.Success("Password reset successful");
    }

    #endregion

    #region Email Verification

    public async Task<BaseResponse<string>> SendEmailVerificationAsync(Guid userId)
    {
        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return BaseResponse<string>.Fail("User not found");

        if (user.EmailConfirmed)
            return BaseResponse<string>.Fail("Email already verified");

        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);

        var verifyUrl = $"{_configuration["App:BaseUrl"]}/api/auth/verify-email" +
                        $"?userId={user.Id}&token={Uri.EscapeDataString(token)}";

       // await _emailService.SendEmailFromTemplateAsync(user.Email, "Verify your email",
        //    $"Click here to verify your email: {verifyUrl}");

        return BaseResponse<string>.Success("Verification email sent");
    }

    public async Task<BaseResponse<string>> VerifyEmailAsync(Guid userId, string token)
    {
        if (string.IsNullOrWhiteSpace(token))
            return BaseResponse<string>.Fail("Invalid token");

        var user = await _userManager.FindByIdAsync(userId.ToString());
        if (user == null)
            return BaseResponse<string>.Fail("User not found");

        if (user.EmailConfirmed)
            return BaseResponse<string>.Fail("Email already verified");

        var result = await _userManager.ConfirmEmailAsync(user, token);
        if (!result.Succeeded)
            return BaseResponse<string>.Fail(string.Join(", ", result.Errors.Select(e => e.Description)));

        return BaseResponse<string>.Success("Email verified successfully");
    }
    #endregion
}
