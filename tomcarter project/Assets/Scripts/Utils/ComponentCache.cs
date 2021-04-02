using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class ComponentCache<T> where T : MonoBehaviour 
{
    private Dictionary<Type, T> allComponents;
    public ComponentCache() { allComponents = new Dictionary<Type, T>(); }
    public ComponentCache(IEnumerable<T> instances) 
    {
        allComponents = new Dictionary<Type, T>(); 
        instances.ToList().ForEach((T instance) => AddInstance(instance));
    }

    public IEnumerable<T> GetAllInstances()
    {
        return allComponents.Values.Distinct();
    }

    public bool GetInstance<V>(out T instance) where V : T 
    {
        return allComponents.TryGetValue(typeof(V), out instance);
    }

    public void AddInstance(T instance)
    {
        Type getType = instance.GetType();

        while (getType != typeof(T)) 
        {
            allComponents[getType] = instance;
            getType = getType.BaseType;
        }
    }

}
