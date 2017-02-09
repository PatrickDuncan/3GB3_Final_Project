using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;
using System.Collections;

public class Weapon : MonoBehaviour {

    bool shooting = false;
    bool[] reloading = {false, false, false, false};
    float[] CLIP_SIZES = {8, 1, 15};
    float[] clip_sizes = new float[3];
    float[] WAIT_TIMES = {0.3f, 0.35f, 1.6f, 0.1f};

    public GameObject bullet;	// Assign the bullet prefab in the editor
    GameObject melee;
    GameObject pistol;
    GameObject smg;
    GameObject shotgun;
    Animator anim;
    AudioSource shootSound;

    enum weapons {
        melee,
        pistol,
        shotgun,
        smg
    }
    weapons curWeapon;

    void Start() {
        curWeapon = weapons.shotgun;
        melee = GameObject.FindWithTag("Knife");
        pistol = GameObject.FindWithTag("Pistol");
        shotgun = GameObject.FindWithTag("Shotgun");
        smg = GameObject.FindWithTag("SubmachineGun");
        melee.SetActive(false);
        pistol.SetActive(false);
        smg.SetActive(false);
        anim = shotgun.GetComponent<Animator>();
        shootSound = shotgun.GetComponent<AudioSource>();
        bullet.tag = "ShotgunBullet";
        clip_sizes[0] = CLIP_SIZES[0];
        clip_sizes[1] = CLIP_SIZES[1];
        clip_sizes[2] = CLIP_SIZES[2];
    }

    void Update() {
        if (shooting) return;
        Vector3 wep = GameObject.FindWithTag("Center").transform.position;
        Vector3 position = new Vector3(wep.x, wep.y, wep.z);
        CheckKeyInput(position);
    }

    void Ammunition() {
        switch(curWeapon) {
            case weapons.pistol:
                --clip_sizes[0];
                if (clip_sizes[0] == 0) {
                    anim.SetTrigger("Reload");
                    StartCoroutine(WaitReload(1));
                    clip_sizes[0] = CLIP_SIZES[0];
                }
                break;
            case weapons.shotgun:
                --clip_sizes[1];
                if (clip_sizes[1] == 0) {
                    anim.SetTrigger("Reload");
                    StartCoroutine(WaitReload(2));
                    clip_sizes[1] = CLIP_SIZES[1];
                }
                break;
            case weapons.smg:
                --clip_sizes[2];
                if (clip_sizes[2] == 0) {
                    anim.SetTrigger("Reload");
                    StartCoroutine(WaitReload(3));
                    clip_sizes[2] = CLIP_SIZES[2];
                }
                break;
        }
    }

    void CheckKeyInput(Vector3 position) {
        // Stuck in reloading animation
        if (reloading[(int) curWeapon]) return;
        // If the key is pressed create a game object (bullet) and then add a velocity
        if (Input.GetKeyDown(KeyCode.Mouse0) && curWeapon != weapons.melee) {

			GameObject gO = Instantiate(bullet, position, Quaternion.Euler(90, 0, 0)) as GameObject;
			gO.GetComponent<Rigidbody>().AddForce(transform.forward * 200);
            shootSound.Play();

            Ammunition();

            StartCoroutine(WaitToShoot());
		}
        // Switch to Knife
        else if (Input.GetKeyDown(KeyCode.Alpha1)) {
            curWeapon = weapons.melee;
            shotgun.SetActive(false);
            smg.SetActive(false);
            pistol.SetActive(false);
            melee.SetActive(true);
        }
        // Switch to Pistol
        else if (Input.GetKeyDown(KeyCode.Alpha2)) {
            curWeapon = weapons.pistol;
            melee.SetActive(false);
            shotgun.SetActive(false);
            smg.SetActive(false);
            pistol.SetActive(true);
            bullet.tag = "PistolBullet";
            shootSound = pistol.GetComponent<AudioSource>();
        }
        // Switch to Shotgun
        else if (Input.GetKeyDown(KeyCode.Alpha3)) {
            curWeapon = weapons.shotgun;
            melee.SetActive(false);
            pistol.SetActive(false);
            smg.SetActive(false);
            shotgun.SetActive(true);
            bullet.tag = "ShotgunBullet";
            anim = shotgun.GetComponent<Animator>();
            shootSound = shotgun.GetComponent<AudioSource>();
        }
        // Switch to SMG
        else if (Input.GetKeyDown(KeyCode.Alpha4)) {
            curWeapon = weapons.smg;
            melee.SetActive(false);
            pistol.SetActive(false);
            shotgun.SetActive(false);
            smg.SetActive(true);
            bullet.tag = "SMGBullet";
            anim = smg.GetComponent<Animator>();
            shootSound = smg.GetComponent<AudioSource>();
        }
    }

    // You just used a weapon, wait to shoot again.
	IEnumerator WaitToShoot() {
        shooting = true;
        yield return new WaitForSeconds(WAIT_TIMES[(int) curWeapon]);
     	shooting = false;
    }

	IEnumerator WaitReload(int i) {
        reloading[i] = true;
        yield return new WaitForSeconds(1.6f);
     	reloading[i] = false;
    }
}
