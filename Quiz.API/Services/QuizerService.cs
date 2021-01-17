using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Quiz.API;
using Quizzer.Domain.Entities;
using Quizzer.Domain.Exceptions;
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

        public override Task<Empty> JoinGame(JoinGameRequest request, ServerCallContext context)
        {
            _manager.JoinGame(request.Id, request.User);

            return Task.FromResult(new Empty());
        }

        public override Task<QuizExistResponse> QuizExist(QuizExistRequest request, ServerCallContext context)
        {
            try
            {
                var quiz = _manager.TryGetQuiz(request.Id);

                return Task.FromResult(new QuizExistResponse()
                {
                    Quiz = new QuizData()
                    {
                        Description = quiz.Quiz.Description,
                        ImageUrl = quiz.Quiz.ImageUrl,
                        Title = quiz.Quiz.Title
                    }
                });
            }
            catch (GameNotFoundException)
            {
                context.Status = new Status(StatusCode.NotFound, "Quiz does not exist");

                return Task.FromResult(new QuizExistResponse());
            }
        }

        public override Task<QuizCreatedResponse> CreateGame(QuizCreateRequest request, ServerCallContext context)
        {
            var quiz = new Domain.Entities.Quiz()
            {
                Title = request.Quiz.Title,
                Description = request.Quiz.Description,
                ImageUrl = request.Quiz.ImageUrl,
                Users = new List<string>(),
                Questions = request.Quiz.Questions.Select(x => new Question()
                {
                    Timeout = x.Timeout,
                    Title = x.Title
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

        public override async Task<Empty> NextQuestion(NextQuestionRequest request, ServerCallContext context)
        {
            var game = _manager.TryGetQuiz(request.Id);

            await _manager.StartQuestion(game.Id, game.CurrentQuestion);

            return new Empty();
        }

        public override async Task<Empty> StartGame(QuizStartRequest request, ServerCallContext context)
        {
            await _manager.Start(request.Id);

            return new Empty();
        }
    }
}
