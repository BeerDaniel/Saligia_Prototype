using SuspiciousGames.Saligia.Core.Entities;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace SuspiciousGames.Saligia.Core.Skills
{
    [System.Serializable]
    public class CollisionArea
    {
        [SerializeField] private LayerMask _affectedEntities;
        [SerializeField] private Vector3 _halfExtents;
        [SerializeField] private AreaType _areaType;
        [SerializeField] private float _radius;
        [SerializeField] private float _capsuleHeight;
        [SerializeField] private float _angle;

        public AreaType SelectedAreaType => _areaType;
        public Vector3 HalfExtents => _halfExtents;
        public float Radius => _radius;
        public float CapsuleHeight => _capsuleHeight;
        public float Angle => _angle;


        private Vector3 _center;
        private Vector3 _forward;

        private List<GameObject> _gameObjects;
        private Vector3 _toCol;

        public enum AreaType
        {
            Box,
            Cone,
            Cylinder,
            Sphere
        }

        public List<GameObject> CheckForObjectsInArea(Vector3 center, Vector3 forward = new Vector3())
        {
            _center = center;
            _forward = forward;
            return CheckForObjectsInArea();
        }

        private List<GameObject> CheckForObjectsInArea()
        {
            _gameObjects = new List<GameObject>();
            switch (_areaType)
            {
                case AreaType.Box:
                    ConvertToGameObjects(ref _gameObjects, Physics.OverlapBox(_center, _halfExtents, Quaternion.identity, _affectedEntities, QueryTriggerInteraction.Ignore));
                    break;
                case AreaType.Cone:
                    //TODO this might be a problem when the origin is not inside the angle but any part of the collider is
                    //TODO maybe use collision.contacts/GetContact
                    var cols = Physics.OverlapSphere(_center, _radius, _affectedEntities, QueryTriggerInteraction.Ignore);
                    List<Collider> colliders = new List<Collider>();
                    foreach (var col in cols)
                    {
                        _toCol = col.transform.position - _center;

                        if (Vector3.Dot(_toCol.normalized, _forward.normalized) >
                                Mathf.Cos(_angle * 0.5f * Mathf.Deg2Rad))
                            colliders.Add(col);
                        //if (Vector3.Angle(_forward, toCol) <= _angle / 2)
                    }
                    ConvertToGameObjects(ref _gameObjects, colliders.ToArray());
                    break;
                case AreaType.Cylinder:
                    ConvertToGameObjects(ref _gameObjects, Physics.OverlapCapsule(_center, _center + Vector3.up * _capsuleHeight, _radius, _affectedEntities, QueryTriggerInteraction.Ignore));
                    break;
                case AreaType.Sphere:
                    //TryConvertToEntities(ref _gameObjects, );
                    ConvertToGameObjects(ref _gameObjects, Physics.OverlapSphere(_center, _radius, _affectedEntities, QueryTriggerInteraction.Ignore));
                    break;
                default:
                    break;
            }
            return _gameObjects;
        }

        public void SetAffectedEntities(LayerMask affectedEntities)
        {
            _affectedEntities = affectedEntities;
        }

        private void ConvertToGameObjects(ref List<GameObject> gameObjects, Collider[] cols)
        {
            foreach (var col in cols)
                gameObjects.Add(col.gameObject);
        }
    }
}




