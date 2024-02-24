using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 2018-10-05
/// This script is meant to display some raw information regarding Unity's native input data
/// </summary>

namespace fwp.inputeer.debug
{
    public class InputTouchRawGUI : MonoBehaviour
    {

        List<Touch> history = new List<Touch>();

        void OnGUI()
        {
            string ct = "[UNITY]";
            ct += "\n touchCount ? " + Input.touchCount;
            ct += "\n touches[] length ? " + Input.touches.Length;

            for (int i = 0; i < Input.touches.Length; i++)
            {
                history.Add(Input.touches[i]);
            }

            int start = history.Count - 10;
            start = Mathf.Max(start, 0);
            int end = history.Count;

            for (int i = start; i < end; i++)
            {
                ct += "\n " + i + " " + history[i].fingerId + " " + history[i].phase.ToString() + " " + history[i].position;
            }

            GUI.Label(new Rect(10, 10, 300, 300), ct);
        }
    }
}