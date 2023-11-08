using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
using NZWalks.API.Repositories;

namespace NZWalks.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IimageRepository imageRepository;

        public ImageController(IimageRepository imageRepository)
        {
            this.imageRepository = imageRepository;
        }

        [HttpPost]
        [Route("Upload")]
        public async Task<IActionResult> Upload([FromForm] ImageUploadRequestDto imageUploadRequestDto)
        {
            ValidateFileUpload(imageUploadRequestDto);
            if (ModelState.IsValid)
            {
                var imageDomainModel = new Image
                {
                    file = imageUploadRequestDto.file,
                    FileName = imageUploadRequestDto.FileName,
                    FileExtension = Path.GetExtension(imageUploadRequestDto.file.FileName),
                    FileSizeInBytes = imageUploadRequestDto.file.Length,
                    FileDescription = imageUploadRequestDto.FileDescription
                };
                await imageRepository.Upload(imageDomainModel);
                return Ok(imageDomainModel);
            }
            return BadRequest(ModelState);
        }

        private void ValidateFileUpload(ImageUploadRequestDto imageUploadRequestDto)
        {
            var allowExtensions = new string[] { ".jpg", ".jpeg", ".png" };
            if (!allowExtensions.Contains(Path.GetExtension(imageUploadRequestDto.file.FileName).ToLower()))
            {
                ModelState.AddModelError("file", "File format not supported");
            }
            else if (imageUploadRequestDto.file.Length > 10485760)
            {
                ModelState.AddModelError("file", "File size not more than 10 mb.");
            }
        }

    }
}
