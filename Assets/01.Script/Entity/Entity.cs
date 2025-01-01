using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Entity : MonoBehaviour
{
    private Dictionary<Type, IEntityComponent> _compoDict;

    protected virtual void Awake()
    {
        _compoDict = new Dictionary<Type, IEntityComponent>();

        ComponentAdd();
        ComponentInit();
        ComponentAfterInit();
    }

    protected virtual void ComponentAdd()
    {
        GetComponentsInChildren<IEntityComponent>().ToList()
            .ForEach(component => _compoDict.Add(component.GetType(), component));
    }

    protected virtual void ComponentInit()
    {
        _compoDict.Values.ToList()
            .ForEach(component => component.Initialize(this));
    }

    protected virtual void ComponentAfterInit()
    {
        _compoDict.Values.OfType<IAfterInitComponent>().ToList()
                    .ForEach(component => component.AfterInit());
    }

    protected virtual void Update()
    {

    }

    protected virtual void OnDestroy()
    {
        GetComponentsInChildren<IEntityDisposeComponent>().ToList()
            .ForEach(component => component.Dispose());
    }

    public T GetEntityComponent<T>(bool isDerived = false) where T : class, IEntityComponent
    {
        if (_compoDict.TryGetValue(typeof(T), out IEntityComponent compo))
        {
            return compo as T;
        }

        if (!isDerived) return default;

        Type findType = _compoDict.Keys.FirstOrDefault(x => x.IsSubclassOf(typeof(T)));
        if (findType != null)
            return _compoDict[findType] as T;

        return default;
    }
    public bool TryGetEntityComponent<T>(out T component, bool isDerived = false) where T : class, IEntityComponent
    {
        component = null;
        if (_compoDict.TryGetValue(typeof(T), out IEntityComponent compo))
        {
            component = compo as T;
            return true;
        }

        if (!isDerived) return false;


        Type findType = _compoDict.Keys.FirstOrDefault(x => x.IsSubclassOf(typeof(T)));
        if (findType != null)
        {
            component = _compoDict[findType] as T;
            return true;
        }

        return false;
    }
}
