using System.Collections.Generic;

namespace Quizzer.Domain.Entities
{
    public record Question
    {
        public int Timeout { get; init; }
        public IList<Answer> Answer { get; init; }
    }
}
