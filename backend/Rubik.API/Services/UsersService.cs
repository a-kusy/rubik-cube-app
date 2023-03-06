using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Rubik.API.Models;

namespace Rubik.API.Services
{
    public class UsersService : EntitiesService<UserEntity>, IUsersService
    {
        public UsersService(ApplicationDbContext dbContext, IConfiguration configuration) : base(dbContext)
        {
            this.configuration = configuration;
        }

        private readonly IConfiguration configuration;

        public override List<UserEntity> GetAll()
        {
            return DbContext.Users.Include(x => x.Scores).ToList();
        }

        public UserEntity? Update(UserEntity userEntity, UserUpdate userUpdate)
        {
            if (userUpdate.Username is not null)
            {
                if (DbContext.Users.Any(x => x.Username == userUpdate.Username))
                {
                    return null;
                }
                userEntity.Username = userUpdate.Username;
            }

            if (userUpdate.Password is not null)
            {
                userEntity.Password = BCrypt.Net.BCrypt.HashPassword(userUpdate.Password);
            }

            if (userUpdate.IsArchival is not null)
            {
                userEntity.IsArchival = userUpdate.IsArchival.Value;
            }

            if (userUpdate.Email is not null)
            {
                userEntity.Email = userUpdate.Email;
            }
            userEntity.ModifiedDate = DateTime.Now;

            DbContext.Update(userEntity);
            DbContext.SaveChanges();

            return userEntity;
        }

        public AuthenticateResponse? Authenticate(AuthenticateRequest req)
        {
            var user = DbContext.Users.SingleOrDefault(x => x.Username == req.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(req.Password, user.Password) || user.IsArchival)
            {
                return null;
            }

            var token = GenerateJwtToken(user);

            return new AuthenticateResponse(user, token);
        }

        private string GenerateJwtToken(UserEntity user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration["JWT:Secret"]);
            var claims = new List<Claim> { new Claim("id", user.Id.ToString()) };
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims.ToArray()),
                Expires = DateTime.UtcNow.AddHours(1),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature),
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }

        public UserEntity? Register(RegisterRequest user)
        {
            if (DbContext.Users.Any(x => x.Username == user.Username))
            {
                return null;
            }

            var hashPassword = BCrypt.Net.BCrypt.HashPassword(user.Password);
            var newUser = new UserEntity(user.Username, hashPassword, user.Email);

            return Add(newUser);
        }
    }
}