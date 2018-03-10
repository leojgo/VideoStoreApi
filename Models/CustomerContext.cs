﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LackLusterVideo.Models;
using Microsoft.EntityFrameworkCore;

namespace VideoStoreApi.Models
{
    public class CustomerContext : DbContext
    {
        public CustomerContext(DbContextOptions<CustomerContext> options) : base(options)
        {}

        public DbSet<Customer> Customers { get; set; }
    }
}