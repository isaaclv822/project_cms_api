using System.ComponentModel.DataAnnotations;

namespace project_cms.DTOs
{
    public class ArticleRequestDTO
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public string Title { get; set; }

        [Required]
        [StringLength(5000)]
        public string Content { get; set; }
    }
}
