using API_CRUD.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace API_CRUD.Context
{
    public class KeyValueServiceContext : DbContext
    {
        public KeyValueServiceContext(DbContextOptions<KeyValueServiceContext> options) : base(options)
        {
        }
        public DbSet<KeyValueRequest> KeyValues { get; set; }
    }
}
