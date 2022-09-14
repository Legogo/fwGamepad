using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace inputeer
{
  public class InputGyroBridge : MonoBehaviour
  {

    Gyroscope gyro;

    public float desktop_scaling = 1f;
    public float mobile_scaling = 1f;

    private void Awake()
    {

      if (!SystemInfo.supportsGyroscope)
      {
        Debug.LogWarning("don't support gyro !");
        return;
      }

      gyro = Input.gyro;
      gyro.enabled = true;

    }

    public float getLateralSlantValue()
    {
      if (InputTouchBridge.isMobile()) return gyro.gravity.x;
      return -gyro.gravity.x;
    }

    private void OnGUI()
    {
      string ct = "[Input Gyro]";

      ct += "\n L supported ? " + SystemInfo.supportsGyroscope;
      if (gyro != null)
      {
        ct += "\n L enabled ? " + gyro.enabled;
        ct += "\n L gravity " + gyro.gravity;
        ct += "\n L userAccel " + gyro.userAcceleration;
        ct += "\n L attitude " + gyro.attitude;
        ct += "\n L rotation " + gyro.rotationRate;
        ct += "\n L rotation (unbiased) " + gyro.rotationRateUnbiased;
      }

      ct += "\n[Acceleration]";
      ct += "\n L " + Input.acceleration;

      GUIStyle style = new GUIStyle();
      style.fontSize = 40;

      GUI.Label(new Rect(10f, 50f, 500f, 500f), ct, style);
    }

  }
}