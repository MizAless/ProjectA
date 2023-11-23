using System.Collections;
using System.Collections.Generic;
using System.Net.Http.Headers;
using System.Threading;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class EnemyAttackScript : MonoBehaviour
{
    private float attackDistance = 3f; // ����������, �� ������� ���� �������� �����
    private float attackCooldown = 1.5f; // ����������, �� ������� ���� �������� �����
    private float projAttackCooldown = 3f; // ����������, �� ������� ���� �������� �����
    private float projAttackDistance = 6f; // ����������, �� ������� ���� �������� �����
    public int Damage = 10; // ���� �� �����
    public GameObject projPrefab;

    private GameObject player; // ������ �� ������
    private bool isAttack = false;
    private bool isProjAttack = false;
    private float timer = 0;
    private Animator animator;
    private float bulletSpeed = 3f;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // ����� ������ �� ���� "Player"
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // ��������� ���������� ����� ������ � �������
        float distance = Vector2.Distance(new Vector2(transform.position.x , transform.position.z), new Vector2(player.transform.position.x, player.transform.position.z));

        //print(distance);

        // ���� ����� ��������� �� ������ ����������, ���������
        if (distance <= attackDistance && !isAttack && !isProjAttack)
        {
            Attack();
        }
        else if (distance <= projAttackDistance && !isAttack && !isProjAttack)
        {
            ProjAttack();
        }

        if (isAttack)
        {
            timer += Time.deltaTime;

            if (timer > attackCooldown) 
            {
                EndAttack();
            }

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

    private void Attack()
    {
        // ��������� ���� � ������
        //PlayerHealth playerHealth = player.GetComponent<PlayerHealth>();
        //if (playerHealth != null)
        //{
        //    playerHealth.TakeDamage(Damage);
        //}
        //print("!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
        isAttack = true;
        animator.SetBool("IsAttack", true);
        
    }

    private void EndAttack()
    {
        timer = 0;
        isAttack = false;
        isProjAttack = false;
        animator.SetBool("IsAttack", false);
    }

    private void ProjAttack()
    {
        isProjAttack = true;
        GameObject proj = Instantiate(projPrefab, transform.position + new Vector3(0f, 0.1f, 0f), Quaternion.identity);
        Vector3 direction = (player.transform.position - transform.position + new Vector3(0f, 0.1f, 0f)).normalized;
        var b = proj.GetComponent<ProjScript>();
        b.Moveing(player, direction, bulletSpeed, Damage);
    }

    


}
