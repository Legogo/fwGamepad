using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Common base class for all InputKeyBridge bridge logic
/// 
/// qq un peut avoir un bridge qui va avoir un ensemble de schematics
/// un schematic va avoir une liste d'action réalisable
/// 
/// vu que le bridge va essayer de générer une instance du schematic quand on lui demande
/// il faut que le constructeur du schematic n'ai PAS DE PARAMETRE pour que ça fonctionne
/// </summary>

namespace inputeer
{
  abstract public class InputKeySchematic
  {
    protected List<InputActionKey> all = new List<InputActionKey>();

    public InputKeySchematic()
    { }

    protected void assign(InputActionKey[] keys)
    {
      all.AddRange(keys);
    }

    public void update()
    {
      for (int i = 0; i < all.Count; i++)
      {
        all[i].update();
      }
    }
  }

  public class InputActionKey : InputAction
  {
    public KeyCode[] tarKeys;

    public InputActionKey(KeyCode key, bool? tarState, string descr) : base(tarState, descr)
    {
      tarKeys = new KeyCode[] { key };
    }
    public InputActionKey(KeyCode[] keys, bool? tarState, string descr) : base(tarState, descr)
    {
      tarKeys = keys;
    }

    protected override bool actionState()
    {
      for (int i = 0; i < tarKeys.Length; i++)
      {
        if (Input.GetKey(tarKeys[i])) return true;
      }
      return false;
    }
  }

  abstract public class InputAction
  {
    public string description;
    public bool? checkState; // false = keydown ; true = key up
    bool _buffState;

    bool _raw = false; // actual raw state
    bool _acted = false; // this frame (must have checkState)

    public System.Action<bool> onActionStateChange;
    public System.Action onAction;

    public InputAction(bool? tarState, string description)
    {
      this.checkState = tarState;
      this.description = description;
    }

    abstract protected bool actionState();

    public bool justActed() => _acted;
    public bool getRaw() => _raw;

    public void update()
    {
      _acted = false;

      _raw = actionState();

      if (_buffState != _raw)
      {
        _buffState = _raw;

        onActionStateChange?.Invoke(_raw);

        if (checkState != null)
        {
          if (_raw && _raw == checkState.Value)
          {
            _acted = true;
            onAction?.Invoke();
          }
        }
      }
    }

  }

}