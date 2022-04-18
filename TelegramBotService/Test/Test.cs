using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using TelegramBotService.Models;
using TelegramBotService.QuestionMarkup;
using TelegramBotService.Repository;

namespace TelegramBotService.Test
{
    public class Test : ITest
    {
        private readonly IRepository<Answer> _answerRepository;
        private readonly IRepository<Grade> _gradeRepository;
        private readonly IRepository<QuestionDiscription> _questionRepository;
        private readonly IRepository<TelegramUser> _userRepository;
        private readonly IQuestionMarkup _questionMarkup;

        public Test(IRepository<Answer> answerRepository, IRepository<Grade> gradeRepository,
            IRepository<QuestionDiscription> questionRepository, IRepository<TelegramUser> userRepository, IQuestionMarkup questionMarkup)
        {
            _answerRepository = answerRepository;
            _gradeRepository = gradeRepository;
            _questionRepository = questionRepository;
            _userRepository = userRepository;
            _questionMarkup = questionMarkup;
        }

        public async Task UserRegistration(ITelegramBotClient telegramBotClient, Update update)
        {
            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                var message = update.Message;
                var newUser = new TelegramUser() { Id = Guid.NewGuid(), Name = Newtonsoft.Json.JsonConvert.SerializeObject(message), Grade = 0 };
                var userId = newUser.
                    Id.ToString();

                await _userRepository.AddAsync(newUser);
                await telegramBotClient.SendTextMessageAsync(chatId: "1052886210", text: userId);
            }
        }

        public async Task StartTest(ITelegramBotClient telegramBotClient, Update update)
        {
            var findAnswer = await _answerRepository.FindAsync(update.CallbackQuery.Data);
            var findQuestion = _questionRepository.FindById(findAnswer.QuestionId);
            var answerNumber = findAnswer.Number;
            var questionNumber = findQuestion.Number;

            var fu = update.Message.Text;
            var userId = Guid.Parse(fu);
            var findUser = _userRepository.FindById(userId);

            if (update.Type == Telegram.Bot.Types.Enums.UpdateType.Message)
            {
                var message = update.Message;
                if (message.Text.ToLower() == "/startTest")
                {

                }
            }

            else if (questionNumber == 1)
            {
                if (findQuestion != null)
                {
                    var firstAnswerGrade = new Grade() { Id = Guid.NewGuid(), QuestionNumber = questionNumber, GradeStatus = 1 };
                    await _gradeRepository.AddAsync(firstAnswerGrade);
                }
                var firstAnsGrade = new Grade() { Id = Guid.NewGuid(), QuestionNumber = questionNumber, GradeStatus = 0 };
                await _gradeRepository.AddAsync(firstAnsGrade);
                var nextNumberQuestion = questionNumber + 1;
                var nextQuestion = await _questionRepository.FindAsync(nextNumberQuestion);
                var questionDiscript = nextQuestion.Discription;
                var nextAnswer = _answerRepository.FindById(nextQuestion.AnswerId);

                Random random = new Random();
                var secondNumber = random.Next(11, 20);
                var threesNumber = random.Next(21, 30);
                var foursNumber = random.Next(31, 40);

                var secondQuestion = await _questionRepository.FindAsync(secondNumber);
                var threesQuestion = await _questionRepository.FindAsync(threesNumber);
                var foursQuestion = await _questionRepository.FindAsync(foursNumber);

                var secondQuestionDiscription = secondQuestion.Discription;
                var threesQuestionDiscription = threesQuestion.Discription;
                var foursQuestionDiscription = foursQuestion.Discription;

                var markup = _questionMarkup.CreateQuestionMarkup(secondQuestionDiscription,
                    threesQuestionDiscription, foursQuestionDiscription, nextAnswer.QuestionAnswer);

                var callbackId = update.CallbackQuery.Id;
                await telegramBotClient.AnswerCallbackQueryAsync(callbackId, text: update.CallbackQuery.Data, showAlert: false);
                await telegramBotClient.SendTextMessageAsync(chatId: "1052886210", questionDiscript, replyMarkup: markup);
                return;
            }



            else if (update.CallbackQuery.Message.Text == "5")
            {
                var userAnswer = update.CallbackQuery.Data;
                var foursAnswer = await _answerRepository.FindAsync(userAnswer);
                var foursQuestion = await _questionRepository.FindAsync(1);
                var foursQuestionNumber = foursQuestion.Number;

                if (userAnswer != null)
                {
                    var threesAnswerGrade = new Grade() { Id = Guid.NewGuid(), QuestionNumber = foursQuestionNumber, GradeStatus = 1 };
                    await _gradeRepository.AddAsync(threesAnswerGrade);
                }
                var threesAnsGrade = new Grade() { Id = Guid.NewGuid(), QuestionNumber = foursQuestionNumber, GradeStatus = 0 };
                await _gradeRepository.AddAsync(threesAnsGrade);
                var allGrades = _gradeRepository.GetAllAsync();

                foreach (var grade in allGrades)
                {
                    var ans = +grade.GradeStatus;
                    findUser.Grade = ans;
                }
                await _userRepository.Edit(findUser);
            }


        }
    }
}
