using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UniHub.Dto;

public class LoginResultDto
{
    public AspNetUserDto User { get; set; }

    public string Token { get; set; } = string.Empty;

    public DateTime Expiration { get; set; }

    public string RefershToken { get; set; }

    public DateTime RefershTokenExpires { get; set; }

    public IEnumerable<TenantUserDto> TenantUsers { get; set; }

    public TenantUserDto TenantUser { get; set; }
}
