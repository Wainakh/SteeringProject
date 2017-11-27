using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WainTestZone
{
    public class MinionsManager : MonoBehaviour
    {
        public static MinionsManager inst;
        void Awake()
        {
            if (inst == null)
                inst = this;
        }

        [SerializeField] GameObject minionPrefab;
        [SerializeField] Transform playerTransform;
        public List<LeaderFollowingSteering> minions = new List<LeaderFollowingSteering>();

        public void CreateNewMinion()
        {
            var gameObject = Instantiate(minionPrefab);
            var playerManager = gameObject.GetComponent<PlayerManager>();
            playerManager.targetTransform = playerTransform;
            playerManager.SetNewController(SteeringControllerType.LeaderFollowing);
            var leaderFollowingSteering = playerManager.currentController as LeaderFollowingSteering;
            minions.Add(leaderFollowingSteering);
        }

        public void DeleteAllMinions()
        {
            foreach (var minion in minions)
                Destroy(minion.transform.parent.gameObject);
            minions = new List<LeaderFollowingSteering>();
        }

        public void SetSeparationRadius(Slider sl)
        {
            if (minions.Count > 0)
                foreach (var minion in minions)
                    minion.separationRadius = sl.value;
        }

        public void SetDrawGizmo(bool value)
        {
            if (minions.Count > 0)
                foreach (var minion in minions)
                    minion.SetIsDrawGizmo(value);
        }

        public void SetMaxSeparation(Slider sl)
        {
            if (minions.Count > 0)
                foreach (var minion in minions)
                    minion.maxSeparation = sl.value;
        }
    }
}
