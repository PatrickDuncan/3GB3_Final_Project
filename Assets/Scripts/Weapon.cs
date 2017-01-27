using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Weapon : MonoBehaviour {

    public GameObject bullet;	// Assign the bullet prefab in the editor

    void Update() {
        //Debug.Log(rot.x);
        // If the key is pressed create a game object (bullet) and then add a velocity
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
			GameObject gO = Instantiate(bullet, transform.position, transform.rotation) as GameObject;
			gO.GetComponent<Rigidbody>().velocity = new Vector3(30, 0, 0);
		}
    }
}
