using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Test.Models.Category
{
    public class BookCategory
    {
        [Key]
        public Guid CategoryId { get; set; }

        [Required(ErrorMessage = "Category can not be blank")]
        public string CategoryName { get; set; }        
        public string Description { get; set; }
  

    }
}
