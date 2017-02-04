using UnityEngine;

public class PlayerHealth : MonoBehaviour {

	int health = 100;

	void OnTriggerEnter(Collider col) {
		GetHurt(col.gameObject.tag);
	}

	void OnCollisionEnter(Collision col) {
		string tag = col.gameObject.tag;
		if (tag.Contains("Enemy")
		&& col.gameObject.GetComponent<LizardEnemy>().GetAttacking()) {
			GetHurt(tag);
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
	}
}
