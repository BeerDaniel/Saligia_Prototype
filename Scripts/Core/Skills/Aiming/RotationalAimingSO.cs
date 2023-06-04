using SuspiciousGames.Saligia.Core.Entities.Components;
using SuspiciousGames.Saligia.Core.Entities.Player;
using UnityEngine;

namespace SuspiciousGames.Saligia.Core.Skills
{
    [CreateAssetMenu(fileName = "newRotationalAimingSO", menuName = "Saligia/Skills/Aiming/RotationalAiming")]
    public class RotationalAimingSO : AimingSO
    {
        [SerializeField] private GameObject _decalPrefab;
        private GameObject _decal;

        protected override bool AimingStart(PlayerEntity playerAimer)
        {
            if (_decalPrefab)
                _decal = Instantiate(_decalPrefab, playerAimer.transform.position, playerAimer.transform.rotation);
            return false;
        }

        protected override void AimingUpdate(PlayerEntity playerAimer)
        {
            if (_decal)
                _decal.transform.rotation = playerAimer.transform.rotation;
        }

        protected override TargetData AimingEnd(PlayerEntity playerAimer)
        {
            Destroy(_decal);
            return new TargetData(playerAimer.transform.position + playerAimer.transform.forward.normalized);
        }
    }
}




