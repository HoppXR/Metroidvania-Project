using UnityEngine;

public class ParticleMoveToPoint : MonoBehaviour
{
    public Transform targetPoint; // The point where particles will move towards
    public float speed = 5f; // Speed at which particles move towards the target point

    ParticleSystem particles;

    void Start()
    {
        particles = GetComponent<ParticleSystem>();
    }

    void Update()
    {
        ParticleSystem.Particle[] allParticles = new ParticleSystem.Particle[particles.main.maxParticles];
        int numParticlesAlive = particles.GetParticles(allParticles);

        Vector3 targetPosition = targetPoint.position;

        for (int i = 0; i < numParticlesAlive; i++)
        {
            Vector3 direction = (targetPosition - allParticles[i].position).normalized;
            allParticles[i].position += direction * speed * Time.deltaTime;
        }

        particles.SetParticles(allParticles, numParticlesAlive);
    }
}