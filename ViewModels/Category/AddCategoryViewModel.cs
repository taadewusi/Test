using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Category
{
    public class AddCategoryViewModel
    {
        //[Key]
        //public Guid CategoryId { get; set ; } 

        [Required(ErrorMessage = "Category can not be blank")]
        public string CategoryName { get; set; }
        public string Description { get; set; }
        [DefaultValue(false)]
        public bool Favorite { get; set; } = false;
    }

    public class AddBookToCategoryViewModel
    {
        [Required(ErrorMessage = "Category Id not be blank")]
        public string CategoryId { get; set; }

        [Required(ErrorMessage = "Category name can not be blank")]
        public string CategoryName { get; set; }
    }
}
