using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using Task2.Data;

namespace Task2.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DataContext _dataContext;

        public AuthController(DataContext dataContext, IConfiguration configuration)
        {
            _configuration = configuration;
            _dataContext = dataContext;
        }

        private readonly IConfiguration _configuration;

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using(var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private string CreateToken(User user, bool isAdmin)
        {
            string userRole = isAdmin ? "Admin" : "User";
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, userRole),
                new Claim("UserId", user.UserId.ToString())
            };

            var authSecretToken = _configuration.GetSection("AuthSecret:Token").Value;
            if (authSecretToken == null)
            {
                throw new Exception("AuthSecret:Token configuration value is missing.");
            }
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(authSecretToken));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;

        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt) 
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash); 
             
            }
        }

        [HttpPost("register")]
        public async Task<ActionResult<User>> RegisterUser(UserDto request)
        {
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var userFromDb = await _dataContext.Users.FirstOrDefaultAsync(user => user.Username == request.Username);

            if (userFromDb != null && userFromDb.UserId != Guid.Empty)
                return BadRequest($"User with username: {request.Username} already exists!");

            User user = new User
            {
                UserId = Guid.NewGuid(),
                Username = request.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            _dataContext.Users.Add(user);
            await _dataContext.SaveChangesAsync();

            return Ok(await _dataContext.Users.FirstOrDefaultAsync(user => user.Username == request.Username));

        }

        [HttpPost("register-admin")]
        public async Task<ActionResult<User>> RegisterAdminUser(UserDto request)
        {

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);

            var userFromDb = await _dataContext.Users.FirstOrDefaultAsync(user => user.Username == request.Username);

            if (userFromDb != null && userFromDb.UserId != Guid.Empty)
                return BadRequest($"User with username: {request.Username} already exists!");

            User user = new User
            {
                UserId = Guid.NewGuid(),
                Username = request.Username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                IsAdminUser = true
            };

            _dataContext.Users.Add(user);
            await _dataContext.SaveChangesAsync();

            return Ok(await _dataContext.Users.FirstOrDefaultAsync(user => user.Username == request.Username));



        }

        [HttpPost("login")]
        public async Task<ActionResult<object>> Login(UserDto request)
        {
            var userFromDb = await _dataContext.Users.FirstOrDefaultAsync(user => user.Username == request.Username);

            if (userFromDb == null || userFromDb.UserId == Guid.Empty)
                return BadRequest(new { Error = $"username: {request.Username} does not exists!" });

            
            if(!VerifyPasswordHash(request.Password, userFromDb.PasswordHash, userFromDb.PasswordSalt))
            {
                return BadRequest(new { Error = "Invalid Password Entered!" });

            }

            string token = CreateToken(userFromDb, userFromDb.IsAdminUser);
            return Ok(new { Token = token, UserId = userFromDb.UserId, Username =  userFromDb.Username, IsAdminUser = userFromDb.IsAdminUser });
        }

    }
}
