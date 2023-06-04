using UnityEngine;

namespace SuspiciousGames.Saligia.Core.Skills
{
    public abstract class ModuleSO : ScriptableObject
    {
        [SerializeField] protected Rune rune;
        public Rune Rune => rune;
        public abstract void Init(GameObject skillGameObject, BaseSkill baseSkill);
    }
}




