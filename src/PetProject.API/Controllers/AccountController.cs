using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using PetProject.DataAccess;
using PetProject.Domain;
using PetProject.DTO;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace PetProject.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : Controller
    {
        private const string JwtExpireHours = "JwtExpireHours";
        private const string Secret = "Secret";
        private const string Issuer = "Iss";
        private const string AuthSection = "Audience";
        private const string Audience = "Aud";
        private readonly IConfiguration _configuration;
        private readonly PetContext _petContext;

        public AccountController(IConfiguration configuration, PetContext petContext)
        {
            _configuration = configuration;
            _petContext = petContext;
        }


        /// <summary>
        /// Get JWT token fo requested client.
        /// </summary>
        /// <param name="model">
        /// </param>
        /// <remarks>The endpoint to get JWT</remarks>
        /// <response code="200">User signed in.</response>
        /// <response code="400">Wrong credentials.</response>
        /// <response code="500">Oops! Something went wrong</response>
        [Route("Token")]
        [HttpPost]
        public async Task<IActionResult> GetToken([FromBody] LoginDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            };

            var user = await _petContext
                .Users
                .FirstOrDefaultAsync(x => x.Phone == model.Phone && x.Password == model.Password);
            if (user is null)
            {
                return BadRequest();
            };

            return GenerateJwtToken(user);
        }

        /// <summary>
        /// Get user role
        /// </summary>
        /// <remarks>The endpoint to current user role</remarks>
        /// <response code="200">User role</response>
        /// <response code="400">User is not exists or you have some validation errors</response>
        /// <response code="500">Oops! Something went wrong</response>
        [Route("Role")]
        [Authorize]
        [HttpGet]
        public string GetRole()
        {
           return HttpContext.User.Claims.First(x => x.Type == ClaimTypes.Role).Value;
        }

        private IActionResult GenerateJwtToken(User model)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.GivenName, model.Name),
                new Claim(JwtRegisteredClaimNames.Sub, model.LastName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sid, model.UserId.ToString()),
            };

            var roleName = Enum.GetName(typeof(Roles), model.Role);
            claims.Add(new Claim(ClaimTypes.Role, roleName));

            var expires = DateTime.Now.AddHours(_configuration.GetValue<double>(JwtExpireHours));
            var section = _configuration.GetSection(AuthSection);
            var signingKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(section.GetValue<string>(Secret)));

            var token = new JwtSecurityToken(
                section.GetValue<string>(Issuer),
                section.GetValue<string>(Audience),
                claims,
                DateTime.Now,
                expires,
                new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256)
            );

            return Ok(new JwtSecurityTokenHandler().WriteToken(token));
        }
    }
}