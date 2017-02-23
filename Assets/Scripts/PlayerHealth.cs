using UnityEngine;
using UnityEngine.UI;

public class PlayerHealth : MonoBehaviour {

	int health;

	Transform myTransform;
	GameObject[] death;

	void Awake() {
		health = 100;
		GameObject.FindWithTag("HP").GetComponent<Text>().text = health.ToString() + " ❤";
		myTransform = transform;
		death = GameObject.FindGameObjectsWithTag("DeathScreen");
		foreach (GameObject gO in death)
			gO.SetActive(false);
	}

	void OnTriggerEnter(Collider col) {
		if (health < 1)
			return;
		GetHurt(col.gameObject);
	}

	void OnCollisionEnter(Collision col) {
		if (health < 1)
			return;
		GameObject gO = col.gameObject;
		if (gO.tag.Contains("Enemy")) {
			GetHurt(gO);
		}
	}

	void DeathCheck() {
		if (health < 1) {
			foreach (GameObject gO in death)
				gO.SetActive(true);
		}
	}

	public int GetHealth() {
		return health;
	}

	void GetHurt(GameObject gO) {
		switch(gO.tag) {
			case "LizardEnemy":
				if (gO.GetComponent<LizardEnemy>().GetAttacking())
					health -= 15;
				break;
			case "RhinoEnemy":
				if (gO.GetComponent<RhinoEnemy>().GetAttacking())
					health -= 30;
				break;
			case "Fire":
				GetComponent<Rigidbody>().isKinematic = true;
	        	health = 0;	// instant death
				break;
		}
		UpdateUI();
		DeathCheck();
	}

	void UpdateUI() {
		GameObject.FindWithTag("HP").GetComponent<Text>().text = health > 0
			? health.ToString() + " ❤"
			: "0 ❤";
	}
}
