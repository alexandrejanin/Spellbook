using UnityEngine;

public class PlayerCamera : MonoBehaviour {
    private Player player;
    [SerializeField] private float rotationSpeed = 10;
    [SerializeField] private uint minZoom = 2, maxZoom = 15;
    [SerializeField] private Vector3 targetOffset;
    [SerializeField] private float smoothingFactor = 0.05f;

    private uint zoom = 10;

    private float yRotation = 45f, xRotation = 30f;

    private Vector3 lastMousePosition;

    private void Update() {
        if (!player) {
            player = FindAnyObjectByType<Player>();
            return;
        }

        // Update zoom level
        if (Input.GetAxis("Mouse ScrollWheel") > 0) {
            zoom = (uint)Mathf.Max(zoom - 1, minZoom);
        } else if (Input.GetAxis("Mouse ScrollWheel") < 0) {
            zoom = (uint)Mathf.Min(zoom + 1, maxZoom);
        }

        // Update rotation
        if (Input.GetMouseButtonDown(2)) {
            lastMousePosition = Input.mousePosition;
        } else if (Input.GetMouseButton(2)) {
            yRotation += rotationSpeed * (Input.mousePosition - lastMousePosition).x * Time.deltaTime;
            xRotation -= rotationSpeed * (Input.mousePosition - lastMousePosition).y * Time.deltaTime;
            lastMousePosition = Input.mousePosition;
        }

        xRotation = Mathf.Clamp(xRotation, 0f, 89f);

        // Update position
        var offset = Quaternion.AngleAxis(yRotation, Vector3.up) * Quaternion.AngleAxis(xRotation, Vector3.right) * Vector3.back * zoom;
        var targetPos = player.IdealPosition + targetOffset;

        if (Time.time < 1f) {
            transform.position = targetPos + offset;
            transform.LookAt(targetPos);
        } else {
            transform.position = Vector3.Lerp(transform.position, targetPos + offset, smoothingFactor);
            transform.LookAt(player.transform.position + targetOffset);
        }
    }
}
