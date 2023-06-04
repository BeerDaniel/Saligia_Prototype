using System;
using System.Collections.Generic;
using UnityEngine;

namespace SuspiciousGames.Saligia.Core.Skills
{
    public class MissileModuleBehaviour : ModuleBehaviour
    {


        private Transform _target;
        private List<ModuleExecutionData> _onTargetReachedModuleExecutionData = new List<ModuleExecutionData>();
        private Seekmode _seekmode = Seekmode.linear;
        private float _speed = 1f;
        private float _curvature = 1f;
        private float _afterglow;

        private Vector3 _startPos;
        private Vector3 _cubicBezierPoint;
        private float _currentDistance;
        private float _traveledDistance;
        private bool _stopped = false;
#if UNITY_EDITOR
        private Vector3 _debugLeftCubicBezier;
        private Vector3 _debugRightCubicBezier;
#endif

        public MissileModuleBehaviour(BaseSkill baseSkill,
            List<ModuleExecutionData> onTargetReachedModuleExecutionData,
            Seekmode seekmode,
            float speed,
            float curvature,
            float afterglow)
        {
            this.baseSkill = baseSkill;
            _target = baseSkill.TargetData.GetTargetObject().transform;
            _onTargetReachedModuleExecutionData = onTargetReachedModuleExecutionData;
            _seekmode = seekmode;
            _speed = speed;
            _curvature = curvature;
            _afterglow = afterglow;
        }

        public void AddExecutionData(ModuleExecutionData data)
        {
            _onTargetReachedModuleExecutionData.Add(data);
        }

        public override void OnAwake()
        {

        }

        public override void OnFixedUpdate()
        {
            if (_stopped)
            {
                _afterglow -= Time.deltaTime;
                if (skillObject && _afterglow <= 0f)
                {
                    GameObject.Destroy(skillObject);
                }
                return;
            }

            if (_target == null)
            {

                Debug.Log("Target Destroyed; Destroying...");
                _stopped = true;
                return;

            }


            _currentDistance = getApproxDistance();
            Vector3 movePos;
            switch (_seekmode)
            {
                case Seekmode.linear:
                    movePos = TrackLinear();
                    break;
                case Seekmode.cubic:
                    movePos = TrackCubic();
                    break;
                default:
                    movePos = Vector3.zero;
                    break;
            }
            _traveledDistance += _speed * Time.deltaTime;
            skillObject.transform.position = movePos;

            //DEBUG
            if (_traveledDistance >= _currentDistance)
            {
                _stopped = true;
                ExecuteModuleDatas();
                return;
            }
        }

        public override void OnStart()
        {
            _startPos = skillObject.transform.position;
            //_startDistance = Vector3.Distance(_startPos, _target.position);
            Debug.Log("Target locked.");
            if (_seekmode == Seekmode.cubic)
            {
                CalculateCubicBezierPoint(skillObject);
                Debug.Log("Created Cubic Helper Point.");
            }

        }

        private void CalculateCubicBezierPoint(GameObject skillObject)
        {
            _cubicBezierPoint = _startPos + skillObject.transform.forward * _curvature;
        }

        private float getApproxDistance()
        {
            switch (_seekmode)
            {
                case Seekmode.linear:
                    return Vector3.Distance(_startPos, _target.position);
                case Seekmode.cubic:
                    return Vector3.Distance(_startPos, _cubicBezierPoint) + Vector3.Distance(_cubicBezierPoint, _target.position);
                default:
                    return 0f;
            }
        }

        private Vector3 TrackLinear()
        {
            return Vector3.Lerp(_startPos, _target.position, getT());
        }

        private Vector3 TrackCubic()
        {
            var a = Vector3.Lerp(_startPos, _cubicBezierPoint, getT()); ;
            var b = Vector3.Lerp(_cubicBezierPoint, _target.position, getT());
#if UNITY_EDITOR
            _debugLeftCubicBezier = a;
            _debugRightCubicBezier = b;
#endif
            return Vector3.Lerp(a, b, getT());
        }

        private float getT()
        {
            return Mathf.Clamp01(_traveledDistance / _currentDistance);
        }

        private void ExecuteModuleDatas()
        {
            foreach (var data in _onTargetReachedModuleExecutionData)
            {
                data.Execute(_target.gameObject, baseSkill);
            }
        }

        public override void OnDrawGizmos()
        {
            base.OnDrawGizmos();
#if UNITY_EDITOR
            Gizmos.color = Color.red;
            if (_startPos != null)
                Gizmos.DrawWireSphere(_startPos, .1f);
            if (_target?.position != null)
                Gizmos.DrawWireSphere(_target.position, .1f);
            if (_cubicBezierPoint != null)
                Gizmos.DrawWireSphere(_cubicBezierPoint, .1f);
            if (_startPos != null && _target?.position != null)
                Gizmos.DrawLine(_startPos, _target.position);
            if (_startPos != null && _cubicBezierPoint != null)
                Gizmos.DrawLine(_startPos, _cubicBezierPoint);
            if (_cubicBezierPoint != null && _target?.position != null)
                Gizmos.DrawLine(_cubicBezierPoint, _target.position);
            Gizmos.color = Color.yellow;
            if (_debugLeftCubicBezier != null)
                Gizmos.DrawWireSphere(_debugLeftCubicBezier, .1f);
            if (_debugRightCubicBezier != null)
                Gizmos.DrawWireSphere(_debugRightCubicBezier, .1f);
            if (_debugLeftCubicBezier != null && _debugRightCubicBezier != null)
                Gizmos.DrawLine(_debugLeftCubicBezier, _debugRightCubicBezier);
            Gizmos.color = Color.green;
            Gizmos.DrawSphere(skillObject.transform.position, 0.075f);
#endif
        }
    }
}
