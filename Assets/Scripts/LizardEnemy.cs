using UnityEngine;
using System.Collections;

public class LizardEnemy : MonoBehaviour, IEnemy {

    bool walking = true;
    bool attacking = true;

    int health = 100;
    int scream;
	int basicAttack;
	int hurt;
	int walk;
	int die;
    const float WALKSPEED = 10f;
    const float MAXVELOCITY = 4f;
    const float TURNSPEED = 2f;

    Animator anim;
    Transform myTransform;

    void Awake() {
		anim = GetComponent<Animator>();
		basicAttack = Animator.StringToHash("Basic Attack");
		hurt = Animator.StringToHash("Get Hit");
		walk = Animator.StringToHash("Walk");
		die = Animator.StringToHash("Die");
        anim.SetTrigger(walk);
        myTransform = transform;
	}

    void FixedUpdate() {
        // Move
        if (walking) {
            ChasePlayer();
            AddMovement();
        }
    }

    void OnCollisionEnter(Collision col) {
        if (health < 1) return;
        string tag = col.gameObject.tag;

        GetHurt(tag);

        if (health < 1) {
            anim.SetTrigger(die);
            gameObject.layer = 2;
            walking = false;
            attacking = false;
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

    void AddMovement() {
        Rigidbody rigid = GetComponent<Rigidbody>();
        if (rigid.velocity.sqrMagnitude < 5f * 5f) {
            rigid.AddForce(myTransform.forward * 20, ForceMode.Impulse);
        }
    }

    void ChasePlayer() {
        Vector3 playerPos = GameObject.FindWithTag("Player").transform.position;
        Vector3 turn = Quaternion.Slerp(
            myTransform.rotation,
            Quaternion.LookRotation(playerPos - myTransform.position),
            Time.deltaTime * TURNSPEED
        ).eulerAngles;
        myTransform.rotation = Quaternion.Euler(0, turn.y, turn.z);
    }

    void GetHurt(string tag) {
        switch(tag) {
            case "PistolBullet":
                health -= 20;
                anim.SetTrigger(hurt);
                break;
            case "Fire":
                health = 0; // instant death
                break;
		}
    }
}
