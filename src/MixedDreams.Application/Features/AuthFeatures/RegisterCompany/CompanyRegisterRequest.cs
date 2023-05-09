using MixedDreams.Application.Features.Common.Address;
using MixedDreams.Domain.ValueObjects;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Application.Features.AuthFeatures.RegisterCompany
{
    public sealed record CompanyRegisterRequest(
        string CompanyName, 
        DateTime Birthday, 
        AddressDto Address
    ) : RegisterDto;
}
