using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] Transform target;   // Player

    [Header("Offset")]
    [SerializeField] Vector3 offset = new Vector3(0, 5, -7);

    [Header("Smooth")]
    [SerializeField] float smoothSpeed = 5f;

    private void Awake()
    {
        if(target != null)
            offset = transform.position - target.position;
    }

    void LateUpdate()
    {
        if (target == null) return;

        // Vị trí mong muốn của camera
        Vector3 desiredPosition = target.position + offset;

        // Lerp cho mượt
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        transform.position = smoothedPosition;

        // Nếu muốn camera luôn nhìn vào player
        //transform.LookAt(target);
    }
}
