namespace SuspiciousGames.Saligia.Core.Skills
{
    public class FollowTargetModuleBehaviour : ModuleBehaviour
    {
        public FollowTargetModuleBehaviour(BaseSkill baseSkill)
        {
            this.baseSkill = baseSkill;
        }

        public override void OnAwake()
        {
            //throw new System.NotImplementedException();
        }

        public override void OnFixedUpdate()
        {
            var target = baseSkill.TargetData.GetTargetObject();
            if (target)
            {
                skillObject.transform.position = target.transform.position;
            }
        }

        public override void OnStart()
        {
            //throw new System.NotImplementedException();
        }
    }
}




