using UnityEngine;

public class WaveLogic : MonoBehaviour {

    int wave1Counter;
    int wave2Counter;
    int wave3Counter;
    int wave4Counter;

    GameObject[] wave1;
    GameObject[] wave2;
    GameObject[] wave3;
    GameObject[] wave4;

    void Start() {
        wave1 = GameObject.FindGameObjectsWithTag("Wave 1");
        wave2 = GameObject.FindGameObjectsWithTag("Wave 2");
        wave3 = GameObject.FindGameObjectsWithTag("Wave 3");
        wave4 = GameObject.FindGameObjectsWithTag("Wave 4");
        foreach (GameObject gO in wave2)
            gO.SetActive(false);
        foreach (GameObject gO in wave3)
            gO.SetActive(false);
        foreach (GameObject gO in wave4)
            gO.SetActive(false);
	}
}
