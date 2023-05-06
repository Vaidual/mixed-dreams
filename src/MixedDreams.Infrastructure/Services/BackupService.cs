using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MixedDreams.Application.ServicesInterfaces;
using MixedDreams.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MixedDreams.Infrastructure.Services
{
    internal class BackupService : IBackupService
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;

        public BackupService(AppDbContext context, IConfiguration configuration)
        {
            _context = context;
            this._configuration = configuration;
        }

        public async Task CreateBackupAsync()
        {
            string backupFolderPath = _configuration["Backup:path"];

            if (!Directory.Exists(backupFolderPath))
            {
                Directory.CreateDirectory(backupFolderPath);
            }
            string sql = $"BACKUP DATABASE [{_context.Database.GetDbConnection().Database}] TO DISK = '{backupFolderPath}/{DateTimeOffset.UtcNow:yyyy.MM.ddTHH.mm.ss.fffffff}.bak'";
            await _context.Database.ExecuteSqlRawAsync(sql);
        }
    }
}
