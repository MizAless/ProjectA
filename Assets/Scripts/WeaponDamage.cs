using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    public int damage = 20;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {

            var enemy = GameObject.FindGameObjectWithTag("Enemy");

            var enemyHealth = enemy.GetComponent<EnemyScript>();
            enemyHealth.TakeDamage(damage);
            //Destroy(gameObject);

        }
    }
}
