using UnityEngine;

namespace Script
{
    public class Weapon : MonoBehaviour
    {
        public int id;
        public int prefabId;
        public float damage;
        public int count;
        public float speed;

        float _timer;
        Player _player;

        private void Awake()
        {
            if (GameManager.Instance == null)
                return;

            _player = GameManager.Instance.player;
        }

        private void Update()
        {
            if (!GameManager.Instance.isLive)
                return;

            switch (id)
            {
                case 0:
                    transform.Rotate(Vector3.back * (speed * Time.deltaTime));
                    break;

                default:
                    _timer += Time.deltaTime;

                    if(_timer > speed)
                    {
                        _timer = 0;
                        Fire();
                    }
                    break;
            }
        }

        public void Init(ItemData data)
        {
            // base setting
            name = "Weapon " + data.itemID;
            transform.parent = _player.transform;
            transform.localPosition = Vector3.zero;
        
            // property setting
            id = data.itemID;
            damage = data.baseDamage * Character.Damage;
            count = data.baseCount + Character.Count;

            for (var i = 0; i < GameManager.Instance.pool.prefabs.Length; i++)
            {
                if (data.projectile == GameManager.Instance.pool.prefabs[i])
                {
                    prefabId = i;
                    break;
                }
            }

            switch (id)
            {
                case 0:
                    speed = 150 * Character.WeaponSpeed;
                    Batch();
                    break;

                default:
                    speed = 0.5f * Character.WeaponRate;
                    break;
            }

            PlayerHand hand = _player.hands[(int)data.itemType];
            hand.spriter.sprite = data.hand;
            hand.gameObject.SetActive(true);
            
            _player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
        }

        void Batch()
        {
            for (int i = 0; i < count; i++)
            {
                Transform bullet;
                
                if(i < transform.childCount)
                {
                    bullet = transform.GetChild(i);
                }
                else
                {
                    bullet = GameManager.Instance.pool.Get(prefabId).transform;
                    bullet.parent = transform;
                }
                
                bullet.localPosition = Vector3.zero;
                bullet.localRotation = Quaternion.identity;

                Vector3 rotVec = Vector3.forward * 360 * i / count;
                bullet.Rotate(rotVec);
                bullet.Translate(bullet.up * 1.5f, Space.World);

                bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero); // -100 is infinity
            }
        }

        void Fire()
        {
            if (!_player.scanner.nearestTarget)
                return;

            Vector3 targetPos = _player.scanner.nearestTarget.position;
            Vector3 dir = targetPos - transform.position;
            dir = dir.normalized;

            Transform bullet = GameManager.Instance.pool.Get(prefabId).transform;
            bullet.position = transform.position;
            bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
            bullet.GetComponent<Bullet>().Init(damage, count, dir);
            
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);
        }

        public void LevelUp(float damage, int count)
        {
            this.damage = damage;
            this.count += count;
            
            if(id == 0)
                Batch();
            
            _player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
        }
    }
}
