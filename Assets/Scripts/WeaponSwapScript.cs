using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SearchService;

public class WeaponSwapScript : MonoBehaviour
{
    // Ссылка на точку правой руки персонажа
    public Transform rightHandPoint;

    public GameObject RWeaponHolder;
    public GameObject LWeaponHolder;

    // Список доступных оружий
    public GameObject[] weapons;
    public MonoBehaviour[] comboScripts;
    public AnimatorController[] animators;

    // Текущее оружие
    private GameObject currentWeapon;
    private MonoBehaviour currentScript;
    private Animator currentAnimator;

    private int weaponIndex = 0;
    private bool first = true;

    private void Start()
    {
        ChangeWeapon();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ChangeWeapon();
        }


    }

    // Функция для изменения оружия
    public void ChangeWeapon()
    {
        //Debug.Log("Swap");
        //currentWeapon = weapons[weaponIndex];
        weaponIndex++;
        weaponIndex %= weapons.Length;
        // Отключаем текущее оружие
        if (CheckIfWeaponAttached(RWeaponHolder))
        {
            currentWeapon = RWeaponHolder.transform.GetChild(0).gameObject;
            currentAnimator = GetComponent<Animator>();
            currentAnimator.runtimeAnimatorController = animators[weaponIndex]; 
                //тут поменять аниматор на animators[weaponIndex]
            Destroy(currentWeapon);
            if (CheckIfWeaponAttached(LWeaponHolder))
            {
                Destroy(LWeaponHolder.transform.GetChild(0).gameObject);
            }
            first = false;
        } 
        else
        {
            weaponIndex = 0;
        }
        currentWeapon = Instantiate(weapons[weaponIndex], RWeaponHolder.transform);


        if (weapons[weaponIndex].name == "Katana")
        {
            Instantiate(weapons[weaponIndex], LWeaponHolder.transform);
            currentScript = GetComponent<ScytheAttack>();
            currentScript.enabled = false;
            currentScript = GetComponent<SwordAttack>();
            currentScript.enabled = false;
            currentScript = GetComponent<KatanaAttack>();
            currentScript.enabled = true;
            if (!first) currentScript.GetComponent<KatanaAttack>().CustomStart();
        }
        else if (weapons[weaponIndex].name == "Sword")
        {
            currentScript = GetComponent<KatanaAttack>();
            currentScript.enabled = false;
            currentScript = GetComponent<ScytheAttack>();
            currentScript.enabled = false;
            currentScript = GetComponent<SwordAttack>();
            currentScript.enabled = true;
            if (!first) currentScript.GetComponent<SwordAttack>().CustomStart();
        }
        else if (weapons[weaponIndex].name == "Scythe")
        {
            currentScript = GetComponent<KatanaAttack>();
            currentScript.enabled = false;
            currentScript = GetComponent<SwordAttack>();
            currentScript.enabled = false;
            currentScript = GetComponent<ScytheAttack>();
            currentScript.enabled = true;
            if (!first) currentScript.GetComponent<ScytheAttack>().CustomStart();
        }

        // Создаем новое оружие
    }

    public bool CheckIfWeaponAttached(GameObject parentObject)
    {
        // Перебираем все дочерние объекты
        for (int i = 0; i < parentObject.transform.childCount; i++)
        {
            GameObject childObject = parentObject.transform.GetChild(i).gameObject;

            // Проверяем, является ли дочерний объект оружием
            if (childObject.CompareTag("Weapon"))
            {
                //Debug.Log("Оружие привязано к WeaponHolder");
                return true;
            }
        }
        return false;
    }
}