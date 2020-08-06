using System;
using System.ComponentModel.DataAnnotations;

namespace DotShopApi.Models
{
    public class User
    {
        public long Id { get; set; }

        public string Name { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateModified { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime DateCreated { get; set; }

        [Required]
        public bool Deleted { get; set; }
    }
}