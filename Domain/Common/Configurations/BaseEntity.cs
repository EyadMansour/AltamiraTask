using Domain.Common.Enums;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Common.Configurations
{
    public class BaseEntity
    {
        public RecordStatus Status { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateModified { get; set; }
        public DateTime? DateDeleted { get; set; }

        public BaseEntity()
        {
            Status = RecordStatus.Active;
            DateCreated = DateTime.Now;
        }
    }
}
