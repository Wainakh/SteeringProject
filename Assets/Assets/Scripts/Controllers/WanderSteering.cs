using UnityEngine;
using UnityEngine.UI;

namespace WainTestZone
{
    public class WanderSteering : SteeringElement
    {
        [SerializeField] LineRenderer circleLineRenderer;
        [SerializeField] Color circleColor = Color.green;
        [SerializeField] LineRenderer angleLineRenderer;
        [SerializeField] Color angleColor = Color.blue;

        [SerializeField] float angleChangeRange = 20;
        [SerializeField] float circleRadius = 3f;
        [SerializeField] float circleDistance = 2f;

        float wanderAngle = 0;

        public override Vector3 DoUpdate()
        {
            velocity = base.DoUpdate();

            var circleCentre = Vector3.Normalize(velocity) * circleDistance;

            if (isDrawGizmo) GizmosManager.DrawCircle(circleLineRenderer, circleCentre, circleRadius, circleColor);
            else GizmosManager.StopDraw(circleLineRenderer);

            wanderAngle += Random.Range(-angleChangeRange, angleChangeRange);

            velocity += circleCentre + Vector3.Normalize(SetAngle(Vector3.one, wanderAngle)) * circleRadius;

            if (isDrawGizmo) GizmosManager.DrawLine(angleLineRenderer, circleCentre, velocity, angleColor, 1);
            else GizmosManager.StopDraw(angleLineRenderer);

            velocity = TruncateFromSpeedAndForce(velocity);

            if (isDrawGizmo) GizmosManager.DrawLine(velocityLineRenderer, velocity, velocityLineColor, playerManager.drawVelosityGizmoKoef);
            else GizmosManager.StopDraw(velocityLineRenderer);

            return velocity;
        }

        public override void StopDrawGizmo()
        {
            GizmosManager.StopDraw(velocityLineRenderer);
            GizmosManager.StopDraw(circleLineRenderer);
            GizmosManager.StopDraw(angleLineRenderer);
        }

        Vector3 SetAngle(Vector3 vector, float value)
        {
            var len = vector.magnitude;
            vector.x = Mathf.Cos(value) * len;
            vector.y = 0;
            vector.z = Mathf.Sin(value) * len;
            return vector;
        }
        
        public void ChangeCircleRadius(Slider sl) { circleRadius = sl.value; }
        public void ChangeCircleDistance(Slider sl) { circleDistance = sl.value; }
        public void ChangeAngleChangeRange(Slider sl) { angleChangeRange = sl.value; }
        public void ResetPlayer() { playerTransform.position = Vector3.zero; }
    }
}
