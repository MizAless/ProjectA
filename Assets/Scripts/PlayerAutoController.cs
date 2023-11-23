using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Threading;

public class PlayerAutoController : MonoBehaviour
{
    public float speed = 10f;
    private Animator animator;
    private NavMeshAgent agent;
    private GameObject[] enemies;
    private int currentEnemyIndex = 0;
    private float delayTime = 5f;
    private bool isDelaying = false;
    private Rigidbody rb;
    private bool isJumping = false;
    private bool isGrounded = true;
    private int curJumpCount = 0;
    private int jumpCount = 2;
    private bool fight = false;
    private float curEnemyHealthPoints;


    IEnumerator DelayBeforeDestroy(float delay)
    {
        isDelaying = true;
        yield return new WaitForSeconds(delay);

        //agent.SetDestination(this.transform.position);

        

        // Удаление врага
        Destroy(enemies[currentEnemyIndex]); 

        // Переход к следующему врагу
        currentEnemyIndex++;

        //if (currentEnemyIndex >= enemies.Length)
        //{
        //    currentEnemyIndex = 0;
        //}

        agent.enabled = true;

        //// Бежим к следующему врагу
        agent.SetDestination(enemies[currentEnemyIndex].transform.position);
        isDelaying = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        rb = GetComponent<Rigidbody>();
        enemies = GameObject.FindGameObjectsWithTag("Enemy");
        agent.speed = speed;


        if (enemies.Length > 0)
        {
            agent.SetDestination(enemies[currentEnemyIndex].transform.position);
        }
        Debug.Log(enemies.Length);
    }

    // Update is called once per frame
    void Update()
    {
        if (enemies.Length == 0)
        {
            return;
        }

        if (enemies.Length > 0 && agent.enabled == true)
        {
            var move = agent.SetDestination(enemies[currentEnemyIndex].transform.position);
            if (move)
            {
                animator.SetFloat("speed", 1);
            }
        }

        jump();

        //Debug.Log(enemies.Length);


        if (fight)
        {
            curEnemyHealthPoints = enemies[currentEnemyIndex].GetComponent<EnemyScript>().healtPoints;

            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                animator.SetBool("isAttack", true);
                curEnemyHealthPoints -= 50;
                Debug.Log("Down");
                Debug.Log(curEnemyHealthPoints);
                //enemies[currentEnemyIndex].GetComponent<EnemyScript>().healtPoints = curEnemyHealthPoints;
            }

            if (Input.GetKeyDown(KeyCode.Mouse1))
            {
                animator.SetBool("isAttack", false);
            }

            if (curEnemyHealthPoints <= 0)
            {
                fight = false;
                agent.enabled = true;
                Destroy(enemies[currentEnemyIndex]);
                currentEnemyIndex++;

                agent.SetDestination(enemies[currentEnemyIndex].transform.position);

            }

        }
        else if (Vector3.Distance(transform.position, enemies[currentEnemyIndex].transform.position) < 2f)
        {
            fight = true;
            animator.SetFloat("speed", 0);
            agent.SetDestination(this.transform.position);
            agent.enabled = false;


            // "Удар" врага
            //animator.SetTrigger("Attack");

            //if (!isDelaying)
            //{
            //    StartCoroutine(DelayBeforeDestroy(delayTime));
            //}

            




            // Удаление врага
            //Destroy(enemies[currentEnemyIndex]);

            //Переход к следующему врагу
            //currentEnemyIndex++;

            //if (currentEnemyIndex >= enemies.Length)
            //{
            //    currentEnemyIndex = 0;
            //}



            //Бежим к следующему врагу
            //agent.SetDestination(enemies[currentEnemyIndex].transform.position);


        }

        // Установка скорости анимации
        //animator.SetFloat("Speed", agent.velocity.magnitude / speed);
    }

    void jump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && curJumpCount < jumpCount)
        {
            isJumping = true;
            curJumpCount++;
            //agent.enabled = false;
            rb.AddForce(Vector3.up * speed, ForceMode.VelocityChange);
            Debug.Log("Jump");
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        // Проверяем, касается ли персонаж земли
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            //agent.enabled = true;
            isJumping = false;
            curJumpCount = 0;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        // Проверяем, перестал ли персонаж касаться земли
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }

}
