using System;
using Core.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace Domain.Entities.Addresses
{
    [Owned]
	public class GeoInfo
	{
		public float Lat { get; set; }
		public float Lng { get; set; }
	}
}























































































