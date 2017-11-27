using UnityEngine;

namespace WainTestZone
{
    [System.Serializable]
    public class SeekSteering : SteeringElement
    {
        public override Vector3 DoUpdate()
        {
            velocity = base.DoUpdate(); 
            if (isDrawGizmo) GizmosManager.DrawLine(velocityLineRenderer, velocity, velocityLineColor, playerManager.drawVelosityGizmoKoef);
            else StopDrawGizmo();
            return velocity;
        }

        public override void StopDrawGizmo()
        {
            GizmosManager.StopDraw(velocityLineRenderer);
        }       
    }
}
