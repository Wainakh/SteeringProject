using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WainTestZone
{
    public class CollisionAvoidanceSteering : SteeringElement
    {
        [SerializeField] LineRenderer collisionAvoidanceLineRenderer;
        [SerializeField] Color collisionAvoidanceLineRendererColor = Color.red;
        [SerializeField] LineRenderer circleLineRenderer;
        [SerializeField] Color circleLineRendererColor = Color.red;
        [SerializeField] LineRenderer toTargetLineRenderer;
        [SerializeField] Color toTargetLineRendererColor = Color.green;

        [SerializeField] float MaxSeeAhead = 1;
        [SerializeField] [Range(0, 1)] float MaxAvoidForce = 1;
        [SerializeField] float ObstacleRadius = 1;

        [SerializeField]
        List<Obstacle> obstacles = new List<Obstacle>();

        public override Vector3 DoUpdate()
        {
            velocity = base.DoUpdate();

            if (isDrawGizmo) GizmosManager.DrawLine(toTargetLineRenderer, velocity, toTargetLineRendererColor, MaxSeeAhead);
            else GizmosManager.StopDraw(toTargetLineRenderer);

            velocity += CollisionAvoidance(playerTransform.position, velocity, obstacles, MaxAvoidForce);

            velocity = TruncateFromSpeedAndForce(velocity);

            if (isDrawGizmo) GizmosManager.DrawLine(velocityLineRenderer, velocity, velocityLineColor, playerManager.drawVelosityGizmoKoef);
            else GizmosManager.StopDraw(velocityLineRenderer);

            return velocity;
        }

        private Vector3 CollisionAvoidance(Vector3 playerPosition, Vector3 velocity, List<Obstacle> obstacles, float maxAvoidForce = 1)
        {
            velocity = Vector3.Normalize(velocity);

            var mostThreatening = FindMostThreateningObstacle(playerPosition, velocity * MaxSeeAhead, obstacles);

            if (mostThreatening != null)
            {
                var avoidance = Vector3.Normalize(CropByY(mostThreatening.transform.position - playerPosition, 0)) * -maxAvoidForce;
                if (isDrawGizmo)
                {
                    GizmosManager.DrawCircle(circleLineRenderer, CropByY(mostThreatening.transform.position, -.5f) - playerPosition, mostThreatening.radius, circleLineRendererColor);
                    GizmosManager.DrawLine(collisionAvoidanceLineRenderer, avoidance.normalized * maxAvoidForce, collisionAvoidanceLineRendererColor, maxAvoidForce);
                }
                return avoidance;
            }
            GizmosManager.StopDraw(circleLineRenderer);
            GizmosManager.StopDraw(collisionAvoidanceLineRenderer);

            return Vector3.zero;
        }

        private Obstacle FindMostThreateningObstacle(Vector3 playerPosition, Vector3 ahead, List<Obstacle> obstacles)
        {
            Obstacle mostThreatening = null;

            if (obstacles.Count > 0)
                for (int i = 0; i < obstacles.Count; i++)
                {
                    var obstacle = obstacles[i];

                    var collision = Vector3.Distance(ahead + playerPosition, obstacle.center) < obstacle.radius ?
                        true : Vector3.Distance((ahead * .5f) + playerPosition, obstacle.center) < obstacle.radius ?
                        true : false;

                    if (collision &&
                        (mostThreatening == null || Vector3.Distance(playerPosition, obstacle.center) < Vector3.Distance(playerPosition, mostThreatening.center)))
                        mostThreatening = obstacle;
                }
            return mostThreatening;
        }

        public void CreateNewObstacle()
        {
            CreateNewObstacle(playerManager.targetTransform.position, ObstacleRadius);
        }

        public void CreateNewObstacle(Vector3 position, float radius = 1)
        {
            var o = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            o.layer = LayerMask.NameToLayer("Ignore Raycast");
            var r = o.GetComponent<Renderer>();
            r.material.color = Color.yellow;
            var c = o.GetComponent<Collider>();
            c.isTrigger = true;
            o.transform.position = position;
            var oo = (Obstacle)o.AddComponent(typeof(Obstacle));
            oo.SetPrefab(o);
            oo.SetRadius(radius);
            obstacles.Add(oo);
        }

        public override void Initialize()
        {
            velocityLineRenderer = GetComponent<LineRenderer>();
            playerManager = GetComponentInParent<PlayerManager>();
            playerTransform = playerManager.transform;
            mass = playerManager.Mass;
        }

        public override void StopDrawGizmo()
        {
            GizmosManager.StopDraw(velocityLineRenderer);
            GizmosManager.StopDraw(collisionAvoidanceLineRenderer);
            GizmosManager.StopDraw(toTargetLineRenderer);
            GizmosManager.StopDraw(circleLineRenderer);
            if (obstacles.Count > 0)
                foreach (var o in obstacles)
                    Destroy(o.gameObject);
            obstacles = new List<Obstacle>();
        }

        public void ChangeMaxSeeAhead(Slider sl) { MaxSeeAhead = sl.value; }
        public void ChangeMaxAvoidForce(Slider sl) { MaxAvoidForce = sl.value; }
        public void ChangeObstacleRadius(Slider sl)
        {
            ObstacleRadius = sl.value;
            if (obstacles.Count > 0)
                foreach (var o in obstacles)
                    o.SetRadius(ObstacleRadius);
        }
    }

    [System.Serializable]
    public class Obstacle : MonoBehaviour
    {
        public GameObject prefab;
        public Vector3 center;
        public float radius = 1;

        public Obstacle(GameObject prefab, float radius)
        {
            this.prefab = prefab;
            center = transform.position;
            this.radius = radius;
            this.prefab.transform.localScale = Vector3.one * this.radius;
        }

        public void SetPrefab(GameObject prefab)
        {
            this.prefab = prefab;
            center = transform.position;
        }

        public void SetRadius(float radius)
        {
            center = transform.position;
            this.radius = radius;
            prefab.transform.localScale = Vector3.one * this.radius;
        }
    }
}
