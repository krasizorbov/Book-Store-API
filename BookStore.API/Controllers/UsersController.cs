namespace BookStore.API.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using BookStore.API.Contracts;
    using BookStore.API.DTOs;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.IdentityModel.Tokens;
    using Microsoft.Extensions.Configuration;
    using System.Security.Claims;
    using System.IdentityModel.Tokens.Jwt;
    using BookStore.API.Common;

    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private const int tokenExpirationDays = 1;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly UserManager<IdentityUser> userManager;
        private readonly ILoggerService logger;
        private readonly IConfiguration config;
        public UsersController(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, ILoggerService logger, IConfiguration config)
        {
            this.signInManager = signInManager;
            this.userManager = userManager;
            this.logger = logger;
            this.config = config;
        }

        /// <summary>
        /// Register a new user
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        [Route("register")]
        [HttpPost]
        public async Task<IActionResult> Register([FromBody] UserDTO userDTO)
        {
            var location = GetControllerActionNames();
            try
            {
                var username = userDTO.EmailAddress;
                var password = userDTO.Password;
                logger.LogInfo($"{location}: {GlobalConstants.TryUserRegistration}");
                var user = new IdentityUser { Email = username, UserName = username };
                var result = await userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    foreach (var error in result.Errors)
                    {
                        logger.LogError($"{location}: {error.Code} {error.Description}");
                    }
                    return InternalError($"{location}: {GlobalConstants.UserRegistrationFailed}");
                }
                logger.LogInfo($"{location}: {GlobalConstants.UserRegistrationSuccess}");
                return Ok(new { result.Succeeded });
            }
            catch (Exception e)
            {
                return InternalError($"{location}: {e.Message} - {e.InnerException}");
            }
        }

        /// <summary>
        /// User login end points
        /// </summary>
        /// <param name="userDTO"></param>
        /// <returns></returns>
        [Route("login")]
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] UserDTO userDTO)
        {
            var location = GetControllerActionNames();
            try
            {
                var username = userDTO.EmailAddress;
                var password = userDTO.Password;
                logger.LogInfo($"{location}: {GlobalConstants.TryUserLogin}");
                var result = await signInManager.PasswordSignInAsync(username, password, false, false);
                if (result.Succeeded)
                {
                    logger.LogInfo($"{location}: {GlobalConstants.UserLoginSuccess}");
                    var user = await userManager.FindByEmailAsync(username);
                    var tokenString = await GenerateJsonWebToken(user);
                    return Ok(new { token = tokenString});
                }
                logger.LogInfo($"{location}: {GlobalConstants.UserLoginFailed}");
                return Unauthorized(userDTO);
            }
            catch (Exception e)
            {
                return InternalError($"{location}: {e.Message} - {e.InnerException}");
            }
            
        }

        private async Task<string> GenerateJsonWebToken(IdentityUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.NameIdentifier, user.Id)
            };
            var roles = await userManager.GetRolesAsync(user);
            claims.AddRange(roles.Select(r => new Claim(ClaimsIdentity.DefaultRoleClaimType, r)));
            var token = new JwtSecurityToken(
                config["Jwt:Issuer"], 
                config["Jwt:Issuer"], 
                claims, null, 
                expires: DateTime.UtcNow.AddDays(tokenExpirationDays),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GetControllerActionNames()
        {
            var controller = ControllerContext.ActionDescriptor.ControllerName;
            var action = ControllerContext.ActionDescriptor.ActionName;
            return $"{controller} - {action}";
        }
        private ObjectResult InternalError(string message)
        {
            logger.LogError(message);
            return StatusCode(500, $"{GlobalConstants.Error500}");
        }
    }
}
