using UnityEngine;

public class Billboard : MonoBehaviour {
    private void Update() {
        // transform.forward = transform.position - Camera.main.transform.position;
        transform.forward = Camera.main.transform.forward;
    }
}
