using UnityEngine;

public class HealthPickup : MonoBehaviour {

    PlayerHealth playerHealth;

    void Start() {
        playerHealth = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
    }

    void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "Player") {
            playerHealth.HealthPickup();
            Destroy(gameObject);    // auto destroy on impact
        }
	}
}
