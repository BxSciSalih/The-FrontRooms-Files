using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class PlayerController : MonoBehaviour {
    private CharacterController characterController;
    public float speed = 1f;
    public float rotationSpeed = 500f;
    public GameObject projectilePrefab; // Drag your projectile prefab here in the Inspector
    public TextMeshProUGUI newPower;

    void Start() {
        characterController = GetComponent<CharacterController>();
    }

    void Update() {
        HandleMovement();
        HandleShooting();

        float mouseX = Input.GetAxis("Mouse X");
        transform.Rotate(Vector3.up * mouseX * rotationSpeed * Time.deltaTime);
    }

    void HandleMovement() {
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(horizontal, 0f, vertical);
        movement.Normalize();

        movement = Camera.main.transform.TransformDirection(movement);
        movement.y = 0f;

        characterController.Move(movement * speed * Time.deltaTime);
    }
    float fireCooldown = 3f; // Adjust this value to set the cooldown time
    float lastFireTime;
    float spawnDistance = 0.3f;
    float projectileSpeed = 10f;

    void HandleShooting() {
        if (Input.GetMouseButtonDown(0) && Time.time - lastFireTime > fireCooldown) {
            // Calculate the spawn position closer to the player
            Vector3 spawnPosition = transform.position + transform.forward * spawnDistance;

            // Instantiate the projectile prefab at the calculated position and rotation
            GameObject projectile = Instantiate(projectilePrefab, spawnPosition, transform.rotation);

            // Add a Rigidbody component to the projectile
            Rigidbody projectileRb = projectile.GetComponent<Rigidbody>();

            // Add forward velocity to the projectile (adjust the speed as needed)
            projectileRb.velocity = transform.forward * projectileSpeed;

            // Update the last fire time to the current time
            lastFireTime = Time.time;
        }
    }

    Boolean teared = false;
    Boolean ranged = false;
    Boolean thorned = false;
    void OnCollisionEnter(Collision collision) {
        if (collision.gameObject.CompareTag("Box")) {
            int power = Random.Range(0, 4);
            
            if (power == 0) {
                Speed();
            }
            if (power == 1 && teared == false) {
                Tears();
                teared = true;
            }
            else {
                power += 1;
            }
            if (power == 2 && ranged == false) {
                Range();
                ranged = true;
            }
            else {
                power += 1;
            }
            if (power == 3 && thorned == false) {
                Thorns();
            }
            else {
                Speed();
            }
        }

        if (collision.gameObject.CompareTag("Finish")) {
            SceneManager.LoadSceneAsync(2);
        }

        if (collision.gameObject.CompareTag("Enemy") && thorned == true) {
            thorned = false;
        }

        if (collision.gameObject.CompareTag("Enemy") && thorned == false) {
            SceneManager.LoadSceneAsync(3);
        }
    }

    void Speed() {
        speed = speed * 2;
        newPower.text = "Speed Up! You are twice as fast now!";
    }

    void Tears() {
        fireCooldown = fireCooldown / 2;
        newPower.text = "Shot Speed Up! You can now fire as fast as you can click!";
    }

    void Range() {
        projectileSpeed = projectileSpeed * 2;
        newPower.text = "Range Up! Your shots go faster and farther!";
    }
    
    void Thorns() {
        thorned = true;
        newPower.text = "You got armor! You can take 1 more hit before losing!";
    }

}