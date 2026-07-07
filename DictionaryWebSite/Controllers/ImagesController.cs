using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using Microsoft.AspNetCore.Mvc;

namespace DictionaryWebSite.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ImagesController : ControllerBase
    {
        private readonly Cloudinary _cloudinary;

        public ImagesController(Cloudinary cloudinary)
        {
            _cloudinary = cloudinary;
        }

        [HttpPost("upload")]
        public async Task<IActionResult> Upload(IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file provided.");

            using var stream = file.OpenReadStream();

            var uploadParams = new ImageUploadParams
            {
                File = new FileDescription(file.FileName, stream),
                Folder = "my-website-images"   // optional folder in Cloudinary
            };

            var result = await _cloudinary.UploadAsync(uploadParams);

            if (result.Error != null)
                return BadRequest(result.Error.Message);

            // Save result.SecureUrl.ToString() to your database here
            return Ok(new { url = result.SecureUrl.ToString(), publicId = result.PublicId });
        }

        //This needs to be changed to get the images from the database instead of hardcoding them, this is just for testing purposes
        [HttpGet]
        public IActionResult GetImages()
        {
            // Replace this with a real database query, e.g. _dbContext.Images.ToList()
            var images = new[]
            {
        new { url = "https://res.cloudinary.com/your_cloud/image/upload/v1/my-website-images/sample.jpg" }
    };

            return Ok(images);
        }
    }
}