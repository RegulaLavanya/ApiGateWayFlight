using AuthenticationService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace AuthenticationService.Controllers
{
    //[Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        public AppDbContext _context;
        private readonly IConfiguration _configuration;

        public UserController(AppDbContext context,IConfiguration config)
        {
            _context = context;
            _configuration = config;
        }

        /// <summary>
        /// Validate credentials for login
        /// </summary>
        /// <param name="value">Contains user credentials</param>
        /// <returns>User info if credentials are correct </returns>

        [HttpPost("api/v1.0/flight/login")]
        public ActionResult<Users> Post(Users value)
        {
            var adminLogin = _context.Users.Where(x => string.Equals(x.Email, value.Email)
                                                    && string.Equals(x.Password, value.Password))
                                                    //.Select(mt => new Users
                                                    //{
                                                    //    Email = mt.Email,
                                                    //    Name = mt.Name,
                                                    //    Role = mt.Role,
                                                    //    ID=mt.ID
                                                    //})
                                                   .FirstOrDefault();
        
            
            if (adminLogin != null)
            {
                CreatePasswordHash(adminLogin.Password, out byte[] passwordHash, out byte[] passwordSalt);
                if (!VerifyPasswordHash(adminLogin.Password, passwordHash, passwordSalt))
                {
                    return BadRequest("Incorrect password");
                }

                else
                {
                    string token = CreateToken(adminLogin);
                    var refreshToken = GenerateRefreshToken();
                    //SetRefreshToken(refreshToken);

                    return Ok(new { adminLogin, token });
                }
                //return adminLogin;
            }

            else
                return NotFound("Invalid Credentials Login Failed");
        }

        /// <summary>
        /// Generates refresh toekn
        /// </summary>
        /// <returns>Token</returns>

        private RefreshToken GenerateRefreshToken()
        {
            var refreshToken = new RefreshToken
            {
                //Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Token = Convert.ToBase64String(BitConverter.GetBytes(64)),
                Expires = DateTime.Now.AddDays(7),
                Created = DateTime.Now
            };
            return refreshToken;
        }

        /// <summary>
        /// Create token
        /// </summary>
        /// <param name="value">user info</param>
        /// <returns></returns>
        private string CreateToken(Users value)
        {

            List<Claim> claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,value.Name)
            };
            if (value.Role == "1")
            {
                claims.Add(new Claim(ClaimTypes.Role, "Admin"));
                claims.Add(new Claim("Role", "Admin"));
            }
            else if (value.Role == "2")
            {

                claims.Add(new Claim(ClaimTypes.Role, "User"));
                claims.Add(new Claim("Role", "User"));
            }

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);
            var token = new JwtSecurityToken(claims: claims, expires: DateTime.Now.AddDays(1), signingCredentials: creds);
            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            return jwt;
        }

        /// <summary>
        /// verifies password
        /// </summary>
        /// <param name="password">user password</param>
        /// <param name="passwordHash">password hash</param>
        /// <param name="passwordSalt">password salt</param>
        /// <returns></returns>

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computeHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computeHash.SequenceEqual(passwordHash);
            }
        }

        /// <summary>
        /// Register the user
        /// </summary>
        /// <param name="user">contains register information</param>
        /// <returns></returns>
   

        [HttpPost("api/v1.0/flight/user/register")]
      
        public  ActionResult<Users> UserRegister( Users user)
        {
            try
            {
                var userLogin = _context.Users.Where(x => string.Equals(x.Email, user.Email) && string.Equals(x.Role, "2")).FirstOrDefault();
                // && string.Equals(x.Role, "2"))
             
                if (userLogin != null)
                    return NotFound("User already registerd with email");
                else
                {
                    CreatePasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);

                    var userData = new Users();
                    userData.Name = user.Name;
                    userData.Email = user.Email;
                    userData.Password = user.Password;
                    userData.Role = "2";
                    userData.PasswordHash = passwordHash;
                    userData.PasswordSalt = passwordSalt;

                    _context.Users.Add(userData);
                     _context.SaveChanges();

                    return Ok(userData);
                }
            }
            catch (Exception ex)
            {
                return BadRequest("Exception in user registration method" + ex.ToString());
            }

        }

        /// <summary>
        /// Creates password hash
        /// </summary>
        /// <param name="password">password</param>
        /// <param name="passwordHash">passwordHash</param>
        /// <param name="passwordSalt">passwordSalt</param>
        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }
    }
}
