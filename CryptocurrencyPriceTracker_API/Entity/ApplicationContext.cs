﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CryptocurrencyPriceTracker_API.Entity
{
    public class ApplicationContext:DbContext
    {
        public ApplicationContext(DbContextOptions<ApplicationContext> options) :base(options)
        {

        }
        public DbSet<UserDetailEntity> UserDetails { get; set; }
    }
}
