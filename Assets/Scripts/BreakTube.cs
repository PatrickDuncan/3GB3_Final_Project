using UnityEngine;
using System.Collections;

public class BreakTube : MonoBehaviour {

    public GameObject broken;

	void OnCollisionEnter(Collision col) {
    	if (col.gameObject.tag.Contains("Bullet") || col.gameObject.tag.Contains("Rhino")) {
            GetComponent<AudioSource>().Play();
            GameObject gO = Instantiate(broken, transform.position, Quaternion.identity) as GameObject;
            StartCoroutine(WaitToDestory());
        }
	}

    // Waiting so player can hear the shatter
    IEnumerator WaitToDestory() {
        yield return new WaitForSeconds(0.4f);
        Destroy(gameObject);
    }
}
