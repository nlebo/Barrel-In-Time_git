using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurseManager : MonoBehaviour {

    public float Curse;
    public int Spawn_GUGU;
    public float Plus_SpawnTime;
    public bool HaveGUGU = false;
    public float SPAWN_SONG, SPAWN_BOMB;
    public int Song_Level,Bomb_Level;
    SoundManager FX;

    public GameObject[] Blind;
	// Use this for initialization
	void Start () {
        Curse = 0;
        Spawn_GUGU = 8;
        Plus_SpawnTime = 0;
        Song_Level = 0; Bomb_Level = 0;
        SPAWN_SONG = 0; SPAWN_BOMB = 0;
        FX = GameManager.Instance.SoundManager;
	}

    public bool CurseUp(float value)
    {
        if (Curse + value > 12)
            return false;

        FX.EffectPlay(FX.SYSTEMS.SYSTEM_CURSE);
        Curse += value;
        return true;
    }

}
