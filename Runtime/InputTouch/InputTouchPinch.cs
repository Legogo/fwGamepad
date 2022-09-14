using System.Collections;
using System;
using UnityEngine;

namespace inputeer
{
  public class InputTouchPinch
  {

    float delta; // last move spread delta
    float magnitude; // spread done by user (relative to original spread)

    float lastRawSpread; // actual spread on screen btw the 2 fingers (absolute)
    float onPressSpread; // local var to solve magnitude

    InputTouchFinger _fingerA;
    InputTouchFinger _fingerB;

    public Action<float, float> onScroll; // delta , total magnitude

    InputTouchBridge iBridge;

    public InputTouchPinch(InputTouchBridge itb)
    {
      iBridge = itb;

      itb.onTouch += onPress;

      //a canceled finger will get an id of -1
      //itb.onRelease += onRelease;

      //no need
      //itb.onTouching += onTouching;

      reset();
    }

    public void onPress(InputTouchFinger finger)
    {
      if (_fingerA == null)
      {
        _fingerA = finger;
        return;
      }

      if (finger == _fingerA) return;

      if (_fingerB == null)
      {
        _fingerB = finger;
      }

      lastRawSpread = getDistanceBetweenFingers(); //current spread is neutral
    }

    public void reset()
    {
      lastRawSpread = 0f;
      delta = 0f;
    }

    float getInputDelta()
    {
      float output = 0f;

      if (Input.mouseScrollDelta.y != 0f)
      {
        output = Input.mouseScrollDelta.y * iBridge.mouseScrollMulFactor;
      }
      else if (isPinching())
      {
        float newSpread = getDistanceBetweenFingers() * iBridge.touchSpreadMulFactor;
        output = newSpread - lastRawSpread;
        lastRawSpread = newSpread;
      }

      return output;
    }

    void cleanFingers()
    {

      if (_fingerA != null && _fingerA.fingerId < 0) _fingerA = null;
      if (_fingerB != null && _fingerB.fingerId < 0) _fingerB = null;

    }

    public void update()
    {
      cleanFingers(); // clean any stored fingers that was released last frame

      delta = getInputDelta();

      float frameMagn = magnitude;

      if (delta != 0f)
      {
        magnitude += delta;
      }

      Vector2 clamp = iBridge.scrollClampMagnitude;

      if (iBridge.useOvershoot)
      {
        float speed = (Time.deltaTime * iBridge.overshootClampMulFactor);

        //can't overshoot too far
        magnitude = Mathf.Clamp(magnitude, clamp.x - iBridge.overshootClampLength, clamp.y + iBridge.overshootClampLength);

        //overshoot go back to original clamping
        if (magnitude < clamp.x)
        {
          magnitude = Mathf.MoveTowards(magnitude, clamp.x, speed);
        }
        else if (magnitude > iBridge.scrollClampMagnitude.y)
        {
          magnitude = Mathf.MoveTowards(magnitude, clamp.y, speed);
        }
      }
      else
      {
        //no overshoot == force clamp
        magnitude = Mathf.Clamp(magnitude, clamp.x, clamp.y);
      }

      //only called when change is applied
      if (frameMagn != magnitude)
      {
        if (onScroll != null) onScroll(delta, magnitude);
      }

    }

    /// <summary>
    /// distance btw the 2 fingers (screen positions)
    /// </summary>
    /// <returns></returns>
    public float getDistanceBetweenFingers()
    {
      return Vector2.Distance(_fingerB.screenPosition, _fingerA.screenPosition);
    }

    public bool atMaxMagnitude()
    {
      if (iBridge.scrollClampMagnitude.sqrMagnitude == 0f) return false;
      return magnitude >= iBridge.scrollClampMagnitude.y;
    }

    public bool isPinching()
    {
      //not assigned
      if (_fingerA == null || _fingerB == null) return false;

      //released, canceled
      //if (_fingerA != null && _fingerA.fingerId < 0) return false;
      //if (_fingerB != null && _fingerB.fingerId < 0) return false;

      return true;
    }

    public string toString()
    {
      string ct = "[PINCH]";

      ct += "\n A : " + ((_fingerA != null && _fingerA.fingerId > -1) ? _fingerA.fingerId.ToString() : "none");
      ct += "\n B : " + ((_fingerB != null && _fingerB.fingerId > -1) ? _fingerB.fingerId.ToString() : "none");

      ct += "\n dlta      : " + delta;
      ct += "\n magn      : " + magnitude;
      return ct;
    }
  }

}
