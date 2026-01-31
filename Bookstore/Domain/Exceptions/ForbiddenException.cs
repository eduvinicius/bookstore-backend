namespace Bookstore.Domain.Exceptions
{
    public class ForbiddenException(string message = "You don't have permission to access this resource") : BookstoreException(message, StatusCodes.Status403Forbidden)
    {
    }
}