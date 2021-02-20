using Domain.Common.Configurations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class ExtendedEntity<TId> : BaseEntity
    {
        public TId Id { get; set; }
    }
}
