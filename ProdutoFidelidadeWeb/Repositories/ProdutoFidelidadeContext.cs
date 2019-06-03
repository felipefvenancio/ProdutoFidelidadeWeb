using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProdutoFidelidadeWeb.Repositories
{
    public class ProdutoFidelidadeContext : DbContext
    {

        public ProdutoFidelidadeContext(DbContextOptions options)
            : base(options)
        {
            Database.EnsureCreated();
        }
        public DbSet<Model.Config> Configs { get; set; }
        public DbSet<Model.Log> Logs { get; set; }


    }
}
