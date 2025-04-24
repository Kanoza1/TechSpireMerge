using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Entities
{
    public class QuestionsSolutions
    {
        public int QuestionId { get; set; }
        public string UserId { get; set; }
        public Question Question { get; set; }
        public AppUser AppUser { get; set; }
        public string SolvedAt { get; set; }
        public string Answer { get; set; }
        public bool isCorrect { get;set; }
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }

    }
}
