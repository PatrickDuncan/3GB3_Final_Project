using UnityEngine;

public class PlayerHealth : MonoBehaviour {

	int health;

	Transform myTransform;
	GameObject death;

	void Awake() {
		health = 100;
		myTransform = transform;
		death = GameObject.FindWithTag("DeathScreen");
		death.SetActive(false);
	}

	void OnTriggerEnter(Collider col) {
		if (health < 1) return;
		GetHurt(col.gameObject.tag);
	}

	void OnCollisionEnter(Collision col) {
		if (health < 1) return;
		string tag = col.gameObject.tag;
		if (tag.Contains("Enemy")
		&& col.gameObject.GetComponent<LizardEnemy>().GetAttacking()) {
			GetHurt(tag);
		}
	}

	void Death() {
		if (health < 1) {
			death.SetActive(true);
		}
	}

	public int GetHealth() {
		return health;
	}

	void GetHurt(string tag) {
		switch(tag) {
			case "LizardEnemy":
				health -= 15;
				break;
			case "Fire":
				GetComponent<Rigidbody>().isKinematic = true;
	        	health = 0;	// instant death
				break;
		}
		Death();
	}
}
