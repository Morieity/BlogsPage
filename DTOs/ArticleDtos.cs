using System.ComponentModel.DataAnnotations;

namespace ArticleAPI.DTOs
{
    public class CreateArticleDto
    {
        [Required(ErrorMessage = "标题不能为空")]
        [StringLength(200, ErrorMessage = "标题长度不能超过200个字符")]
        public string Title { get; set; } = string.Empty;
        
        [StringLength(5000, ErrorMessage = "内容长度不能超过5000个字符")]
        public string Content { get; set; } = string.Empty;
    }

    public class UpdateArticleDto
    {
        [Required(ErrorMessage = "标题不能为空")]
        [StringLength(200, ErrorMessage = "标题长度不能超过200个字符")]
        public string Title { get; set; } = string.Empty;
        
        [StringLength(5000, ErrorMessage = "内容长度不能超过5000个字符")]
        public string Content { get; set; } = string.Empty;
    }

    public class ArticleDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
