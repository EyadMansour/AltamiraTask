using Domain.Common.Enums;
using System;

namespace Domain.Common.Configurations
{
    public interface IEntity
    {
        public RecordStatus Status { get; set; }

        DateTime DateCreated { get; set; }
        DateTime? DateModified { get; set; }
        DateTime? DateDeleted { get; set; }

    }
}
