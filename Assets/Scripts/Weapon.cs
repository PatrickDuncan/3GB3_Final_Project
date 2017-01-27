using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Weapon : MonoBehaviour {

    public GameObject bullet;	// Assign the bullet prefab in the editor
    public Camera cam;

    void Update() {
        Vector3 v = transform.forward;
        Quaternion blah = Quaternion.Euler(90, v.y, v.z);
        // If the key is pressed create a game object (bullet) and then add a velocity
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
			GameObject gO = Instantiate(bullet, transform.position, blah) as GameObject;
			gO.GetComponent<Rigidbody>().velocity = v;
			//gO.GetComponent<Rigidbody>().AddForce(transform.forward);
		}
    }
}
