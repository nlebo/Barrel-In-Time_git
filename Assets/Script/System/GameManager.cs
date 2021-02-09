using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager :MonoBehaviour {

    public event System.Action<Player> LocalPlayerJoined;

    private GameObject _gameObject;

    private static GameManager m_Instance;
    public static GameManager Instance
    {
        get
        {


            return m_Instance;
        }
    }

    public void Awake()
    {
        _gameObject = this.gameObject;
        if (m_Instance == null)
        {
            m_Instance = this;
            m_Instance._gameObject.AddComponent<InputManager>();
            m_Instance._gameObject.AddComponent<Timer>();
            m_Instance._gameObject.AddComponent<MapInfo>();
            // m_Instance._gameObject.AddComponent<InventoryManager>();
        }
    }
    

    private InputManager m_InputManager;
    public InputManager InputManager
    {
        get {
            if (m_InputManager == null)
                m_InputManager = _gameObject.GetComponent<InputManager>();

            return m_InputManager;

        }
    }

    private Timer m_Timer;
    public Timer Timer
    {
        get {
            if (m_Timer == null)
                m_Timer = _gameObject.GetComponent<Timer>();

            return m_Timer;
        }
    }

    private MapInfo m_Mapinfo;
    public MapInfo Mapinfo
    {
        get
        {
            if (m_Mapinfo == null)
                m_Mapinfo = _gameObject.GetComponent<MapInfo>();

            return m_Mapinfo;
        }
    }

    private Player m_LocalPlayer;
    public Player LocalPlayer
    {
        get {
            return m_LocalPlayer;
        }
        set {
            m_LocalPlayer = value;
            if (LocalPlayerJoined != null)
                LocalPlayerJoined(m_LocalPlayer);
        }
    }

    private MouseCursor m_MouseCursor;
    public MouseCursor MouseCursor
    {
        get
        {
            if (m_MouseCursor == null)
                m_MouseCursor = _gameObject.GetComponent<MouseCursor>();

            return m_MouseCursor;
        }
    }

    private SoundManager m_SoundManager;
    public SoundManager SoundManager
    {
        get
        {
            if (m_SoundManager == null)
                m_SoundManager = _gameObject.GetComponent<SoundManager>();

            return m_SoundManager;
        }
    }





}