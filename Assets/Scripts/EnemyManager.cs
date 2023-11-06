using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // Добавляем пространство имен для доступа к NavMeshAgent

public class EnemyManager : MonoBehaviour
{
    public Collider groundCollider;
    public GameObject enemyPrefab; // Префаб врага
    public int numberOfEnemies = 5; // Количество врагов
    

    void Awake()
    {
        // Получаем позицию и размеры платформы
        Vector3 platformPosition = transform.position;
        Vector3 platformSize = transform.localScale;

        // Вычисляем расстояние между врагами
        float distanceBetweenEnemies = platformSize.z / (numberOfEnemies + 1);
        Debug.Log(distanceBetweenEnemies);

        // Создаем врагов на платформе
        for (int i = 0; i < numberOfEnemies; i++)
        {
            // Вычисляем позицию врага на платформе
            float enemyPosition = -80 + (i + 1) * distanceBetweenEnemies;

            // Создаем врага на позиции
            Vector3 spawnPosition = new Vector3(0, 10, enemyPosition);
            GameObject enemy = Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
            enemy.transform.Rotate(new Vector3(0, -180, 0));
            enemy.tag = "Enemy";

            // Добавляем компонент NavMeshAgent к врагу
            //NavMeshAgent navMeshAgent = enemy.AddComponent<NavMeshAgent>();
            // Настройте параметры NavMeshAgent по вашему усмотрению
            // navMeshAgent.speed = 3f;
            // navMeshAgent.stoppingDistance = 2f;
            // Установите целевую точку для перемещения врага
            //navMeshAgent.SetDestination(Vector3.zero);
        }
    }

    //void OnCollisionEnter(Collision collision)
    //{
    //    Debug.Log("123");
    //    // Проверяем, что столкновение произошло с полом
    //    if (collision.collider == this.BoxCollider)
    //    {
    //        // Активируем компонент NavMeshAgent на враге
    //        NavMeshAgent navMeshAgent = collision.gameObject.GetComponent<NavMeshAgent>();
    //        if (navMeshAgent != null)
    //        {
    //            navMeshAgent.enabled = true;
    //            // Настройте параметры NavMeshAgent по вашему усмотрению
    //            navMeshAgent.speed = 3f;
    //            navMeshAgent.stoppingDistance = 2f;
    //            // Установите целевую точку для перемещения врага
    //            navMeshAgent.SetDestination(Vector3.zero);
    //        }
    //    }
    //}

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}




    
