namespace GeneralUpdate.Common.CustomAwaiter
{
    public interface IAwaitable<out TAwaiter> where TAwaiter : IAwaiter
    {
        TAwaiter GetAwaiter();
    }

    public interface IAwaitable<out TAwaiter, out TResult> where TAwaiter : IAwaiter<TResult>
    {
        TAwaiter GetAwaiter();
    }
}