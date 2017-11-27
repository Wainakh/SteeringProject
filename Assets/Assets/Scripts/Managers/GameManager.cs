using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WainTestZone
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField]
        SteeringControllerType startMode = SteeringControllerType.Seek;

        [SerializeField]
        List<CanvasElement> canvases = new List<CanvasElement>();

        private void Start()
        {
            SetNewSteering(startMode);
        }

        public void SetNewSteering(Dropdown dropdown)
        {
            SetNewSteering(
                (SteeringControllerType)
                SteeringControllerType.Parse(typeof(SteeringControllerType), 
                dropdown.captionText.text.Replace(" ", string.Empty), true));            
        }

        void SetNewCanvas(SteeringControllerType newType)
        {
            foreach (var canvas in canvases)
            {
                if (canvas.type == newType)
                {
                    if (canvas.canvasObject != null)
                        canvas.canvasObject.SetActive(true);
                }
                else if (canvas.canvasObject != null)
                    canvas.canvasObject.SetActive(false);
            }
        }

        void SetNewSteering(SteeringControllerType steeringControllerType)
        {
            var playerManagers = FindObjectsOfType<PlayerManager>();
            if (playerManagers != null)
            {
                foreach (var playerManager in playerManagers)
                {
                    playerManager.SetNewController(steeringControllerType);
                }
                SetNewCanvas(steeringControllerType);
            }
        }
    }

    public enum SteeringControllerType
    {
        Seek,
        Flee,
        Arrival,
        Wander,
        CollisionAvoidance,
        LeaderFollowing,
        DontWorks,
    }

    [System.Serializable]
    public class CanvasElement
    {
        public SteeringControllerType type;
        public GameObject canvasObject;
    }


    
}