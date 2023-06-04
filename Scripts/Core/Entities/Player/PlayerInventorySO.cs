using PixelCrushers;
using UnityEngine;

namespace SuspiciousGames.Saligia.Core.Entities.Player
{
    [CreateAssetMenu(menuName = "Saligia/Player Inventory")]
    public class PlayerInventorySO : ScriptableObject
    {
        [field: SerializeField] public PlayerInventoryData PlayerInvetoryData { get; private set; }

        private void OnValidate()
        {
            Debug.Log(SaveSystem.Serialize(PlayerInvetoryData));
        }
    }
}