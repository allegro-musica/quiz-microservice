using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Quizzer.Domain.Entities
{
    public record QuizGame
    {
        public ulong Id { get; init; }
        public Quiz Quiz { get; init; }
        public DateTime? Started { get; init; }
        public int CurrentQuestion { get; set; }
    }
}
