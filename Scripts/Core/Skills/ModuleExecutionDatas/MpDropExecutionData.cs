using Assets.Scripts.Core.SpawnableEffects;
using UnityEngine;
using UnityEngine.AI;

namespace SuspiciousGames.Saligia.Core.Skills
{
    [CreateAssetMenu(fileName = "newMpDropExecutionData", menuName = "Saligia/Skills/ModuleEexecutionData/MpDropExecutionData")]
    public class MpDropExecutionData : ModuleExecutionData
    {
        [SerializeField] private float _mpAmount;
        //[SerializeField] private int _maxDrops = 5;
        [SerializeField, Tooltip("Time in Seconds the Drop is alive, 0 is unlimited lifetime")]
        private float _lifetime = 1;
        [SerializeField] private float _yOffset;
        [SerializeField] private float spawnRadius;
        [SerializeField] private GameObject _prefab;
        [SerializeField] private CollisionArea _collisionArea;
        [SerializeField] private DropAudio _audio;

        //private int _dropped = 0;
        protected override void Logic(GameObject gameObject)
        {
            //if (_dropped > _maxDrops)
            //    return;

            bool hasHit = false;
            NavMeshHit hit;

            do
            {
                Vector2 random = Random.insideUnitCircle;
                random.Normalize();
                random *= spawnRadius;
                Vector3 offset = new Vector3(random.x, 0, random.y);
                if (NavMesh.SamplePosition(gameObject.transform.position + offset, out hit, 0.1f, NavMesh.AllAreas))
                    hasHit = true;

            } while (!hasHit);


            if (_audio.clip != null)
                MpDrop.Create(_mpAmount, _collisionArea, hit.position + new Vector3(0, _yOffset, 0), _prefab, _lifetime, _audio);
            else
                MpDrop.Create(_mpAmount, _collisionArea, hit.position + new Vector3(0, _yOffset, 0), _prefab, _lifetime);
            //_dropped++;
        }

    }
}
