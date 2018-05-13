using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * [DisallowMultipleComponent] is an attribute that ensures the same script 
 *    cannot be attached to the same game object multiple times
**/
[DisallowMultipleComponent]
public class Object_movement: MonoBehaviour {

    [SerializeField] Vector3 movementVector = new Vector3(10f, 10f, 10f);
    [SerializeField] float period = 2f;

    [Range(0,1)]
    [SerializeField] float movementFactor; // 0 for not moved, 1 for fully moved.

    private Vector3 startPos;

	// Use this for initialization
	void Start () {
        startPos = transform.position; 
	}
	
	// Update is called once per frame
	void Update () {
        //TODO make sure we do not try to divide by 0
        if (period <= Mathf.Epsilon) // Mathf.Epsilon is the smalest value that is available
        {
            return;
        }
        float cycles = Time.time / period; // keeps growing from 0, time divided by period gives number of completed cycles e.g 10 seconds time divided by 2 seconds gives us 5 complete cycles.
        const float tau = Mathf.PI * 2; // around 6.28
        float rawSinWave = Mathf.Sin(tau * cycles);
        // print(rawSinWave);

        movementFactor = rawSinWave / 2f;
        // print(movementFactor);
        Vector3 offset = (movementFactor + 0.5f) * movementVector;
        transform.position = startPos + offset;
	}
}
