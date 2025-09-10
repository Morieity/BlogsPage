using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArticleAPI.Data;
using ArticleAPI.Models;
using ArticleAPI.DTOs;

namespace ArticleAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ArticlesController : ControllerBase
    {
        private readonly ArticleDbContext _context;

        public ArticlesController(ArticleDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 获取所有文章
        /// </summary>
        /// <returns>文章列表</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ArticleDto>>> GetArticles()
        {
            var articles = await _context.Articles
                .Select(a => new ArticleDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    Content = a.Content,
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt
                })
                .ToListAsync();

            return Ok(articles);
        }

        /// <summary>
        /// 根据ID获取单个文章
        /// </summary>
        /// <param name="id">文章ID</param>
        /// <returns>文章详情</returns>
        [HttpGet("{id}")]
        public async Task<ActionResult<ArticleDto>> GetArticle(int id)
        {
            var article = await _context.Articles
                .Where(a => a.Id == id)
                .Select(a => new ArticleDto
                {
                    Id = a.Id,
                    Title = a.Title,
                    Content = a.Content,
                    CreatedAt = a.CreatedAt,
                    UpdatedAt = a.UpdatedAt
                })
                .FirstOrDefaultAsync();

            if (article == null)
            {
                return NotFound($"文章 ID {id} 不存在");
            }

            return Ok(article);
        }

        /// <summary>
        /// 创建新文章
        /// </summary>
        /// <param name="createArticleDto">文章创建数据</param>
        /// <returns>创建的文章</returns>
        [HttpPost]
        public async Task<ActionResult<ArticleDto>> CreateArticle(CreateArticleDto createArticleDto)
        {
            if (string.IsNullOrWhiteSpace(createArticleDto.Title))
            {
                return BadRequest("标题不能为空");
            }

            if (string.IsNullOrWhiteSpace(createArticleDto.Content))
            {
                return BadRequest("内容不能为空");
            }

            var article = new Article
            {
                Title = createArticleDto.Title.Trim(),
                Content = createArticleDto.Content.Trim(),
                CreatedAt = DateTime.UtcNow
            };

            _context.Articles.Add(article);
            await _context.SaveChangesAsync();

            var articleDto = new ArticleDto
            {
                Id = article.Id,
                Title = article.Title,
                Content = article.Content,
                CreatedAt = article.CreatedAt
            };

            return CreatedAtAction(nameof(GetArticle), new { id = article.Id }, articleDto);
        }

        /// <summary>
        /// 更新文章
        /// </summary>
        /// <param name="id">文章ID</param>
        /// <param name="updateArticleDto">文章更新数据</param>
        /// <returns>更新结果</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateArticle(int id, UpdateArticleDto updateArticleDto)
        {
            if (string.IsNullOrWhiteSpace(updateArticleDto.Title))
            {
                return BadRequest("标题不能为空");
            }

            if (string.IsNullOrWhiteSpace(updateArticleDto.Content))
            {
                return BadRequest("内容不能为空");
            }

            var article = await _context.Articles.FindAsync(id);

            if (article == null)
            {
                return NotFound($"文章 ID {id} 不存在");
            }

            article.Title = updateArticleDto.Title.Trim();
            article.Content = updateArticleDto.Content.Trim();
            article.UpdatedAt = DateTime.UtcNow;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ArticleExists(id))
                {
                    return NotFound($"文章 ID {id} 不存在");
                }
                throw;
            }

            return NoContent();
        }

        /// <summary>
        /// 删除文章
        /// </summary>
        /// <param name="id">文章ID</param>
        /// <returns>删除结果</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteArticle(int id)
        {
            var article = await _context.Articles.FindAsync(id);

            if (article == null)
            {
                return NotFound($"文章 ID {id} 不存在");
            }

            _context.Articles.Remove(article);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ArticleExists(int id)
        {
            return _context.Articles.Any(e => e.Id == id);
        }
    }
}
