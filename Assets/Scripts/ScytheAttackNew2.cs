using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

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

    public Attack(float duration, string boolName, Attack? nextAttack, Attack? nextPauseAttack)
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

            if (i == pauseAttackParentIndex)
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
        if (CurrentAttack != null)
        {
            CurrentAttack = CurrentAttack.NextAttack;
        }
    }

    public void NextPauseAttack()
    {
        if (CurrentAttack != null)
        {
            CurrentAttack = CurrentAttack.NextPauseAttack;
        }
    }

    public void ResetCombo()
    {
        CurrentAttack = InitialAttack;
    }

    public void PrintCombo(Attack initAttack)
    {
        Debug.Log(initAttack.Duration);
        if (initAttack.NextAttack != null)
        {
            PrintCombo(initAttack.NextAttack);
        }
        if (initAttack.NextPauseAttack != null)
        {
            PrintCombo(initAttack.NextPauseAttack);
        }
    }

}

public class ScytheAttackNew2 : MonoBehaviour
{
    private Rigidbody rb;
    private AttackCombo ComboA;
    private Animator animator;
    private float comboTimer = 0f; // Таймер для отслеживания длительности комбо

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        float[] inputAttackDuration = { 1.033f, 1.333f, 1.233f };
        float[] pauseAttackDuration = { 1.200f };
        ComboA = new AttackCombo(new List<float>(inputAttackDuration), new List<float>(pauseAttackDuration), 1);

        //ComboA.PrintCombo(ComboA.InitialAttack);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
