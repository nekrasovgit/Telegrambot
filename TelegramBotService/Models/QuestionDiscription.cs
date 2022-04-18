using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TelegramBotService.Models
{
    public class QuestionDiscription
    {
        [Key]
        public Guid Id { get; set; }
        [Range(1, 100)]
        public int Number { get; set; }
        [StringLength(50)]
        public string Discription { get; set; }
        public Guid AnswerId { get; set; }
        public Answer Answer { get; set; }
    }
}
