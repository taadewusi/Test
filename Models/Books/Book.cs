﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Test.Models.Books
{
    public class Book
    {
        [Key]
        public Guid BookId { get; set; }

        [Required(ErrorMessage = "Category can not be blank")]
        public string BookName { get; set; }
        public string Description { get; set; }
        public int Pages { get; set; }
        public string CategoryId { get; set; }
        public string Category { get; set; }
      
    }
}
