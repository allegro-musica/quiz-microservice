namespace Quizzer.Domain.Entities
{
    public record Answer
    {
        public string Description { get; init; }
        public bool IsCorrect { get; init; }
    }
}
