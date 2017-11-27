using UnityEngine;
using UnityEngine.UI;

namespace WainTestZone
{
    public class LeaderSteering : SteeringElement
    {
        [SerializeField] LineRenderer FollowLine;
        [SerializeField] Color FollowLineColor = Color.green;
        [SerializeField] LineRenderer LeaderCircle;
        [SerializeField] Color LeaderCircleColor = Color.green;
        [SerializeField] LineRenderer LeaderAheadCircle;
        [SerializeField] Color LeaderAheadCircleColor = Color.green;

        public Color MinionVelocityColor = Color.blue;

        [SerializeField] float slowingRadius = 10f;
        [SerializeField] float DistanceToFollow = 2;
        [SerializeField] public float LeaderAhead = 2;
        [SerializeField] public float LeaderSightRadius = 2;
        [HideInInspector] public Vector3 follovingPosition;
        [HideInInspector] public Vector3 Velocity { get { return velocity; } }
            
        public override Vector3 DoUpdate()
        {
            velocity = base.DoUpdate();

            follovingPosition = velocity.normalized * -DistanceToFollow;

            if (isDrawGizmo)
            {
                GizmosManager.DrawLine(FollowLine, follovingPosition, FollowLineColor, 1);
                GizmosManager.DrawCircle(LeaderCircle, Vector3.zero, LeaderSightRadius, LeaderCircleColor);
                GizmosManager.DrawCircle(LeaderAheadCircle, velocity * LeaderAhead, LeaderSightRadius, LeaderAheadCircleColor);
            }
            else
            {
                GizmosManager.StopDraw(FollowLine);
                GizmosManager.StopDraw(LeaderCircle);
                GizmosManager.StopDraw(LeaderAheadCircle);
            }

            follovingPosition += playerTransform.position;

            var currentDistance = Vector3.Distance(playerManager.targetTransform.position, playerTransform.position);

            if (currentDistance < slowingRadius)
                velocity *= (currentDistance / slowingRadius);

            if (isDrawGizmo) GizmosManager.DrawLine(velocityLineRenderer, velocity, velocityLineColor, playerManager.drawVelosityGizmoKoef);
            else GizmosManager.StopDraw(velocityLineRenderer);

            return velocity;
        }

        public override void StopDrawGizmo()
        {
            GizmosManager.StopDraw(FollowLine);
            GizmosManager.StopDraw(velocityLineRenderer);
            GizmosManager.StopDraw(LeaderCircle);
            GizmosManager.StopDraw(LeaderAheadCircle);
            MinionsManager.inst.DeleteAllMinions();
        }
        public override void ChangeDrawGizmo(Toggle tg)
        {
            isDrawGizmo = tg.isOn;
            MinionsManager.inst.SetDrawGizmo(isDrawGizmo);
        }
        public void ChangeSlowingRadius(Slider sl) { slowingRadius = sl.value; }
        public void ChangeDistanceToFollow(Slider sl) { DistanceToFollow = sl.value; }
        public void ChangeLeaderAhead(Slider sl) { LeaderAhead = sl.value; }
        public void ChangeLeaderSightRadius(Slider sl) { LeaderSightRadius = sl.value; }
    }
}
