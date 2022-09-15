using System;
using System.Collections.Generic;

namespace ProjetFinal.Models
{
    public partial class Question
    {
        public Question()
        {
            ItemOption = new HashSet<ItemOption>();
            QuestionQuiz = new HashSet<QuestionQuiz>();
        }

        public int QuestionId { get; set; }
        public string Text { get; set; }
        public int? CategoryId { get; set; }

        public virtual Category Category { get; set; }
        public virtual ICollection<ItemOption> ItemOption { get; set; }
        public virtual ICollection<QuestionQuiz> QuestionQuiz { get; set; }

        internal object Select(Func<object, object> p)
        {
            throw new NotImplementedException();
        }
    }
}
