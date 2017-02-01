using UnityEngine;
using System.Collections;

public class LizardEnemy : MonoBehaviour, IEnemy {

    bool walking = true;
    bool attacking = true;

    public int health = 100;
    int scream;
	int basicAttack;
	int hurt;
	int walk;
	int die;
    const float WALKSPEED = 8;
    const float MAXVELOCITY = 4;
    const float TURNSPEED = 2;

    public Animator anim;
    Transform myTransform;

    void Awake () {
		anim = GetComponent<Animator>();
		scream = Animator.StringToHash("Scream");
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
        }
    }

    void OnCollisionEnter(Collision col) {
        if (health < 1)
            return;
        string tag = col.gameObject.tag;

        GetHurt(tag);

        if (health < 1) {
            anim.SetTrigger(die);
            gameObject.layer = 2;
            walking = false;
            attacking = false;
        }
    }

    // http://answers.unity3d.com/questions/26177/how-to-create-a-basic-follow-ai.html
    void AddMovement() {
        Vector3 velocity = GetComponent<Rigidbody>().velocity;
        Vector3 forward = myTransform.forward;
        if (velocity.x > MAXVELOCITY) {
            velocity.x = MAXVELOCITY;
            forward.x = 0;
        }
        if (velocity.y > MAXVELOCITY) {
            velocity.y = MAXVELOCITY;
            forward.y = 0;
        }
        if (velocity.z > MAXVELOCITY) {
            velocity.z = MAXVELOCITY;
            forward.z = 0;
        }
        //GetComponent<Rigidbody>().AddForce(forward * walkSpeed);
        //GetComponent<Rigidbody>().velocity = velocity;
        myTransform.position += myTransform.forward * WALKSPEED * Time.deltaTime;
    }

    void ChasePlayer() {
        Vector3 playerPos = GameObject.FindWithTag("Player").transform.position;
        myTransform.rotation = Quaternion.Slerp(
            myTransform.rotation,
            Quaternion.LookRotation(playerPos - myTransform.position),
            Time.deltaTime * TURNSPEED
        );
        AddMovement();
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
