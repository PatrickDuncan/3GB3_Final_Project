using UnityEngine;

public class PlayerHealth : MonoBehaviour {

	int health = 100;

	void OnTriggerEnter(Collider col) {
		string tag = col.gameObject.tag;
		GetHurt(tag);
	}

	public int GetHealth() {
		return health;
	}

	void GetHurt(string tag) {
		switch(tag) {
			case "Fire":
				GetComponent<Rigidbody>().isKinematic = true;
	        	health = 0;	// instant death
				break;
		}
	}
}
