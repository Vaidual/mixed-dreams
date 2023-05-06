using MixedDreams.Domain.Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MixedDreams.Domain.ValueObjects
{
    public class Address : ValueObject
    {

        public String Street { get; private set; }
        public String City { get; private set; }
        public String State { get; private set; }
        public String Country { get; private set; }

        [DataType(DataType.PostalCode)]
        public String ZipCode { get; private set; }
        public String? Apartament { get; private set; }

        public Address() { }

        public Address(string street, string city, string state, string country, string zipcode, string? apartament)
        {
            Street = street;
            City = city;
            State = state;
            Country = country;
            ZipCode = zipcode;
            Apartament = apartament;
        }

        protected override IEnumerable<object> GetEqualityComponents()
        {
            // Using a yield return statement to return each element one at a time
            yield return Street;
            yield return City;
            yield return State;
            yield return Country;
            yield return ZipCode;
            if (Apartament != null) 
            { 
                yield return Apartament;
            }
        }
    }
}
