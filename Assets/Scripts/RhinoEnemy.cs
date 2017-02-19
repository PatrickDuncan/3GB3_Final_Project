using UnityEngine;
using System.Collections;

public class RhinoEnemy : MonoBehaviour, IEnemy {

    bool attacking = false;
    bool charging = false;
    bool waitToAttack = false;
    int die;
    int health;
	int hurt;
	int run;
	int walk;
    int playerCollisions;
    const float MAXVELOCITY = 10f;
    const float TURNSPEED = 0.8f;
    const float WAIT_ATTACK = 6f;

    Animator anim;
    Transform myTransform;
    Transform playerTrans;

    void Awake() {
        health = 200;
        playerCollisions = 0;
		anim = GetComponent<Animator>();
        die = Animator.StringToHash("Die");
		hurt = Animator.StringToHash("Get Hit");
		run = Animator.StringToHash("Run");
		walk = Animator.StringToHash("Walk");
        myTransform = transform;
        playerTrans = GameObject.FindWithTag("Player").transform;
        anim.SetTrigger(walk);
	}

    void FixedUpdate() {
        if (health < 1) return;
        if (!charging)
            ChasePlayer();
        Debug.Log(attacking + " " + charging + " " + waitToAttack + " " + playerCollisions);
        if (!waitToAttack && (FacingPlayer() || charging)) {
            Charge();
        }
    }

    void OnCollisionEnter(Collision col) {
        if (health < 1) return;
        string tag = col.gameObject.tag;
        CheckCollisions(tag);

        if (health < 1) {
            anim.SetTrigger(die);
            gameObject.layer = 2;
            attacking = false;
        }
    }

    void OnTriggerEnter(Collider col) {
        if (col.gameObject.tag == "AirBlast") {
            GetComponent<Rigidbody>().AddForce(
                col.gameObject.transform.forward * 100,
                ForceMode.Impulse
            );
        }
    }

    void Charge() {
        if (!attacking && !charging && !waitToAttack) {
            attacking = true;
            charging = true;
            anim.SetTrigger(run);
        }
        Rigidbody rigid = GetComponent<Rigidbody>();
        if (rigid.velocity.sqrMagnitude < 400f) {
            rigid.AddForce(myTransform.forward * 1400, ForceMode.Impulse);
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

    void CheckCollisions(string tag) {
        switch(tag) {
            case "Wall":
                StartCoroutine(WaitToAttack());
                anim.SetTrigger(walk);
                break;
            case "PistolBullet":
                health -= 15;
                anim.SetTrigger(hurt);
                anim.SetTrigger(attacking ? run : walk);
                break;
            case "ShotgunBullet":
                health -= 35;
                anim.SetTrigger(hurt);
                anim.SetTrigger(attacking ? run : walk);
                break;
            case "SMGBullet":
                health -= 8;
                anim.SetTrigger(hurt);
                anim.SetTrigger(attacking ? run : walk);
                break;
            case "Player":
                if (playerCollisions > 0)
                    attacking = false;
                ++playerCollisions;
                break;
		}
    }

    bool FacingPlayer() {
        // http://answers.unity3d.com/questions/503934/chow-to-check-if-an-object-is-facing-another.html
        float rotationDiff = Vector3.Dot(
            myTransform.forward,
            (playerTrans.position - myTransform.position).normalized
        );
        return rotationDiff > 0.99;
    }

    public bool GetAttacking() {
        return attacking;
    }

    IEnumerator WaitToAttack() {
        waitToAttack = true;
        charging = false;
        attacking = false;
        yield return new WaitForSeconds(WAIT_ATTACK);
     	waitToAttack = false;
        playerCollisions = 0;
    }
}
