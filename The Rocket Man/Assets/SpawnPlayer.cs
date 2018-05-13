using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPlayer : MonoBehaviour {

    public GameObject player;
    public float playerSpawnYFromParent = 10f;

    private Vector3 playerStartingPos;


    // Use this for initialization
    void Start () {
        playerStartingPos = new Vector3(transform.position.x, transform.position.y + playerSpawnYFromParent, transform.position.z);

        Instantiate(player, playerStartingPos, transform.rotation);
	}
	
	// Update is called once per frame
	void Update () {
    }
}
