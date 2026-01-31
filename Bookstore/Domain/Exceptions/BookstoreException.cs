namespace Bookstore.Domain.Exceptions
{
    public abstract class BookstoreException(string message, int statusCode) : Exception(message)
    {
        public int StatusCode { get; } = statusCode;
    }
}