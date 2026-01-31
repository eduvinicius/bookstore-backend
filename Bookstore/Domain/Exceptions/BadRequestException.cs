namespace Bookstore.Domain.Exceptions
{
    public class BadRequestException(string message) : BookstoreException(message, StatusCodes.Status400BadRequest)
    {
    }
}