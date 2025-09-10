using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ArticleAPI.Data;
using ArticleAPI.Models;
using ArticleAPI.DTOs;

namespace ArticleAPI.Controllers
{
    /// <summary>
    /// 文章管理 API 控制器
    /// 提供文章的增删改查功能
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    [Produces("application/json")]
    public class ArticlesController : ControllerBase
    {
        private readonly ArticleDbContext _context;

        public ArticlesController(ArticleDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 获取所有文章列表
        /// </summary>
        /// <returns>返回所有文章的列表</returns>
        /// <response code="200">成功返回文章列表</response>
        /// <response code="500">服务器内部错误</response>
        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<ArticleDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        /// 根据ID获取单个文章详情
        /// </summary>
        /// <param name="id">文章的唯一标识符</param>
        /// <returns>返回指定ID的文章详情</returns>
        /// <response code="200">成功返回文章详情</response>
        /// <response code="404">未找到指定ID的文章</response>
        /// <response code="500">服务器内部错误</response>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ArticleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        /// <param name="createArticleDto">包含文章标题和内容的创建数据</param>
        /// <returns>返回创建成功的文章信息</returns>
        /// <response code="201">文章创建成功</response>
        /// <response code="400">请求参数无效（标题或内容为空）</response>
        /// <response code="500">服务器内部错误</response>
        [HttpPost]
        [ProducesResponseType(typeof(ArticleDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        /// 更新指定ID的文章信息
        /// </summary>
        /// <param name="id">要更新的文章ID</param>
        /// <param name="updateArticleDto">包含更新数据的文章信息</param>
        /// <returns>更新操作的结果</returns>
        /// <response code="200">文章更新成功</response>
        /// <response code="400">请求参数无效</response>
        /// <response code="404">未找到指定ID的文章</response>
        /// <response code="500">服务器内部错误</response>
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
        /// 删除指定ID的文章
        /// </summary>
        /// <param name="id">要删除的文章ID</param>
        /// <returns>删除操作的结果</returns>
        /// <response code="204">文章删除成功</response>
        /// <response code="404">未找到指定ID的文章</response>
        /// <response code="500">服务器内部错误</response>
        [HttpDelete("{id}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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
