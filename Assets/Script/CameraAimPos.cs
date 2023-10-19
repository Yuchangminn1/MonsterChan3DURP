using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using UnityEngine;

public class CameraAimPos : MonoBehaviour
{
    [SerializeField] RaycastHit hit;
    [SerializeField] LayerMask mask;

    // ray�� ����
    [SerializeField]
    private float _maxDistance = 20.0f;

    // ray�� ����
    [SerializeField]
    private Color _rayColor = Color.red;

    [SerializeField] float sphereScale;


    [SerializeField] Transform aimPos;
    private void Start()
    {
        aimPos = GameObject.Find("AimPos").GetComponent<Transform>();
    }

    void OnDrawGizmos()
    {
        Gizmos.color = _rayColor;

        // �Լ� �Ķ���� : ���� ��ġ, Sphere�� ũ��(x,y,z �� ���� ū ���� ũ�Ⱑ ��), Ray�� ����, RaycastHit ���, Sphere�� ȸ����, SphereCast�� ������ �Ÿ�
        if (true == Physics.SphereCast(transform.position, sphereScale / 20.0f, transform.forward, out hit, _maxDistance, mask))
        {
            // Hit�� �������� ray�� �׷��ش�.
            Gizmos.DrawRay(transform.position, transform.forward * hit.distance);

            // Hit�� ������ Sphere�� �׷��ش�.
            Gizmos.DrawWireSphere(transform.position + transform.forward * hit.distance, sphereScale / 2.0f);

            aimPos.transform.position = (transform.position + transform.forward * hit.distance);
        }
        else
        {
            // Hit�� ���� �ʾ����� �ִ� ���� �Ÿ��� ray�� �׷��ش�.
            Gizmos.DrawRay(transform.position, transform.forward * _maxDistance);
        }
    }

    public Vector3 GetAimPos()
    {
        return aimPos.position;
    }

}
