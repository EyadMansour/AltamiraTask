using System;
using Core.Constants;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Common.Configurations;
using Domain.Entities.Identity;

namespace Domain.Entities.Addresses
{
	public class Address : BaseEntity, IEntity
	{
        [Key]
        [ForeignKey("User")]
        public string UserId { get; set; }
        public UserDetail User { get; set; }

		[StringLength(StringLengths.LengthLg)]
		public string Street { get; set; }
		[StringLength(StringLengths.LengthLg)]
		public string Suite { get; set; }
		[StringLength(StringLengths.LengthLg)]
		public string City { get; set; }
		[StringLength(StringLengths.LengthLg)]
		public string ZipCode { get; set; }
		public NetTopologySuite.Geometries.Point Geo { get; set; }
	}
}





















































































