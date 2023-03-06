using Rubik.API.Models;

namespace Rubik.API.Services
{
    public interface IUsersService : IEntitiesService<UserEntity>
    {
        UserEntity Update(UserEntity userEntity, UserUpdate userUpdate);
        AuthenticateResponse? Authenticate(AuthenticateRequest req);
        UserEntity? Register(RegisterRequest req);
    }
}