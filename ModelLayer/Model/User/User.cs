﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ModelLayer.Model.User
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
        [Required]
        public string UserName { get; set; }

        public bool IsActive { get; set; }=true;

        [Required]
        public DateTime CreatedDate { get; set; }

        [Required]
        public Role role { get; set; }

        public ModelLayer.Model.Address.Address address { get; set; }   
    }
}
