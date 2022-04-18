using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TelegramBotService.Models
{
    public class TelegramContext : DbContext
    {
        public DbSet<Answer> Answers { get; set; }
        public DbSet<Grade> Grades { get; set; }
        public DbSet<QuestionDiscription> QuestionDiscription { get; set; }
        public DbSet<TelegramUser> Users { get; set; }

        public TelegramContext(DbContextOptions<TelegramContext> options) : base(options)
        {

        }
    }
}
