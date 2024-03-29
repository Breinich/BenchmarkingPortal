﻿using Newtonsoft.Json;

namespace BenchmarkingPortal.Web.Pages;

public static class SessionExtensions
{
    public static T? GetComplexData<T>(this ISession session, string key)
    {
        var data = session.GetString(key);
        return data == null ? default : JsonConvert.DeserializeObject<T>(data);
    }

    public static void SetComplexData(this ISession session, string key, object value)
    {
        session.SetString(key, JsonConvert.SerializeObject(value));
    }
}