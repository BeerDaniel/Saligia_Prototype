using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace SuspiciousGames.Saligia.Core.Skills
{
    public class ProjectileModuleBehaviour : ModuleBehaviour
    {
        public ProjectileData _projectileData;
        private NavMeshAgent _navMeshAgent;

        private float _distanceTraveled = 0;
        private int _shootDir = 1;
        protected bool _reachedEndpoint = false;
        private Vector3 _translationAxis;
        private bool destroy = false;

        public ProjectileModuleBehaviour(ProjectileData projectileData)
        {
            _projectileData = projectileData;
        }

        public override void OnAwake()
        {
            throw new System.NotImplementedException();
        }

        public override void OnFixedUpdate()
        {
            if (destroy)
            {
                GameObject.Destroy(skillObject);
            }

            var percentageTraveled = _distanceTraveled / _projectileData.distance;

            //CalculateForwardMovement
            var movement = skillObject.transform.forward * _projectileData.speedCurve.Evaluate(percentageTraveled) * Time.deltaTime * _shootDir;
            _distanceTraveled += movement.magnitude * _shootDir;
            //Add Sideways Translation
            movement += _translationAxis * (_projectileData.translationCurve.Evaluate(percentageTraveled) - _projectileData.translationCurve.Evaluate(_distanceTraveled / _projectileData.distance));


            if (NavMesh.Raycast(skillObject.transform.position, skillObject.transform.position + movement, out var _, _navMeshAgent.areaMask))
            {
                destroy = true;
            }
            else
            {

                if (percentageTraveled >= 1 && !_reachedEndpoint)
                {
                    if (_projectileData.isReturning)
                    {
                        _reachedEndpoint = true;
                        _shootDir *= -1;
                    }
                    else
                    {
                        destroy = true;
                    }
                }
                if (_reachedEndpoint)
                {
                    if (percentageTraveled <= 0)
                        destroy = true;
                }
                if (!destroy)
                    _navMeshAgent.Move(movement);
            }
        }

        public override void OnStart()
        {
            bool settingsFound = false;
            _navMeshAgent = skillObject.AddComponent<NavMeshAgent>();
            for (int i = 0; i < NavMesh.GetSettingsCount(); i++)
            {
                var current = NavMesh.GetSettingsByIndex(i);
                if (NavMesh.GetSettingsNameFromID(current.agentTypeID) == _projectileData.AgentTypeName)
                {
                    _navMeshAgent.agentTypeID = current.agentTypeID;
                    _navMeshAgent.baseOffset = _projectileData.agentBaseOffset;
                    _navMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.NoObstacleAvoidance;
                    _navMeshAgent.avoidancePriority = int.MaxValue;
                    settingsFound = true;
                    break;
                }
            }
            if (!settingsFound)
                throw new Exception("No corresponding NavMesh Agent found for Agent Type Name: " + _projectileData.AgentTypeName);

            _translationAxis = skillObject.transform.right;
            if (_projectileData.isMirrored)
                _translationAxis *= -1;
            Debug.Log(_projectileData.isMirrored);
            Debug.Log(_translationAxis);
        }
    }
}
