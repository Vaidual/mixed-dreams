using MixedDreams.Application.Common;
using MixedDreams.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Dto.Auth
{
    public record CompanyRegisterDto : RegisterDto
    {
        [Required(ErrorMessage = "Company name  is required.")]
        public string CompanyName { get; set; }

        [Required(ErrorMessage = "Birthday  is required.")]
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }

        [Required(ErrorMessage = "Address is required.")]
        public Address Address { get; set; }
    }
}
