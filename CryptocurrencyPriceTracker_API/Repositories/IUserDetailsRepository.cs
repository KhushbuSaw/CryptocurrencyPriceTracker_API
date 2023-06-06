using CryptocurrencyPriceTracker_API.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptocurrencyPriceTracker_API.Repositories
{
    public interface IUserDetailsRepository
    {
        public Task<UserDetailEntity> GetUserDetail(string username);
        public void RegisterUser(UserDetailEntity entity);
        public Task<int> SaveChangesAsync();
        public Task<bool> CheckUserNameExist(string userName);
        public Task<bool> CheckEmailExist(string email);
        public Task<IEnumerable<UserDetailEntity>> GetUserDetailsList();
        public Task<UserDetailEntity> GetUserDetailUsingEmail(string email);
        public void UpdateUserPassword(UserDetailEntity entity);
    }
}
