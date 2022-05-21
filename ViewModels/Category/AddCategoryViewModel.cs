using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ViewModels.Category
{
    public class AddCategoryViewModel
    {
        [Key]
        public Guid CategoryId { get; set; }

        [Required(ErrorMessage = "Category can not be blank")]
        public string CategoryName { get; set; }
        public string Description { get; set; }
    }
}
