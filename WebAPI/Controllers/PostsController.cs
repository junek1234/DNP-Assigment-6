using System.Collections;
using ApiContracts;
using Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly IPostRepository PostRepo;
        private readonly IUserRepository userRepository;
        public PostsController(IPostRepository PostRepo, IUserRepository userRepository)
        {
            this.PostRepo = PostRepo;
            this.userRepository = userRepository;
        }

        [HttpPost]
        public async Task<ActionResult<Post>> AddPost([FromBody] CreatePostDto request)
        {
            Post Post = new(request.Title, request.Body, request.UserId);
            Post created = await PostRepo.AddAsync(Post);
            PostDto dto = new()
            {
                Id = created.Id,
                Title = created.Title,
                Body = created.Body,
                UserId = created.UserId
            };
            return Created($"/Posts/{dto.Id}", created);
        }

        [HttpPatch("{id:int}")]
        public async Task<ActionResult<Post>> UpdatePost([FromRoute] int id, [FromBody] UpdatePostDto request)
        {
            Post PostToUpdate = await PostRepo.GetSingleAsync(id);
            PostToUpdate.Body = request.Body;
            PostToUpdate.Title = request.Title;
            await PostRepo.UpdateAsync(PostToUpdate);

            return NoContent();
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<Post>> GetPost([FromRoute] int id)
        {
            Post PostToGet = await PostRepo.GetSingleAsync(id);
            return Ok(PostToGet);
        }
        [HttpGet]
        public async Task<ActionResult<List<Post>>> GetPosts([FromQuery] string? titleContains = null, [FromQuery] int? writtenById = null, [FromQuery] string? writtenByName = null)
        {
            IEnumerable<Post> posts = PostRepo.GetMany();
            
            if (titleContains!=null)
            {
                posts = posts.Where(p => p.Title.Contains(titleContains));
            }

            if (writtenById!=null)
            {
                posts = posts.Where(p => p.UserId == writtenById);
            }

            if (writtenByName!=null)
            {
                var userIds = userRepository.GetMany()
                    .Where(u => u.Username.Equals(writtenByName))
                    .Select(u => u.Id);
                posts = posts.Where(p => userIds.Contains(p.UserId));
            }

            return Ok(posts.ToList());
        }
        [HttpDelete("{id:int}")]
        public async Task<ActionResult<Post>> DeletePost([FromRoute] int id)
        {
            await PostRepo.DeleteAsync(id);
            return NoContent();
        }
    }

    
}
