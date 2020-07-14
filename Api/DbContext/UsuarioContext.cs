using Microsoft.EntityFrameworkCore;
using usuarios_backend.Api.Entities;

namespace usuarios_backend.Api.DbContexts
{
    public class UsuarioContext : DbContext
    {
        public UsuarioContext()
        {

        }
     
        public UsuarioContext(DbContextOptions<UsuarioContext> options)
           : base(options)
        {

        }

        public DbSet<Usuarios> Usuarios { get; set; }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {          

            base.OnModelCreating(modelBuilder);
        }

    }
}
