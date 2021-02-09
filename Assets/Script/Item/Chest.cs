using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour {

    public GameObject[] _Item;
	// Use this for initialization
	void Start () {
        Transform child = Instantiate(_Item[Random.Range(0, _Item.Length)], transform.Find("Chest")).transform;
	}
}
