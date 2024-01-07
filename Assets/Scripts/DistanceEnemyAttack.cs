using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceEnemyAttack : MonoBehaviour
{

    private float projAttackCooldown = 3f; // Расстояние, на котором враг начинает атаку
    private float projAttackDistance = 6f; // Расстояние, на котором враг начинает атаку
    public int Damage = 10; // Урон от атаки
    public GameObject projPrefab;

    private GameObject player; // Ссылка на игрока
    private bool isProjAttack = false;
    private float timer = 0;
    private Animator animator;
    private float bulletSpeed = 3f;



    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // Найти игрока по тегу "Player"
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(player.transform.position.x, player.transform.position.z));

        if (distance <= projAttackDistance && !isProjAttack)
        {
            ProjAttack();
        }


        if (isProjAttack)
        {
            timer += Time.deltaTime;

            if (timer > projAttackCooldown)
            {
                EndAttack();
            }

        }

    }

    private void ProjAttack()
    {
        isProjAttack = true;
        GameObject proj = Instantiate(projPrefab, transform.position + new Vector3(0f, 1.5f, 0f), Quaternion.identity);
        Vector3 direction = (player.transform.position - transform.position + new Vector3(0f, 1.5f, 0f)).normalized;
        var b = proj.GetComponent<ProjScript>();
        b.Moveing(player, direction, bulletSpeed, Damage);
    }

    private void EndAttack()
    {
        timer = 0;
        isProjAttack = false;
        animator.SetBool("IsAttack", false);
    }


}
