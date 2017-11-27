using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace WainTestZone
{
    public class PlayerManager : MonoBehaviour
    {
        [Header("Set 0 to stop updating")]
        [Range(0, 1)]
        public float updateCycle = .3f;

        public float drawVelosityGizmoKoef = 3;

        public Transform targetTransform;
        
        public SteeringElement currentController;

        [SerializeField]
        public Dictionary<SteeringControllerType, SteeringElement> controllers = new Dictionary<SteeringControllerType, SteeringElement>();

        Vector3 nextPosition;
        Rigidbody myRigidbody;

        private void Awake()
        {
            myRigidbody = GetComponent<Rigidbody>();
            var steeringControllers = GetComponentsInChildren<SteeringElement>();
            foreach (var steeringController in steeringControllers)
                controllers.Add(steeringController.Type, steeringController);
            StartCoroutine(DoUpdate());
        }
        public float Mass {
            get { return myRigidbody == null ? 1 : myRigidbody.mass > 1 ? myRigidbody.mass : 1; }
            set { if (myRigidbody != null) myRigidbody.mass = value;  }
        }

        IEnumerator DoUpdate()
        {
            while (true)
            {
                while (updateCycle > 0)
                {
                    if (currentController != null)
                        nextPosition = transform.position + currentController.DoUpdate();
                    yield return new WaitForSeconds(updateCycle);
                }

                yield return new WaitForSeconds(1);
            }
        }

        void Update()
        {
            transform.position = Vector3.Lerp(transform.position, nextPosition, (1.1f - updateCycle) / 10);
        }

        public void SetNewController(SteeringControllerType steeringControllerType)
        {
            if (controllers.ContainsKey(steeringControllerType))
            {
                if (currentController != null)
                    currentController.StopDrawGizmo();
                currentController = controllers[steeringControllerType];
                currentController.Initialize();
            }
        }
    }
}
