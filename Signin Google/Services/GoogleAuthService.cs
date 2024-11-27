namespace Signin_Google.Services;

using Google.Apis.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Signin_Google.Database;
using Signin_Google.Dtos;

public class GoogleAuthService : IGoogleAuthService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly GoogleAuthConfig _authSettings;

    public GoogleAuthService(
        UserManager<ApplicationUser> userManager,
        IOptions<GoogleAuthConfig> authSettings)
    {
        _userManager = userManager;
        _authSettings = authSettings.Value;
    }

    public async Task<BaseResponse<ApplicationUser>> GoogleSignInAsync(GoogleSignInVM model)
    {
        try
        {
            // Validate the ID Token
            var payload = await GoogleJsonWebSignature.ValidateAsync(model.IdToken, new GoogleJsonWebSignature.ValidationSettings
            {
                Audience = new[] { _authSettings.ClientId }
            });

            if (payload == null)
                return new BaseResponse<ApplicationUser>(null, new List<string> { "Invalid ID token." });

            var loginInfo = new UserLoginInfo("Google", payload.Subject, "Google");

            // Check if the user exists by external login (Google)
            var user = await _userManager.FindByLoginAsync(loginInfo.LoginProvider, loginInfo.ProviderKey);
            if (user == null)
            {
                // If not found via external login, check by email
                user = await _userManager.FindByEmailAsync(payload.Email);
                if (user == null)
                {
                    // Create a new user if no email match
                    user = new ApplicationUser
                    {
                        UserName = payload.Email,
                        Email = payload.Email,
                        FirstName = payload.GivenName,
                        LastName = payload.FamilyName,
                        ProfilePictureUrl = payload.Picture
                    };

                    var createResult = await _userManager.CreateAsync(user);
                    if (!createResult.Succeeded)
                    {
                        return new BaseResponse<ApplicationUser>(
                            null,
                            createResult.Errors.Select(e => e.Description).ToList()
                        );
                    }
                }

                // Add the external login to the user
                var addLoginResult = await _userManager.AddLoginAsync(user, loginInfo);
                if (!addLoginResult.Succeeded)
                {
                    return new BaseResponse<ApplicationUser>(
                        null,
                        addLoginResult.Errors.Select(e => e.Description).ToList()
                    );
                }
            }

            // At this point, the user exists and is associated with the Google login
            return new BaseResponse<ApplicationUser>(user);
        }
        catch (Exception ex)
        {
            return new BaseResponse<ApplicationUser>(null, new List<string> { ex.Message });
        }
    }

}
