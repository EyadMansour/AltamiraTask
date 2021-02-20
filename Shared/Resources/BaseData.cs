using Domain.Common.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shared.Resources
{
    public class BaseData<TId>
    {
        public TId Id { get; set; }

    }
}
