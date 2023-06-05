using CryptocurrencyPriceTracker_API.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptocurrencyPriceTracker_API.Repositories
{
    public class UserDetailsRepository: IUserDetailsRepository
    {
        private readonly ApplicationContext _dbContext;
        public UserDetailsRepository(ApplicationContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<bool> CheckUserNameExist(string userName)
        {
            return await _dbContext.UserDetails.AnyAsync(x => x.UserName == userName);
        }
        public async Task<bool> CheckEmailExist(string email)
        {
            return await _dbContext.UserDetails.AnyAsync(x => x.Email == email);
        }
        public void RegisterUser(UserDetailEntity entity)
        {
            _dbContext.UserDetails.Add(entity);
        }

        public async Task<int> SaveChangesAsync()
        {
           return await _dbContext.SaveChangesAsync();
        }

        public async Task<IEnumerable<UserDetailEntity>> GetUserDetailsList()
        {
            return await _dbContext.UserDetails.ToListAsync();
        }

        public async Task<UserDetailEntity> GetUserDetail(string username)
        {
            return await _dbContext.UserDetails.Where(x => x.UserName == username).FirstOrDefaultAsync();
        }
    }
}
