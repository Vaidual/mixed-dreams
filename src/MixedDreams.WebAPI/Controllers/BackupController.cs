using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MixedDreams.Application.ServicesInterfaces;
using MixedDreams.Infrastructure.Constants;

namespace MixedDreams.WebAPI.Controllers
{
    [Route("api/backup")]
    [Authorize(Roles = Roles.Administrator)]
    [ApiController]
    public class BackupController : ControllerBase
    {
        private readonly IBackupService _backupService;
        public BackupController(IBackupService backupService)
        {
            _backupService = backupService;
        }

        [HttpPost("create")]
        //[Authorize(Roles = Roles.Administrator)]
        public async Task<IActionResult> CreateBackupAsync()
        {
            await _backupService.CreateBackupAsync();
            return Ok("Backup is successful");
        }
    }
}
