using UnityEngine;
public class ProjectileScript : MonoBehaviour {
    public float projectileLifetime = 2f;

    void Start() {
        Destroy(gameObject, projectileLifetime);
    }

    void OnCollisionEnter(Collision collision) {
        Destroy(gameObject);
    }
}

