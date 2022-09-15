using System;
using System.Collections.Generic;

namespace ProjetFinal.Models
{
    public partial class ItemOption
    {
        public ItemOption()
        {
            Answer = new HashSet<Answer>();
        }

        public int OptionId { get; set; }
        public string Text { get; set; }
        public bool IsRight { get; set; }
        public int? QuestionId { get; set; }

        public virtual Question Question { get; set; }
        public virtual ICollection<Answer> Answer { get; set; }
    }
}
