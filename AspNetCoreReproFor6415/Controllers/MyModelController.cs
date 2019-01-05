using System.ComponentModel.DataAnnotations;
using System.Threading;
using AspNetCoreReproFor6415.Models;
using Microsoft.AspNetCore.Mvc;

namespace AspNetCoreReproFor6415.Controllers
{
    public class MyModelController : ControllerBase
    {
        [HttpPost]
        [Route("{a}/x/{b}/y/{c}/z", Name = "MyRoute")]
        public IActionResult Post(
            [FromRoute] string a,
            [FromRoute] string b,
            [FromRoute] string c,
            [FromBody] [Required] MyModel content,
            CancellationToken cancellationToken = default)
        {
            return NoContent();
        }

        [IgnoreAntiforgeryToken]
        [Route("/error", Name = "Error")]
        public IActionResult Error([FromQuery] int? id = null)
        {
            return StatusCode(id ?? 500, new { id });
        }
    }
}
