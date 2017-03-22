using UnityEngine;

public class Bullet : MonoBehaviour {

	void Start() {
		Destroy(gameObject, 5f);
	}

	void OnCollisionEnter(Collision col) {
		if (col.gameObject.tag != "Player")
        	Destroy(gameObject);    // auto destroy on impact
	}
}
