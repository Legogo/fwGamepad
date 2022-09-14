using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// carry de tout les blueprints de touches d'un objet
/// 
/// chaque objet peut créer une version de ce mgr
/// le manager va contenir tout le code lié a l'état des inputs "action context"
/// 
/// </summary>

namespace inputeer
{
  public class InputKeyBridge
  {
    protected Dictionary<Type, InputKeySchematic> list = new Dictionary<Type, InputKeySchematic>();

    public T get<T>() where T : InputKeySchematic
    {
      if (list.ContainsKey(typeof(T))) return list[typeof(T)] as T;

      T ik = (T)Activator.CreateInstance(typeof(T));
      list.Add(typeof(T), ik as InputKeySchematic);
      //Debug.Log("created : " + ik);

      return ik;
    }

    public void remove<T>() where T : InputKeySchematic
    {
      if (!list.ContainsKey(typeof(T))) return;
      list.Remove(typeof(T));
    }

  }
}