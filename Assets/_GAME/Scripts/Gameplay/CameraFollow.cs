using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    [SerializeField] Transform target;   // Player

    [Header("Smooth")]
    [SerializeField] float smoothSpeed = 5f;

    [SerializeField] Vector3 _offSet;

    private void OnEnable()
    {
        LevelCtrl.OnPlayerInitAction += SetTarget;  
    }

    private void OnDestroy()
    {
        LevelCtrl.OnPlayerInitAction -= SetTarget;
    }

    void Update()
    {
        if (target == null) return;

        // Vị trí mong muốn của camera
        Vector3 desiredPosition = target.position + _offSet;

        // Lerp cho mượt
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);

        transform.position = smoothedPosition;

        transform.LookAt(target);
    }

    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
        if (target != null)
        {
            if(_offSet != Vector3.zero)
                transform.position = target.position + _offSet;
            else
                _offSet = transform.position - target.position;
        }
    }   
}
