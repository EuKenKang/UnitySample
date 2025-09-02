using UnityEngine;

namespace Script
{
    public class Character : MonoBehaviour
    {
        public static float Speed
        {
            get { return GameManager.Instance.playerID == 0 ? 1.1f : 1.0f; }
        }

        public static float WeaponSpeed
        {
            get { return GameManager.Instance.playerID == 1 ? 1.1f : 1.0f; }
        }

        public static float WeaponRate
        {
            get { return GameManager.Instance.playerID == 1 ? 1.2f : 1.0f; }
        }

        public static float Damage
        {
            get { return GameManager.Instance.playerID == 2 ? 1.2f : 1.0f; }
        }

        public static int Count
        {
            get { return GameManager.Instance.playerID == 3 ? 3 : 1; }
        }
    }
}
