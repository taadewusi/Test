using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Category
{
    public class AddBookViewModel
    {
        [Key]
        public Guid BookId { get; set; }

        [Required(ErrorMessage = "Book Title can not be blank")]
        public string BookName { get; set; }
        public string Description { get; set; }
        public int Pages { get; set; }
        public string CategoryId { get; set; }
        public string CategoryName { get; set; }
        public bool Favorite { get; set; }
    }
}
