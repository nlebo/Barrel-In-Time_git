using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Timer : MonoBehaviour {

    public bool TimeStop = false;
    public bool TimeSlow = false;
    bool PauseDown = false; 

	private class TimedEvent
	{
		public float TimeToExecute;
		public Callback Method;
	}

	private List<TimedEvent> events;

	public delegate void Callback();

	void Awake(){
		events = new List<TimedEvent>();
        StopAllCoroutines();
	}

	public void Add(Callback method,float inSeconds)
	{
		events.Add (new TimedEvent{ 
			Method = method, 
			TimeToExecute = Time.time + inSeconds 
		});
	} 
	
	// Update is called once per frame
	void Update () {
        if (GameManager.Instance.InputManager.Pause)
        {

            PauseDown ^= true;

            if (PauseDown)
                TimeStop = true;
            else if(!InventoryManager.Instance.Inventory.activeInHierarchy)
                TimeStop = false;
        }

        if (TimeStop)
            Time.timeScale = 0;
        else
            Time.timeScale = 1;

        if (TimeSlow && !TimeStop)
            Time.timeScale = 0.1f;

        if (events.Count == 0)
			return;

		for (int i = 0; i < events.Count; i++) {
			var timedEvent = events [i];
			if (timedEvent.TimeToExecute <= Time.time) {
				timedEvent.Method ();
				events.Remove (timedEvent);
			}
		}

        
	}
}
