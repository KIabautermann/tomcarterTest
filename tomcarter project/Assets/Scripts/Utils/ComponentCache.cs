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
    private Dictionary<Type, T> inactiveComponents;
    private Dictionary<Type, T> activeComponents;
    public ComponentCache() { activeComponents = new Dictionary<Type, T>(); inactiveComponents = new Dictionary<Type, T>(); }
    public ComponentCache(IEnumerable<T> instances, IEnumerable<T> inactive) 
    {
        ValidateInputs(instances, inactive);
        activeComponents = new Dictionary<Type, T>(); 
        inactiveComponents = new Dictionary<Type, T>();
        instances.ToList().ForEach((T instance) => AddActiveInstance(instance));
        inactive.ToList().ForEach((T instance) => inactiveComponents[instance.GetType()] = instance);
    }

    private void ValidateInputs(IEnumerable<T> instances, IEnumerable<T> inactive) 
    {
        var sum = instances.ToList();
        sum.AddRange(inactive);
        if (sum.Distinct().Count() != (instances.Count() + inactive.Count())) {
            Debug.LogWarning("There are duplicated instances being stored in the Component Cache");
        }
    }

    public IEnumerable<T> GetAllInstances()
    {
        var allComponents = activeComponents.Values.Distinct().ToList();
        allComponents.AddRange(inactiveComponents.Values.Distinct());
        return allComponents;
    }

    public bool GetInstance(Type type, out T instance)
    {
        return activeComponents.TryGetValue(type, out instance);
    }

    public void SetActive(Type type) {
        AddActiveInstance(inactiveComponents[type]);
        inactiveComponents.Remove(type);
    }
    public void SetInactive(Type type) {
        inactiveComponents[type] = activeComponents[type];
        RemoveActiveInstance(type);
    }

    private void AddActiveInstance(T instance)
    {
        Type getType = instance.GetType();

        while (getType != typeof(T)) 
        {
            activeComponents[getType] = instance;
            getType = getType.BaseType;
        }
    }
    private List<T> RemoveActiveInstance(Type type)
    {
        T instance = activeComponents[type];
     
        List<T> removedInstances = new List<T>() { instance };

        var entriesToRemove = activeComponents.Where(v => v.Value == instance).ToList();
        foreach(var entry in entriesToRemove)
        {
            removedInstances.Add(entry.Value);
            activeComponents.Remove(entry.Key);
        }
        return removedInstances;
    }
}
