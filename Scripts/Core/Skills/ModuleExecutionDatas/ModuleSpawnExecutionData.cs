using System.Collections.Generic;
using UnityEngine;

namespace SuspiciousGames.Saligia.Core.Skills
{
    [CreateAssetMenu(fileName = "newModuleSpawnExecutionData", menuName = "Saligia/Skills/ModuleEexecutionData/ModuleSpawnExecutionData")]
    public class ModuleSpawnExecutionData : ModuleExecutionData
    {
        [SerializeField] private List<ModuleSO> _modules;

        protected override void Logic(GameObject gameObject)
        {
            var go = new GameObject();
            go.transform.position = gameObject.transform.position;
            foreach (var module in _modules)
                if (baseSkill.Runes.Contains(module.Rune))
                    module.Init(go, baseSkill);
        }

    }
}




