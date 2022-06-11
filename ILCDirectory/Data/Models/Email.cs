using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using ILCDirectory.Enums;

namespace ILCDirectory.Data.Models
{
    public class Email
    {
        [StringLength(100)]
        public string EmailAddress { get; set; }
        [Column(TypeName = "tinyint")]
        public EmailAddressType EmailAddressType { get; set; }
    }
}
