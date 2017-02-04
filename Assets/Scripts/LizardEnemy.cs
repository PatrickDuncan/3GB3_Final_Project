using UnityEngine;
using System;
using System.Collections;

public class LizardEnemy : MonoBehaviour, IEnemy {

    bool walking = true;
    bool attacking = false;         // if they can damage the enemy
    bool waitToAttack = false;      // if they can attack again

    int health = 100;
    int scream;
	int attack;
	int hurt;
	int walk;
	int die;
    int playerCollisions = 0;
    const float MAXVELOCITY = 12f;
    const float TURNSPEED = 2f;
    const float WAIT_ATTACK = 4f;

    Animator anim;
    Transform myTransform;
    Transform playerTrans;

    void Awake() {
		anim = GetComponent<Animator>();
		attack = Animator.StringToHash("Basic Attack");
		hurt = Animator.StringToHash("Get Hit");
		walk = Animator.StringToHash("Walk");
		die = Animator.StringToHash("Die");
        anim.SetTrigger(walk);
        myTransform = transform;
        playerTrans = GameObject.FindWithTag("Player").transform;
	}

    void FixedUpdate() {
        if (health < 1) return;
        playerTrans = GameObject.FindWithTag("Player").transform;
        // Attack the player
        if (PlayerProximity() && !attacking && !waitToAttack) {
            AttackPlayer();
        }
        // Move
        else if (walking && !attacking) {
            ChasePlayer();
            AddMovement();
        }
    }

    void OnCollisionEnter(Collision col) {
        if (health < 1) return;
        string tag = col.gameObject.tag;

        CheckCollisions(tag);

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
        if (rigid.velocity.sqrMagnitude < 25f) {
            rigid.AddForce(myTransform.forward * 70, ForceMode.Impulse);
        }
    }

    void AttackPlayer() {
        anim.SetTrigger(attack);
        GetComponent<Rigidbody>().AddForce(myTransform.forward * 600, ForceMode.Impulse);
        StartCoroutine(WaitToAttack());
    }

    void ChasePlayer() {
        Vector3 turn = Quaternion.Slerp(
            myTransform.rotation,
            Quaternion.LookRotation(playerTrans.position - myTransform.position),
            Time.deltaTime * TURNSPEED
        ).eulerAngles;
        myTransform.rotation = Quaternion.Euler(0, turn.y, turn.z);
    }

    bool PlayerProximity() {
        bool closeEnough = Math.Sqrt(
            Math.Pow(myTransform.position.x - playerTrans.position.x, 2)
            +
            Math.Pow(myTransform.position.z - playerTrans.position.z, 2)
        ) < 16;
        // http://answers.unity3d.com/questions/503934/chow-to-check-if-an-object-is-facing-another.html
        float rotationDiff = Vector3.Dot(myTransform.forward, (playerTrans.position - myTransform.position).normalized);
        return closeEnough && rotationDiff > 0.8;
    }

    public bool GetAttacking() {
        return attacking;
    }

    void CheckCollisions(string tag) {
        switch(tag) {
            case "PistolBullet":
                health -= 20;
                anim.SetTrigger(hurt);
                break;
            case "Player":
                if (playerCollisions > 0) {
                    attacking = false;
                }
                ++playerCollisions;
                break;
		}
    }

    IEnumerator WaitToAttack() {
        attacking = true;
        waitToAttack = true;
        yield return new WaitForSeconds(WAIT_ATTACK);
     	attacking = false;
     	waitToAttack = false;
        playerCollisions = 0;
    }
}
