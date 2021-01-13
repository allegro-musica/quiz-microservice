using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizzer.Domain.Exceptions
{
    public class GameNotFoundException : Exception
    {
        public GameNotFoundException(ulong id) : base($"Game {id} not found")
        {
        }

        public GameNotFoundException() : base()
        {
        }
    }
}
