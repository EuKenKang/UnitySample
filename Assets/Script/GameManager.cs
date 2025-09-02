using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Script
{
    public class GameManager : Singleton<GameManager>
    {
        [Header("# Game Control")]
        public bool isLive;
        public float gameTime;
        public float maxGameTime = 2 * 10f;

        [Header("# Player Info")]
        public int playerID;
        public float health;
        public int maxHealth = 100;
        public int level;
        public int kill;
        public int exp;
        public int[] nextExp = { 3, 5, 10, 30, 60, 100, 150, 210, 280, 360, 450, 600 };

        [Header("# Game Object")]
        public PoolManager pool;
        public Player player;
        public LevelUp uiLevelup;
        public Result uiResult;
        public GameObject enemyCleaner;

        public void GameStart(int id)
        {
            playerID = id;
            health = maxHealth;
            
            player.gameObject.SetActive(true);
            uiLevelup.Select(playerID % 2);
            Resume();
            
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
            AudioManager.instance.PlayBGM(true);
        }

        public void GameOver()
        {
            StartCoroutine(GameOverRoutine());
        }

        IEnumerator GameOverRoutine()
        {
            isLive = false;
            
            yield return new WaitForSeconds(0.5f);
            
            uiResult.gameObject.SetActive(true);
            uiResult.Lose();
            Stop();
            
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Lose);
            AudioManager.instance.PlayBGM(false);
        }

        public void GameVictory()
        {
            StartCoroutine(GameVictoryRoutine());
        }

        IEnumerator GameVictoryRoutine()
        {
            isLive = false;
            enemyCleaner.SetActive(true);
            
            yield return new WaitForSeconds(0.5f);
            
            uiResult.gameObject.SetActive(true);
            uiResult.Win();
            Stop();
            
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Win);
            AudioManager.instance.PlayBGM(false);
        }
        public void GameRetry()
        {
            SceneManager.LoadScene(0);
        }

        private void Update()
        {
            if (!isLive)
                return;
            
            gameTime += Time.deltaTime;

            if(gameTime > maxGameTime)
            {
                gameTime = maxGameTime;
                GameVictory();
            }
        }

        public void GetExp()
        {
            if (!isLive)
                return;
            
            exp++;
            if (exp == nextExp[Mathf.Min(level, nextExp.Length - 1)])
            {
                level++;
                exp = 0;
                uiLevelup.Show();
            }
        }

        public void Stop()
        {
            isLive = false;
            Time.timeScale = 0;
        }

        public void Resume()
        {
            isLive = true;
            Time.timeScale = 1;
        }
    }
}
