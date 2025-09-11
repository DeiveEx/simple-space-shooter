using System;
using System.Collections.Generic;

public static class SimpleServiceLocator
{
    private static readonly Dictionary<Type, object> Services = new();

    public static void RegisterService<T>(T service)
    {
        var type = typeof(T);
        
        if(!Services.TryAdd(type, service))
            throw new Exception($"Service of type {type} is already registered!");
    }

    public static void UnregisterService<T>()
    {
        var type = typeof(T);
        Services.Remove(type);
    }
    
    public static T GetService<T>()
    {
        var type = typeof(T);
        
        if (!Services.TryGetValue(type, out var service))
            throw new Exception($"Service of type {type} is not registered!");
        
        return (T) service;
    }

    public static void DisposeServices()
    {
        Services.Clear();
    }
}
