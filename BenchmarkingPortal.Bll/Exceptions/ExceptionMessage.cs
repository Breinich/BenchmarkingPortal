namespace BenchmarkingPortal.Bll.Exceptions;

/// <summary>
/// Generic exception messages for the given type.
/// </summary>
/// <typeparam name="T"></typeparam>
public static class ExceptionMessage<T>
{
    public static string ObjectNotFound { get; } = "The " + typeof(T).Name + " with the given Id is not found.";
    public static string NoPrivilege { get; } = "The user is neither the owner of the " + typeof(T).Name + ", nor an admin.";
}