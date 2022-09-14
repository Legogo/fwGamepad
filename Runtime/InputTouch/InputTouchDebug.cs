using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// this will display data on screen about whatever is going on in the whole InputTouchBridge thing
/// </summary>
namespace fwp.inputeer
{
  public class InputTouchDebug : MonoBehaviour
  {

    InputTouchBridge _bridge;

    int trackingFingerId = 0; // change this index to display more info about one specific finger
    List<Vector3> deltas = new List<Vector3>();

    void Awake()
    {
      _bridge = InputTouchBridge.get();
    }

    Rect guiRec = new Rect();
    GUIStyle _style;
    private void OnGUI()
    {
      if (_bridge == null) return;

      if (_style == null)
      {
        _style = new GUIStyle();
        _style.fontSize = 30;
      }

      string ct = "[INPUT-DEBUG]";

      ct += "\n framerate : " + Application.targetFrameRate;
      ct += "\n fingers count : " + _bridge.countFingers();

      if (_bridge.countFingers() <= 0)
      {

        deltas.Clear();

      }

      InputTouchFinger[] _fingers = _bridge.getFingers();

      for (int i = 0; i < _fingers.Length; i++)
      {
        ct += "\n  " + _fingers[i].fingerId + " (" + _fingers[i].phase.ToString() + ") position : " + _fingers[i].screenPosition + " ,  delta : " + _fingers[i].screenDeltaPosition;

        if (_fingers[i].fingerId == trackingFingerId)
        {
          if (deltas.Count <= 0) deltas.Add(_fingers[i].screenDeltaPosition);
          else
          {
            //log only changing deltas

            /*
            Debug.Log("last delta");
            Debug.Log(deltas[deltas.Count - 1].toStringDetailed());

            Debug.Log("current finger delta");
            Debug.Log(_fingers[i].deltaPosition.toStringDetailed());
            */

            //only moving deltas
            if (_fingers[i].screenDeltaPosition.sqrMagnitude != 0f)
            {
              deltas.Add(_fingers[i].screenDeltaPosition);
              if (deltas.Count > 50) deltas.RemoveAt(0);
            }

          }

          for (int j = 0; j < deltas.Count; j++)
          {
            ct += "\n    " + j + " " + deltas[j];
          }

        }
      }

      guiRec.x = guiRec.y = 10;
      guiRec.width = Screen.width;
      guiRec.height = Screen.height;

      GUI.Label(guiRec, ct, _style);

      //toggle speed
      if (GUI.Button(new Rect(30, Screen.height - 300f, 200, 100f), "framerate (" + Application.targetFrameRate + ")", _style))
      {
        Application.targetFrameRate = (Application.targetFrameRate >= 60) ? 1 : 60;
      }
    }

  }
}