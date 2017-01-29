using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class Weapon : MonoBehaviour {

    public GameObject bullet;	// Assign the bullet prefab in the editor
    public Camera cam;
    enum weapons {
        melee,
        pistol,
        shotgun,
        smg
    }
    weapons curWeapon = weapons.pistol;

    void Update() {
        Vector3 wep;
        Vector3 position;
        switch(curWeapon) {
            case weapons.pistol:
                wep = GameObject.FindWithTag("Pistol").transform.position;
                position = new Vector3(wep.x, wep.y, wep.z);
                break;
            default:
                position = new Vector3(0, 0, 0);
                break;
        }

        // If the key is pressed create a game object (bullet) and then add a velocity
        if (Input.GetKeyDown(KeyCode.Mouse0)) {
			GameObject gO = Instantiate(bullet, position, Quaternion.Euler(90, 0, 0)) as GameObject;
			gO.GetComponent<Rigidbody>().AddForce(transform.forward * 80);
		}
    }
}
