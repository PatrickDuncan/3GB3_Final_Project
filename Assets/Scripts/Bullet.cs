using UnityEngine;

public class Bullet : MonoBehaviour {

	private Weapon weapon;						// Reference to the Gun class in Player.

	private void Start () {
		weapon = GameObject.FindWithTag("Weapon").GetComponentInChildren<Weapon>();
	//	Destroy(gameObject, 3f);			// Automatically destroy the bulletType in 3 seconds
	}

	private void OnTriggerEnter2D (Collider2D col) {
        Destroy(gameObject);    // auto destroy on impact
	}
}
