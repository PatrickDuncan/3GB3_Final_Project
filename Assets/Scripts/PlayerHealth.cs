using UnityEngine;

public class PlayerHealth : MonoBehaviour {

	public int health;

	private void OnCollisionEnter(Collision col) {
		string tag = col.gameObject.tag;
		if (tag != "Player") {
        	//Destroy(gameObject);    // auto destroy on impact
		}
	}
}
