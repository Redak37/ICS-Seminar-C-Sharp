using Microsoft.EntityFrameworkCore;

namespace Data
{
    public class MainDbContext : DbContext
    {
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Member> Members { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<Team> Teams { get; set; }
        public DbSet<Membership> Memberships { get; set; }

        public MainDbContext()
        {
        }
        public MainDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Post>()
                .HasMany(s => s.Comments)
                .WithOne(g => g.ParentPost);

            modelBuilder.Entity<Member>()
                .HasMany(s => s.Comments)
                .WithOne(g => g.Author);

            modelBuilder.Entity<Member>()
                .HasMany(s => s.Posts)
                .WithOne(g => g.Author);

            modelBuilder.Entity<Member>()
                .HasIndex(m => m.Email)
                .IsUnique();

            modelBuilder.Entity<Team>()
                .HasMany(s => s.Posts)
                .WithOne(g => g.TeamWithThisPost);

            modelBuilder.Entity<Team>()
                .HasMany(s => s.Members)
                .WithOne(g => g.Team);

            modelBuilder.Entity<Member>()
                .HasMany(s => s.Teams)
                .WithOne(g => g.Member);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Data Source = (localdb)\MSSQLLocalDB; Initial Catalog = ICSProjekt; Integrated Security = True; Connect Timeout = 30; Encrypt = False; TrustServerCertificate = False; ApplicationIntent = ReadWrite; MultiSubnetFailover = False");
            }

            base.OnConfiguring(optionsBuilder);
        }
    }
}