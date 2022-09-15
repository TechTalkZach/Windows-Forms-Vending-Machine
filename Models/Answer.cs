using System;
using System.Collections.Generic;

namespace ProjetFinal.Models
{
    public partial class Answer
    {
        public int AnswerId { get; set; }
        public int? OptionId { get; set; }
        public int? QuizId { get; set; }

        public virtual ItemOption Option { get; set; }
        public virtual Quiz Quiz { get; set; }
    }
}
