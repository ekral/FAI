namespace Evaluator
{
    public record TestResult(string Description, string Expected, string Actual, bool IsCorrect, int MaxPoints, int Points);
}


