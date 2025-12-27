using restful_code.Entities;

namespace restful_code.Data
{
    public class DataContext
    {
        // 🌟 static = משותף לכל האובייקטים!
        private static List<Artist>? _artists;
        private static List<Artwork>? _artworks;
        private static List<Exhibition>? _exhibitions;
        private static List<Sale>? _sales;

        private static int _nextArtistId;
        private static int _nextArtworkId;
        private static int _nextExhibitionId;
        private static int _nextSaleId;

        // Properties לגישה לנתונים
        public List<Artist> Artists => _artists ??= InitializeArtists();
        public List<Artwork> Artworks => _artworks ??= InitializeArtworks();
        public List<Exhibition> Exhibitions => _exhibitions ??= InitializeExhibitions();
        public List<Sale> Sales => _sales ??= InitializeSales();

        public int NextArtistId
        {
            get => _nextArtistId;
            set => _nextArtistId = value;
        }

        public int NextArtworkId
        {
            get => _nextArtworkId;
            set => _nextArtworkId = value;
        }

        public int NextExhibitionId
        {
            get => _nextExhibitionId;
            set => _nextExhibitionId = value;
        }

        public int NextSaleId
        {
            get => _nextSaleId;
            set => _nextSaleId = value;
        }

        // מתודות אתחול פרטיות
        private static List<Artist> InitializeArtists()
        {
            _nextArtistId = 3;
            return new List<Artist>
            {
                new Artist
                {
                    Id = 1,
                    Name = "לאונרדו דה וינצ'י",
                    Biography = "אמן רנסנס איטלקי",
                    Nationality = "איטליה",
                    BirthDate = new DateTime(1452, 4, 15),
                    Style = "רנסנס",
                    Status = "active"
                },
                new Artist
                {
                    Id = 2,
                    Name = "פבלו פיקאסו",
                    Biography = "אמן קוביסטי ספרדי",
                    Nationality = "ספרד",
                    BirthDate = new DateTime(1881, 10, 25),
                    Style = "קוביזם",
                    Status = "active"
                }
            };
        }

        private static List<Artwork> InitializeArtworks()
        {
            _nextArtworkId = 3;
            return new List<Artwork>
            {
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
            };
        }

        private static List<Exhibition> InitializeExhibitions()
        {
            _nextExhibitionId = 2;
            return new List<Exhibition>
            {
                new Exhibition
                {
                    Id = 1,
                    Name = "תערוכת הרנסנס",
                    Description = "תערוכה של יצירות מתקופת הרנסנס",
                    StartDate = new DateTime(2024, 1, 1),
                    EndDate = new DateTime(2024, 3, 31),
                    Location = "אולם ראשי",
                    CuratorName = "ד\"ר שרה כהן",
                    ArtworkIds = new List<int> { 1 }
                }
            };
        }

        private static List<Sale> InitializeSales()
        {
            _nextSaleId = 2;
            return new List<Sale>
            {
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
            };
        }
    }
}