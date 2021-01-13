using System;
using System.Collections.Concurrent;
using Quizzer.Domain.Entities;
using Quizzer.Domain.Exceptions;

namespace Quizzer.Infrastructure
{
    public class QuizManager
    {

        private readonly ConcurrentDictionary<ulong, QuizGame> _runningGame = new ();


        //public bool Add(string game) => _runningGame.Add(userId);
        //public bool Remove(ulong userId) => _runningGame.TryRemove(userId);
        public void Clear() => _runningGame.Clear();

        /// <summary>
        /// Create new game
        /// </summary>
        /// <param name="game"></param>
        /// <returns></returns>
        public QuizGame CreateNew(QuizGame game)
        {
            Console.WriteLine("Creating new game");
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
            if (!_runningGame.TryGetValue(id, out QuizGame game))
            {
                throw new GameNotFoundException(id);
            }

            return game;
        }
    }
}
