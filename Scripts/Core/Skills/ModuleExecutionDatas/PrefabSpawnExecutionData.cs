using UnityEngine;

namespace SuspiciousGames.Saligia.Core.Skills
{
    [CreateAssetMenu(fileName = "newPrefabSpawnExecutionData", menuName = "Saligia/Skills/ModuleEexecutionData/PrefabSpawnExecutionData")]
    public class PrefabSpawnExecutionData : ModuleExecutionData
    {
        [SerializeField] private GameObject _prefab;
        [SerializeField] private bool _spawnUnparented;

        protected override void Logic(GameObject gameObject)
        {
            if (_spawnUnparented)
                Instantiate(_prefab, gameObject.transform.position, Quaternion.identity);
            else
                Instantiate(_prefab, gameObject.transform);
        }

    }
}




