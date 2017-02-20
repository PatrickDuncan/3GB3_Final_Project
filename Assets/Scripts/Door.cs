using UnityEngine;
using UnityEngine.UI;

public class Door : MonoBehaviour {

    bool colliding = false;
	bool keyPressed = false;
	bool open = false;

    Transform myTransform;
    Quaternion closeRotation;
    Quaternion openRotation;
    Vector3 closePosition;
    Vector3 openPosition;

	void Awake() {
        myTransform = transform;
        closePosition = myTransform.position;
        closeRotation = myTransform.rotation;
        Vector3 oriRot = closeRotation.eulerAngles;
        openRotation = Quaternion.Euler(oriRot.x, -120, oriRot.z);
        openPosition = new Vector3(closePosition.x + 1.02f, closePosition.y, closePosition.z + 1.207f);
    }

	void OnTriggerEnter(Collider col) {
    	if (col.gameObject.tag == "Player") {
            colliding = true;
        }
	}

    void OnTriggerExit(Collider col) {
        if (col.gameObject.tag == "Player") {
            colliding = false;
        }
    }

    void Update() {
        if (open) {
            Vector3 rot = Quaternion.Slerp(
                myTransform.rotation,
                openRotation,
                Time.deltaTime * 2
            ).eulerAngles;
            myTransform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z);
            myTransform.position = Vector3.Slerp(
                myTransform.position,
                openPosition,
                Time.deltaTime * 2
            );
        } else {
            Vector3 rot = Quaternion.Slerp(
                myTransform.rotation,
                closeRotation,
                Time.deltaTime * 2
            ).eulerAngles;
            myTransform.rotation = Quaternion.Euler(rot.x, rot.y, rot.z);
            myTransform.position = Vector3.Slerp(
                myTransform.position,
                closePosition,
                Time.deltaTime * 2
            );
        }
        // Open the door
        if (colliding && Input.GetKeyDown(KeyCode.E)) {
            open = !open;
        }
    }
}
