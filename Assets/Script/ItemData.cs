using UnityEngine;

namespace Script
{
    [CreateAssetMenu(fileName = "Item", menuName = "ScriptableObjects/ItemData")]

    public class ItemData : ScriptableObject
    {
        public enum ItemType
        {
            Melee,
            Range,
            Glove,
            Shoe,
            Heal
        }

        [Header("# Main Info")]
        public ItemType itemType;
        public int itemID;
        public string itemName;
        [TextArea]
        public string itemDesc;
        public Sprite itemIcon;

        [Header("# Level Info")]
        public float baseDamage;
        public int baseCount;
        public float[] damages;
        public int[] counts;

        [Header("# Weapon")]
        public GameObject projectile;

        public Sprite hand;
    }
}
