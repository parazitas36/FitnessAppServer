using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FitnessAppAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class FilesController : ControllerBase
    {
        [HttpGet("file/{path}")]
        public async Task<IActionResult> GetFile(string path)
        {
            var dir = new DirectoryInfo("Files");
            var files = dir.GetFiles().ToList();
            var file = files.First(x => x.FullName.Contains(path));
            FileStream stream = System.IO.File.Open(file.FullName, FileMode.Open);
            var ext = file.Extension == ".jpg" ? "image/jpeg" : "video/mp4";
            return File(stream, ext);
        }
    }
}
