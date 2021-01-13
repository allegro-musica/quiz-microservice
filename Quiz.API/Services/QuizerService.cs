using System;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Quiz.API;
using Quizzer.Domain.Entities;
using Quizzer.Infrastructure;

namespace Quizzer.API.Services
{
    public class QuizerService : Quiz.API.Quizer.QuizerBase
    {
        private readonly QuizManager _manager;

        public QuizerService(QuizManager manager)
        {
            _manager = manager;
        }

        public override Task<QuizCreatedResponse> CreateGame(QuizCreateRequest request, ServerCallContext context)
        {
            var quiz = new Domain.Entities.Quiz()
            {
                Title = request.Quiz.Title,
                Description = request.Quiz.Description,
                ImageUrl = request.Quiz.ImageUrl,
                Questions = request.Quiz.Questions.Select(x => new Question()
                {
                    Timeout = x.Timeout
                }).ToList()
            };

            var random = new Random();

            ulong id = (ulong)random.Next(21000000, 25000000);

            var game = new QuizGame()
            {
                Quiz = quiz,
                Id = id,
                Started = DateTime.UtcNow
            };

            var result = _manager.CreateNew(game);

            var response = new QuizCreatedResponse()
            {
                Quiz = request.Quiz,
                Id = result.Id
            };

            return Task.FromResult(response);
        }
    }
}
