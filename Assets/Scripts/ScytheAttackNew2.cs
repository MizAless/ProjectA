using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

public class Attack
{
    public float Duration { get; set; }
    public string BoolName { get; set; }
    public Attack NextAttack { get; set; }
    public Attack NextPauseAttack { get; set; }

    public Attack(float duration, string boolName)
    {
        Duration = duration;
        BoolName = boolName;
        NextAttack = null;
        NextPauseAttack = null;
    }

    public Attack(float duration, string boolName, Attack nextAttack, Attack nextPauseAttack)
    {
        Duration = duration;
        BoolName = boolName;
        NextAttack = nextAttack;
        NextPauseAttack = nextPauseAttack;
    }

}

public class AttackCombo
{
    private static string[] STRNBRS = { "First", "Second", "Third", "Fourth", "Fifth" };
    public Attack InitialAttack { get; set; }
    public Attack CurrentAttack { get; set; }

    public AttackCombo(List<float> durations, List<float> pauseAttackDuration, int pauseAttackParentIndex)
    {
        if (durations.Count == 0)
        {
            throw new ArgumentException("The list of durations cannot be empty.");
        }

        InitialAttack = new Attack(durations[0], "Is" + STRNBRS[0] + "Attack");
        Attack previousAttack = InitialAttack;

        for (int i = 1; i < durations.Count; i++)
        {
            Attack newAttack = new Attack(durations[i], "Is" + STRNBRS[i] + "Attack");
            previousAttack.NextAttack = newAttack;

            if (i == pauseAttackParentIndex + 1)
            {
                Attack nextPauseAttack = new Attack(pauseAttackDuration[0], "Is" + STRNBRS[0] + "PauseAttack");
                previousAttack.NextPauseAttack = nextPauseAttack;
                previousAttack = nextPauseAttack;
                for (int j = 1; j < pauseAttackDuration.Count; j++)
                {
                    nextPauseAttack = new Attack(pauseAttackDuration[j], "Is" + STRNBRS[j] + "PauseAttack");
                    previousAttack.NextAttack = nextPauseAttack;
                    previousAttack = nextPauseAttack;
                }
            }

            previousAttack = newAttack;
        }

        CurrentAttack = InitialAttack;
    }

    public void NextAttack()
    {
        if (CurrentAttack.NextAttack == null)
        {
            CurrentAttack = InitialAttack;
        }
        else if (CurrentAttack != null)
        {
            CurrentAttack = CurrentAttack.NextAttack;
        }
    }

    public void NextPauseAttack()
    {
        if (CurrentAttack != null && CurrentAttack.NextPauseAttack != null)
        {
            CurrentAttack = CurrentAttack.NextPauseAttack;
        }
    }

    public void ResetCombo()
    {
        CurrentAttack = InitialAttack;
    }

    public void PrintCombo(Attack initAttack, int i = 0)
    {
        Debug.Log(i.ToString() + " " +  initAttack.Duration.ToString());
        if (initAttack.NextPauseAttack != null)
        {
            PrintCombo(initAttack.NextPauseAttack, ++i);
        }
        if (initAttack.NextAttack != null)
        {
            PrintCombo(initAttack.NextAttack, ++i);
        }
        
    }

    public float GetCurAttack()
    {
        return CurrentAttack.Duration;
    }

    public bool NextIsPauseAttack()
    {
        return CurrentAttack.NextPauseAttack != null;
    }
    public bool NextIsRegularAttack()
    {
        return CurrentAttack.NextAttack != null;
    }

    public string GetCurBoolName()
    {
        return CurrentAttack.BoolName;
    }
}

public class ScytheAttackNew2 : MonoBehaviour
{
    private bool isAttack = false; // Переменная для управления анимацией атаки
    private bool isSpecialAttack = false;
    //private bool isPauseAttack = false;
    private bool nextAttack = false;
    private bool nextPauseAttack = false;
    //private float comboBDuration = 0.8f;
    //private float comboDuration = 0.6f; // Длительность комбо в секундах
    //private float cuurentComboDuration = 0.6f; // Длительность комбо в секундах
    private int comboCounter = 0;
    //private bool isJumping = false;
    //private bool isGrounded = true;
    private Rigidbody rb;
    private AttackCombo ComboA;

    public float accelerationSpeed = 5f; // Скорость ускорения
    public float decelerationSpeed = 10f; // Скорость замедления
    private float currentSpeed = 0f; // Текущая скорость
    private float progress = 0f; // Прогресс ускорения/замедления (от 0 до 1)


    private Animator animator;
    private float comboTimer = 0f; // Таймер для отслеживания длительности комбо

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        float[] inputAttackDuration = { 1.033f, 1.333f, 1.233f };
        float[] pauseAttackDuration = { 1.200f };
        ComboA = new AttackCombo(new List<float>(inputAttackDuration), new List<float>(pauseAttackDuration), 1);
        ComboA.PrintCombo(ComboA.InitialAttack);

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (!isAttack)
            {
                StartCombo();
            }
            else if (isAttack && comboTimer >= ComboA.GetCurAttack() - 0.2f && !nextAttack)
            {
                nextPauseAttack = true;
            }
            else if (isAttack && comboTimer >= ComboA.GetCurAttack() - 0.5f)
            {
                nextAttack = true;
            }

        }
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            StartSpecAttack();

        }
        if (isAttack)
        {
            comboTimer += Time.deltaTime;

            if (comboTimer < ComboA.GetCurAttack() - 0.5f)
            {
                nextAttack = false;
                nextPauseAttack = false;
            }
            //else if (comboTimer > ComboA.GetCurAttack() && ComboA.NextIsPauseAttack() && nextPauseAttack)
            //{
            //    print("NextPauseAttack");
            //    NextPauseAttack();
            //}
            //else if (comboTimer > ComboA.GetCurAttack() && !ComboA.NextIsPauseAttack() && nextPauseAttack)
            //{
            //    print("NextComboAttack");
            //    NextComboAttack();
            //}
            //else if (comboTimer > ComboA.GetCurAttack() && nextAttack)
            //{
            //    print("NextComboAttack");
            //    NextComboAttack();
            //}
            //else if (comboTimer > ComboA.GetCurAttack())
            //{
            //    print("EndCombo");
            //    EndCombo();
            //}

            if (comboTimer > 1.067f)
            {
                isAttack = false;
                isSpecialAttack = false;
                animator.SetBool("IsSpecialAttack", false); // Устанавливаем переменную аниматора в true
                animator.bodyPosition = Vector3.zero;
            }

        }
        if (isSpecialAttack)
        {
            //transform.Translate(Vector3.forward * 15 * Time.deltaTime);

            // Увеличиваем прогресс ускорения/замедления
            //progress += accelerationSpeed * Time.deltaTime;

            //// Применяем кривую анимации для плавного изменения скорости
            //float curveValue = Mathf.Clamp01(animationCurve.Evaluate(progress));

            //// Вычисляем текущую скорость на основе прогресса и кривой анимации
            //currentSpeed = Mathf.Lerp(0f, 15f, curveValue);

            //// Перемещаем персонаж вперед с текущей скоростью
            //transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);

            //// Если прогресс достиг 1, начинаем замедление
            //if (progress >= 1f)
            //{
            //    // Увеличиваем прогресс замедления
            //    progress += decelerationSpeed * Time.deltaTime;

            //    // Применяем кривую анимации для плавного изменения скорости
            //    curveValue = Mathf.Clamp01(animationCurve.Evaluate(progress));

            //    // Вычисляем текущую скорость на основе прогресса и кривой анимации
            //    currentSpeed = Mathf.Lerp(15f, 0f, curveValue);

            //    // Перемещаем персонаж вперед с текущей скоростью
            //    transform.Translate(Vector3.forward * currentSpeed * Time.deltaTime);

            //}
        }
    }
    private void StartCombo()
    {
        isAttack = true;
        comboTimer = 0f;
        animator.SetBool(ComboA.GetCurBoolName(), true); // Устанавливаем переменную аниматора в true
        comboCounter++;
    }

    private void StartSpecAttack()
    {
        isAttack = true;
        isSpecialAttack = true;
        comboTimer = 0f;
        animator.SetBool("IsSpecialAttack", true); // Устанавливаем переменную аниматора в true
        comboCounter++;
    }

    private void NextComboAttack()
    {
        comboTimer = 0f;
        animator.SetBool(ComboA.GetCurBoolName(), false); // Устанавливаем переменную аниматора в false
        ComboA.NextAttack();
        animator.SetBool(ComboA.GetCurBoolName(), true); // Устанавливаем переменную аниматора в true
        comboCounter++;
        nextAttack = false;
    }

    private void NextPauseAttack()
    {
        comboTimer = 0f;
        animator.SetBool(ComboA.GetCurBoolName(), false); // Устанавливаем переменную аниматора в false
        ComboA.NextPauseAttack();
        animator.SetBool(ComboA.GetCurBoolName(), true); // Устанавливаем переменную аниматора в true
        comboCounter++;
        nextAttack = false;
        nextPauseAttack = false;
    }


    private void EndCombo()
    {
        isAttack = false;
        comboTimer = 0f;
        animator.SetBool(ComboA.GetCurBoolName(), false); // Устанавливаем переменную аниматора в true
        comboCounter = 0;
        nextAttack = false;
        ComboA.ResetCombo();
    }
}
    