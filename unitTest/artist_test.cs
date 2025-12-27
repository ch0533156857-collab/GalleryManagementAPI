using Microsoft.AspNetCore.Mvc;
using restful_code.Controllers;
using restful_code.Entities;

namespace unitTest
{
    public class artist_test
    {
        private readonly ArtistsController _artistsController;

        // Constructor - מאתחל את ה-Controller לפני כל טסט
        public artist_test()
        {
            _artistsController = new ArtistsController();
        }

        // טסט 1: בדיקת GetAllArtists - צריך להחזיר OkObjectResult
        [Fact]
        public void GetAllArtists_ReturnsOk()
        {
            // Act - הרצת הפונקציה
            var result = _artistsController.GetAllArtists();

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
    }
}