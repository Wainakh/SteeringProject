using UnityEngine;

namespace WainTestZone
{
    [System.Serializable]
    public class LeaderFollowingSteering : SteeringElement
    {
        [SerializeField] Color normalRenderColor = Color.blue;
        [SerializeField] Color leaderAheadRenderColor = Color.blue;

        [SerializeField] LineRenderer leaderAvoidanceLineRenderer;
        [SerializeField] Color leaderAvoidanceLineRendererColor = Color.blue;

        [SerializeField] float slowingRadius = 10f;

        public float separationRadius = 3;
        public float maxSeparation = 1;
        Material material;
        public LeaderSteering Leader;

        public override Vector3 DoUpdate()
        {
            velocity = Vector3.Normalize(velocity) * maxVelocity;
            oldPosition = playerTransform.position;
            var currentDistance = CropByY(Leader.follovingPosition- playerTransform.position,0);
            desiredVelocity = currentDistance.normalized * maxVelocity;
            var steering = desiredVelocity - velocity;
            if (steering.magnitude > maxForce)
                steering = Vector3.Normalize(steering) * maxForce;
            steering = new Vector3(steering.x / mass, 0, steering.z / mass);
            velocity = Vector3.Normalize(velocity + steering) * maxSpeed;

            if (currentDistance.magnitude < slowingRadius)
                velocity *= (currentDistance.magnitude / slowingRadius);

            if ((Vector3.Distance(Leader.Velocity.normalized * Leader.LeaderAhead + Leader.transform.position, this.transform.position) <= Leader.LeaderSightRadius) ||
                (Vector3.Distance(Leader.transform.position, this.transform.position) <= Leader.LeaderSightRadius))
            {
                var leaderAhead = LeaderAhead();
                velocity += leaderAhead;
                SetColor(leaderAheadRenderColor);
                if (isDrawGizmo) GizmosManager.DrawLine(leaderAvoidanceLineRenderer, leaderAhead, leaderAvoidanceLineRendererColor, Leader.LeaderAhead);
                else GizmosManager.StopDraw(leaderAvoidanceLineRenderer);
            }
            else
            {
                velocity += Separation();
                SetColor(normalRenderColor);
                GizmosManager.StopDraw(leaderAvoidanceLineRenderer);
            }

            velocity = TruncateFromSpeedAndForce(velocity);
            if (isDrawGizmo) GizmosManager.DrawLine(velocityLineRenderer, velocity, velocityLineColor, playerManager.drawVelosityGizmoKoef);
            else GizmosManager.StopDraw(velocityLineRenderer);

            return velocity;
        }

        private Vector3 Separation()
        {
            if (MinionsManager.inst.minions.Count > 0)
            {
                var force = new Vector3();
                int neighborCount = 0;

                for (var i = 0; i < MinionsManager.inst.minions.Count; i++)
                {
                    LeaderFollowingSteering minionController = MinionsManager.inst.minions[i];

                    if (minionController != this &&
                        Vector3.Distance(minionController.transform.position, playerTransform.position) <= separationRadius)
                    {
                        force.x += minionController.transform.position.x - this.transform.position.x;
                        force.z += minionController.transform.position.z - this.transform.position.z;
                        neighborCount++;
                    }
                }

                if (neighborCount != 0)
                {
                    force.x /= neighborCount;
                    force.z /= neighborCount;
                    force = force.normalized * -1;
                }
                return force.normalized * maxSeparation;
            }
            return Vector3.zero;
        }

        public Vector3 LeaderAhead()
        {
            Vector3 leaderAheadForce = Leader.transform.position - transform.position;
            leaderAheadForce = leaderAheadForce.normalized * -Leader.LeaderAhead;
            return leaderAheadForce;
        }

        public override void Initialize()
        {
            base.Initialize();
            var r = playerManager.gameObject.GetComponent<Renderer>();
            if (r != null) material = r.material;
            var playerManagerComponent = playerManager.targetTransform.GetComponent<PlayerManager>();
            if (playerManagerComponent != null)
            {
                var curController = playerManagerComponent.currentController;
                if (curController.Type == SteeringControllerType.LeaderFollowing)
                    Leader = curController as LeaderSteering;
            }
        }

        public override void StopDrawGizmo()
        {
            GizmosManager.StopDraw(velocityLineRenderer);
            GizmosManager.StopDraw(leaderAvoidanceLineRenderer);
        }

        public void SetColor(Color color)
        {
            if (material != null)
            {
                if (material.color != color)
                    material.color = color;
            }
        }
        public void SetIsDrawGizmo(bool value)
        {
            isDrawGizmo = value;
        }
    }


    
}
