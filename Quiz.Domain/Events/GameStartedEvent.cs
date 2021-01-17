using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizzer.Domain.Events
{
    public record GameStartedEvent
    {
        public ulong GameId { get; init; }
        public IList<string> Users { get; init; }
    }
}
