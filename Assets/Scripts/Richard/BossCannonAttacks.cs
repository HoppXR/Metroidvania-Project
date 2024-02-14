using UnityEngine;

public class BossCannonAttack : MonoBehaviour
{
    public GameObject prefab; 
    public float rotationSpeed = 5f;
    public float shootingSpeed = 10f;

    void Update()
    {
        RotateTowardsPlayer();
    }

    void RotateTowardsPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            Vector3 directionToPlayer = player.transform.position - transform.position;
            float angle = Mathf.Atan2(directionToPlayer.y, directionToPlayer.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        }
    }

    public void ShootCannon()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {

            GameObject bullet = Instantiate(prefab, transform.position, Quaternion.identity);
            Vector3 directionToPlayer = (player.transform.position - bullet.transform.position).normalized;
            Rigidbody2D bulletRb = bullet.GetComponent<Rigidbody2D>();
            bulletRb.velocity = directionToPlayer * shootingSpeed;
        }
    }
}