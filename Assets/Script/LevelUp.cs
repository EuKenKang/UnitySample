using UnityEngine;

namespace Script
{
    public class LevelUp : MonoBehaviour
    {
        private RectTransform _rect;
        private Item[] _items;

        private void Awake()
        {
            _rect = GetComponent<RectTransform>();
            _items = GetComponentsInChildren<Item>(true);
        }

        public void Show()
        {
            Next();
            _rect.localScale = Vector3.one;
            GameManager.Instance.Stop();
            
            AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);
            AudioManager.instance.EffectBgm(true);
        }

        public void Hide()
        {
            _rect.localScale = Vector3.zero;
            GameManager.Instance.Resume();
            
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
            AudioManager.instance.EffectBgm(false);
        }

        public void Select(int index)
        {
            _items[index].OnClick();
        }

        void Next()
        {
            foreach (var item in _items)
            {
                item.gameObject.SetActive(false);
            }

            int[] ran = new int[3];
            while (true)
            {
                ran[0] = UnityEngine.Random.Range(0, _items.Length);
                ran[1] = UnityEngine.Random.Range(0, _items.Length);
                ran[2] = UnityEngine.Random.Range(0, _items.Length);
                
                if(ran[0] != ran[1] && ran[1] != ran[2] && ran[2] != ran[0])
                    break;
            }

            for (var i = 0; i < ran.Length; i++)
            {
                var ranItem = _items[ran[i]];

                if (ranItem.level == ranItem.data.damages.Length)
                {
                    _items[4].gameObject.SetActive(true);
                }
                else
                {
                    ranItem.gameObject.SetActive(true);
                }
            }
        }
        
    }
}
