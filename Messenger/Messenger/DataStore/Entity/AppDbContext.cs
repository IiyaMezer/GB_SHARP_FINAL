using Microsoft.EntityFrameworkCore;

namespace SEM4_Swagger.DataStore.Entity
{
    public class AppDbContext: DbContext
    {
        private static string _connectionstring;
        public AppDbContext()
        {

        }
        public AppDbContext(string connectionstring)
        {
            _connectionstring = connectionstring;
        }
        
        public DbSet<UserEntity> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(_connectionstring).UseLazyLoadingProxies();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserEntity>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.UserName).IsUnique();

                entity.Property(e => e.Password)
                .HasMaxLength(255)
                .IsRequired();

                entity.HasOne(e => e.Role)
                .WithMany(e => Users);

            });
            modelBuilder.Entity<Role>(e =>
            {
                e.HasKey(x => x.RoleType);
                e.HasIndex(x => x.Name).IsUnique();
            });
        }
    }
}
