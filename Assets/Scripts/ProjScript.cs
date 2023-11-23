using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjScript : MonoBehaviour
{
    private float projSpeed;
    private int damage;
    private Vector3 direction;
    private GameObject target;


    void Update()
    {
        transform.position += direction * projSpeed * Time.deltaTime;
    }

    public void Moveing(GameObject target, Vector3 direction, float projSpeed, int damage)
    {
        this.direction = direction - new Vector3(0f, direction.y, 0f);
        this.projSpeed = projSpeed;
        this.damage = damage;
        this.target = target;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            var player = GameObject.FindGameObjectWithTag("Player");

            var playerHealth = player.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(damage);
            Destroy(gameObject);

        }
    }
}
