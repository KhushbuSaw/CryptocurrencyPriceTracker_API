using CryptocurrencyPriceTracker_API.Entity;
using CryptocurrencyPriceTracker_API.Model.User;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace CryptocurrencyPriceTracker_API.Managers
{
    public static class UserDetailsMapper
    {
        public static UserDetailEntity ToEntity(this UserDetailModel model)
        {
            UserDetailEntity entity = new UserDetailEntity
            {
                FirstName = model.FirstName,
                LastName=model.LastName,
                UserName=model.UserName,
                Password=model.Password,
                Email=model.Email,
                Role=model.Role,   
                PhoneNumber="9999999999"
            };
            return entity;
        }
        public static UserDetailModel ToModel([NotNull] this UserDetailEntity entity)
        {
            return new UserDetailModel
            {
                FirstName = entity.FirstName,
                LastName = entity.LastName,
                UserName = entity.UserName,
                Password = entity.Password,
                Email = entity.Email,
                Role = entity.Role,
            };
        }
    }
}
