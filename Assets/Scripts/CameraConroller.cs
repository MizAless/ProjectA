using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraConroller : MonoBehaviour
{
    private float mouseX;
    private float mouseY;
    private Rigidbody rb;
    private Quaternion initialRotation;

    public float sensitivityMouse = 100;

    

    public Transform camera;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true; // Блокируем вращение Rigidbody
        initialRotation = transform.rotation; // Сохраняем начальное вращение персонажа

    }

    // Update is called once per frame
    void Update()
    {
        mouseX = Input.GetAxis("Mouse X") * sensitivityMouse * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * sensitivityMouse * Time.deltaTime;

        transform.Rotate(mouseX * new Vector3(0,1,0));

        // if (camera.rotation.x >= 0 && camera.position.x <= 30){
            camera.Rotate(-mouseY * new Vector3(1,0,0));
        // }

        // } else if(camera.rotation.x <= 0){
        //     camera.Rotate(0.01f * new Vector3(1,0,0));
        // } else {
        //     camera.Rotate(-0.01f * new Vector3(1,0,0));
        // }
        // camera.Translate(mouseY * new Vector3(0,1,0) * 0.1f);
    }
}

