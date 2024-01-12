using System.Linq.Expressions;

public interface IHangfireJobEnqueuer
{
    void Enqueue(Expression<Action> methodCall);
}