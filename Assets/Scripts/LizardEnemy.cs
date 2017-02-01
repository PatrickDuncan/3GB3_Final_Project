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
            AddMovement();
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
        Vector3 newPos = myTransform.forward * WALKSPEED * Time.deltaTime;
        myTransform.position += new Vector3(newPos.x, 0, newPos.z);
    }

    void ChasePlayer() {
        Vector3 playerPos = GameObject.FindWithTag("Player").transform.position;
        myTransform.rotation = Quaternion.Slerp(
            myTransform.rotation,
            Quaternion.LookRotation(playerPos - myTransform.position),
            Time.deltaTime * TURNSPEED
        );
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
