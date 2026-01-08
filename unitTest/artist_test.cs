using GalleryManagement.Core.Entities;
using GalleryManagement.Core.Interfaces;
using GalleryManagement.Data.DataContext;
using GalleryManagement.Data.Repositories;
using GalleryManagement.Service.Services;
using Microsoft.AspNetCore.Mvc;
using restful_code.Controllers;
using Microsoft.EntityFrameworkCore;


namespace unitTest
{
    public class artist_test
    {
        private readonly ArtistsController _artistsController;
        private readonly IArtistService _artistService;
        private readonly IArtistRepository _artistRepository;
        private readonly GalleryDataContext _context;

        // Constructor - מאתחל את כל השכבות לפני כל טסט
        public artist_test()
        {
            // 1. יצירת DataContext (המחסן)
            _context = new GalleryDataContext(new Microsoft.EntityFrameworkCore.DbContextOptionsBuilder<GalleryDataContext>()
                .UseInMemoryDatabase(databaseName: "GalleryTestDB")
                .Options);

            // 2. יצירת Repository (גישה לנתונים)
            _artistRepository = new ArtistRepository(_context);

            // 3. יצירת Service (הלוגיקה)
            _artistService = new ArtistService(_artistRepository, _context);

            // 4. יצירת Controller (הממשק)
            _artistsController = new ArtistsController(_artistService);
        }

        // טסט 1: בדיקת GetAllArtists - צריך להחזיר OkObjectResult
        [Fact]
        public void GetAllArtists_ReturnsOk()
        {
            // Act - הרצת הפונקציה
            var result = _artistsController.GetAllArtists(null);

            // Assert - בדיקה שהתוצאה היא OkObjectResult
            Assert.IsType<OkObjectResult>(result.Result);
        }

        // טסט 2: בדיקת GetArtist עם ID קיים - צריך להחזיר את האמן הנכון
        [Fact]
        public void GetArtist_WithValidId_ReturnsCorrectArtist()
        {
            // Arrange - הכנת הנתונים
            int artistId = 1;

            // Act - הרצת הפונקציה
            var result = _artistsController.GetArtist(artistId);

            // Assert - בדיקות
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var artist = Assert.IsType<Artist>(okResult.Value);
            Assert.Equal(artistId, artist.Id);
            Assert.Equal("לאונרדו דה וינצ'י", artist.Name);
        }

        // טסט 3: בדיקת GetArtist עם ID לא קיים - צריך להחזיר NotFound
        [Fact]
        public void GetArtist_WithInvalidId_ReturnsNotFound()
        {
            // Arrange - הכנת נתונים (ID שלא קיים)
            int invalidId = 999;

            // Act - הרצת הפונקציה
            var result = _artistsController.GetArtist(invalidId);

            // Assert - בדיקה שהתוצאה היא NotFoundObjectResult
            Assert.IsType<NotFoundObjectResult>(result.Result);
        }

        // טסט 4: בדיקת CreateArtist - האמן נוצר בהצלחה
        [Fact]
        public void CreateArtist_ReturnsCreatedAtAction()
        {
            // Arrange - יצירת אמן חדש
            var newArtist = new Artist
            {
                Name = "וינסנט ואן גוך",
                Biography = "צייר פוסט-אימפרסיוניסטי הולנדי",
                Nationality = "הולנד",
                BirthDate = new DateTime(1853, 3, 30),
                Style = "פוסט-אימפרסיוניזם"
            };

            // Act - הרצת הפונקציה
            var result = _artistsController.CreateArtist(newArtist);

            // Assert - בדיקות
            var createdResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var artist = Assert.IsType<Artist>(createdResult.Value);
            Assert.Equal("וינסנט ואן גוך", artist.Name);
            Assert.True(artist.Id > 0); // בדיקה שקיבל ID
            Assert.Equal("active", artist.Status); // בדיקת ברירת המחדל
        }

        // טסט 5: בדיקת UpdateArtistStatus עם סטטוס תקין
        [Fact]
        public void UpdateArtistStatus_WithValidStatus_ReturnsOk()
        {
            // Arrange - הכנת נתונים
            int artistId = 1;
            var statusUpdate = new StatusUpdate { Status = "inactive" };

            // Act - הרצת הפונקציה
            var result = _artistsController.UpdateArtistStatus(artistId, statusUpdate);

            // Assert - בדיקות
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var artist = Assert.IsType<Artist>(okResult.Value);
            Assert.Equal("inactive", artist.Status);
        }

        // טסט 6: בדיקת UpdateArtistStatus עם סטטוס לא תקין - צריך להחזיר BadRequest
        [Fact]
        public void UpdateArtistStatus_WithInvalidStatus_ReturnsBadRequest()
        {
            // Arrange - הכנת נתונים (סטטוס לא חוקי)
            int artistId = 1;
            var statusUpdate = new StatusUpdate { Status = "deleted" };

            // Act - הרצת הפונקציה
            var result = _artistsController.UpdateArtistStatus(artistId, statusUpdate);

            // Assert - בדיקה שהתוצאה היא BadRequest
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        // טסט 7: בדיקת GetArtistArtworks - מחזיר את היצירות של האמן
        [Fact]
        public void GetArtistArtworks_ReturnsCorrectArtworks()
        {
            // Arrange - ID של לאונרדו (יש לו את מונה ליזה)
            int artistId = 1;

            // Act - הרצת הפונקציה
            var result = _artistsController.GetArtistArtworks(artistId);

            // Assert - בדיקות
            var okResult = Assert.IsType<OkObjectResult>(result.Result);
            var artworks = Assert.IsAssignableFrom<IEnumerable<Artwork>>(okResult.Value);
            Assert.NotEmpty(artworks); // יש לפחות יצירה אחת
        }

        // טסט 8: בדיקת CreateArtist עם שם ריק - צריך להחזיר BadRequest
        [Fact]
        public void CreateArtist_WithEmptyName_ReturnsBadRequest()
        {
            // Arrange - אמן עם שם ריק
            var newArtist = new Artist
            {
                Name = "", // שם ריק!
                Biography = "ביוגרפיה",
                Nationality = "ישראל",
                BirthDate = new DateTime(1990, 1, 1),
                Style = "מודרני"
            };

            // Act - הרצת הפונקציה
            var result = _artistsController.CreateArtist(newArtist);

            // Assert - בדיקה שהתוצאה היא BadRequest
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }

        // טסט 9: בדיקת CreateArtist עם תאריך לידה בעתיד - צריך להחזיר BadRequest
        [Fact]
        public void CreateArtist_WithFutureBirthDate_ReturnsBadRequest()
        {
            // Arrange - אמן עם תאריך לידה בעתיד
            var newArtist = new Artist
            {
                Name = "אמן עתידי",
                Biography = "עוד לא נולד",
                Nationality = "מאדים",
                BirthDate = DateTime.Now.AddYears(10), // בעתיד!
                Style = "מודרני"
            };

            // Act - הרצת הפונקציה
            var result = _artistsController.CreateArtist(newArtist);

            // Assert - בדיקה שהתוצאה היא BadRequest
            Assert.IsType<BadRequestObjectResult>(result.Result);
        }
    }
}