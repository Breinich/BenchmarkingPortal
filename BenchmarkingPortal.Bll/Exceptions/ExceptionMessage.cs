namespace BenchmarkingPortal.Bll.Exceptions;

/// <summary>
/// Generic exception messages for the given type.
/// </summary>
/// <typeparam name="T"></typeparam>
public class ExceptionMessage<T>
{
    public string ObjectNotFound { get; } = "The " + typeof(T).Name + " with the given Id is not found.";
    public string NoPrivilege { get; } = "The user is neither the owner of the " + typeof(T).Name + ", nor an admin.";
}