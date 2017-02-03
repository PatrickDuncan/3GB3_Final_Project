using UnityEngine;
using System.Collections;

public class RhinoEnemy : MonoBehaviour, IEnemy {

    //bool walking = true;
    //bool attacking = true;

    int health = 200;
	int hurt;
	int die;
    const float WALKSPEED = 20f;
    const float MAXVELOCITY = 10f;
    const float TURNSPEED = 1.2f;

    Animator anim;
    //Transform myTransform;

    void Awake() {
		anim = GetComponent<Animator>();
		hurt = Animator.StringToHash("Get Hit");
		die = Animator.StringToHash("Die");
    //    myTransform = transform;
	}

    void OnCollisionEnter(Collision col) {
        if (health < 1) return;

        string tag = col.gameObject.tag;
        GetHurt(tag);

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

    void GetHurt(string tag) {
        switch(tag) {
            case "PistolBullet":
                health -= 15;
                anim.SetTrigger(hurt);
                break;
		}
    }
}
