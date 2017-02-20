using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour {

    bool colliding = false;
	bool keyPressed = false;
	bool open = false;

    Transform myTransform;
    Quaternion closeRotation;
    Quaternion openRotation;

	void Awake() {
        myTransform = transform;
        closeRotation = myTransform.rotation;
        Vector3 oriRot = closeRotation.eulerAngles;
        openRotation = Quaternion.Euler(oriRot.x, -120, oriRot.z);
    }

	void OnTriggerEnter(Collider col) {
    	if (col.gameObject.tag == "Player")
            colliding = true;
	}

    void OnTriggerExit(Collider col) {
        if (col.gameObject.tag == "Player")
            colliding = false;
    }

    void Update() {
        if (open) {
            Vector3 rot = Quaternion.Slerp(
                myTransform.rotation,
                openRotation,
                Time.deltaTime * 2
            ).eulerAngles;
            myTransform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z);
        } else {
            Vector3 rot = Quaternion.Slerp(
                myTransform.rotation,
                closeRotation,
                Time.deltaTime * 2
            ).eulerAngles;
            myTransform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z);
        }
        // Open the door
        if (colliding && Input.GetKeyDown(KeyCode.E)) {
            open = !open;
        }
    }
}
