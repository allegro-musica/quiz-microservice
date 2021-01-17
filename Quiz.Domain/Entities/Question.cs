using System.Collections.Generic;

namespace Quizzer.Domain.Entities
{
    public record Question
    {
        public string Title { get; init; }
        public int Timeout { get; init; }
        public IList<Answer> Answer { get; init; }
    }
}
