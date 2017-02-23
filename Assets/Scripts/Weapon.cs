using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Weapon : MonoBehaviour {

    bool shooting = false;
    bool[] reloading = {false, false, false, false};
    int[] CLIP_SIZES = {8, 1, 15};
    int[] clip_sizes = new int[3];
    int[] ammo_ammounts = {20, 12, 30};   // the amount of bullet is this + clip_sizes
    float[] WAIT_TIMES = {0.3f, 0.3f, 1.6f, 0.075f};

    Animator anim;
    public AudioClip pistol_reload_clip;
    public AudioClip pistol_shoot_clip;
    public AudioClip smg_reload_clip;
    public AudioClip smg_shoot_clip;
    AudioSource shootSound;
    public GameObject bullet;	// Assign the bullet prefab in the editor
    GameObject melee;
    GameObject pistol;
    GameObject smg;
    GameObject shotgun;
    PlayerHealth playerHealth;
    Transform myTransform;

    enum weapons {
        melee,
        pistol,
        shotgun,
        smg
    }
    weapons curWeapon;

    void Start() {
        myTransform = transform;
        curWeapon = weapons.shotgun;
        melee = GameObject.FindWithTag("Knife");
        pistol = GameObject.FindWithTag("Pistol");
        shotgun = GameObject.FindWithTag("Shotgun");
        smg = GameObject.FindWithTag("SubmachineGun");
        playerHealth = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
        melee.SetActive(false);
        pistol.SetActive(false);
        smg.SetActive(false);
        anim = shotgun.GetComponent<Animator>();
        shootSound = shotgun.GetComponent<AudioSource>();
        bullet.tag = "ShotgunBullet";
        clip_sizes[0] = CLIP_SIZES[0];
        clip_sizes[1] = CLIP_SIZES[1];
        clip_sizes[2] = CLIP_SIZES[2];
        GameObject.FindWithTag("Ammo").GetComponent<Text>().text = " 1 / " + ammo_ammounts[1] + " ";
    }

    void Update() {
        if (shooting)
            return;
        Vector3 wep = GameObject.FindWithTag("Center").transform.position;
        Vector3 position = new Vector3(wep.x, wep.y, wep.z);
        CheckKeyInput(position);
    }

    void Ammunition() {
        switch(curWeapon) {
            case weapons.pistol:
                GunReloading(0);
                break;
            case weapons.shotgun:
                GunReloading(1);
                break;
            case weapons.smg:
                GunReloading(2);
                break;
        }
    }

    void CheckKeyInput(Vector3 position) {
        int current = (int) curWeapon;
        // Stuck in reloading animation and can't shoot while dead
        if (reloading[current] || playerHealth.GetHealth() < 1)
            return;
        // If the key is pressed create a game object (bullet) and then apply a force
        if (Input.GetKey(KeyCode.Mouse0) && curWeapon != weapons.melee) {
            // can't shoot if there's no ammo of that weapon's type
            if ((current == 1 && clip_sizes[current - 1] == 0)
             || (current == 2 && clip_sizes[current - 1] == 0)
             || (current == 3 && clip_sizes[current - 1] == 0)) {
                return;
            }
			GameObject gO = Instantiate(bullet, position, Quaternion.Euler(90, 0, 0)) as GameObject;
			gO.GetComponent<Rigidbody>().AddForce(myTransform.forward * 200);
            shootSound.Play();

            Ammunition();
            // Ammunition has a side effect on reloading[current]
            if (reloading[current])
                return;

            if (curWeapon != weapons.shotgun)
                anim.SetTrigger("Shoot");

            StartCoroutine(WaitToShoot());
		}
        // Switch to Knife
        else if (Input.GetKeyDown(KeyCode.Alpha1) && current != 0) {
            curWeapon = weapons.melee;
            shotgun.SetActive(false);
            smg.SetActive(false);
            pistol.SetActive(false);
            melee.SetActive(true);
            UpdateUI();
        }
        // Switch to Pistol
        else if (Input.GetKeyDown(KeyCode.Alpha2) && current != 1) {
            curWeapon = weapons.pistol;
            melee.SetActive(false);
            shotgun.SetActive(false);
            smg.SetActive(false);
            pistol.SetActive(true);
            bullet.tag = "PistolBullet";
            anim = pistol.GetComponent<Animator>();
            shootSound = pistol.GetComponent<AudioSource>();
            UpdateUI();
        }
        // Switch to Shotgun
        else if (Input.GetKeyDown(KeyCode.Alpha3) && current != 2) {
            curWeapon = weapons.shotgun;
            melee.SetActive(false);
            pistol.SetActive(false);
            smg.SetActive(false);
            shotgun.SetActive(true);
            bullet.tag = "ShotgunBullet";
            anim = shotgun.GetComponent<Animator>();
            shootSound = shotgun.GetComponent<AudioSource>();
            UpdateUI();
        }
        // Switch to SMG
        else if (Input.GetKeyDown(KeyCode.Alpha4) && current != 3) {
            curWeapon = weapons.smg;
            melee.SetActive(false);
            pistol.SetActive(false);
            shotgun.SetActive(false);
            smg.SetActive(true);
            bullet.tag = "SMGBullet";
            anim = smg.GetComponent<Animator>();
            shootSound = smg.GetComponent<AudioSource>();
            UpdateUI();
        }
    }

    void GunReloading(int i) {
        --clip_sizes[i];
        UpdateUI();
        // Reload the gun
        if (clip_sizes[i] == 0 && ammo_ammounts[i] > 0) {
            if (curWeapon != weapons.smg) {
                anim.SetTrigger("Reload");
                if (curWeapon == weapons.pistol) {
                    AudioSource aS = pistol.GetComponent<AudioSource>();
                    aS.clip = pistol_reload_clip;
                    aS.Play();
                }
            } else {
                AudioSource aS = smg.GetComponent<AudioSource>();
                smg.transform.Find("ScifiRifle").GetComponent<Animator>().SetTrigger("Reload");
                aS.clip = smg_reload_clip;
                aS.Play();
            }
            StartCoroutine(WaitReload(i + 1));
            int subtration = ammo_ammounts[i] - CLIP_SIZES[i];
            int amount = subtration > 0 ? subtration : ammo_ammounts[i];
            // If ammo is more than max clip size then subtract it, if not make it 0
            ammo_ammounts[i] = subtration > 0 ? subtration : 0;
            // Current clip size is the max clip size, the left overs, or 0
            clip_sizes[i] = amount > 0
                ? (amount > CLIP_SIZES[i] ? CLIP_SIZES[i] : amount)
                : 0;
        }
        // Shotgun reloads when others don't
        else if (ammo_ammounts[i] == 0 && i == 1) {
            anim.SetTrigger("Reload");
        }
    }

    void UpdateUI() {
        string ammo = "";
        switch(curWeapon) {
            case weapons.pistol:
                ammo = " " + clip_sizes[0] + " / " + ammo_ammounts[0] + " ";
                break;
            case weapons.shotgun:
                ammo = " " + clip_sizes[1] + " / " + ammo_ammounts[1] + " ";
                break;
            case weapons.smg:
                ammo = (clip_sizes[2] < 10 ? " " : "") + clip_sizes[2] + " / " + ammo_ammounts[2];
                break;
        }
		GameObject.FindWithTag("Ammo").GetComponent<Text>().text = ammo;
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
        UpdateUI();
        // Reset audio sources to the shooting clips
        if (curWeapon == weapons.pistol)
            pistol.GetComponent<AudioSource>().clip = pistol_shoot_clip;
        else if (curWeapon == weapons.smg)
            smg.GetComponent<AudioSource>().clip = smg_shoot_clip;
     	reloading[i] = false;
    }
}
