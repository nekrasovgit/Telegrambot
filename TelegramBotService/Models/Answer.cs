using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TelegramBotService.Models
{
    public class Answer
    {
        [Key]
        public Guid Id { get; set; }
        [StringLength(50)]
        public string QuestionAnswer { get; set; }
        public int Number { get; set; }
        public Guid QuestionId { get; set; }
        public QuestionDiscription Question { get; set; }
    }
}
