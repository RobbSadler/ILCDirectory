using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ILCDirectory.Models
{
    public partial class ViewPersonDisplayName
    {
        public int Id { get; set; }
        [StringLength(25)]
        public string LastName { get; set; }
        [StringLength(10)]
        public string TitleName { get; set; }
        [StringLength(25)]
        public string FirstName { get; set; }
        [StringLength(25)]
        public string MiddleName { get; set; }
        [StringLength(10)]
        public string SuffixName { get; set; }
        [StringLength(25)]
        public string NickName { get; set; }
        [StringLength(90)]
        public string FullNameByLast { get; set; }
        [StringLength(89)]
        public string FullNameByFirst { get; set; }
    }
}
