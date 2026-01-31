namespace Bookstore.Domain.Exceptions
{
    public class ConflictException(string message) : BookstoreException(message, StatusCodes.Status409Conflict)
    {
    }
}