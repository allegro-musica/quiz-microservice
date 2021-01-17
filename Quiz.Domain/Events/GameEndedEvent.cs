using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizzer.Domain.Events
{
    public class GameEndedEvent
    {
        public ulong GameId { get; init; }
    }
}
