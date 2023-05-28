using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Options
{
    internal class BackupOptions
    {
        public const string Backup = "Backup";

        [Required]
        public string Path { get; set; }
    }
}
