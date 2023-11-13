//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine;
//using System;

//public class AttackCombo
//{
//    private static string[] STRNBRS = { "First", "Second", "Third", "Fourth", "Fifth" };
//    private List<float> AttackDuration; // ���� ������� �������� ��������� �����
//    private List<float> PauseAttackDuration; // ���� ������� �������� ��������� ����� � ������
//    private int PauseAttackParentIndex; // ������ ����� �� ��������� ������ � ������ ���������� ����� � ������
//    private List<string> AttackBoolNames; // ���� �������� �������� ���������� ��� �������� �����
//    private List<string> PauseAttackBoolNames; // ���� �������� �������� ���������� ��� �������� ���� c ������
//    private int AttackCount; // ���������� ����
//    private int PauseAttackCount; // ���������� ���� � ������
//    private int AttackIndex; // ������ �������� �����
//    public bool IsPauseAttack;
//    public AttackCombo(List<float> attackDuration, List<float> pauseAttackDuration, int pauseAttackParentIndex)
//    {

//        AttackDuration = attackDuration;
//        AttackCount = AttackDuration.Count;
//        AttackIndex = 0;
//        AttackBoolNames = new List<string>();
//        for (int i = 0; i < AttackCount; i++)
//        {
//            AttackBoolNames.Add("Is" + STRNBRS[i] + "Attack");
//        }
//        PauseAttackDuration = pauseAttackDuration;
//        PauseAttackCount = PauseAttackDuration.Count;
//        PauseAttackParentIndex = pauseAttackParentIndex;
//        PauseAttackBoolNames = new List<string>();
//        for (int i = 0; i < PauseAttackCount; i++)
//        {
//            PauseAttackBoolNames.Add("Is" + STRNBRS[i] + "PauseAttack");
//        }
//        IsPauseAttack = false;

//    }

//    public void NextAttack()
//    {
//        AttackIndex++;
//        if (AttackIndex == AttackDuration.Count)
//        {
//            ResetCombo();
//        }
//    }

//    public void NextPauseAttack()
//    {
//        if (AttackIndex == PauseAttackDuration.Count && IsPauseAttack)
//        {
//            ResetCombo();
//        }
//        else if(IsPauseAttack)
//        {
//            AttackIndex++;
//        }
//        else if (IsPauseIndexAttack())
//        {
//            AttackIndex = 0;
//            IsPauseAttack = true;
//        }
//    }

//    public float GetCurAttack()
//    {
//        return !IsPauseAttack ?  AttackDuration[AttackIndex] : PauseAttackDuration[AttackIndex];
//    }

//    public string GetCurBoolName()
//    {
//        return !IsPauseAttack ? AttackBoolNames[AttackIndex] : PauseAttackBoolNames[AttackIndex];
//    }

//    public void ResetCombo()
//    {
//        AttackIndex = 0;
//        IsPauseAttack = false;
//    }

//    public bool IsPauseIndexAttack()
//    {
//        return AttackIndex == PauseAttackParentIndex && !IsPauseAttack;
//    }

//    public int GetIndexPauseAttack()
//    {
//        return PauseAttackParentIndex;
//    }


//}

//public class ScytheAttackNew : MonoBehaviour
//{
//    private bool isAttack = false; // ���������� ��� ���������� ��������� �����
//    private bool isSpecialAttack = false;
//    private bool isPauseAttack = false;
//    private bool nextAttack = false;
//    private bool nextPauseAttack = false;
//    private float comboBDuration = 0.8f;
//    private float comboDuration = 0.6f; // ������������ ����� � ��������
//    private float cuurentComboDuration = 0.6f; // ������������ ����� � ��������
//    private int comboCounter = 0;
//    private bool isJumping = false;
//    private bool isGrounded = true;
//    private Rigidbody rb;
//    private AttackCombo ComboA;


//    private Animator animator;
//    private float comboTimer = 0f; // ������ ��� ������������ ������������ �����

//    private void Start()
//    {
//        animator = GetComponent<Animator>();
//        rb = GetComponent<Rigidbody>();

//        float[] inputAttackDuration = { 1.033f, 1.333f, 1.233f };
//        float[] pauseAttackDuration = { 1.200f };
//        ComboA = new AttackCombo(new List<float>(inputAttackDuration), new List<float>(pauseAttackDuration), 1);

//    }

//    // Update is called once per frame
//    void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.Mouse0))
//        {
//            if (!isAttack)
//            {
//                StartCombo();
//            }
//            else if (isAttack && comboTimer >= ComboA.GetCurAttack() - 0.2f && ComboA.IsPauseIndexAttack() && !nextAttack)
//            {
//                nextPauseAttack = true;
//            }
//            else if (isAttack && comboTimer >= ComboA.GetCurAttack() - 0.5f)
//            {
//                nextAttack = true;
//            }
            
//        }
//        if (isAttack)
//        {
//            comboTimer += Time.deltaTime;

//            if (comboTimer < ComboA.GetCurAttack() - 0.5f)
//            {
//                nextAttack = false;
//                nextPauseAttack = false;
//            }
//            else if (comboTimer > ComboA.GetCurAttack() && nextPauseAttack)
//            {
//                print("NextPauseAttack");
//                NextPauseAttack();
//            }
//            else if (comboTimer > ComboA.GetCurAttack() && !nextAttack)
//            {
//                print("EndCombo");
//                EndCombo();
//            }
//            else if ((comboTimer > ComboA.GetCurAttack()) && nextAttack)
//            {
//                print("NextComboAttack");
//                NextComboAttack();
//            }
            
//        }
//    }
//    private void StartCombo()
//    {
//        isAttack = true;
//        this.comboTimer = 0f;
//        animator.SetBool(ComboA.GetCurBoolName(), true); // ������������� ���������� ��������� � true
//        comboCounter++;
//    }

//    private void NextComboAttack()
//    {
//        comboTimer = 0f;
//        animator.SetBool(ComboA.GetCurBoolName(), false); // ������������� ���������� ��������� � false
//        ComboA.NextAttack();
//        animator.SetBool(ComboA.GetCurBoolName(), true); // ������������� ���������� ��������� � true
//        comboCounter++;
//        nextAttack = false;
//    }

//    private void NextPauseAttack()
//    {
//        comboTimer = 0f;
//        animator.SetBool(ComboA.GetCurBoolName(), false); // ������������� ���������� ��������� � false
//        ComboA.NextPauseAttack();
//        animator.SetBool(ComboA.GetCurBoolName(), true); // ������������� ���������� ��������� � true
//        comboCounter++;
//        nextAttack = false;
//        nextPauseAttack = false;
//    }


//    private void EndCombo()
//    {
//        isAttack = false;
//        comboTimer = 0f;
//        animator.SetBool(ComboA.GetCurBoolName(), false); // ������������� ���������� ��������� � true
//        comboCounter = 0;
//        nextAttack = false;
//        ComboA.ResetCombo();
//    }
//}
