using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Weapon : MonoBehaviour {

    public GameObject bullet;	// Assign the bullet prefab in the editor
    public Camera camera;       // For the rotation

    void Update() {
        Quaternion rot = GameObject.FindWithTag("MainCamera").transform.rotation;
        Debug.Log(rot.x);
        // If the key is pressed create a game object (bullet) and then add a velocity
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
			GameObject gO = Instantiate(bullet, transform.position, Quaternion.identity) as GameObject;
            gO.transform.rotation = Quaternion.Euler(rot.x, 0, 0);
			gO.GetComponent<Rigidbody>().velocity = new Vector3(10, 0, 0);
		}
    }
}
