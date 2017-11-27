using UnityEngine;
using UnityEngine.UI;

namespace WainTestZone
{
    public class ArrivalSteering : SteeringElement
    {
        [SerializeField] float slowingRadius = 10f;

        public override Vector3 DoUpdate()
        {
            velocity = base.DoUpdate();

            var currentDistance = Vector3.Magnitude(CropByY(playerManager.targetTransform.position - playerTransform.position));

            if (currentDistance < slowingRadius)
                velocity *= (currentDistance / slowingRadius);

            if (isDrawGizmo) GizmosManager.DrawLine(velocityLineRenderer, velocity, velocityLineColor, playerManager.drawVelosityGizmoKoef);
            else StopDrawGizmo();

            return velocity;
        }

        public override void StopDrawGizmo()
        {
            GizmosManager.StopDraw(velocityLineRenderer);
        }

        public void ChangeSlowingRadius(Slider sl) { slowingRadius = sl.value; }
    }
}

