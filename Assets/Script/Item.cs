using System;
using UnityEngine;
using UnityEngine.UI;

namespace Script
{
    public class Item : MonoBehaviour
    {
        public ItemData data;
        public int level;
        public Weapon weapon;
        public Gear gear;

        private Image _icon;
        private Text _textLevel;
        private Text _textName;
        private Text _textDesc;

        private void Awake()
        {
            _icon = GetComponentsInChildren<Image>()[1];
            _icon.sprite = data.itemIcon;

            Text[] texts = GetComponentsInChildren<Text>();
            _textLevel = texts[0];
            _textName = texts[1];
            _textDesc = texts[2];

            _textName.text = data.itemName;
            
        }

        private void OnEnable()
        {
            _textLevel.text = "Lv." + (level + 1);

            switch (data.itemType)
            {
                case ItemData.ItemType.Melee:
                case ItemData.ItemType.Range:
                    _textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100, data.counts[level]);
                    break;

                case ItemData.ItemType.Glove:
                case ItemData.ItemType.Shoe:
                    _textDesc.text = string.Format(data.itemDesc, data.damages[level] * 100);
                    break;
                
                default:
                    _textDesc.text = string.Format(data.itemDesc);
                    break;
                
            } 
        }

        private void OnDestroy()
        {
            _icon.sprite = null;
        }

        void LateUpdate()
        {
            _textLevel.text = "Lv." + (level + 1);
        }

        public void OnClick()
        {
            switch (data.itemType)
            {
                case ItemData.ItemType.Melee:
                case ItemData.ItemType.Range:
                {
                    if (level == 0)
                    {
                        var newWeapon = new GameObject();
                        weapon = newWeapon.AddComponent<Weapon>();
                        weapon.Init(data);
                    }
                    else
                    {
                        float nextDamage = data.baseDamage;
                        int nextCount = 0;

                        nextDamage += data.baseDamage * data.damages[level];
                        nextCount += data.counts[level];
                        
                        weapon.LevelUp(nextDamage, nextCount);
                    }
                }
                    break;
                case ItemData.ItemType.Glove:
                case ItemData.ItemType.Shoe:
                {
                    if (level == 0)
                    {
                        GameObject newGear = new GameObject();
                        gear = newGear.AddComponent<Gear>();
                        gear.Init(data);
                    }
                    else
                    {
                        var nextRate = data.damages[level];
                        gear.LevelUp(nextRate);
                    }
                }
                    break;
                case ItemData.ItemType.Heal:
                    break;
            }

            level++;

            if (level != data.damages.Length) return;
            var button = GetComponent<Button>();
            if(button != null)
            {
                button.interactable = false;
            }
        }
        
    }
}
