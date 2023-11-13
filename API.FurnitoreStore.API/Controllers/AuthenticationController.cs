using API.FurnitoreStore.API.Configuration;
using API.FurnitoreStore.Shared.Auth;
using API.FurnitoreStore.Shared.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace API.FurnitoreStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtConfig _jwtConfig;
        private readonly IEmailSender _emailSender;

        //IOptions trae elementos desde cofig especificando el tipo de output
        public AuthenticationController(UserManager<IdentityUser> userManager,
                                        IOptions<JwtConfig> jwtConfig,
                                        IEmailSender emailSender)
        {
            _userManager = userManager;
            _jwtConfig = jwtConfig.Value;
            _emailSender = emailSender;
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDto request)
        {
            if (!ModelState.IsValid) return BadRequest();
            //Verificar si el email existe
            var emailExist = await _userManager.FindByEmailAsync(request.EmailAdress);
            if (emailExist != null)
                return BadRequest(new AuthResult()
                {
                    Result = false,
                    Errors = new List<string>()
                    {
                        "Email already exists"
                    }
                });
            //crear usuario
            var user = new IdentityUser()
            {
                Email = request.EmailAdress,
                UserName = request.EmailAdress,
                EmailConfirmed = false
            };

            var isCreated = await _userManager.CreateAsync(user, request.Password);
            if (isCreated.Succeeded)
            {
                await SendVerificationUser(user);
                //var token = GenerateToken(user);
                return Ok(new AuthResult()
                {
                    Result = true,

                });
            }
            else
            {
                var errors = new List<string>();
                foreach (var err in isCreated.Errors)
                    errors.Add(err.Description);

                return BadRequest(new AuthResult
                {
                    Result= false,
                    Errors = errors
                });
            }

            return BadRequest(new AuthResult
            {
                Result = false,
                Errors = new List<string> { "User couldn't be created" }
            });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestDto request)
        {
            if (!ModelState.IsValid) return BadRequest();
            //revisar si existe el usuario
            var existingUser = await _userManager.FindByEmailAsync(request.Email);
            if (existingUser == null)
                return BadRequest(new AuthResult
                {
                    Errors = new List<string> { "Invalid Payload" },
                    Result = false
                });
            if(!existingUser.EmailConfirmed)
                return BadRequest(new AuthResult
                {
                    Errors = new List<string> { "Email needs to be confirmed" },
                    Result = false
                });

            var checkUserAndPass = await _userManager.CheckPasswordAsync(existingUser, request.Password);
            if (!checkUserAndPass) return BadRequest(new AuthResult 
            { 
                 Result = false,
                 Errors = new List<string> { "Invalid Credentials" } 
            });

            var token = GenerateToken(existingUser);

            return Ok(new AuthResult { Result = true, Token = token });
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if(string.IsNullOrEmpty(userId) || string.IsNullOrEmpty(code))
                return BadRequest(new AuthResult
                {
                    Errors = new List<string> { "Invalid email confirmation URL"},
                    Result = false
                });
            var user = await _userManager.FindByIdAsync(userId);

            if(user == null)
               return NotFound($"Unable to load user with id '{userId}'.");
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));
            var result = await _userManager.ConfirmEmailAsync(user, code);
            var status = result.Succeeded ? "Thank you for confirm your email."
                                          : "There has been an error confirming your email.";
            return Ok(status);
        }




        private string GenerateToken(IdentityUser user)
        {
            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.UTF8.GetBytes(_jwtConfig.Secret);

            //se agregan las claims que van encriptadas
            var tokenDescriptor = new SecurityTokenDescriptor()
            {
                Subject =  new ClaimsIdentity(new ClaimsIdentity(new[]
                {
                    new Claim("Id",user.Id),
                    new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iat,DateTime.Now.ToUniversalTime().ToString()) //creacion iat
                })),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),SecurityAlgorithms.HmacSha256) //expiracion exp
                
            };

            //ahora va el token en si mismo
            var token = jwtTokenHandler.CreateToken(tokenDescriptor);
            return jwtTokenHandler.WriteToken(token);

        }


        //Email con url de callback, para verificar
        private async Task SendVerificationUser(IdentityUser user)
        {
            var verificationCode = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            verificationCode = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(verificationCode));

            //example: https://localholst:8080/api/authentication/verifyemail/userId=exampleuserId&code=examplecode
            var callbackUrl = $@"{Request.Scheme}://{Request.Host}{Url.Action(
                "ConfirmEmail", controller: "Authentication", new { userId = user.Id, code = verificationCode })}";

            var emailBody = $"Please confirm your acount by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>";
            await _emailSender.SendEmailAsync(user.Email, "Confirm your email" , emailBody);
        }

    }
}
