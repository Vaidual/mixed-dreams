using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MixedDreams.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Domain.ValueObjects
{
    public class Address : ValueObject
    {
        [Column(TypeName = "nvarchar(100)")]
        public String Street { get; private set; }

        [Column(TypeName = "nvarchar(100)")]
        public String City { get; private set; }

        [Column(TypeName = "nvarchar(100)")]
        public String State { get; private set; }

        [Column(TypeName = "nvarchar(100)")]
        public String Country { get; private set; }

        [Column(TypeName = "varchar(12)")]
        public String ZipCode { get; private set; }

        [Column(TypeName = "nvarchar(100)")]
        public String? Apartment { get; private set; }

        public Address() { }

        public Address(string street, string city, string state, string country, string zipcode, string? apartament)
        {
            Street = street;
            City = city;
            State = state;
            Country = country;
            ZipCode = zipcode;
            Apartment = apartament;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            // Using a yield return statement to return each element one at a time
            yield return Street;
            yield return City;
            yield return State;
            yield return Country;
            yield return ZipCode;
            if (Apartment != null) 
            { 
                yield return Apartment;
            }
        }
    }
}
