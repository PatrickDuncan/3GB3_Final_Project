using UnityEngine;

public class AmmoPickup : MonoBehaviour {

    Weapon weapon;

    void Start() {
        weapon = GameObject.FindWithTag("MainCamera").GetComponent<Weapon>();
    }

    void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "Player") {
            weapon.AmmoPickup();
            GameObject.FindWithTag("Ammo Sound").GetComponent<AudioSource>().Play();
            Destroy(gameObject);    // auto destroy on impact
        }
	}
}
