using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    [Header("Mouse Rotation")]
    GameObject player;
    
    [SerializeField] private float sensitivity = 2.0f;
    [SerializeField] private float xRotation = 0.0f;
    [SerializeField] private float yRotation = 0.0f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //�÷��̾� ��ġ ����
        transform.position = player.transform.position;

    }
    private void FixedUpdate()
    {
        MouseAngle();
    }
     public void MouseAngle()
    {
        // ���콺 �Է��� ����
        float mouseX = Input.GetAxis("Mouse X") * sensitivity;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity;


        //// ���� ȸ���� ī�޶� ��������
        xRotation -= mouseY;
        yRotation += mouseX;
        if (Mathf.Abs(yRotation) >= 360)
            yRotation = 0f;
        xRotation = Mathf.Clamp(xRotation, -20.0f, 45.0f);
        yRotation = Mathf.Clamp(yRotation, -360.0f, 360.0f);

        //// ī�޶� ȸ�� ����
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0.0f);
        //transform.position = (Vector3.MoveTowards(transform.position, playerPos.position, 1f));
    }



    
}
