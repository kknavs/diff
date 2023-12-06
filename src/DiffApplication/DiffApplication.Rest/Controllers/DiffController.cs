using Microsoft.AspNetCore.Mvc;

namespace DiffApplication.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DiffController : ControllerBase
    {

        private readonly ILogger<DiffController> _logger;

        public DiffController(ILogger<DiffController> logger)
        {
            _logger = logger;
        }

    }
}
