using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TelegramBotService.Models
{
    public class Grade
    {
        public Guid Id { get; set; }
        public int GradeStatus { get; set; }
        public int QuestionNumber { get; set; }
    }
}
