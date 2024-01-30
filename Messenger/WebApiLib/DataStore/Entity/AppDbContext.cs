using Microsoft.EntityFrameworkCore;


namespace WebApiLib.DataStore.Entity
{
    public class AppDbContext : DbContext
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
        public DbSet<MessageEntity> Messages { get; set; }
        public DbSet<RoleEntity> Roles { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_connectionstring);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<UserEntity>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.UserName).IsUnique();

                entity.Property(e => e.Password)
                .HasMaxLength(20)
                .IsRequired();
                entity.Property(e => e.UserName)
                .HasMaxLength(255);

                entity.HasOne(e => e.RoleType)
                .WithMany(e => Users);

            });

            modelBuilder.Entity<MessageEntity>(entity =>
            {
                entity.HasKey(x => x.Id);
                entity.HasIndex(x => x.SenderId).IsUnique();
                entity.HasIndex(x => x.RecipientId).IsUnique();

                entity.Property(e => e.Text)
                    .HasMaxLength(1000);

                entity.HasOne(x => x.Sender)
                    .WithMany(x => x.SendMessages)
                    .HasForeignKey(x => x.SenderId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(x => x.Recipient)
                    .WithMany(x => x.ReceiveMessages)
                    .HasForeignKey(x => x.RecipientId)
                    .IsRequired()
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Role>(e =>
            {
                e.HasKey(x => x.RoleType);
                e.HasIndex(x => x.Name).IsUnique();
            });
        }
    }
}
