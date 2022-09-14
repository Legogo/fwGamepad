using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// 2018-10-05
///   now a class that can be used by any Mono
///   you'll need to call the subscribe AFTER touch input exists
/// 2018-07-14
///   now parent need to call the setup() for that behavior to subscribe to input manager
/// </summary>

namespace inputeer
{
  public class HelperInputObject
  {
    MonoBehaviour owner;

    public InputTouchFinger finger;
    
    protected bool _overring = true; // est-ce que l'objet réagit a la callback d'un doigt qui passe au dessus de lui (après touch event)
    protected bool _interactive = true; // réagit aux touch/release/overring
    protected bool _touching = false; // état press/release (besoin de pas utiliser le doigt directement pour debug possible)
    protected bool _inputLayerAtStart = false;

    public bool onlyDuringArenaLiveState = false;
    public bool dontReactToCapturedFinger = false;
    public bool captureFingerOnTouch = false;

    protected InputTouchBridge _input;
    protected Collider2D[] _colliders;

    //globals
    public Action<InputTouchFinger> cbTouch;
    public Action<InputTouchFinger> cbRelease;
    
    //specifics
    public Action<InputTouchFinger> cbStayOver; // each frame
    public Action<InputTouchFinger, RaycastHit2D> cbTouchOver; // touch
    public Action<InputTouchFinger, RaycastHit2D> cbReleaseOver; // release
    
    public HelperInputObject(MonoBehaviour owner)
    {
      this.owner = owner;
      
      _input = InputTouchBridge.get();
      if (_input == null)
      {
        Debug.LogError("can't subscribe to input if InputTouchBridge doesn't exist at this point");
        return;
      }

      _inputLayerAtStart = (owner.gameObject.layer == LayerMask.NameToLayer("input"));

      List<Collider2D> list = new List<Collider2D>();
      list.AddRange(owner.transform.GetComponents<Collider2D>());
      list.AddRange(owner.transform.GetComponentsInChildren<Collider2D>());
      _colliders = list.ToArray();

      //subscribe to manager
      _input.onTouch += eventOnTouch;
      _input.onRelease += eventOnRelease;
      _input.onOverring += eventOnOverring;

    }

    public void setupCollider(Collider2D[] newColliders)
    {
      _colliders = newColliders;
    }

    virtual public void unlink()
    {
      _input.onTouch -= eventOnTouch;
      _input.onRelease -= eventOnRelease;
    }

    /* un doigt passe par là et y a que moi en dessous */
    private void eventOnOverring(InputTouchFinger finger)
    {
      if (!reactToTouch()) return;

      if (!_overring) return;

      if (finger.isCaptured() && dontReactToCapturedFinger) return;

      RaycastHit2D? hit = getLocalMatchingColliderWithFinger(finger);
      if (hit != null)
      {
        //Debug.Log("overring "+finger.touchedObjects[0].collider.gameObject.name+" , count = "+finger.touchedObjects.Count);
        onOverring(finger);
      }

    }

    /* global touch event (everybody get this event) */
    private void eventOnTouch(InputTouchFinger finger)
    {
      if (!reactToTouch()) return;

      if (dontReactToCapturedFinger)
      {
        if (!finger.isCapturedBy(this) && finger.isCaptured())
        {
          //Debug.Log(name + " can't react to finger " + finger.fingerId + " because is captured by someone else : " + finger.captured, gameObject);
          return;
        }
      }

      //Debug.Log(finger.fingerId + " on touch "+name);

      onTouch(finger);

      RaycastHit2D? hit = getLocalMatchingColliderWithFinger(finger);

      if (hit != null)
      {
        _touching = true;

        //Debug.Log("{InputObject} input touch over " + hit.Value.transform.name);
        eventTouchOver(finger, hit.Value);
      }
    }

    private void eventTouchOver(InputTouchFinger finger, RaycastHit2D hit)
    {
      if (!reactToTouch()) return;

      if (!finger.isCaptured() && captureFingerOnTouch)
      {
        //Debug.Log(name + " <b>captured</b> finger " + finger.fingerId);
        finger.captured = this;
      }

      onTouchOver(finger, hit);
    }

    private void eventOnRelease(InputTouchFinger finger)
    {
      if (!reactToTouch()) return;

      //Debug.Log(name+" release finger "+finger.fingerId);

      if (finger.isCapturedBy(this))
      {
        //Debug.Log(name + " <b>released</b> finger " + finger.fingerId);
        finger.captured = null;
      }

      if (!_interactive) return;

      if (finger.isCaptured() && dontReactToCapturedFinger) return;

      //on a besoin que touching soit MAJ avant d'apl ça pour le cas ou on desactive l'interactivité de l'extérieur
      bool wasTouching = _touching;
      _touching = false;

      onRelease(finger);

      if (wasTouching)
      {
        //chopper le hit correspondant a mes colliders
        RaycastHit2D? hit = getLocalMatchingColliderWithFinger(finger);

        if (hit != null)
        {
          //Debug.Log("{InputObject} input released over " + hit.Value.transform.name);
          onReleaseOver(finger, hit.Value);
        }

      }

      //if (cbRelease != null) cbRelease(finger);

    }


    public void assignFinger(InputTouchFinger finger)
    {
      eventOnTouch(finger);
    }


    virtual protected void onTouch(InputTouchFinger finger)
    {
      if (!reactToTouch()) return;

      this.finger = finger;
      if (cbTouch != null) cbTouch(finger);
    }
    virtual protected void onRelease(InputTouchFinger finger)
    {
      if (!reactToTouch()) return;

      if (finger != this.finger) return;

      this.finger = null;

      InputSelectionManager.manager.remove(this);

      if (cbRelease != null) cbRelease(finger);
    }
    virtual protected void onOverring(InputTouchFinger finger)
    {
      //Debug.Log(name + " overring");
      if (cbStayOver != null) cbStayOver(finger);
    }
    virtual protected void onTouchOver(InputTouchFinger finger, RaycastHit2D hit)
    {
      //Debug.Log(name + " touch over");Debug.Log(cbTouchOver);
      InputSelectionManager.manager.add(this);
      if (cbTouchOver != null) cbTouchOver(finger, hit);
    }
    virtual protected void onReleaseOver(InputTouchFinger finger, RaycastHit2D hit)
    {
      //Debug.Log(name + " release over");
      if (cbReleaseOver != null) cbReleaseOver(finger, hit);
    }



    protected RaycastHit2D? getLocalMatchingColliderWithFinger(InputTouchFinger finger)
    {
      List<RaycastHit2D> rCollisions = finger.touchedObjects;
      for (int i = 0; i < rCollisions.Count; i++)
      {
        //check si le collider trouvé par le doigt correspond a l'un des miens
        if (compareWithLocalCollider(rCollisions[i].collider)) return rCollisions[i];
      }
      return null;
    }
    protected bool compareWithLocalCollider(Component collider)
    {
      for (int i = 0; i < _colliders.Length; i++)
      {
        if (_colliders[i] == collider) return true;
      }
      return false;
    }

    public void setOverringCapacity(bool flag)
    {
      _overring = flag;
    }

    /// <summary>
    /// context that tells the helper if it should react to touch events
    /// </summary>
    /// <returns></returns>
    bool reactToTouch()
    {
      if (!_interactive) return false;

      return true;
    }

    public virtual void setInteractive(bool flag)
    {
      _interactive = flag;

      if (!_interactive) owner.gameObject.layer = 0;
      else
      {
        //reinjecte la layer input si besoin
        if (_inputLayerAtStart) owner.gameObject.layer = LayerMask.NameToLayer("input");
      }

      //Debug.Log(name + " swap to layer " + gameObject.layer, gameObject);

      //LogHomo.interactor("{InputObject} setInteracte(" + flag+") for "+name);
    }
    public bool isInteractive() { return _interactive; }

    public bool isTouching()
    {
      return _touching;
    }
  }
}