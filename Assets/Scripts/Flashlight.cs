using System;
using UnityEngine;

public class Flashlight : MonoBehaviour {

    bool off;

    PlayerHealth player;

    void Start() {
        player = GameObject.FindWithTag("Player").GetComponent<PlayerHealth>();
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.F) && player.GetHealth() > 0) {
            GetComponent<Light>().intensity = (off ? 8 : 0);
            off = !off;
        }
    }
}
