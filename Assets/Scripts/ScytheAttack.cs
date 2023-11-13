using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using UnityEngine.Rendering.VirtualTexturing;
using static UnityEngine.GraphicsBuffer;
using UnityEngine.UIElements;

public enum AttackType
{
    NormalAttack,
    PauseAttack,
    SpecialAttack
}
public class Attack
{
    public float Duration { get; set; }
    public string BoolName { get; set; }
    public Attack NextAttack { get; set; }
    public Attack NextPauseAttack { get; set; }
    public AttackType Type { get; set; }

    public Attack(float duration, string boolName, AttackType type = AttackType.NormalAttack)
    {
        Duration = duration;
        BoolName = boolName;
        NextAttack = null;
        NextPauseAttack = null;
        Type = type;
    }

    public Attack(float duration, string boolName, Attack nextAttack, Attack nextPauseAttack, AttackType type = AttackType.NormalAttack)
    {
        Duration = duration;
        BoolName = boolName;
        NextAttack = nextAttack;
        NextPauseAttack = nextPauseAttack;
        Type = type;
    }

}

public class AttackCombo
{
    private static string[] STRNBRS = { "First", "Second", "Third", "Fourth", "Fifth" };
    public Attack InitialAttack { get; set; }
    public Attack CurrentAttack { get; set; }
    public Attack SpecialAttack { get; set; }

    public AttackCombo(List<float> durations, List<float> pauseAttackDuration, int pauseAttackParentIndex)
    {
        if (durations.Count == 0)
        {
            throw new ArgumentException("The list of durations cannot be empty.");
        }

        InitialAttack = new Attack(durations[0], "Is" + STRNBRS[0] + "Attack");
        Attack previousAttack = InitialAttack;
        if (pauseAttackParentIndex == 0)
        {
            InitialAttack.NextPauseAttack = new Attack(pauseAttackDuration[0], "Is" + STRNBRS[0] + "PauseAttack");
        }

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

    public AttackCombo(float duration, string boolName, AttackType type)
    {
        if (type == AttackType.SpecialAttack)
        {
            InitialAttack = new Attack(duration, boolName);
            CurrentAttack = InitialAttack;
        }
    }

    public void AddAttack(Attack attack) 
    {
        if (attack.Type == AttackType.SpecialAttack)
        {
            SpecialAttack = attack;
        }
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

    public void NextSpecialAttack()
    {
        if (SpecialAttack != null)
        {
            CurrentAttack = SpecialAttack;
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

    public AttackType GetCurAttacType()
    {
        return CurrentAttack.Type;
    }
}



public class AttackComboManager
{
    public Animator animator;
    public List<AttackCombo> AttackComboList = new List<AttackCombo>();
    public AttackCombo currentAttackCombo;
    
    public AttackComboManager(List<AttackCombo> attackComboList)
    {
        AttackComboList = attackComboList;
        currentAttackCombo = attackComboList[0];
    }

}



public class ScytheAttack : MonoBehaviour
{
    private bool isAttack = false; // Переменная для управления анимацией атаки
    private bool isSpecialAttack = false;
    //private bool isPauseAttack = false;
    private bool nextAttack = false;
    private bool nextPauseAttack = false;
    private bool nextSpecialAttack = false;
    //private float comboBDuration = 0.8f;
    //private float comboDuration = 0.6f; // Длительность комбо в секундах
    //private float cuurentComboDuration = 0.6f; // Длительность комбо в секундах
    private int comboCounter = 0;
    //private bool isJumping = false;
    //private bool isGrounded = true;
    private Rigidbody rb;
    private AttackCombo ComboA;
    private Attack SpecialAttack;
    private AttackComboManager attackComboManager;

    //public float accelerationSpeed = 5f; // Скорость ускорения
    //public float decelerationSpeed = 10f; // Скорость замедления
    //private float currentSpeed = 0f; // Текущая скорость
    //private float progress = 0f; // Прогресс ускорения/замедления (от 0 до 1)

    private Vector3 target;
    private float speed = 2.0f;


    private Animator animator;
    private float comboTimer = 0f; // Таймер для отслеживания длительности комбо

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        float[] inputAttackDuration = { 1.033f, 1.333f, 1.233f };
        float[] pauseAttackDuration = { 1.200f };
        ComboA = new AttackCombo(new List<float>(inputAttackDuration), new List<float>(pauseAttackDuration), 1);
        SpecialAttack = new Attack(1.067f, "IsSpecialAttack", AttackType.SpecialAttack);
        ComboA.AddAttack(SpecialAttack);

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
            if (!isAttack)
            {
                StartCombo(AttackType.SpecialAttack);
            }
            else
            {
                nextSpecialAttack = true;
            }
        }
        if (isAttack)
        {
            comboTimer += Time.deltaTime;

            if (comboTimer < ComboA.GetCurAttack() - 0.5f)
            {
                nextAttack = false;
                nextPauseAttack = false;
                nextSpecialAttack = false;
            }
            else if(comboTimer > ComboA.GetCurAttack())
            {
                if (ComboA.NextIsPauseAttack() && nextPauseAttack)
                {
                    print("NextPauseAttack");
                    NextPauseAttack();
                }
                else if (!ComboA.NextIsPauseAttack() && nextPauseAttack)
                {
                    print("NextComboAttack");
                    NextComboAttack();
                }
                else if (nextAttack)
                {
                    print("NextComboAttack");
                    NextComboAttack();
                }
                else if (nextSpecialAttack && ComboA.GetCurAttacType() != AttackType.SpecialAttack)
                {
                    print("NextSpecialAttack");
                    NextSpecialAttack();
                }
                else
                {
                    print("EndCombo");
                    EndCombo();
                }
            }
        }
        if (isSpecialAttack)
        {
            //transform.Translate(Vector3.forward * 15 * Time.deltaTime);
            if (comboTimer > ComboA.GetCurAttack() * 0.2 && comboTimer < ComboA.GetCurAttack() * 0.8)
            {
                float step = speed * Time.deltaTime;
                transform.position = Vector3.Lerp(transform.position, target, step);
            }
            else
            {
                float step = speed/3 * Time.deltaTime;
                transform.position = Vector3.Lerp(transform.position, target, step);
            }
        }
    }
    private void StartCombo(AttackType type = AttackType.NormalAttack)
    {
        isAttack = true;
        comboTimer = 0f;
        if (type == AttackType.SpecialAttack)
        {
            isSpecialAttack = true;
            ComboA.NextSpecialAttack();
            target = transform.position + transform.forward * 15;
        }
        animator.SetBool(ComboA.GetCurBoolName(), true); // Устанавливаем переменную аниматора в true
        comboCounter++;
    }

    private void NextSpecialAttack()
    {
        isSpecialAttack = true;
        comboTimer = 0f;
        animator.SetBool(ComboA.GetCurBoolName(), false); // Устанавливаем переменную аниматора в false
        ComboA.NextSpecialAttack();
        animator.SetBool(ComboA.GetCurBoolName(), true); // Устанавливаем переменную аниматора в true
        comboCounter++;
        target = transform.position + transform.forward * 10;
        nextAttack = false;
        nextSpecialAttack = false;
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
        nextPauseAttack = false;
        nextSpecialAttack = false;
        isSpecialAttack = false;
        ComboA.ResetCombo();
    }
}
    