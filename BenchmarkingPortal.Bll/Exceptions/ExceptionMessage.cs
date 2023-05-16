namespace BenchmarkingPortal.Bll.Exceptions;

public class ExceptionMessage<T>
{
    public ExceptionMessage()
    {
        ObjectNotFound = "The " + nameof(T) + " with the given Id is not found.";
        NoPrivilege = "The user is neither the owner of the " + nameof(T) + ", nor an admin.";
    }

    public string ObjectNotFound { get; }
    public string NoPrivilege { get; }
}