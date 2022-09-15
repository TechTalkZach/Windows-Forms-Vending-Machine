using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace ProjetFinal.Models
{
    public partial class Quiz
    {
        public Quiz()
        {
            Answer = new HashSet<Answer>();
            QuestionQuiz = new HashSet<QuestionQuiz>();
        }

        public int QuizId { get; set; }
        public string UserName { get; set; }

        [DataType(DataType.EmailAddress, ErrorMessage = "E-mail is not valid")]
        public string Email { get; set; }

        public virtual ICollection<Answer> Answer { get; set; }
        public virtual ICollection<QuestionQuiz> QuestionQuiz { get; set; }
    }
}
