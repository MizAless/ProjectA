using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class SwordAttack : MonoBehaviour
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

    private Vector3 target;
    private float speed = 2.0f;

    private Animator animator;
    private float comboTimer = 0f; // Таймер для отслеживания длительности комбо

    private bool isGrounded = true;
    private bool isJumping = false;
    private GameObject Sword;

    public void CustomStart()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        float[] inputAttackDuration = { 0.733f, 0.567f, 1.033f };
        float[] pauseAttackDuration = { 2.167f };
        ComboA = new AttackCombo(new List<float>(inputAttackDuration), new List<float>(pauseAttackDuration), 1);
        SpecialAttack = new Attack(1.200f, "IsSpecialAttack", AttackType.SpecialAttack);
        ComboA.AddAttack(SpecialAttack);

        var WeaponList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Weapon"));
        Sword = WeaponList.FirstOrDefault(weapon => weapon.name == "Sword(Clone)");
        Sword.GetComponent<Collider>().enabled = false;
        Sword.GetComponent<Collider>().isTrigger = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        CustomStart();
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
        if (Input.GetKeyDown(KeyCode.Mouse1) && isGrounded)
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
            else if (comboTimer > ComboA.GetCurAttack())
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
                else if (nextSpecialAttack && ComboA.GetCurAttackType() != AttackType.SpecialAttack && isGrounded)
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
            //if (comboTimer > ComboA.GetCurAttack() * 0.2 && comboTimer < ComboA.GetCurAttack() * 0.8)
            //{
            //    float step = speed * Time.deltaTime;
            //    transform.position = Vector3.Lerp(transform.position, target, step);
            //}
            //else
            //{
            //    float step = speed / 3 * Time.deltaTime;
            //    transform.position = Vector3.Lerp(transform.position, target, step);
            //}

            if (!isJumping)
            {
                //rb.AddForce(Vector3.up * 10, ForceMode.Impulse);
                rb.velocity = new Vector3(rb.velocity.x, 10, rb.velocity.z);
                isJumping = true;
            }
        }
    }
    private void StartCombo(AttackType type = AttackType.NormalAttack)
    {
        Sword.GetComponent<Collider>().enabled = true;
        Sword.GetComponent<Collider>().isTrigger = true;
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
        Sword.GetComponent<Collider>().enabled = false;
        Sword.GetComponent<Collider>().isTrigger = false;
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
}
