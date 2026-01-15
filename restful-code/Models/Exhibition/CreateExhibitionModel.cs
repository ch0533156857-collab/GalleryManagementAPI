using System.ComponentModel.DataAnnotations;

namespace restful_code.Models.Exhibition
{
    public class CreateExhibitionModel
    {
        [Required(ErrorMessage = "שם התערוכה הוא שדה חובה")]
        [StringLength(200, ErrorMessage = "שם התערוכה לא יכול להיות ארוך מ-200 תווים")]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "תיאור לא יכול להיות ארוך מ-1000 תווים")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "תאריך התחלה הוא שדה חובה")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "תאריך סיום הוא שדה חובה")]
        public DateTime EndDate { get; set; }

        [StringLength(200, ErrorMessage = "מיקום לא יכול להיות ארוך מ-200 תווים")]
        public string Location { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "שם האוצר לא יכול להיות ארוך מ-100 תווים")]
        public string CuratorName { get; set; } = string.Empty;
    }
}
