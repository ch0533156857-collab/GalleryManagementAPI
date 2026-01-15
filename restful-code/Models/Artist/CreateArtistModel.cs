using System.ComponentModel.DataAnnotations;

namespace restful_code.Models.Artist
{
    public class CreateArtistModel
    {
        [Required(ErrorMessage = "שם האמן הוא שדה חובה")]
        [StringLength(100, ErrorMessage = "שם האמן לא יכול להיות ארוך מ-100 תווים")]
        public string Name { get; set; } = string.Empty;

        [StringLength(1000, ErrorMessage = "הביוגרפיה לא יכולה להיות ארוכה מ-1000 תווים")]
        public string Biography { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "הלאום לא יכול להיות ארוך מ-50 תווים")]
        public string Nationality { get; set; } = string.Empty;

        [Required(ErrorMessage = "תאריך הלידה הוא שדה חובה")]
        public DateTime BirthDate { get; set; }

        [StringLength(100, ErrorMessage = "הסגנון לא יכול להיות ארוך מ-100 תווים")]
        public string Style { get; set; } = string.Empty;
    }
}
