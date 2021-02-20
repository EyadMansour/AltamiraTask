using System;
using Core.Constants;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Identity;
using Domain.Common.Configurations;

namespace Domain.Entities.Companies
{
	public class Company : ExtendedEntity<int>, IEntity
	{
		[StringLength(StringLengths.LengthLg)]
		[Required]
		public string Name { get; set; }

        [StringLength(StringLengths.LengthXl)]
		public string CatchPhrase { get; set; }

        [StringLength(StringLengths.LengthXl)]
		public string BusinessSegment { get; set; }
        public ICollection<UserDetail> Users { get; set; } = new Collection<UserDetail>();
	}
}






















































































