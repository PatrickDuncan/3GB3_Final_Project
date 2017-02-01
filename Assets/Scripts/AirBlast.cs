using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections;

public class AirBlast : MonoBehaviour {

    bool allowedToShoot = true;
    const float DISPPEAR_TIME = 0.8f;
    const float WAIT_TIME = 5f;

    public GameObject airWall;

    private void Update() {
        if (!allowedToShoot)
            return;
        // If the key is pressed create a game object (wall) and then add a velocity
        if (Input.GetKeyDown(KeyCode.Mouse1)) {
            Vector3 position = GameObject.FindWithTag("AirBlastOrigin").transform.position;
            Quaternion rotatoin = GameObject.FindWithTag("MainCamera").transform.rotation;
            GameObject gO = Instantiate(airWall, position, rotatoin) as GameObject;
			gO.GetComponent<Rigidbody>().AddForce(transform.forward * 200000);

            Destroy(gO, DISPPEAR_TIME);
            StartCoroutine(WaitToShoot());
		}
    }

    // You just used a weapon, wait to shoot again.
	private IEnumerator WaitToShoot() {
        allowedToShoot = false;
        yield return new WaitForSeconds(WAIT_TIME);
     	allowedToShoot = true;
    }
}
