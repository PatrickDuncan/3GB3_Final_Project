using UnityEngine;
using System.Collections;

public class BreakTube : MonoBehaviour {

    public GameObject broken;

	void OnCollisionEnter(Collision col) {
    	if (col.gameObject.tag.Contains("Bullet") || col.gameObject.tag.Contains("Rhino")) {
            GameObject gO = Instantiate(broken, transform.position, Quaternion.identity) as GameObject;
            gO.GetComponent<AudioSource>().Play();
            Destroy(gameObject);
        }
	}
}
