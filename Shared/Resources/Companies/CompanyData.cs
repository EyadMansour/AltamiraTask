using System;
using Core.Constants;
using System.ComponentModel.DataAnnotations;
namespace Shared.Resources.Companies
{
	public class CompanyData : BaseData<int>,IData
	{
        [StringLength(StringLengths.LengthLg)]
        [Required]
        public string Name { get; set; }

        [StringLength(StringLengths.LengthXl)]
        public string CatchPhrase { get; set; }

        [StringLength(StringLengths.LengthXl)]
        public string BusinessSegment { get; set; }
	}
}
























































































