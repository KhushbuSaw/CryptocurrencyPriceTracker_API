using CryptocurrencyPriceTracker_API.Entity;
using CryptocurrencyPriceTracker_API.Model.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace CryptocurrencyPriceTracker_API.Interfaces
{
    public interface IUserDetailsManager
    {
        public Task<TokenDtoModel> AuthenticateUser(UserDetailModel model);
        public Task RegisterUser(UserDetailModel model);
        public string CheckPasswordStregth(string password);
        public Task<IEnumerable<UserDetailModel>> GetUserDetailsList();
        public ClaimsPrincipal GetPrincipalFromExpiredToken(string expiredToken);
        public Task<UserDetailEntity> GetUserDetail(string username);
        public string CreateRefreshToken();
        public string CreateJwt(UserDetailEntity user);
        public Task<UserDetailEntity> GetUserDetailUsingEmail(string email);
        public Task<UserDetailEntity> SetResetPasswordToken(UserDetailEntity entity, string email);
        public Task<UserDetailEntity> ResetUserPassword(ResetPasswordModel model, UserDetailEntity entity);
    }
}
