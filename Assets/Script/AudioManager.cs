using UnityEngine;

namespace Script
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager instance;

        [Header("BGM")]
        public AudioClip bgmClip;
        public float bgmVolume;
        private AudioSource bgmPlayer;
        AudioHighPassFilter bgmHighPassFilter;

        [Header("SFX")]
        public AudioClip[] sfxClips;
        public float sfxVolume;
        public int channels;
        private AudioSource[] sfxPlayers;
        private int channelIndex;

        public enum Sfx
        {
            Dead,
            Hit,
            LevelUp = 3,
            Lose,
            Melee,
            Range = 7,
            Select,
            Win
        };

        private void Awake()
        {
            instance = this;
        
            Init();
        }

        void Init()
        {
            GameObject bgmObj = new GameObject("BgmPlayer");
            bgmObj.transform.parent = transform;
            bgmPlayer = bgmObj.AddComponent<AudioSource>();
            bgmPlayer.playOnAwake = false;
            bgmPlayer.loop = true;
            bgmPlayer.volume = bgmVolume;
            bgmPlayer.clip = bgmClip;

            bgmHighPassFilter = Camera.main.GetComponent<AudioHighPassFilter>();

            GameObject sfxObj = new GameObject("SfxPlayer");
            sfxObj.transform.parent = transform;
            sfxPlayers = new AudioSource[channels];

            for (var i = 0; i < sfxPlayers.Length; i++)
            {
                sfxPlayers[i] = sfxObj.AddComponent<AudioSource>();
                sfxPlayers[i].playOnAwake = false;
                sfxPlayers[i].bypassListenerEffects = true;
                //sfxPlayers[i].loop = false;
                sfxPlayers[i].volume = sfxVolume;
            }
        }

        public void PlayBGM(bool isPlay)
        {
            if(isPlay)
                bgmPlayer.Play();
            else
                bgmPlayer.Stop();
        }
        
        public void EffectBgm(bool isPlay)
        {
            bgmHighPassFilter.enabled = isPlay;
        }

        public void PlaySfx(Sfx sfx)
        {
            for (var i = 0; i < sfxPlayers.Length; i++)
            {
                int loopIndex = (i + channelIndex) % sfxPlayers.Length;
            
                if(sfxPlayers[loopIndex].isPlaying)
                    continue;

                int ranIndex = 0;
                if(sfx == Sfx.Hit || sfx == Sfx.Melee)
                {
                    ranIndex = UnityEngine.Random.Range(0, 2);
                }
            
                channelIndex = loopIndex;
                sfxPlayers[i].clip = sfxClips[(int)sfx];
                sfxPlayers[i].Play();
                break;
            }
        }
    }
}
