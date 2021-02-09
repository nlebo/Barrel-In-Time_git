using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    
    [System.Serializable]
    public struct BGM
    {
        public AudioClip TITLE_BGM;
        public AudioClip MAIN_BGM;
        public AudioClip SHOP_BGM;
        public AudioClip BOSS_BGM;
    }
    [System.Serializable]
    public struct PC
    {
        public AudioClip PC_MOVE;
        public AudioClip PC_DASH;
        public AudioClip PC_SWING;
        public AudioClip PC_HIT;
        public AudioClip PC_GUNFIRE;
        public AudioClip PC_RELOAD;
        public AudioClip PC_OUCH;
    }
    [System.Serializable]
    public struct SKILL
    {
        public AudioClip ARMOR;
        public AudioClip BOMBBULLET_FIRE;
        public AudioClip BOMBBULLET_BOMB;
        public AudioClip SHOCKWAVE;
        public AudioClip TURRET_SET;
        public AudioClip TURRET_FIRE;
    }
    [System.Serializable]
    public struct SYSTEM
    {
        public AudioClip CLOCK_TICKING;
        public AudioClip DOOR_OPEN;
        public AudioClip DOOR_CLOSE;
        public AudioClip SYSTEM_BUYITEMS;
        public AudioClip SYSTEM_FAIL;
        public AudioClip SYSTEM_CURSE;
        public AudioClip SYSTEM_GETGEARS;
        public AudioClip SYSTEM_GETITEMS;
        public AudioClip UI_CURSORON;
        public AudioClip UI_SELECT;
        public AudioClip ENEMY_APPEAR;
        public AudioClip ENEMY_DEAD;
        public AudioClip ENEMY_READY;
        public AudioClip TIME_HEARTBEAT;
    }
    [System.Serializable]
    public struct Enemy
    {
        public AudioClip BIRD_RUSH;
        public AudioClip BOSS_CUCKOO_BEEP;
        public AudioClip BOSS_CUCKOO_CANNONFIRE;
        public AudioClip BOSS_CUCKOO_CANNONREADY;
        public AudioClip BOSS_CUCKOO_FIRST_BEEP;
        public AudioClip CUCKOO_BOOM;
        public AudioClip CUCKOO_CLOCK;
        public AudioClip CUCKOO_DOOR_CLOSE;
        public AudioClip CUCKOO_DOOR_OPEN;
        public AudioClip CUCKOO_HUNT;
        public AudioClip CUCKOO_SONG;
        public AudioClip DOLLS_GUN;
        public AudioClip DOLLS_SPEAR;
        public AudioClip HAWK_RUSH;
        public AudioClip KNIGHT_ATTACK_01;
        public AudioClip KNIGHT_ATTACK_02;
        public AudioClip KNIGHT_JUMP;
        public AudioClip KNIGHT_MOVE;
        public AudioClip MAGE_ATTACK_01;
        public AudioClip MAGE_ATTACK_02;
    }

    public AudioSource Sound;
    public AudioSource[] EFFECT;
    public AudioSource _Loop;
    public bool Fade;
    public bool Changing,EfChanging;
    int Loop = -1;

    [SerializeField]
    public BGM BGMS;

    [SerializeField]
    public PC PCS;

    [SerializeField]
    public SKILL SKILLS;

    [SerializeField]
    public SYSTEM SYSTEMS;
    public Enemy ENEMYS;

    public void Start()
    {
        EFFECT = new AudioSource[10];
        for(int i=0;i<10;i++)
        {
            EFFECT[i] = gameObject.AddComponent<AudioSource>();
            EFFECT[i].loop = false;
            EFFECT[i].volume = 0.1f;
            
        }
        _Loop = gameObject.AddComponent<AudioSource>();
        _Loop.volume = 1;
    }

    public void AudioPlay(AudioClip AUDIO)
    {
        if (Changing)
            return;

        Changing = true;
        Fade = false;
        StartCoroutine(SoundFadeOut(Sound));
        StartCoroutine(SoundFadeIn(Sound, AUDIO));

    }
    public void EffectPlay(AudioClip AUDIO)
    {
        int i;
        for (i = 0; i < 10; i++)
        {
            if (EFFECT[i].isPlaying)
                continue;

            EFFECT[i].clip = AUDIO;
            EFFECT[i].Play();
            break;
        }
        if (i == 10) Sound.PlayOneShot(AUDIO, EFFECT[0].volume);
    }
    public void FadeOut()
    {
        StartCoroutine(SoundFadeOut(Sound));
    }
    public void LoopPlay(AudioClip AUDIO)
    {
        if (Loop != -1)
            return;
        _Loop.clip = AUDIO;
        _Loop.Play();
        Loop = 0;
        _Loop.loop = true;
    }

    public void StopLoop()
    {
        if (Loop == -1)
            return;
        _Loop.loop = false;
        _Loop.Stop();
        Loop = -1;
    }

    IEnumerator SoundFadeOut(AudioSource Sound)
    {
        float StartVolume = Sound.volume;
        while(Sound.volume > 0)
        {
            Sound.volume -= StartVolume * Time.deltaTime / 0.5f;
            yield return null;
        }

        Sound.Stop();
        Sound.volume = StartVolume;
        Fade = true;
    }
    IEnumerator SoundFadeIn(AudioSource Sound,AudioClip _Audio)
    {
        while (!Fade) { yield return null; }

        float StartVolume = Sound.volume;
        Sound.volume = 0;
        Sound.clip = _Audio;
        Sound.Play();
        while (Sound.volume < StartVolume)
        {
            yield return null;
            Sound.volume += StartVolume * Time.deltaTime / 0.5f;
            
        }
        Sound.volume = StartVolume;
        Fade = false;
        Changing = false;
    }
}
