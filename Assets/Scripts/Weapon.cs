using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Weapon : MonoBehaviour {

    public GameObject bullet;	// Assign the bullet prefab in the editor
    public Camera cam;

    void Update() {
        Vector3 pistol = GameObject.FindWithTag("Weapon").transform.position;
        Vector3 position = new Vector3(pistol.x, pistol.y, pistol.z);
        // If the key is pressed create a game object (bullet) and then add a velocity
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
			GameObject gO = Instantiate(bullet, position, Quaternion.identity) as GameObject;
			gO.GetComponent<Rigidbody>().AddForce(transform.forward * 50);
		}
    }
}
