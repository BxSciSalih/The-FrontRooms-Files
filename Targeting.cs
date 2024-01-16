using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class Targeting : MonoBehaviour {
    public Transform target;
    public Transform target2;
    public float chaseRadius = 10f;
    public int maxHealth = 100;
    private int currentHealth;
    public GameObject spawnPrefab;
    private NavMeshAgent agent;

    void Start() {
        agent = GetComponent<NavMeshAgent>();
        currentHealth = maxHealth;
    }

    void Update() {
        SetTarget();
    }

    void SetTarget() {
        if (target != null) {
            float distanceToTarget = Vector3.Distance(transform.position, target.position);

            if (distanceToTarget <= chaseRadius) {
                agent.SetDestination(target.position);
                agent.speed = 0.8f;
            }
            else {
                agent.SetDestination(target2.position);
                agent.speed = 0.25f;
            }
        }
    }

    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("PlayerProjectile")) {
            TakeDamage();
        }
        if (collision.gameObject.CompareTag("Finish")) {
            SceneManager.LoadSceneAsync(3);
        }
    }

    void TakeDamage() {
        currentHealth -= 10;

        if (currentHealth <= 0) {
            DespawnAndSpawn();
        }
    }

    void DespawnAndSpawn() {
        Instantiate(spawnPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
}
