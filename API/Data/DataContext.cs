using System;
using API.Entities;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

// public class DataContext: DbContext
// {
//         public DataContext(DbContextOptions<DataContext> options) : base (options)
//         {
            
//         }

// }

public class DataContext(DbContextOptions options): DbContext(options){

        public DbSet<AppUser> Users {get; set;}
}