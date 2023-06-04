using System.Collections.Generic;
using UnityEngine;

namespace SuspiciousGames.Saligia.Core.Skills
{
    public class SkillBehaviour : MonoBehaviour
    {
        private List<ModuleBehaviour> _moduleBehaviours = new List<ModuleBehaviour>();
        public List<ModuleBehaviour> ModuleBehaviours => _moduleBehaviours;
        public static SkillBehaviour Create(GameObject objectToCreateOn)
        {
            if (objectToCreateOn.GetComponent<SkillBehaviour>() == null)
            {
                objectToCreateOn.AddComponent<SkillBehaviour>();
            }
            return objectToCreateOn.GetComponent<SkillBehaviour>();
        }

        public void AddModuleBehaviour(ModuleBehaviour moduleBehaviour)
        {
            moduleBehaviour.SetGameobject(gameObject);
            _moduleBehaviours.Add(moduleBehaviour);
        }

        private void Awake()
        {
            foreach (var behaviour in _moduleBehaviours)
            {
                behaviour.OnAwake();
            }
        }

        private void Start()
        {
            foreach (var behaviour in _moduleBehaviours)
            {
                behaviour.OnStart();
            }
        }

        private void FixedUpdate()
        {
            foreach (var behaviour in _moduleBehaviours)
            {
                behaviour.OnFixedUpdate();
            }
        }
        private void OnDrawGizmos()
        {
            foreach (var behaviour in _moduleBehaviours)
            {
                behaviour.OnDrawGizmos();
            }
        }
        private void OnDrawGizmosSelected()
        {
            foreach (var behaviour in _moduleBehaviours)
            {
                behaviour.OnDrawGizmosSelected();
            }
        }
    }
}




