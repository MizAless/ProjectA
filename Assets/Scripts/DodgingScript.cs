using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgingScript : MonoBehaviour
{
    private Animator animator;
    public float dodgeDistance = 4f; // ��������� �������
    public float dodgeSpeed = 8f; // �������� �������

    private bool isDodging = false; // ����, �����������, ����������� �� ������
    private float currentDodgeDistance = 0f; // ������� ��������� �������

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // ���������, ������ �� ������ "Shift" � ������ �� �����������
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDodging)
        {
            StartCoroutine(DodgeRoutine());
        }

        // ��������� ������
        if (isDodging)
        {

            animator.SetFloat("dodge", 1);

            // ��������� ��������� � ������ ����������� �����
            float dodgeStep = dodgeSpeed * Time.deltaTime;
            currentDodgeDistance += dodgeStep;


            // ������� ������ �� ��������� ����
            transform.Translate(-1 * Vector3.forward * dodgeStep);

            // ���������, ���������� �� ����������� ���������
            if (currentDodgeDistance >= dodgeDistance)
            {
                // ������ ��������
                isDodging = false;
                currentDodgeDistance = 0f;
                animator.SetFloat("dodge", 0);
            }

        }
    }

    IEnumerator DodgeRoutine()
    {
        isDodging = true;

        // �������� ��������� �������
        yield return new WaitForSeconds(0.5f); // �������� ��� �������� �� ������������ �������

        // ������ ��������
        isDodging = false;
        currentDodgeDistance = 0f;
        animator.SetFloat("dodge", 0);
    }
}
