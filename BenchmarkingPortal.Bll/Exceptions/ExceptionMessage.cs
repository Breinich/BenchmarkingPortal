namespace BenchmarkingPortal.Bll.Exceptions;

public class ExceptionMessage<T>
{
    public ExceptionMessage()
    {
        ObjectNotFound = "The " + typeof(T).Name + " with the given Id is not found.";
        NoPrivilege = "The user is neither the owner of the " + typeof(T).Name + ", nor an admin.";
    }

    public string ObjectNotFound { get; }
    public string NoPrivilege { get; }
}