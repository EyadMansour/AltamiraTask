using System;
using Core.Constants;
using System.ComponentModel.DataAnnotations;
namespace Shared.Resources.Addresses
{
	public class AddressData : IData
	{
        [StringLength(StringLengths.LengthLg)]
        public string Street { get; set; }
        [StringLength(StringLengths.LengthLg)]
        public string Suite { get; set; }
        [StringLength(StringLengths.LengthLg)]
        public string City { get; set; }
        [StringLength(StringLengths.LengthLg)]
        public string ZipCode { get; set; }
        public GeoData Geo { get; set; }
	}
}























































































