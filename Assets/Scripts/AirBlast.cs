using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections;

public class AirBlast : MonoBehaviour {

    bool allowedToShoot = true;
    int blastAnim;
    const float DISPPEAR_TIME = 0.7f;
    const float WAIT_TIME = 5f;

    Animator anim;
    AudioSource shootSound;
    public GameObject airWall;

    void Start() {
        anim = GameObject.FindWithTag("AirPushHand").GetComponent<Animator>();
        blastAnim = Animator.StringToHash("Push");
        shootSound = GameObject.FindWithTag("AirBlastOrigin").GetComponent<AudioSource>();
    }

    void Update() {
        if (!allowedToShoot)
            return;
        // If the key is pressed create a game object (wall) and then add a velocity
        if (Input.GetKeyDown(KeyCode.Mouse1)) {
            anim.SetTrigger(blastAnim);
            Vector3 position = GameObject.FindWithTag("AirBlastOrigin").transform.position;
            Quaternion rotation = GameObject.FindWithTag("MainCamera").transform.rotation;
            GameObject gO = Instantiate(airWall, position, rotation) as GameObject;
			gO.GetComponent<Rigidbody>().AddForce(transform.forward * 500000);

            shootSound.Play();
            Destroy(gO, DISPPEAR_TIME);
            StartCoroutine(WaitToShoot());
		}
    }

    // You just used a weapon, wait to shoot again.
	IEnumerator WaitToShoot() {
        allowedToShoot = false;
        yield return new WaitForSeconds(WAIT_TIME);
     	allowedToShoot = true;
    }
}
