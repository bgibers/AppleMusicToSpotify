using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SpotifyEnhancer.BuisnessLogic.Interfaces;

namespace SpotifyEnhancer.Controllers
{
    [ApiController]
    [Route("/migrate")]
    public class MigrationController : Controller
    {
        private readonly IMigrationService _migrationService;

        public MigrationController(IMigrationService migrationService)
        {
            _migrationService = migrationService;
        }

        /// <summary>
        /// Migrates an entire apple music library to spotify
        /// </summary>
        /// <param name="userToken"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("full_library")]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public async Task<ActionResult<bool>> CheckJobRunStatus([FromBody] string userToken)
        {
            return await _migrationService.MigrateLibrary(userToken);
        }
        
    }
}