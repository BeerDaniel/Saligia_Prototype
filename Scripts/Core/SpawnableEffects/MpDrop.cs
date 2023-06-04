using SuspiciousGames.Saligia.Core.Entities;
using SuspiciousGames.Saligia.Core.Entities.Components;
using SuspiciousGames.Saligia.Core.Skills;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Core.SpawnableEffects
{
    public class MpDrop : MonoBehaviour
    {
        public static void Create(float mpAmount, CollisionArea collisionArea, Vector3 center, GameObject prefab, float lifetime = 0f, DropAudio audio = null)
        {
            GameObject newGKDrop = Instantiate(prefab, center, Quaternion.identity);
            var gkDrop = newGKDrop.AddComponent<MpDrop>();
            gkDrop.Setup(mpAmount, collisionArea, lifetime, audio);
        }

        //public static void Create(float mpAmount, CollisionArea collisionArea, Vector3 center, GameObject prefab, string affectedTag)
        //{
        //    var listDummy = new List<string>();
        //    listDummy.Add(affectedTag);
        //    Create(mpAmount, collisionArea, center, prefab, listDummy);
        //}

        private float _mpAmount;

        private CollisionArea _collisionArea;
        private float _lifetime;
        private DropAudio _audio;

        private void Setup(float mpAmount, CollisionArea collisionArea, float lifetime = 0f, DropAudio audio = null)
        {
            _mpAmount = mpAmount;
            _collisionArea = collisionArea;
            _lifetime = lifetime;
            _audio = audio;
        }

        private void Start()
        {
            if (_lifetime == 0)
                return;
            Destroy(gameObject, _lifetime);
        }

        private void FixedUpdate()
        {
            var collisions = _collisionArea.CheckForObjectsInArea(transform.position);
            if (collisions.Count > 0)
            {
                foreach (var collision in collisions)
                {
                    CastCostComponent castCostComponent = collision.GetComponent<CastCostComponent>();
                    if (castCostComponent)
                    {
                        castCostComponent.AddMindPower(_mpAmount);
                        if (_audio != null)
                            AudioSource.PlayClipAtPoint(_audio.clip, gameObject.transform.position, _audio.volume);

                        Destroy(gameObject);
                    }
                }
            }
        }

        //private void OnTriggerEnter(Collider other)
        //{
        //    if (!_affectedTags.Contains(other.tag))
        //        return;
        //    CastCostComponent component = other.GetComponent<CastCostComponent>();
        //    if (!component)
        //        return;

        //    component.AddMindPower(_gkAmount);
        //    Destroy(gameObject);
        //}
    }
    [System.Serializable]
    public class DropAudio
    {
        public float volume = 0f;
        public AudioClip clip;
    }
}