using UnityEngine;

public class Boomerang : MonoBehaviour
{
    public float speed = 10f;

    private Transform player;
    private Vector3 direction;
    private float maxDistance;
    private Vector3 startPos;
    private bool isReturning = false;

    public void Init(Transform playerTf, Vector3 dir, float force)
    {
        player = playerTf;
        direction = dir.normalized;
        maxDistance = force;   // dùng force làm tầm bay
        startPos = transform.position;
    }

    void Update()
    {
        if (!isReturning)
        {
            transform.position += direction * speed * Time.deltaTime;

            float distance = Vector3.Distance(startPos, transform.position);
            if (distance >= maxDistance)
            {
                isReturning = true;
            }
        }
        else
        {
            Vector3 returnDir = (player.position - transform.position).normalized;
            transform.position += returnDir * speed * Time.deltaTime;

            if (Vector3.Distance(transform.position, player.position) < 0.5f)
            {
                Destroy(gameObject);
            }
        }

        // xoay boomerang cho đẹp
        transform.Rotate(0, 720 * Time.deltaTime, 0);
    }
}
