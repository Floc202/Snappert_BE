using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SWD392.Snappet.API.RequestModel;
using SWD392.Snappet.API.ResponseModel;
using SWD392.Snappet.Repository.BusinessModels;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWD392.Snappet.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class PostController : ControllerBase
    {
        private readonly SWD392_SNAPPET_DBContext _context;

        public PostController(SWD392_SNAPPET_DBContext context)
        {
            _context = context;
        }

        // GET: api/Post
        [HttpGet]
        public async Task<ActionResult<IEnumerable<PostResponse>>> GetPosts()
        {
            var posts = await _context.Posts.ToListAsync();
            var postResponses = new List<PostResponse>();

            foreach (var post in posts)
            {
                var postResponse = await mapEntityToResponse(post);
                postResponses.Add(postResponse);
            }

            return Ok(postResponses);
        }


        private async Task<PostResponse> mapEntityToResponse(Post post)
        {
            var user = await _context.Users.FindAsync(post.UserId);
            var photo = await _context.Photos.FindAsync(post.PhotoId);

            return new PostResponse
            {
                Content = post.Content,
                //TagName = post.TagName ?? string.Empty,
                PhotoUrl = photo?.PhotoUrl,
                UserName = user?.Username ?? "Uknown User"
            };
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Post>> GetPost(int id)
        {
            var post = await _context.Posts.FindAsync(id);

            if (post == null)
            {
                return NotFound();
            }
            var postResponse = await mapEntityToResponse(post);

            return Ok(postResponse);
        }

        // POST: api/Post
        [HttpPost]
        public async Task<ActionResult<Post>> CreatePost(PostRequest postRequest)
        {
            var latestPost = await _context.Posts.OrderByDescending(p => p.PostId).FirstOrDefaultAsync();
            var newPostId = (latestPost != null) ? latestPost.PostId + 1 : 1;
            // Create a new post entity with the PhotoId
            // Validate user is valid in db
            var user = await _context.Users.FindAsync(postRequest.UserId);
            if (user == null)
            {
                return BadRequest("Invalid user ID.");
            }
            Photo photo = null;
            if (postRequest.PhotoUrl != null)
            {
                // Create a new photo entity
                var latestPhoto = await _context.Photos.OrderByDescending(p => p.PhotoId).FirstOrDefaultAsync();
                var newPhotoId = (latestPhoto != null) ? latestPhoto.PhotoId + 1 : 1;

                photo = new Photo()
                {
                    PhotoId = newPhotoId,
                    PhotoUrl = postRequest.PhotoUrl, // Assuming postRequest contains PhotoUrl
                    CreatedAt = DateTime.Now
                };
                // Add the photo to the context and save changes to get the PhotoId
                _context.Photos.Add(photo);
                await _context.SaveChangesAsync();
            }

            var post = new Post()
            {
                PostId = newPostId,
                UserId = postRequest.UserId,
                Content = postRequest.Content,
                //PhotoId = photo != null ? photo.PhotoId : null, // Assign the newly created PhotoId
                PhotoId = photo.PhotoId,
                //TagName = postRequest.TagName,
                CreatedAt = DateTime.Now,
                //PostEmotionId = Emotion,
            };
            _context.Posts.Add(post);
            await _context.SaveChangesAsync();
            var response = await mapEntityToResponse(post);
            return Ok(response);
        }

        // PUT: api/Post/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdatePost(int id, PostRequest postRequest)
        {
            var existingPost = await _context.Posts.FindAsync(id);
            if (existingPost == null)
            {
                return NotFound();
            }
            // Create a new post entity with the PhotoId
            // Validate user is valid in db
            var user = await _context.Users.FindAsync(postRequest.UserId);
            if (user == null)
            {
                return BadRequest("Invalid user ID.");
            }

            Photo photo = null;
            if (postRequest.PhotoUrl != null)
            {
                // Create a new photo entity
                var latestPhoto = await _context.Photos.OrderByDescending(p => p.PhotoId).FirstOrDefaultAsync();
                var newPhotoId = (latestPhoto != null) ? latestPhoto.PhotoId + 1 : 1;

                photo = new Photo()
                {
                    PhotoId = newPhotoId,
                    PhotoUrl = postRequest.PhotoUrl, // Assuming postRequest contains PhotoUrl
                    CreatedAt = DateTime.Now
                };
                // Add the photo to the context and save changes to get the PhotoId
                _context.Photos.Add(photo);
                await _context.SaveChangesAsync();
            }
            var post = new Post()
            {
                UserId = postRequest.UserId,
                Content = postRequest.Content,
                PhotoId =  photo.PhotoId  , // Assign the newly created PhotoId
                //TagName = postRequest.TagName,
                CreatedAt = DateTime.Now,
                PostId = id
            };
            _context.Entry(post).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PostExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            var response = await mapEntityToResponse(post);
            return Ok(response);
        }

        // DELETE: api/Post/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletePost(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null)
            {
                return NotFound();
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Deleted Success" });
        }

        private bool PostExists(int id)
        {
            return _context.Posts.Any(e => e.PostId == id);
        }
    }
}