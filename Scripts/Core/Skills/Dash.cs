using SuspiciousGames.Saligia.Core.Entities.Player;
using System.Collections;
using UnityEngine;

namespace SuspiciousGames.Saligia.Core.Skills
{
    [CreateAssetMenu(fileName = "newDashSkill", menuName = "Saligia/Skills/Movement/Dash")]
    public class Dash : BaseSkill
    {
        [SerializeField] private float _dashSpeed;
        private Coroutine _dashRoutine;

        private UnityEngine.AI.ObstacleAvoidanceType _previousAvoidanceType;

        private void OnDestroy()
        {
            if (_dashRoutine != null && CasterEntity)
                CasterEntity.StopCoroutine(_dashRoutine);
        }

        protected override void Logic()
        {
            var playerEntity = (PlayerEntity)CasterEntity;
            _previousAvoidanceType = CasterEntity.MovementComponent.Agent.obstacleAvoidanceType;
            CasterEntity.MovementComponent.Agent.obstacleAvoidanceType = UnityEngine.AI.ObstacleAvoidanceType.NoObstacleAvoidance;
            if (playerEntity != null)
            {
                playerEntity.ForceForward();
                playerEntity.ActivateDashTrail(true);
            }
            CasterEntity.MovementComponent.BlockMovement(true);
            CasterEntity.MovementComponent.BlockRotation(true);

            _dashRoutine = CasterEntity.MovementComponent.StartCoroutine(DashRoutine());
        }

        public override void CleanUp()
        {
            var playerEntity = (PlayerEntity)CasterEntity;
            if (playerEntity != null)
                playerEntity.ActivateDashTrail(false);

            CasterEntity.MovementComponent.BlockMovement(false);
            CasterEntity.MovementComponent.BlockRotation(false);
            CasterEntity.MovementComponent.StopCoroutine(_dashRoutine);
            CasterEntity.MovementComponent.Agent.obstacleAvoidanceType = _previousAvoidanceType;
        }

        private IEnumerator DashRoutine()
        {
            while (true)
            {
                CasterEntity.MovementComponent.ForceMove(CasterEntity.transform.forward * Time.fixedDeltaTime * _dashSpeed, true);
                yield return new WaitForFixedUpdate();
            }
        }
    }
}
