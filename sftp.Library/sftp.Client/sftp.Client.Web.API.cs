using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using SftpIntegrationLibrary;

namespace YourAspNetApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SftpController : ControllerBase
    {
        private readonly SftpClient _sftpClient;

        public SftpController(IConfiguration configuration)
        {
            _sftpClient = new SftpClient(configuration);
        }

        [HttpGet("download")]
        public IActionResult DownloadFile()
        {
            _sftpClient.DownloadFile();
            return Ok("File downloaded successfully");
        }

        [HttpGet("upload")]
        public IActionResult UploadFile()
        {
            _sftpClient.UploadFile();
            return Ok("File uploaded successfully");
        }

        [HttpGet("mkdir")]
        public IActionResult CreateRemoteDirectory(string directory)
        {
            _sftpClient.CreateRemoteDirectory(directory);
            return Ok($"Directory {directory} created successfully");
        }
    }
}
