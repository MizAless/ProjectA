using UnityEngine;

public class WeaponSwapScript : MonoBehaviour
{
    // ������ �� ����� ������ ���� ���������
    public Transform rightHandPoint;

    public GameObject RWeaponHolder;
    public GameObject LWeaponHolder;

    // ������ ��������� ������
    public GameObject[] weapons;

    // ������� ������
    private GameObject currentWeapon;

    private int weaponIndex = 0;

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

    // ������� ��� ��������� ������
    public void ChangeWeapon()
    {
        //Debug.Log("Swap");
        //currentWeapon = weapons[weaponIndex];
        weaponIndex++;
        weaponIndex %= weapons.Length;
        // ��������� ������� ������
        if (CheckIfWeaponAttached(RWeaponHolder))
        {
            currentWeapon = RWeaponHolder.transform.GetChild(0).gameObject;
            Destroy(currentWeapon);
            if (CheckIfWeaponAttached(LWeaponHolder))
            {
                Destroy(LWeaponHolder.transform.GetChild(0).gameObject);
            }
        } else
        {
            weaponIndex = 0;
        }

        if (weapons[weaponIndex].name == "Katana")
        {
            Instantiate(weapons[weaponIndex], LWeaponHolder.transform);
        }

        // ������� ����� ������
        currentWeapon = Instantiate(weapons[weaponIndex], RWeaponHolder.transform);
    }

    private bool CheckIfWeaponAttached(GameObject parentObject)
    {
        // ���������� ��� �������� �������
        for (int i = 0; i < parentObject.transform.childCount; i++)
        {
            GameObject childObject = parentObject.transform.GetChild(i).gameObject;

            // ���������, �������� �� �������� ������ �������
            if (childObject.CompareTag("Weapon"))
            {
                //Debug.Log("������ ��������� � WeaponHolder");
                return true;
            }
        }
        return false;
    }
}