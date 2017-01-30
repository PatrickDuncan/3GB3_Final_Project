using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections;

public class Weapon : MonoBehaviour {

    bool allowedToShoot = true;
    float[] waitTimes = {1f, 0.2f, 1f, 1f};

    public GameObject bullet;	// Assign the bullet prefab in the editor
    public Camera cam;

    enum weapons {
        melee,
        pistol,
        shotgun,
        smg
    }
    weapons curWeapon = weapons.pistol;

    private void Update() {
        if (!allowedToShoot)
            return;
        Vector3 wep;
        Vector3 position;
        // Get current weapon's position
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
			gO.GetComponent<Rigidbody>().AddForce(transform.forward * 120);
            StartCoroutine(Wait());
		}
    }

    // You just used a weapon, wait to shoot again.
	private IEnumerator Wait() {
        allowedToShoot = false;
        yield return new WaitForSeconds(waitTimes[(int) curWeapon]);
     	allowedToShoot = true;
    }
}
