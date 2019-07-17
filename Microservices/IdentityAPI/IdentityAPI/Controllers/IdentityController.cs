using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using IdentityAPI.Infrastructure;
using IdentityAPI.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace IdentityAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IdentityController : ControllerBase
    {
        private IdentityDbContext db;
        private IConfiguration configuration;

        public IdentityController(IdentityDbContext identityDbContext, IConfiguration config)
        {
            db = identityDbContext;
            configuration = config;
        }

        //POST /api/identity/register
        [HttpPost("register")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]

        public async Task<ActionResult<dynamic>> RegisterUser([FromBody]UserInfo user)
        {
            if (ModelState.IsValid)
            {
                var addUser = await db.Users.AddAsync(user);
                await db.SaveChangesAsync();
                return Created("", addUser.Entity);
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        //POST /api/identity/token
        [HttpPost("token")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public ActionResult<dynamic> GetToken([FromBody]LoginModel login)
        {
            TryValidateModel(login);
            if (ModelState.IsValid)
            {
                var gentoken = GenerateToken(login);
                if (string.IsNullOrEmpty(gentoken))
                {
                    return Unauthorized();
                }
                else
                {
                    return Ok(new
                    {
                        status = true,
                        token = gentoken
                    });
                }
            }
            else
            {
                return BadRequest(ModelState);
            }
        }

        private string GenerateToken(LoginModel login)
        {
            var user = db.Users.SingleOrDefault(u => u.Email == login.Email && u.Password == login.Password);

            if (user == null)
            {
                return null;
            }
            else
            {
                var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.FirstName),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
                };

                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetValue<string>("Jwt:Secret")));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var token = new JwtSecurityToken(
                    issuer: configuration.GetValue<string>("Jwt:Issuer"),
                    audience: configuration.GetValue<string>("Jwt:Audience"),
                    claims: claims,
                    expires: DateTime.Now.AddMinutes(30),
                    signingCredentials: credentials
                    );

                return new JwtSecurityTokenHandler().WriteToken(token);
            }
        }
    }
}