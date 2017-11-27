using UnityEngine;

namespace WainTestZone
{
    public class GizmosManager : MonoBehaviour
    {
        public static void DrawLine(LineRenderer lr, Vector3 end, Color color, float koef)
        {
            if (lr != null)
            {
                lr.startColor = lr.endColor = color;
                lr.positionCount = 2;
                lr.SetPosition(0, Vector3.zero);
                lr.SetPosition(1, new Vector3(end.x, 0, end.z) * koef);
            }
        }

        public static void DrawLine(LineRenderer lr, Vector3 start, Vector3 end, Color color, float koef)
        {
            if (lr != null)
            {
                lr.startColor = lr.endColor = color;
                lr.positionCount = 2;
                lr.SetPosition(0, start);
                lr.SetPosition(1, new Vector3(end.x, 0, end.z) * koef);
            }
        }

        public static void DrawCircle(LineRenderer lr, Vector3 center, float radius, Color color)
        {
            if (lr != null)
            {
                lr.startColor = lr.endColor = color;
                lr.loop = true;
                lr.positionCount = 13;
                for (int k = 0; k < 13; k++)
                {
                    lr.SetPosition(k, (center + new Vector3(Mathf.Sin((float)k / 2), 0, Mathf.Cos((float)k / 2)) * radius));
                }
            }
        }

        public static void StopDraw(LineRenderer lr)
        {
            if (lr != null)
            {
                lr.endColor = lr.startColor = new Color(1, 1, 1, 0);
                lr.positionCount = 0;
            }
        }
    }
}
