using SuspiciousGames.Saligia.Core.Entities;
using UnityEngine;

namespace SuspiciousGames.Saligia.Core.Skills
{
    [CreateAssetMenu(fileName = "newMugThrow", menuName = "Saligia/Skills/Monster/Boss/Mug Throw")]
    public class MugThrow : BaseSkill
    {
        public override void CleanUp()
        {
            if ((BossEntity)CasterEntity)
                ((BossEntity)CasterEntity).MugObject.SetActive(true);
        }

        protected override void Logic()
        {
            if ((BossEntity)CasterEntity)
                ((BossEntity)CasterEntity).MugObject.SetActive(false);
            var go = new GameObject("MugTarget");
            go.transform.position = TargetData.GetTargetPosition();
            TargetData = new Entities.Components.TargetData(go);

            skillObject = CreateSkillobject();
            skillObject.transform.Rotate(skillObject.transform.right, 70);

            if (skillObject)
                skillObject.transform.position = ((BossEntity)CasterEntity).MugObject.transform.position;
        }
    }
}
