using System;
using System.Collections;
using UnityEngine;

namespace Script
{
    public class AchiveManager : Singleton<AchiveManager>
    {
        public GameObject[] lockCharacter;
        public GameObject[] unlockCharacter;
        public GameObject uiNotice;

        enum Achive
        {
            UnlockPotato,
            UnlockBean
        }

        private Achive[] achives;
        private WaitForSecondsRealtime wait;

        private void Awake()
        {
            achives = (Achive[])Enum.GetValues(typeof(Achive));
            wait =  new WaitForSecondsRealtime(5f);

            if (!PlayerPrefs.HasKey("MyData"))
            {
                Init();
            }
        }

        void Init()
        {
            PlayerPrefs.SetInt("MyData", 1);
        
            foreach (var achive in achives)
            {
                PlayerPrefs.SetInt(achive.ToString(), 0);
            }
        }

        private void Start()
        {
            UnlockCharacter();
        }

        void UnlockCharacter()
        {
            for (var i = 0; i < lockCharacter.Length; i++)
            {
                var achiveName = achives[i].ToString().ToString();
                bool isUnlock = PlayerPrefs.GetInt(achiveName) == 1;
                lockCharacter[i].SetActive(!isUnlock);
                unlockCharacter[i].SetActive(isUnlock);
            }
        }

        private void LateUpdate()
        {
            foreach (var achive in achives)
            {
                CheckAchive(achive);
            }
        }

        void CheckAchive(Achive achive)
        {
            bool isAchive = false;

            switch (achive)
            {
                case Achive.UnlockPotato:
                    isAchive = GameManager.Instance.kill >= 10;
                    break;
                case Achive.UnlockBean:
                    isAchive = GameManager.Instance.gameTime == GameManager.Instance.maxGameTime;
                    break;
            }

            if (isAchive && PlayerPrefs.GetInt(achive.ToString()) == 0)
            {
                PlayerPrefs.SetInt(achive.ToString(), 1);

                for (var i = 0; i < uiNotice.transform.childCount; i++)
                {
                    bool isActive = i == (int)achive;
                    uiNotice.transform.GetChild(i).gameObject.SetActive(isActive);
                }
            
                StartCoroutine(NoticeRoutine());
            }
        }

        IEnumerator NoticeRoutine()
        {
            uiNotice.SetActive(true);
        
            yield return wait;
        
            uiNotice.SetActive(false);
        
            AudioManager.instance.PlaySfx(AudioManager.Sfx.LevelUp);
        }
    }
}
