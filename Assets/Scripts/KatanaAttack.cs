using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

public class KatanaComboAttack : AttackCombo
{
    public List<float> SpecDurations = new List<float>();
    public KatanaComboAttack(List<float> durations, List<float> pauseAttackDuration, int pauseAttackParentIndex) : base(durations, pauseAttackDuration, pauseAttackParentIndex)
    {
    }

    public void AddSpecDuration(float dur)
    {
        SpecDurations.Add(dur);
    }

}

public class KatanaAttack : MonoBehaviour
{
    public GameObject specParticles;


    private bool isAttack = false; // ���������� ��� ���������� ��������� �����
    private bool isSpecialAttack = false;
    //private bool isPauseAttack = false;
    private bool nextAttack = false;
    private bool nextPauseAttack = false;
    private bool nextSpecialAttack = false;
    //private float comboBDuration = 0.8f;
    //private float comboDuration = 0.6f; // ������������ ����� � ��������
    //private float cuurentComboDuration = 0.6f; // ������������ ����� � ��������
    private int comboCounter = 0;
    //private bool isJumping = false;
    //private bool isGrounded = true;
    private Rigidbody rb;
    private KatanaComboAttack ComboA;
    private Attack SpecialAttack;
    private AttackComboManager attackComboManager;
    private List<GameObject> Katanas;


    //public float accelerationSpeed = 5f; // �������� ���������
    //public float decelerationSpeed = 10f; // �������� ����������
    //private float currentSpeed = 0f; // ������� ��������
    //private float progress = 0f; // �������� ���������/���������� (�� 0 �� 1)

    private Vector3 target;
    private float speed = 2.0f;


    private Animator animator;
    private float comboTimer = 0f; // ������ ��� ������������ ������������ �����

    public void CustomStart()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();

        float[] inputAttackDuration = { 0.833f };
        float[] pauseAttackDuration = { 0.933f };
        ComboA = new KatanaComboAttack(new List<float>(inputAttackDuration), new List<float>(pauseAttackDuration), 0);
        //SpecialAttack = new Attack(1.067f, "IsSpecialAttack", AttackType.SpecialAttack);
        //ComboA.AddAttack(SpecialAttack);
        ComboA.AddSpecDuration(0.633f); // ������ � ������� ������ ���� ������ ��� �� ��������
        ComboA.PrintCombo(ComboA.InitialAttack);
        var WeaponList = new List<GameObject>(GameObject.FindGameObjectsWithTag("Weapon"));
        Katanas = WeaponList.Where(weapon => weapon.name == "Katana(Clone)").ToList();

        foreach (GameObject katana in Katanas)
        {
            katana.GetComponent<Collider>().enabled = false;
            katana.GetComponent<Collider>().isTrigger = false;
        }

        //foreach (var weapon in WeaponList)
        //{
        //    weapon.gameObject.GetComponent<Collider>().enabled = false;
        //}
    }

    private void Start()
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
        if (Input.GetKeyDown(KeyCode.Mouse1))
        {
            if (isAttack && Mathf.Abs(comboTimer - 0.633f) < 0.10)
            {
                //Instantiate(specParticles,transform.position, Quaternion.identity);

                GameObject rWeaponHolder = GameObject.Find("RWeaponHolder"); // ������� ������ RWeaponHolder �� ��� ����� ��� ������ ��������
                if (rWeaponHolder != null)
                {
                    GameObject katana = rWeaponHolder.transform.Find("Katana(Clone)").gameObject; // ������� ������ ������ RWeaponHolder
                    if (katana != null)
                    {
                        GameObject specParticlesInstance = Instantiate(specParticles, katana.transform.position, Quaternion.identity);
                        specParticlesInstance.transform.SetParent(katana.transform);

                        Destroy(specParticlesInstance, 0.5f); // ���������� ������ specParticles ����� 0.1 �������

                        //ParticleSystem particleSystem = specParticlesInstance.GetComponent<ParticleSystem>();
                        //if (particleSystem != null)
                        //{
                        //    particleSystem.Stop(); // ���������� ��������������� ������

                        //    float fadeTime = 2f; // ����� �������� � ��������
                        //    float startAlpha = particleSystem.main.startColor.color.a; // ��������� ������������ ������

                        //    // ��������� �������� ��� ���������� ��������� ������
                        //    StartCoroutine(FadeOutParticles(particleSystem, fadeTime, startAlpha));


                        //}
                    }
                }
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
                else if (!ComboA.NextIsPauseAttack() && nextPauseAttack && ComboA.GetCurAttackType() != AttackType.NormalAttack)
                {
                    print("NextComboAttack");
                    NextComboAttack();
                }
                else if (nextAttack && ComboA.GetCurAttackType() != AttackType.NormalAttack)
                {
                    print("NextComboAttack");
                    NextComboAttack();
                }
                else if (nextSpecialAttack && ComboA.GetCurAttackType() != AttackType.SpecialAttack)
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
        //if (isSpecialAttack)
        //{
        //    //transform.Translate(Vector3.forward * 15 * Time.deltaTime);
        //    if (comboTimer > ComboA.GetCurAttack() * 0.2 && comboTimer < ComboA.GetCurAttack() * 0.8)
        //    {
        //        float step = speed * Time.deltaTime;
        //        transform.position = Vector3.Lerp(transform.position, target, step);
        //    }
        //    else
        //    {
        //        float step = speed / 3 * Time.deltaTime;
        //        transform.position = Vector3.Lerp(transform.position, target, step);
        //    }
        //} 
    }
    private void StartCombo(AttackType type = AttackType.NormalAttack)
    {
        foreach (GameObject katana in Katanas)
        {
            katana.GetComponent<Collider>().enabled = true;
            katana.GetComponent<Collider>().isTrigger = true;
        }
        isAttack = true;
        comboTimer = 0f;
        if (type == AttackType.SpecialAttack)
        {
            isSpecialAttack = true;
            ComboA.NextSpecialAttack();
            target = transform.position + transform.forward * 15;
        }
        animator.SetBool(ComboA.GetCurBoolName(), true); // ������������� ���������� ��������� � true
        comboCounter++;
    }

    private void NextSpecialAttack()
    {
        isSpecialAttack = true;
        comboTimer = 0f;
        animator.SetBool(ComboA.GetCurBoolName(), false); // ������������� ���������� ��������� � false
        ComboA.NextSpecialAttack();
        animator.SetBool(ComboA.GetCurBoolName(), true); // ������������� ���������� ��������� � true
        comboCounter++;
        target = transform.position + transform.forward * 10;
        nextAttack = false;
        nextSpecialAttack = false;
    }

    private void NextComboAttack()
    {
        comboTimer = 0f;
        animator.SetBool(ComboA.GetCurBoolName(), false); // ������������� ���������� ��������� � false
        ComboA.NextAttack();
        animator.SetBool(ComboA.GetCurBoolName(), true); // ������������� ���������� ��������� � true
        comboCounter++;
        nextAttack = false;
    }

    private void NextPauseAttack()
    {
        comboTimer = 0f;
        animator.SetBool(ComboA.GetCurBoolName(), false); // ������������� ���������� ��������� � false
        ComboA.NextPauseAttack();
        animator.SetBool(ComboA.GetCurBoolName(), true); // ������������� ���������� ��������� � true
        comboCounter++;
        nextAttack = false;
        nextPauseAttack = false;
    }


    private void EndCombo()
    {
        isAttack = false;
        comboTimer = 0f;
        animator.SetBool(ComboA.GetCurBoolName(), false); // ������������� ���������� ��������� � true
        comboCounter = 0;
        nextAttack = false;
        nextPauseAttack = false;
        nextSpecialAttack = false;
        isSpecialAttack = false;
        ComboA.ResetCombo();
        foreach (GameObject katana in Katanas)
        {
            katana.GetComponent<Collider>().enabled = false;
            katana.GetComponent<Collider>().isTrigger = false;
        }
    }

    //private IEnumerator FadeOutParticles(ParticleSystem particleSystem, float fadeTime, float startAlpha)
    //{
    //    float timer = 0f;

    //    while (timer < fadeTime)
    //    {
    //        float alpha = Mathf.Lerp(startAlpha, 0f, timer / fadeTime); // ������������ ������������ �� ���������� �������� � 0

    //        var mainModule = particleSystem.main;
    //        var startColor = mainModule.startColor;
    //        startColor.color = new Color(startColor.color.r, startColor.color.g, startColor.color.b, alpha); // ���������� ����� ������������

    //        timer += Time.deltaTime;
    //        yield return null;
    //    }

    //    Destroy(particleSystem.gameObject); // ���������� ������ ������ ����� ��������
    //}

}