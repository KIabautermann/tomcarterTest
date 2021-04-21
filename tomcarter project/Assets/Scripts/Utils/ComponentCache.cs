using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

// La mecha de esta gilada, que es medio cabeza, es que es un "reverse" Map, donde en vez de tener un mapa de Type -> List<Type>
// donde la lista representa todos los types que la instancia Es y Hereda, planteamos el Mapa de manera inversa.
// De esta manera, dado un Type que busquemos (sea instancia, base, o abstracto), ya lo tenemos mappeado a una instancia
// Es similar al GetComponent<> en el sentido que es "arbitraria" que instancia se devuelve al pedir un Type de tipo abstracto,
// que puede tener multiples interpretaciones.
// A futuro, si queremos hacer menos arbiraria esta decision, podemos poner cierto grado de "prioridad" para que, por ejemplo si hay dos instancias,
// que compiten por el Type key de la clase abstracta de la que ambos heredan, una de ellas siempre va a ganar esa Key:
//      - Ej: si queremos agregar ambos DashSkillStates, pero que al pedir el Abstract DashSkill, siempre se priorize el DashBaseSkill
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

    public bool GetInstance(Type type, out T instance)
    {
        return allComponents.TryGetValue(type, out instance);
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
    public List<T> RemoveInstance(Type type)
    {
        T instance = allComponents[type];
     
        List<T> removedInstances = new List<T>() { instance };

        var entriesToRemove = allComponents.Where(v => v.Value == instance).ToList();
        foreach(var entry in entriesToRemove)
        {
            removedInstances.Add(entry.Value);
            allComponents.Remove(entry.Key);
        }
        return removedInstances;
    }
}
