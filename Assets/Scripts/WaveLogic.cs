using UnityEngine;

public class WaveLogic : MonoBehaviour {

    GameObject[] wave1;
    GameObject[] wave2;
    GameObject[] wave3;
    GameObject[] wave4;

    void Start() {
        wave1 = GameObject.FindGameObjectsWithTag("Wave1");
        wave2 = GameObject.FindGameObjectsWithTag("Wave2");
        wave3 = GameObject.FindGameObjectsWithTag("Wave3");
        wave4 = GameObject.FindGameObjectsWithTag("Wave4");
        foreach (GameObject gO in wave2)
            gO.SetActive(false);
        foreach (GameObject gO in wave3)
            gO.SetActive(false);
        foreach (GameObject gO in wave4)
            gO.SetActive(false);
	}
}
