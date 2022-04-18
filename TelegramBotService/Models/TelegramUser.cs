using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TelegramBotService.Models
{
    public class TelegramUser
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public decimal Grade { get; set; }
    }
}
