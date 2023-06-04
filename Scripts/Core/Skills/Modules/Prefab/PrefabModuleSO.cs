using UnityEngine;

namespace SuspiciousGames.Saligia.Core.Skills
{
    [CreateAssetMenu(fileName = "newPrefabModule", menuName = "Saligia/Skills/Modules/PrefabModule")]
    public class PrefabModuleSO : ModuleSO
    {
        public GameObject prefab;

        public override void Init(GameObject gameObject, BaseSkill baseSkill)
        {
            Instantiate(prefab, gameObject.transform);
        }
    }
}




