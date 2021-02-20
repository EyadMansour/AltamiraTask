using System;
using System.Collections.Generic;
using Core.Constants;
using System.ComponentModel.DataAnnotations;
using Shared.Resources.User;

namespace Shared.Resources.Companies
{
	public class CompanyGetData : BaseData<string>, IGetData
	{
		public string Name { get; set; }
		
		public string CatchPhrase { get; set; }
		
		public string BusinessSegment { get; set; }
		
	}
}
























































































