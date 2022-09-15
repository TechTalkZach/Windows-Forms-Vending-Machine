using System;
using System.Collections.Generic;

namespace ProjetFinal.Models
{
    public partial class QuestionQuiz
    {
        public int QuestionId { get; set; }
        public int QuizId { get; set; }

        public virtual Question Question { get; set; }
        public virtual Quiz Quiz { get; set; }
    }
}
