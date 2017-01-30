using UnityEngine;

public class LizardEnemy : MonoBehaviour, IEnemy {

    bool walking = true;
    bool attacking = true;

    public int health = 100;
    public float walkSpeed;
    int scream;
	int basicAttack;
	int hurt;
	int walk;
	int die;
    float maxVelocity = 5;

    public Animator anim;

    void Awake () {
		anim = GetComponent<Animator>();
		scream = Animator.StringToHash("Scream");
		basicAttack = Animator.StringToHash("Basic Attack");
		hurt = Animator.StringToHash("Get Hit");
		walk = Animator.StringToHash("Walk");
		die = Animator.StringToHash("Die");
        anim.SetTrigger(walk);
	}

    void FixedUpdate() {
        // Move
        if (walking) {
            GetComponent<Rigidbody>().AddForce(transform.forward * walkSpeed);
        }
        RegulateVelocity();
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

    void RegulateVelocity() {
        Vector3 velocity = GetComponent<Rigidbody>().velocity;
        if (velocity.x > 5)
            velocity.x = 5;
        if (velocity.y > 5)
            velocity.y = 5;
        if (velocity.z > 5)
            velocity.z = 5;
        GetComponent<Rigidbody>().velocity = velocity;
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
