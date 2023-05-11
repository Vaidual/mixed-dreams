using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Domain.Entities
{
    public class BackblazeFile
    {
        [Key]
        public string Id { get; set; }
        public string FileName { get; set; }
        public string Path { get; set; }

        public Guid ProductId { get; set; }
        public Product Product { get; set; }
    }
}
