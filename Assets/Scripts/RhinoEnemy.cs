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
    const float WAIT_ATTACK = 3f;

    Animator anim;
    public AudioClip dead;
    Transform myTransform;
    Transform playerTrans;
    WaveLogic waveLogic;
    Weapon plyrWeapon;

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
        waveLogic = GameObject.FindWithTag("Wave Logic").GetComponent<WaveLogic>();
        plyrWeapon = GameObject.FindWithTag("MainCamera").GetComponent<Weapon>();
	}

    void FixedUpdate() {
        if (health < 1 || DontUpdate()) return;
        if (!charging)
            ChasePlayer();
        if (!waitToAttack && (FacingPlayer(true) || charging)) {
            Charge();
        }
    }

    void OnCollisionEnter(Collision col) {
        if (health < 1)
            return;
        CheckCollisions(col.gameObject.tag);
        Death();
    }

    void OnTriggerEnter(Collider col) {
        // Stop the charge if hit with air blast from the front
        if (col.gameObject.tag == "AirBlast" && FacingPlayer(false)) {
            CheckCollisions("Wall");
        }
    }

    void Charge() {
        if (!attacking && !charging && !waitToAttack) {
            attacking = true;
            charging = true;
            anim.SetTrigger(run);
            GetComponent<AudioSource>().Play();
        }
        Rigidbody rigid = GetComponent<Rigidbody>();
        if (rigid.velocity.sqrMagnitude < 700f) {
            rigid.AddForce(myTransform.forward * 1600, ForceMode.Impulse);
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
            case "Door":
            case "LizardEnemy":
            case "Wall":
                StartCoroutine(WaitToAttack());
                anim.SetTrigger(walk);
                GetComponent<AudioSource>().Stop();
                break;
            case "PistolBullet":
                Damaged(15);
                break;
            case "ShotgunBullet":
                anim.SetTrigger(hurt);  // only the shotgun makes it flinch
                Damaged(40);
                break;
            case "SMGBullet":
                Damaged(10);
                break;
            case "Player":
                if (plyrWeapon.GetSwinging()) {
                    Damaged(10);
                    plyrWeapon.StopSwinging();
                    if (!waitToAttack)
                        anim.SetTrigger(hurt);
                } else {
                    HitPlayer();
                }
                break;
		}
    }

    void Death() {
        if (health < 1) {
            anim.SetTrigger(die);
            AudioSource aS = GetComponent<AudioSource>();
            aS.Stop();
            aS.loop = false;
            aS.clip = dead;
            aS.Play();
            gameObject.layer = 2;
            attacking = false;
            charging = false;
            waveLogic.EnemyKilled();
        }
    }

    bool DontUpdate() {
        RaycastHit hit;
        int layer_mask = ~(1 << gameObject.layer);  // bit shift then invert
        Vector3 fixedPlayerPos = new Vector3(
            playerTrans.position.x,
            playerTrans.position.y + 3f,
            playerTrans.position.z
        );
        if (Physics.Linecast(myTransform.position, fixedPlayerPos, out hit, layer_mask))
            return hit.collider.gameObject.name != "Player";
        else
            return true;
    }

    bool FacingPlayer(bool toCharge) {
        // http://answers.unity3d.com/questions/503934/chow-to-check-if-an-object-is-facing-another.html
        float rotationDiff = Vector3.Dot(
            myTransform.forward,
            (playerTrans.position - myTransform.position).normalized
        );
        return rotationDiff > (toCharge ? 0.99 : 0.8);
    }

    public bool GetAttacking() {
        return attacking;
    }

    void Damaged(int dmg) {
        health -= dmg;
        if (health > 0) // fixes async issue with death animation
            anim.SetTrigger(attacking ? run : walk);
    }

    void HitPlayer() {
        if (!FacingPlayer(false))    // can only hurt if facing the player
            return;
        if (playerCollisions > 0)
            attacking = false;
        ++playerCollisions;
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
