using Core.Constants;
using Domain.Common.Configurations;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Domain.Entities.Addresses;
using Domain.Entities.Companies;


namespace Domain.Entities.Identity
{
    public class UserDetail : BaseEntity, IEntity
    {
        [Key]
        [ForeignKey("User")]
        public string UserId { get; set; }
        public User User { get; set; }
        
        [ForeignKey("Company")]
        public int? CompanyId { get; set; }
        public Company Company { get; set; }
        public Address Address { get; set; }
        //[Required]
        [StringLength(StringLengths.LengthXs)]
        public string Name { get; set; }
        //[Required]
        [StringLength(StringLengths.LengthXs)]
        public string SurName { get; set; }

        [StringLength(StringLengths.LengthXs)]
        public string Phone { get; set; }

        [StringLength(StringLengths.LengthXs)]
        public string Website { get; set; }
    }
}
