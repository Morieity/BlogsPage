using System.ComponentModel.DataAnnotations;

namespace ArticleAPI.DTOs
{
    /// <summary>
    /// 创建文章的数据传输对象
    /// </summary>
    public class CreateArticleDto
    {
        /// <summary>
        /// 文章标题
        /// </summary>
        /// <example>我的第一篇文章</example>
        [Required(ErrorMessage = "标题不能为空")]
        [StringLength(200, ErrorMessage = "标题长度不能超过200个字符")]
        public string Title { get; set; } = string.Empty;
        
        /// <summary>
        /// 文章内容
        /// </summary>
        /// <example>这是文章的主要内容...</example>
        [StringLength(5000, ErrorMessage = "内容长度不能超过5000个字符")]
        public string Content { get; set; } = string.Empty;
    }

    /// <summary>
    /// 更新文章的数据传输对象
    /// </summary>
    public class UpdateArticleDto
    {
        /// <summary>
        /// 文章标题
        /// </summary>
        /// <example>更新后的文章标题</example>
        [Required(ErrorMessage = "标题不能为空")]
        [StringLength(200, ErrorMessage = "标题长度不能超过200个字符")]
        public string Title { get; set; } = string.Empty;
        
        /// <summary>
        /// 文章内容
        /// </summary>
        /// <example>更新后的文章内容...</example>
        [StringLength(5000, ErrorMessage = "内容长度不能超过5000个字符")]
        public string Content { get; set; } = string.Empty;
    }

    /// <summary>
    /// 文章信息的数据传输对象
    /// </summary>
    public class ArticleDto
    {
        /// <summary>
        /// 文章唯一标识符
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }
        
        /// <summary>
        /// 文章标题
        /// </summary>
        /// <example>ASP.NET Core 开发指南</example>
        public string Title { get; set; } = string.Empty;
        
        /// <summary>
        /// 文章内容
        /// </summary>
        /// <example>这是关于 ASP.NET Core 开发的详细指南...</example>
        public string Content { get; set; } = string.Empty;
        
        /// <summary>
        /// 文章创建时间
        /// </summary>
        /// <example>2024-01-01T10:00:00Z</example>
        public DateTime CreatedAt { get; set; }
        
        /// <summary>
        /// 文章最后更新时间
        /// </summary>
        /// <example>2024-01-02T15:30:00Z</example>
        public DateTime? UpdatedAt { get; set; }
    }
}
