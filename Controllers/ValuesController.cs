using Backend.Model;
using Backend.Service;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly ImageService _imageService;
        private readonly IWebHostEnvironment _env;


        public ValuesController(ImageService imageService, IWebHostEnvironment env)
        {
            _imageService = imageService;
            _env = env;
        }

        // Constructor Injection
        //public ValuesController(ImageService imageService)
        //{
        //    _imageService = imageService;
        //}

        // GET: api/values/test
        //[HttpGet("test")]
        //public async Task<IActionResult> Test()
        //{
        //    await _imageService.InsertAsync(new ImageModel
        //    {
        //        Id = Guid.NewGuid().ToString(),
        //        ImageUrl = "test.png"
        //    });

        //    return Ok("MongoDB Connected Successfully!");
        //}

        [HttpPost("upload")]
        public async Task<IActionResult> UploadImage([FromForm] IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return BadRequest("Image file is required");

            var webRoot = _env.WebRootPath
                ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

            var imagesFolder = Path.Combine(webRoot, "images");
            if (!Directory.Exists(imagesFolder))
                Directory.CreateDirectory(imagesFolder);

            // Generate Mongo ObjectId automatically
            var image = new ImageModel
            {
                ImageUrl = "" // temporary
            };

            await _imageService.InsertAsync(image);

            // Use generated ID as filename
            var fileName = image.Id + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(imagesFolder, fileName);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            image.ImageUrl = "/images/" + fileName;
            await _imageService.UpdateImageUrlAsync(image.Id, image.ImageUrl);

            return Ok(new
            {
                imageId = image.Id,
                imageUrl = image.ImageUrl
            });
        }

        //[HttpGet("view/{id}")]
        //public async Task<IActionResult> ViewImage(string id)
        //{
        //    var image = await _imageService.GetByIdAsync(id);

        //    if (image == null)
        //        return NotFound("Image not found");

        //    var imagePath = Path.Combine(
        //        Directory.GetCurrentDirectory(),
        //        "wwwroot",
        //        image.ImageUrl.TrimStart('/')
        //    );

        //    if (!System.IO.File.Exists(imagePath))
        //        return NotFound("Image file not found");

        //    var imageBytes = await System.IO.File.ReadAllBytesAsync(imagePath);
        //    return File(imageBytes, "image/png");
        //}


        [HttpGet("getbyid/{id}")]
        public async Task<IActionResult> GetImageById(string id)
        {
            var image = await _imageService.GetByIdAsync(id);
            if (image == null) return NotFound("Image not found");

            return Ok(image); // returns JSON: { id, imageUrl }
        }


    }
}
