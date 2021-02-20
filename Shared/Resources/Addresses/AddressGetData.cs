using System;
using Core.Constants;
using System.ComponentModel.DataAnnotations;
namespace Shared.Resources.Addresses
{
	public class AddressGetData:IGetData
	{
		public string Street { get; set; }
		public string Suite { get; set; }
		public string City { get; set; }
		public string ZipCode { get; set; }
		public GeoGetData Geo { get; set; }=new GeoGetData();
	}
}























































































