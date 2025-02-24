using EstateFInder.Data;
using EstateFInder.Models.JwtModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Transactions;

public class AccountService: IAccountService
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly JWTSettings _jwtSettings;
    private readonly AuthDbContext _context;

    public AccountService(UserManager<ApplicationUser> userManager,
            RoleManager<IdentityRole> roleManager,
            IOptions<JWTSettings> jwtSettings, SignInManager<ApplicationUser> signInManager, AuthDbContext context)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
        _jwtSettings = jwtSettings.Value;
        _signInManager = signInManager;

    }

    public async Task<Response<string>> RegisterAsync(RegisterRequest request)
    {
        using (TransactionScope ts = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled))
        {

            var userWithSameUserName = await _userManager.FindByNameAsync(request.Email);

            if (userWithSameUserName != null)
            {
                throw new Exception($"Username '{request.Email}' is already taken.");
            }

            var user = new ApplicationUser
            {
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName,
                UserName = request.Email,
                PhoneNumber = request.PhoneNumber,
                EmailConfirmed = true
            };

            var userWithSameEmail = await _userManager.FindByEmailAsync(request.Email);

            if (userWithSameEmail == null)
            {
                var result = await _userManager.CreateAsync(user, request.Password);

                if (result.Succeeded)
                {

                    await _userManager.AddToRoleAsync(user, ((RoleEnum)request.RoleId).ToString());

                    ts.Complete();

                    return new Response<string>("Your registeration was successful. Welcome on board!")
                    {
                        Succeeded = true,
                       Message = "Successful"
                    };

                }
                else
                {
                    await _userManager.DeleteAsync(user);
                    throw new Exception($"{result.Errors}");
                }
            }
            else
            {
                throw new Exception($"Email {request.Email} is already registered.");
            }
        }

    }

    public async Task<Response<AuthenticationResponse>> AuthenticateAsync(AuthenticationRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);

        if (user == null)
        {
            throw new Exception($"No Accounts Registered with {request.Email}.");
        }
        var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password, false, lockoutOnFailure: false);

        if (!result.Succeeded)
        {
            throw new Exception($"Invalid Credentials!");
        }

        JwtSecurityToken jwtSecurityToken = await GenerateJWToken(user);

        AuthenticationResponse response = new AuthenticationResponse();

        response.Id = user.Id;
        response.JWToken = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        response.Email = user.Email;
        response.FirstName = user.FirstName;
        response.LastName = user.LastName;

        var rolesList = await _userManager.GetRolesAsync(user).ConfigureAwait(false);

        response.Roles = rolesList.ToList();
        response.TokenExpires = jwtSecurityToken.ValidTo;
      

        return new Response<AuthenticationResponse>(response, $"Authenticated {user.UserName}");
    }

    private async Task<JwtSecurityToken> GenerateJWToken(ApplicationUser user)
    {
        var userClaims = await _userManager.GetClaimsAsync(user);
        var roles = await _userManager.GetRolesAsync(user);

        var roleClaims = new List<Claim>();

        for (int i = 0; i < roles.Count; i++)
        {
            roleClaims.Add(new Claim("roles", roles[i]));
        }

        var claims = new[]
        {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                 new Claim("id", user.Id),
                new Claim("rol", roles.FirstOrDefault() ?? null!),
            }
        .Union(userClaims)
        .Union(roleClaims);

        var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));
        var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);

        var jwtSecurityToken = new JwtSecurityToken(
           issuer: _jwtSettings.Issuer,
           audience: _jwtSettings.Audience,
           claims: claims,
           expires: DateTime.Now.AddMinutes(_jwtSettings.DurationInMinutes),
           signingCredentials: signingCredentials);

        return jwtSecurityToken;
    }
}
