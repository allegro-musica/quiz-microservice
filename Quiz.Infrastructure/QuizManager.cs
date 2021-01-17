using System;
using System.Collections.Concurrent;
using System.Threading.Tasks;
using MassTransit;
using Quizzer.Domain.Entities;
using Quizzer.Domain.Events;
using Quizzer.Domain.Exceptions;

namespace Quizzer.Infrastructure
{
    public class QuizManager
    {
        private readonly IBusControl _bus;

        public QuizManager(IBusControl bus)
        {
            _bus = bus;
        }

        private readonly ConcurrentDictionary<ulong, QuizGame> _runningGame = new ();

        private readonly ConcurrentDictionary<ulong, Quiz> _runningQuiz = new();

        //public bool Add(string game) => _runningGame.Add(userId);
        //public bool Remove(ulong userId) => _runningGame.TryRemove(userId);
        public void Clear() => _runningGame.Clear();

        public QuizGame TryGetQuiz(ulong id)
        {
            if (!_runningGame.TryGetValue(id, out QuizGame game))
            {
                throw new GameNotFoundException(id);
            }

            return game;
        }

        /// <summary>
        /// Join a user in the game
        /// </summary>
        /// <param name="id"></param>
        /// <param name="user"></param>
        public void JoinGame(ulong id, string user)
        {
            var game = TryGetQuiz(id);

            game.Quiz.Users.Add(user);
        }

        /// <summary>
        /// Create new game
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public QuizGame CreateNew(QuizGame game)
        {
            _runningGame.TryAdd(game.Id, game);

            return game;
        }

        /// <summary>
        /// End a running game
        /// </summary>
        /// <exception cref="GameNotFoundException"></exception>
        /// <param name="id"></param>
        /// <returns></returns>
        public QuizGame EndGame(ulong id)
        {
            return TryGetQuiz(id);
        }

        public async Task Start(ulong id)
        {
            var game = TryGetQuiz(id);

            _runningQuiz.TryAdd(id, game.Quiz);

            var gameStartedEvent = new GameStartedEvent()
            {
                GameId = id,
                Users = game.Quiz.Users
            };

            await _bus.Publish(gameStartedEvent);

            await Task.Delay(500);

            await StartQuestion(id, game.CurrentQuestion);
        }

        public async Task<Question> StartQuestion(ulong quizId, int index)
        {
            var quiz = TryGetQuiz(quizId);

            if (quiz.Quiz.Questions.Count <= index)
            {
                await _bus.Publish(new GameEndedEvent() {GameId = quizId});
                return null;
            }

            var question = quiz.Quiz.Questions[index];

            if (question == null) return null; // Throw exception 

            quiz.CurrentQuestion = quiz.CurrentQuestion += 1;

            var questionStartedEvent = new QuestionStartedEvent()
            {
                Title = question.Title
            };

            await _bus.Publish(questionStartedEvent);

            return question;
        }
    }
}
