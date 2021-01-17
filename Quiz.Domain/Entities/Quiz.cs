using System.Collections.Generic;

namespace Quizzer.Domain.Entities
{
    public record Quiz
    {
        public string Title { get; init; }
        public string Description { get; init; }
        public string ImageUrl { get; init; }
        public IList<Question> Questions { get; init; }
        public IList<string> Users { get; init; }
    }
}
