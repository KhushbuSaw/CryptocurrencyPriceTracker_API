using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace CryptocurrencyPriceTracker_API.Entity
{
    public class UserDetailEntity
    {
        [Key]
        public int UserId{ get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        [NotMappedAttribute]
        public string Token { get; set; }
        public int Role { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }
}
