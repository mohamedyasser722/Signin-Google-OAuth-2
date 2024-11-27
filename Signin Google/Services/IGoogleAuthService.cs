using Signin_Google.Database;
using Signin_Google.Dtos;

namespace Signin_Google.Services;

public interface IGoogleAuthService
{
    Task<BaseResponse<ApplicationUser>> GoogleSignInAsync(GoogleSignInVM model);
}