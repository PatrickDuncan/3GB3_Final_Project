using UnityEngine;

public class Bullet : MonoBehaviour {

	void Start() {
		Destroy(gameObject, 5f);
	}

	void OnCollisionEnter(Collision col) {
        Destroy(gameObject);    // auto destroy on impact
	}
}
