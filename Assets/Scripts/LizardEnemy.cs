using UnityEngine;
using System;
using System.Collections;

public class LizardEnemy : MonoBehaviour, IEnemy {

    bool walking;
    bool attacking;         // if they can damage the enemy
    bool waitToAttack;      // if they can attack again

    int health;
    int playerCollisions;
    int scream;
	int attack;
	int hurt;
	int walk;
	int die;
    const float MAXVELOCITY = 12f;
    const float TURNSPEED = 2f;
    const float CAN_DAMAGE = 1f;
    const float WAIT_ATTACK = 3.5f;

    Animator anim;
    public AudioClip dead;
    Transform myTransform;
    Transform playerTrans;
    WaveLogic waveLogic;
    Weapon plyrWeapon;

    void Awake() {
        walking = true;
        attacking = false;
        waitToAttack = false;
        health = 100;
        playerCollisions = 0;
		anim = GetComponent<Animator>();
		attack = Animator.StringToHash("Basic Attack");
        die = Animator.StringToHash("Die");
		hurt = Animator.StringToHash("Get Hit");
		walk = Animator.StringToHash("Walk");
        anim.SetTrigger(walk);
        myTransform = transform;
        playerTrans = GameObject.FindWithTag("Player").transform;
        waveLogic = GameObject.FindWithTag("Wave Logic").GetComponent<WaveLogic>();
        plyrWeapon = GameObject.FindWithTag("MainCamera").GetComponent<Weapon>();
	}

    void FixedUpdate() {
        if (health < 1 || DontUpdate()) return;
        playerTrans = GameObject.FindWithTag("Player").transform;
        // Attack the player
        if (AttackConditions() && !attacking && !waitToAttack) {
            AttackPlayer();
        }
        // Move
        else if (walking && !attacking && !waitToAttack) {
            ChasePlayer();
            AddMovement();
        }
    }

    void OnCollisionEnter(Collision col) {
        if (health < 1)
            return;
        CheckCollisions(col.gameObject.tag);
    }

    void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "AirBlast") {
            GetComponent<Rigidbody>().AddForce(
                col.gameObject.transform.forward * 1500,
                ForceMode.Impulse
            );
        }
    }

    void AddMovement() {
        Rigidbody rigid = GetComponent<Rigidbody>();
        if (rigid.velocity.sqrMagnitude < 25f) {
            rigid.AddForce(myTransform.forward * 80, ForceMode.Impulse);
        }
    }

    void AttackPlayer() {
        anim.SetTrigger(attack);
        GetComponent<AudioSource>().Play();
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

    void CheckCollisions(string tag) {
        switch(tag) {
            case "PistolBullet":
                health -= 20;
                if (!waitToAttack)
                    anim.SetTrigger(hurt);
                break;
            case "ShotgunBullet":
                health -= 50;
                if (!waitToAttack)
                    anim.SetTrigger(hurt);
                break;
            case "SMGBullet":
                health -= 13;
                if (!waitToAttack)
                    anim.SetTrigger(hurt);
                break;
            case "Player":
                if (plyrWeapon.GetSwinging()) {
                    health -= 10;
                    plyrWeapon.StopSwinging();
                    if (!waitToAttack)
                        anim.SetTrigger(hurt);
                } else {
                    HitPlayer();
                }
                break;
        }
        Death();
    }

    void Death() {
        if (health < 1) {
            anim.SetTrigger(die);
            AudioSource aS = GetComponent<AudioSource>();
            aS.Stop();
            aS.clip = dead;
            aS.Play();
            gameObject.layer = 2;
            walking = false;
            attacking = false;
            waveLogic.EnemyKilled();
        }
    }

    bool DontUpdate() {
        RaycastHit hit;
        int layer_mask = ~(1 << gameObject.layer);  // bit shift then invert
        Vector3 fixedSelf = new Vector3(
            myTransform.position.x,
            myTransform.position.y + 1f,
            myTransform.position.z
        );
        Vector3 fixedPlayerPos = new Vector3(
            playerTrans.position.x,
            playerTrans.position.y + 3f,
            playerTrans.position.z
        );
        if (Physics.Linecast(fixedSelf, fixedPlayerPos, out hit, layer_mask))
            return hit.collider.gameObject.name != "Player";
        else
            return true;
    }

    bool FacingPlayer() {
        // http://answers.unity3d.com/questions/503934/chow-to-check-if-an-object-is-facing-another.html
        return Vector3.Dot(
            myTransform.forward,
            (playerTrans.position - myTransform.position).normalized
        ) > 0.8;
    }

    // If close enough to the player to attack and looking at the player
    bool AttackConditions() {
        bool closeEnough = Math.Sqrt(
            Math.Pow(myTransform.position.x - playerTrans.position.x, 2)
            +
            Math.Pow(myTransform.position.z - playerTrans.position.z, 2)
        ) < 16;
        return closeEnough && FacingPlayer();
    }

    public bool GetAttacking() {
        return attacking;
    }

    void HitPlayer() {
        if (!FacingPlayer())    // can only hurt if facing the player
            return;
        if (playerCollisions > 0)
            attacking = false;
        ++playerCollisions;
    }

    IEnumerator WaitToAttack() {
        attacking = true;
        waitToAttack = true;
        yield return new WaitForSeconds(CAN_DAMAGE);
        attacking = false;
        yield return new WaitForSeconds(WAIT_ATTACK);
     	waitToAttack = false;
        playerCollisions = 0;
    }
}
