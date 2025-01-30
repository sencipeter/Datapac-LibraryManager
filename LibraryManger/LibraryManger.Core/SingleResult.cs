
namespace LibraryManger.Core
{
    public class SingleResult<T>
    {
        public T? Result { get; set; }
        public Exception? Exception { get; set; }

        public T? GetResultOrThrowException()
        {
            if (Exception != null)
                throw Exception;

            return Result;
        }
    }
}
