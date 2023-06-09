﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MixedDreams.Application.Hubs.Clients;
using MixedDreams.Application.Data;
using MixedDreams.Application.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace MixedDreams.Application.Services
{
    internal class BackupService : IBackupService
    {
        private readonly AppDbContext _context;
        private readonly BackupOptions _backupOptions;

        public BackupService(AppDbContext context, IOptions<BackupOptions> backupOptions)
        {
            _context = context;
            _backupOptions = backupOptions.Value;
        }

        public async Task CreateBackupAsync()
        {
            string backupFolderPath = _backupOptions.Path;

            if (!Directory.Exists(backupFolderPath))
            {
                Directory.CreateDirectory(backupFolderPath);
            }
            string sql = $"BACKUP DATABASE [{_context.Database.GetDbConnection().Database}] TO DISK = '{backupFolderPath}/{DateTimeOffset.UtcNow:yyyy.MM.ddTHH.mm.ss.fffffff}.bak'";
            await _context.Database.ExecuteSqlRawAsync(sql);
        }
    }
}
