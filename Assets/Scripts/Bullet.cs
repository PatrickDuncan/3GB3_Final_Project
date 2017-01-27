using UnityEngine;

public class Bullet : MonoBehaviour {

	private void Start () {
		Destroy(gameObject, 5f);			// Automatically destroy the bulletType in 3 seconds
	}

	private void OnCollisionEnter(Collision col) {
    //    Destroy(gameObject);    // auto destroy on impact
	}
}
