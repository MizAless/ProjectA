using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DodgingScript : MonoBehaviour
{
    private Animator animator;
    public float dodgeDistance = 4f; // Дистанция уворота
    public float dodgeSpeed = 8f; // Скорость уворота

    private bool isDodging = false; // Флаг, указывающий, выполняется ли уворот
    private float currentDodgeDistance = 0f; // Текущая дистанция уворота

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Проверяем, нажата ли кнопка "Shift" и уворот не выполняется
        if (Input.GetKeyDown(KeyCode.LeftShift) && !isDodging)
        {
            StartCoroutine(DodgeRoutine());
        }

        // Применяем уворот
        if (isDodging)
        {

            animator.SetFloat("dodge", 1);

            // Вычисляем дистанцию с каждым обновлением кадра
            float dodgeStep = dodgeSpeed * Time.deltaTime;
            currentDodgeDistance += dodgeStep;


            // Смещаем объект на смещенный угол
            transform.Translate(-1 * Vector3.forward * dodgeStep);

            // Проверяем, достигнута ли необходимая дистанция
            if (currentDodgeDistance >= dodgeDistance)
            {
                // Уворот завершен
                isDodging = false;
                currentDodgeDistance = 0f;
                animator.SetFloat("dodge", 0);
            }

        }
    }

    IEnumerator DodgeRoutine()
    {
        isDodging = true;

        // Ожидание окончания уворота
        yield return new WaitForSeconds(0.5f); // Измените это значение на длительность уворота

        // Уворот завершен
        isDodging = false;
        currentDodgeDistance = 0f;
        animator.SetFloat("dodge", 0);
    }
}
