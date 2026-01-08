using GalleryManagement.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace GalleryManagement.Data.DataContext
{
    public class GalleryDataContext : DbContext
    {
        // Constructor - חובה עבור EF!
        public GalleryDataContext(DbContextOptions<GalleryDataContext> options)
            : base(options)
        { }

        // DbSet = טבלה ב-DB
        public DbSet<Artist> Artists { get; set; }
        public DbSet<Artwork> Artworks { get; set; }
        public DbSet<Exhibition> Exhibitions { get; set; }
        public DbSet<Sale> Sales { get; set; }

        // הגדרת Connection String (אופציה אם לא עושים ב-Program.cs)
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Server=(localdb)\MSSQLLocalDB;Database=GalleryManagementDB");
            }
        }

        // Seed Data - נתונים התחלתיים
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // הוספת נתונים התחלתיים
            modelBuilder.Entity<Artist>().HasData(
                new Artist
                {
                    Id = 1,
                    Name = "לאונרדו דה וינצ'י",
                    Biography = "אמן רנסנס איטלקי",
                    Nationality = "איטליה",
                    BirthDate = new DateTime(1452, 4, 15),
                    Style = "רנסנס",
                    Status = "active",
                    CreatedAt = new DateTime(2024, 1, 1)
                },
                new Artist
                {
                    Id = 2,
                    Name = "פבלו פיקאסו",
                    Biography = "אמן קוביסטי ספרדי",
                    Nationality = "ספרד",
                    BirthDate = new DateTime(1881, 10, 25),
                    Style = "קוביזם",
                    Status = "active",
                    CreatedAt = new DateTime(2024, 1, 1)
                }
            );

            modelBuilder.Entity<Artwork>().HasData(
                new Artwork
                {
                    Id = 1,
                    Title = "מונה ליזה",
                    ArtistId = 1,
                    Medium = "שמן על עץ",
                    YearCreated = 1503,
                    Price = 1000000000,
                    Dimensions = "77x53 ס\"מ",
                    Status = "sold",
                    Description = "ציור מפורסם מאת לאונרדו דה וינצ'י"
                },
                new Artwork
                {
                    Id = 2,
                    Title = "גרניקה",
                    ArtistId = 2,
                    Medium = "שמן על קנבס",
                    YearCreated = 1937,
                    Price = 200000000,
                    Dimensions = "349x776 ס\"מ",
                    Status = "available",
                    Description = "יצירת מופת קוביסטית"
                }
            );

            modelBuilder.Entity<Exhibition>().HasData(
                new Exhibition
                {
                    Id = 1,
                    Name = "תערוכת הרנסנס",
                    Description = "תערוכה של יצירות מתקופת הרנסנס",
                    StartDate = new DateTime(2024, 1, 1),
                    EndDate = new DateTime(2024, 3, 31),
                    Location = "אולם ראשי",
                    CuratorName = "ד\"ר שרה כהן"
                }
            );

            modelBuilder.Entity<Sale>().HasData(
                new Sale
                {
                    Id = 1,
                    ArtworkId = 1,
                    BuyerName = "ג'ון סמית'",
                    BuyerEmail = "john@example.com",
                    SalePrice = 50000,
                    SaleDate = new DateTime(2024, 6, 15),
                    PaymentMethod = "כרטיס אשראי",
                    Status = "completed"
                }
            );
            modelBuilder.Entity<Sale>()
                .Property(s => s.SalePrice).HasPrecision(18, 2);
        }
    }
}