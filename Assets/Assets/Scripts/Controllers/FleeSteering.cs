using UnityEngine;
using UnityEngine.UI;

namespace WainTestZone
{
    public class FleeSteering : SteeringElement
    {   
        [SerializeField] float slowingRadius = 10f;

        public override Vector3 DoUpdate()
        {
            velocity = base.DoUpdate() *-1;

            velocity *= ArrivalForce();

            if (isDrawGizmo) GizmosManager.DrawLine(velocityLineRenderer,velocity, velocityLineColor,playerManager.drawVelosityGizmoKoef);
            else GizmosManager.StopDraw(velocityLineRenderer);

            return velocity;
        }

        float ArrivalForce()
        {
            var distance = Vector3.Distance(playerManager.targetTransform.position, playerTransform.position);
            if (distance > slowingRadius) return 0;
            else return (slowingRadius - distance) / slowingRadius;
        }

        public override void StopDrawGizmo()
        {
            GizmosManager.StopDraw(velocityLineRenderer);
        }

        public void ChangeSlowingRadius(Slider sl) { slowingRadius = sl.value; }
    }
}


