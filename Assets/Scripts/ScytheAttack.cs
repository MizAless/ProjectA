using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


//public class AttackCombo
//{
//    private List<float> AttackDuration; // Лист времени анимации очередной атаки
//    private int AttackCount; // Индекс нынешней атаки
//    private int AttackNumber; // Индекс нынешней атаки
//    public AttackCombo(List<float> attackDuration)
//    {
//        AttackDuration = attackDuration;
//        AttackCount = AttackDuration.Count;
//        AttackNumber = 0;
//    }

//    public void NextAttack()
//    {
//        AttackNumber++;
//    }


//}

public class ScytheAttack : MonoBehaviour
{
    private bool isAttack = false; // Переменная для управления анимацией атаки
    private bool isSpecialAttack = false;
    private bool isPauseAttack = false;
    private bool nextAttack = false;
    private float comboBDuration = 0.8f;
    private float comboDuration = 0.6f; // Длительность комбо в секундах
    private float cuurentComboDuration = 0.6f; // Длительность комбо в секундах
    private int comboCounter = 0;
    private bool isJumping = false;
    private bool isGrounded = true;
    private Rigidbody rb;
    //private AttackCombo ComboA;


    private Animator animator;
    private float comboTimer = 0f; // Таймер для отслеживания длительности комбо

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
       
        //float[] inputAttackDuration = { 1.033f, 1.333f, 1.233f };
        //ComboA = new AttackCombo(new List<float>(inputAttackDuration));

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {

            if (!isAttack)
            {
                StartCombo();
            }
            else if (comboTimer % comboDuration > 0.5f && comboCounter < 2)
            {
                if (!nextAttack)
                {
                    NextComboPhase();
                }
            }
            else if (comboTimer > comboDuration * 2 - 0.3 && comboTimer < comboDuration * 2)
            {
                StartPauseAttack();
            }
            else if (comboTimer > comboDuration * 2 - 0.6 && comboTimer < comboDuration * 2 - 0.3)
            {
                NextComboPhase();
            }

        }


        if (isAttack) // Если атака активна, проверяем длительность комбо
        {
            comboTimer += Time.deltaTime;

            if (comboTimer % comboDuration < 0.5f)
            {
                nextAttack = false;
            }

            //if (comboCounter < 2)
            {
                if (comboTimer >= cuurentComboDuration)
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

        //if (isSpecialAttack) // Если атака активна, проверяем длительность комбо
        //{
        //    comboTimer += Time.deltaTime;

        //    if (Math.Abs(comboTimer - 0.1f) < 0.010 && !isJumping)
        //    {
        //        rb.AddForce(Vector3.up * 10, ForceMode.Impulse);
        //        isJumping = true;
        //    }

        //    if (comboTimer % 1.2f < 0.5f)
        //    {
        //        nextAttack = false;
        //    }

        //    if (comboTimer >= comboDuration)
        //    {
        //        EndSpecialAttack();
        //    }

        //}

        if (isPauseAttack)
        {
            comboTimer += Time.deltaTime;

            if (comboTimer % comboDuration < 0.5f)
            {
                nextAttack = false;
            }

            if (comboTimer >= comboBDuration)
            {
                EndPauseAttack();
            }
        }

        //if (Input.GetKeyDown(KeyCode.Mouse1))
        //{

        //    EndCombo();
        //    Debug.Log(rb);
        //    isSpecialAttack = true;
        //    comboTimer = 0f;
        //    animator.SetBool("IsSpecialAttack", true); // Устанавливаем переменную аниматора в true

        //}



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
        cuurentComboDuration += comboDuration;
    }

    private void EndComboPhase()
    {
        animator.SetBool("IsAttack", false); // Устанавливаем переменную аниматора в false
    }

    private void EndCombo()
    {
        isAttack = false;
        animator.SetBool("IsAttack", false);
        cuurentComboDuration = comboDuration;
        nextAttack = false;
        comboCounter = 0;
    }

    private void EndSpecialAttack()
    {
        isSpecialAttack = false;
        animator.SetBool("IsSpecialAttack", false);
        cuurentComboDuration = comboDuration;
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
        cuurentComboDuration = comboDuration;
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
