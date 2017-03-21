using UnityEngine;
using UnityEngine.SceneManagement;

public class Music : MonoBehaviour {

	bool initial = true;

	void Awake () {
		DontDestroyOnLoad(transform.gameObject);
	}

	public void LowerPitch() {
         GetComponent<AudioSource>().pitch -= 0.2f;
    }
}
