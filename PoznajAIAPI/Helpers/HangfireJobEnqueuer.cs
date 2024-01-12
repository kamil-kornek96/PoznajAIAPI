using Hangfire;
using System.Linq.Expressions;

public class HangfireJobEnqueuer : IHangfireJobEnqueuer
{
    public void Enqueue(Expression<Action> methodCall)
    {
        BackgroundJob.Enqueue(methodCall);
    }
}
