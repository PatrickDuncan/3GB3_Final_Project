using UnityEngine;
using System.Collections;

public class RhinoEnemy : MonoBehaviour, IEnemy {

    //bool walking = true;
    //bool attacking = true;

    int health;
	int hurt;
	int die;
    const float WALKSPEED = 20f;
    const float MAXVELOCITY = 10f;
    const float TURNSPEED = 0.8f;

    Animator anim;
    Transform myTransform;
    Transform playerTrans;

    void Awake() {
        health = 200;
		anim = GetComponent<Animator>();
		hurt = Animator.StringToHash("Get Hit");
		die = Animator.StringToHash("Die");
        myTransform = transform;
        playerTrans = GameObject.FindWithTag("Player").transform;
	}

    void FixedUpdate() {
        ChasePlayer();
    }

    void OnCollisionEnter(Collision col) {
        if (health < 1) return;

        string tag = col.gameObject.tag;
        CheckCollisions(tag);

        if (health < 1) {
            anim.SetTrigger(die);
            gameObject.layer = 2;
            //walking = false;
            //attacking = false;
        }
    }

    void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "AirBlast") {
            GetComponent<Rigidbody>().AddForce(
                col.gameObject.transform.forward * 1000,
                ForceMode.Impulse
            );
        }
    }

    void CheckCollisions(string tag) {
        switch(tag) {
            case "PistolBullet":
                health -= 15;
                anim.SetTrigger(hurt);
                break;
		}
    }

    void ChasePlayer() {
        Vector3 turn = Quaternion.Slerp(
            myTransform.rotation,
            Quaternion.LookRotation(playerTrans.position - myTransform.position),
            Time.deltaTime * TURNSPEED
        ).eulerAngles;
        myTransform.rotation = Quaternion.Euler(0, turn.y, turn.z);
    }
}
