using UnityEngine;

namespace SuspiciousGames.Saligia.Core.Skills
{
    public class TimedDestroyModuleBehaviour : ModuleBehaviour
    {
        private float _destroyAfterSeconds;

        public TimedDestroyModuleBehaviour(float destroyAfterSeconds)
        {
            _destroyAfterSeconds = destroyAfterSeconds;
        }

        public override void OnAwake() { }
        public override void OnStart() { }

        public override void OnFixedUpdate()
        {
            _destroyAfterSeconds -= Time.fixedDeltaTime;
            if (_destroyAfterSeconds <= 0)
                Object.Destroy(skillObject);
        }
    }
}




