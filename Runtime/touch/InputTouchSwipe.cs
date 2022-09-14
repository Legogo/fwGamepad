using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace inputeer
{
  public class InputTouchSwipe
  {
    //InputTouchBridge iBridge;
    public Action<int, int> onSwipe;
    public Action<Vector2> onSwipeDelta;

    protected InputTouchFinger _finger;

    float limitLifeTime;
    float limitSwipeAmplitude;

    InputTouchBridge iBridge;

    public InputTouchSwipe(InputTouchBridge itb)
    {
      iBridge = itb;
      iBridge.onTouch += delegate (InputTouchFinger finger)
      {
        _finger = finger;
      };

      iBridge.onRelease += onRelease;

      limitLifeTime = iBridge.limitLifeTime;
      limitSwipeAmplitude = iBridge.limitSwipeAmplitude;
    }

    protected void onRelease(InputTouchFinger finger)
    {
      if (finger != _finger) return;

      Vector2 delta = _finger.getVectorFromStart();

      if (_finger.lifeTime < limitLifeTime)
      {
        int horizontal = 0;
        if (Mathf.Abs(delta.x) > limitSwipeAmplitude) horizontal = (int)Mathf.Sign(delta.x);

        int vertical = 0;
        if (Mathf.Abs(delta.y) > limitSwipeAmplitude) vertical = (int)Mathf.Sign(delta.y);

        if (horizontal != 0 || vertical != 0)
        {
          //if(Mathf.Abs(delta.x) >= Mathf.Abs(delta.y)) if (onSwipe != null) onSwipe(horizontal, 0);
          //else if (Mathf.Abs(delta.x) < Mathf.Abs(delta.y)) if (onSwipe != null) onSwipe(0, vertical);
          if (onSwipe != null) onSwipe(horizontal, vertical);

          if (onSwipeDelta != null) onSwipeDelta(delta);
        }

      }

    }
  }

}
