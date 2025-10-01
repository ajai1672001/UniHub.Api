using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UniHub.Dto;

namespace UniHub.Domain.Interface
{
    public interface IAuthService
    {
        Task<LoginResultDto> LoginAsync(LoginDetailDto login);

        Task LogoutAsync(TokenDto tokenDto);

        Task<LoginResultDto> RefreshTokenAsync(TokenDto model);
    }
}
