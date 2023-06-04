using SuspiciousGames.Saligia.Core.Entities;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace SuspiciousGames.Saligia.Core.Skills
{
    //TODO better naming???
    //TODO make clear in inspector that this doesn't apply for Missiles. Missiles do their own checking
    public class OnCollisionModuleBehaviour : ModuleBehaviour
    {
        [System.Serializable]
        public class UpdateIntervalContainer
        {
            [SerializeField] private float _interval;
            [SerializeField] private List<ModuleExecutionData> _executionDatas;

            private float _lastTimeTicked = float.MinValue;
            private bool _tickedThisFrame = false;

            public void Init()
            {
                _lastTimeTicked = float.MinValue;
            }

            public bool TryExecute(GameObject target, BaseSkill baseSkill)
            {
                _tickedThisFrame = false;
                if (Time.time - _lastTimeTicked >= _interval)
                {
                    foreach (var moduleExecutionData in _executionDatas)
                        moduleExecutionData.Execute(target, baseSkill);
                    _tickedThisFrame = true;
                    return true;
                }
                return false;
            }

            public void FinishedExecution()
            {
                if (_tickedThisFrame)
                    _lastTimeTicked = Time.time;
            }

            public LayerMask GetAffectedEntities()
            {
                LayerMask temp = new LayerMask();

                foreach (var moduleExecutionData in _executionDatas)
                    temp |= moduleExecutionData.AffectedEntitites;
                return temp;
            }

        }

        private CollisionArea _collisionArea;
        private List<ModuleExecutionData> _onEnterModuleExecutionData;
        private List<UpdateIntervalContainer> _onStayModuleExecutionData;
        private List<ModuleExecutionData> _onExitModuleExecutionData;

        private HashSet<GameObject> _objectsHitInLastFrame;

        private List<GameObject> _objectsHitInCurrentFrame;

        public OnCollisionModuleBehaviour(List<ModuleExecutionData> onEnterModuleExecutionData,
            List<UpdateIntervalContainer> onStayModuleExecutionData,
            List<ModuleExecutionData> onExitModuleExecutionData,
            CollisionArea collisionArea, BaseSkill baseSkill)
        {
            this.baseSkill = baseSkill;

            _objectsHitInCurrentFrame = new List<GameObject>();
            _objectsHitInLastFrame = new HashSet<GameObject>();

            _onEnterModuleExecutionData = onEnterModuleExecutionData;
            _onStayModuleExecutionData = onStayModuleExecutionData;
            _onExitModuleExecutionData = onExitModuleExecutionData;

            _collisionArea = collisionArea;

            LayerMask temp = new LayerMask();

            foreach (var moduleExecutionData in _onEnterModuleExecutionData)
                temp |= moduleExecutionData.AffectedEntitites;
            foreach (var updateIntervalContainer in _onStayModuleExecutionData)
            {
                updateIntervalContainer.Init();
                temp |= updateIntervalContainer.GetAffectedEntities();
            }
            foreach (var moduleExecutionData in _onExitModuleExecutionData)
                temp |= moduleExecutionData.AffectedEntitites;

            _collisionArea.SetAffectedEntities(temp);
        }

        public override void OnAwake()
        {
        }

        public override void OnFixedUpdate()
        {
            _objectsHitInCurrentFrame = _collisionArea.CheckForObjectsInArea(skillObject.transform.position);

            var onupdate = new List<GameObject>(_objectsHitInLastFrame.Except(_objectsHitInCurrentFrame));

            foreach (var gameObject in new List<GameObject>(_objectsHitInLastFrame.Except(_objectsHitInCurrentFrame)))
            {
                //OnExit
                _onExitModuleExecutionData.ForEach(moduleExecutionData => moduleExecutionData.Execute(gameObject, baseSkill));
                _objectsHitInLastFrame.Remove(gameObject);
            }

            foreach (var gameObject in _objectsHitInLastFrame)
            {
                //OnStay
                _onStayModuleExecutionData.ForEach(moduleExecutionData => moduleExecutionData.TryExecute(gameObject, baseSkill));
            }
            _onStayModuleExecutionData.ForEach(moduleExecutionData => moduleExecutionData.FinishedExecution());

            foreach (var gameObject in _objectsHitInCurrentFrame.Except(_objectsHitInLastFrame))
            {
                //OnEnter
                _onEnterModuleExecutionData.ForEach(moduleExecutionData => moduleExecutionData.Execute(gameObject, baseSkill));
                _objectsHitInLastFrame.Add(gameObject);
            }
        }

        public override void OnStart()
        {
        }
    }
}




