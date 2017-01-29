using UnityEngine;

public class LizardEnemy : MonoBehaviour {

    public int health = 100;
    private int scream;
	private int basicAttack;
	private int hurt;
	private int walk;
	private int die;

    public Animator anim;

    private void Awake () {
		anim = GetComponent<Animator>();
		scream = Animator.StringToHash("Scream");
		basicAttack = Animator.StringToHash("Basic Attack");
		hurt = Animator.StringToHash("Get Hit");
		walk = Animator.StringToHash("Walk");
		die = Animator.StringToHash("Die");
	}

    private void OnCollisionEnter(Collision col) {
        if (health < 1)
            return;
        string tag = col.gameObject.tag;
		switch(tag) {
            case "PistolBullet":
                health -= 20;
                anim.SetTrigger(hurt);
                break;
		}
        if (health < 1) {
            anim.SetTrigger(die);
            gameObject.layer = 2;
        }
    }
}
