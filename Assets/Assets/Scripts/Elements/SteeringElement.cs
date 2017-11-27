using UnityEngine;
using UnityEngine.UI;

namespace WainTestZone
{
    [System.Serializable]
    public abstract class SteeringElement : MonoBehaviour
    {
        public SteeringControllerType Type;
        [SerializeField] protected bool isDrawGizmo = true;
        [SerializeField] protected Color velocityLineColor = Color.yellow;
        [SerializeField] protected float maxForce = 1;
        [SerializeField] protected float maxSpeed = 1;
        [SerializeField] protected float maxVelocity = 1;

        protected float mass = 1;
        protected LineRenderer velocityLineRenderer;
        protected Transform playerTransform;
        protected PlayerManager playerManager;
        protected Vector3 velocity, oldPosition, desiredVelocity;

        public abstract void StopDrawGizmo();

        public virtual Vector3 DoUpdate()
        {
            velocity = Vector3.Normalize(velocity) * maxVelocity;
            oldPosition = playerTransform.position;
            desiredVelocity = Vector3.Normalize(CropByY(playerManager.targetTransform.position - playerTransform.position))*maxVelocity;
            var steering = desiredVelocity - velocity;
            if (steering.magnitude > maxForce)
                steering =  Vector3.Normalize(steering) * maxForce;
            steering = new Vector3(steering.x / mass, 0, steering.z / mass);
            velocity = Vector3.Normalize(velocity + steering)* maxSpeed;
            return velocity;
        }

        public virtual void Initialize()
        {
            velocityLineRenderer = GetComponent<LineRenderer>();
            playerManager = GetComponentInParent<PlayerManager>();
            playerTransform = playerManager.transform;
            mass = playerManager.Mass;
        }

        protected Vector3 TruncateFromSpeedAndForce(Vector3 value)
        {            
            if (value.magnitude > maxForce)
                value = Vector3.Normalize(value) * maxForce;
            value /= mass;
            return Vector3.Normalize(velocity + value) * maxSpeed;
        }

        protected Vector3 Truncate(Vector3 vector, float maxValue)
        {
            return new Vector3(
                Mathf.Abs(vector.x) > maxValue ? maxValue * Mathf.Sign(vector.x) : vector.x,
                Mathf.Abs(vector.y) > maxValue ? maxValue * Mathf.Sign(vector.y) : vector.y,
                Mathf.Abs(vector.z) > maxValue ? maxValue * Mathf.Sign(vector.z) : vector.z);
        }

        protected  Vector3 CropByY(Vector3 value, float y =0)
        {
            return new Vector3(value.x, y, value.z);
        }

        protected Vector3 Normalize(Vector3 value)
        {
            return new Vector3(Mathf.Sign(value.x), Mathf.Sign(value.y), Mathf.Sign(value.z));
        }

        public virtual void ChangeDrawGizmo(Toggle tg) { isDrawGizmo = tg.isOn; }
        public virtual void ChangeMaxForce(Slider sl) { maxForce = sl.value; }
        public virtual void ChangeMaxSpeed(Slider sl) { maxSpeed = sl.value; }
        public virtual void ChangeMaxVelocity(Slider sl) { maxVelocity = sl.value; }
        public virtual void ChangeMass(Slider sl) { mass = sl.value; }
    }
}