using System;
using System.Collections.Generic;

namespace ProjetFinal.Models
{
    public partial class Category
    {
        public Category()
        {
            Question = new HashSet<Question>();
        }

        public int CategoryId { get; set; }
        public string Description { get; set; }

        public virtual ICollection<Question> Question { get; set; }
    }
}
