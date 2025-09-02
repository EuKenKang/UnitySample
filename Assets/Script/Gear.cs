using UnityEngine;

namespace Script
{
    public class Gear : MonoBehaviour
    {
        public ItemData.ItemType type;
        public float rate;

        public void Init(ItemData data)
        {
            name = "Gear " + data.itemID;
            transform.parent = GameManager.Instance.player.transform;
            transform.localPosition = Vector3.zero;

            type = data.itemType;
            rate = data.damages[0];
            
            ApplyGear();
        }

        void ApplyGear()
        {
            switch (type)
            {
                case ItemData.ItemType.Glove:
                    RateUp();
                    break;
                case ItemData.ItemType.Shoe:
                    SpeedUp();
                    break;
            }
        }

        public void LevelUp(float rate)
        {
            this.rate = rate;
            ApplyGear();
        }

        void RateUp()
        {
            Weapon[] weapons = transform.parent.GetComponents<Weapon>();
            foreach (var weapon in weapons)
            {
                switch (weapon.id)
                {
                    case 0:
                        float speed = 150 * Character.WeaponSpeed;
                        weapon.speed = speed + (speed * rate);
                        break;
                    default:
                        speed = 0.5f * Character.WeaponRate;
                        weapon.speed = speed * (1f - rate);
                        break;
                }
            }
        }

        void SpeedUp()
        {
            float speed = 3 * Character.Speed;
            GameManager.Instance.player.speed = speed + speed * rate;
        }
    }
}
