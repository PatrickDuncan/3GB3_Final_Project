using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class WaveLogic : MonoBehaviour {

    bool won;
    int currentWave;
    int[] waveCounter;
    const float WAIT_TEXT = 2f;

    GameObject gameStart;
    GameObject[] wave1;
    GameObject[] wave2;
    GameObject[] wave3;
    GameObject[] wave4;
    Music music;

    void Start() {
        won = false;
        currentWave = -1;
        waveCounter = new int[4];
        // Get all waves
        wave1 = GameObject.FindGameObjectsWithTag("Wave 1");
        wave2 = GameObject.FindGameObjectsWithTag("Wave 2");
        wave3 = GameObject.FindGameObjectsWithTag("Wave 3");
        wave4 = GameObject.FindGameObjectsWithTag("Wave 4");

        // Set waves to inactive except for the first
        foreach (GameObject gO in wave2)
            gO.SetActive(false);
        foreach (GameObject gO in wave3)
            gO.SetActive(false);
        foreach (GameObject gO in wave4)
            gO.SetActive(false);

        // Count the number of enemies
        int index = wave1[0].name.Contains("Enemy") ? 0 : 1; // 2 folders with wave tag
        waveCounter[0] = wave1[index].GetComponentsInChildren(typeof(AudioSource)).Length;
        index = wave2[0].name.Contains("Enemy") ? 0 : 1;
        waveCounter[1] = wave2[index].GetComponentsInChildren(typeof(AudioSource)).Length;
        index = wave3[0].name.Contains("Enemy") ? 0 : 1;
        waveCounter[2] = wave3[index].GetComponentsInChildren(typeof(AudioSource)).Length;
        index = wave4[0].name.Contains("Enemy") ? 0 : 1;
        waveCounter[3] = wave4[index].GetComponentsInChildren(typeof(AudioSource)).Length;

        gameStart = GameObject.FindWithTag("Wave Start");

        music = GameObject.FindWithTag("Music").GetComponent<Music>();

        StartCoroutine(WaveStartText());
        UpdateUI();
	}

    public void EnemyKilled() {
        --waveCounter[currentWave];
        // next wave and there are more levels
        if (waveCounter[currentWave] == 0 && currentWave < 3) {
            StartCoroutine(WaveStartText());
            UpdateUI();
        }
        // Game complete
        else if (waveCounter[currentWave] == 0) {
            won = true;
            GameObject.FindWithTag("Wave").GetComponent<Text>().text = "0 Remaining";
            gameStart.GetComponent<Text>().text = "You Survived";
            gameStart.SetActive(true);
        } else {
            UpdateUI();
        }
    }

    void UpdateUI() {
        string text = waveCounter[currentWave] + " Remaining";
        GameObject.FindWithTag("Wave").GetComponent<Text>().text = text;
    }

    public bool Won() {
        return won;
    }

    IEnumerator WaveStartText() {
        ++currentWave;
        music.LowerPitch();
        string text = "Wave " + (currentWave + 1) + " Start";
        gameStart.GetComponent<Text>().text = text;
        gameStart.SetActive(true);
        switch (currentWave) {
            case 1:
                foreach (GameObject gO in wave2)
                    gO.SetActive(true);
                break;
            case 2:
                foreach (GameObject gO in wave3)
                    gO.SetActive(true);
                break;
            case 3:
                foreach (GameObject gO in wave4)
                    gO.SetActive(true);
                break;
        }
        yield return new WaitForSeconds(WAIT_TEXT);
        gameStart.SetActive(false);
    }
}
