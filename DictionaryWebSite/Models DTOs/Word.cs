using System.ComponentModel.DataAnnotations;

namespace DictionaryWebSite.Models_DTOs
{
    public class Word
    {
        public int Id { get; set; }

        [Required (ErrorMessage = "The indigenous word is required.")]
        public string IndigenousWord { get; set; } = string.Empty;

        [Required (ErrorMessage = "The Spanish word is required.")]
        public string SpanishWord { get; set; } = string.Empty;

        [Required (ErrorMessage = "The meaning is required.")]
        public string Meaning { get; set; } = string.Empty;

        [Required (ErrorMessage = "The image URL is required.")]
        public string ImageUrl { get; set; } = string.Empty;

        [Required (ErrorMessage = "The origin is required.")]
        public string Origin { get; set; } = string.Empty;

        [Required (ErrorMessage = "The category is required.")]
        public string Category { get; set; } = string.Empty;
    }
}