using UnityEngine;
using System;

public class SwordAttack : MonoBehaviour
{
    private bool isAttack = false; // Переменная для управления анимацией атаки
    private bool isSpecialAttack = false;
    private bool isPauseAttack = false;
    private bool nextAttack = false;
    private float comboBDuration = 1.8f;
    private float comboDuration = 1.177f; // Длительность комбо в секундах
    private int comboCounter = 0;
    private bool isJumping = false;
    private bool isGrounded = true;
    private Rigidbody rb;
    private AttackCombo ComboA;


    private Animator animator;
    private float comboTimer = 0f; // Таймер для отслеживания длительности комбо

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {

            if (!isAttack)
            {
                StartCombo();
            }
            else if (comboTimer % 1.177f > 0.5f && comboCounter < 2)
            {
                if (!nextAttack)
                {
                    NextComboPhase();
                }
            } 
            else if(comboTimer > 1.177f * 2 - 0.3 && comboTimer < 1.177f * 2)
            {
                StartPauseAttack();
            }
            else if (comboTimer > 1.177f * 2 - 0.6 && comboTimer < 1.177f * 2 - 0.3)
            {
                NextComboPhase();
            }

        }


        if (isAttack) // Если атака активна, проверяем длительность комбо
        {
            comboTimer += Time.deltaTime;

            if (comboTimer % 1.177f < 0.5f)
            {
                nextAttack = false;
            }

            //if (comboCounter < 2)
            {
                if (comboTimer >= comboDuration)
                {
                    EndCombo();
                }
            } 
            //else 
            //{
            //    if (comboTimer >= comboDuration + comboBDuration)
            //    {
            //        EndCombo();
            //    }
            //}

            

        }

        if (isSpecialAttack) // Если атака активна, проверяем длительность комбо
        {
            comboTimer += Time.deltaTime;

            if (Math.Abs( comboTimer - 0.1f) < 0.010 && !isJumping)
            {
                rb.AddForce(Vector3.up * 10, ForceMode.Impulse);
                isJumping = true;
            }

            if (comboTimer % 1.2f < 0.5f)
            {
                nextAttack = false;
            }

            if (comboTimer >= comboDuration)
            {
                EndSpecialAttack();
            }

        }

        if (isPauseAttack)
        {
            comboTimer += Time.deltaTime;

            if (comboTimer % 1.2f < 0.5f)
            {
                nextAttack = false;
            }

            if (comboTimer >= comboBDuration)
            {
                EndPauseAttack();
            }
        }

        if (Input.GetKeyDown(KeyCode.Mouse1))
        {

            EndCombo();
            Debug.Log(rb);
            isSpecialAttack = true;
            comboTimer = 0f;
            animator.SetBool("IsSpecialAttack", true); // Устанавливаем переменную аниматора в true

        }



    }

    private void StartCombo()
    {
        isAttack = true;
        comboTimer = 0f;
        animator.SetBool("IsAttack", true); // Устанавливаем переменную аниматора в true
        comboCounter++;
    }

    private void NextComboPhase()
    {
        comboCounter++;
        nextAttack = true;
        comboDuration += 1.177f;
    }

    private void EndComboPhase()
    {
        animator.SetBool("IsAttack", false); // Устанавливаем переменную аниматора в false
    }

    private void EndCombo()
    {
        isAttack = false;
        animator.SetBool("IsAttack", false);
        comboDuration = 1.177f;
        nextAttack = false;
        comboCounter = 0;
    }

    private void EndSpecialAttack()
    {
        isSpecialAttack = false;
        animator.SetBool("IsSpecialAttack", false);
        comboDuration = 1.177f;
        nextAttack = false;
    }

    private void StartPauseAttack()
    {
        EndCombo();
        Debug.Log("12312123");
        isPauseAttack = true;
        comboTimer = 0f;
        nextAttack = false;
        animator.SetBool("IsPauseAttack", true); // Устанавливаем переменную аниматора в true
    }

    private void EndPauseAttack()
    {
        isPauseAttack = false;
        animator.SetBool("IsPauseAttack", false);
        comboDuration = 1.177f;
        nextAttack = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Проверяем, касается ли персонаж земли
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
            isJumping = false;
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