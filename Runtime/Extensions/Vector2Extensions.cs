using UnityEngine;

static public class Vector2Extensions
{

    /// <summary>
    /// [X,Y] normalized ?
    /// 
    /// DIRECTION
    ///      0
    ///  -90   90
    ///     180
    /// </summary>
    static public Vector2 get6D(this Vector2 joyRaw, float deadZone = 0.5f, bool verbose = false)
    {
        // not too small (more than half way)
        if (joyRaw.magnitude < deadZone) return Vector2.zero;

        float aSection = 360f / 8f;

        //Debug.Log("---");
        if (verbose) Debug.Log("?" + joyRaw);
        //Debug.Log("section?" + aSection);

        joyRaw = joyRaw.normalized;

        float hSection = aSection * 0.5f;

        //Vector2 origin = rotate(Vector2.right, hSection);

        // CW +
        // CCW -
        float angle = -Vector2.SignedAngle(joyRaw, Vector2.right);
        float sCenter;

        //ret.y = angle != 180f && angle != 0f ? 1f : 0f;
        int cnt = 0;

        float bAngle = angle;

        // -180,0
        if (angle < 0f)
        {
            while (bAngle < -hSection)
            {
                bAngle += aSection;
                cnt++;
                //if (bAngle < -hSection) cnt++;
            }
        }
        else if (angle > 0f)
        {
            while (bAngle > hSection)
            {
                bAngle -= aSection;
                cnt++;
                //if (bAngle > hSection) cnt++;
            }
        }

        sCenter = cnt * aSection * Mathf.Sign(angle);

        if (verbose) Debug.Log("angle ? " + angle + " , cnt?" + cnt + " , center?" + sCenter);

        sCenter *= Mathf.Deg2Rad;

        float c = Mathf.Cos(sCenter);
        float s = Mathf.Sin(sCenter);
        //if (Mathf.Abs(c) <= Mathf.Epsilon) c = 0f;
        //if (Mathf.Abs(s) <= Mathf.Epsilon) s = 0f;
        if (verbose) Debug.Log(c + "x" + s);

        Vector2 ret;
        ret.x = Mathf.Round(c);
        ret.y = Mathf.Round(s);

        return ret;
    }

    public static Vector2 rotate(this Vector2 v, float delta)
    {
        return new Vector2(
            v.x * Mathf.Cos(delta) - v.y * Mathf.Sin(delta),
            v.x * Mathf.Sin(delta) + v.y * Mathf.Cos(delta)
        );
    }

}
