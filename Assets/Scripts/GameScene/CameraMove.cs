using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public Transform target;
    public Vector3 offsetPos;
    public float bodyHeight;

    public float moveSpeed;
    public float rotationSpeed;
    private Vector3 targetPos;
    private Quaternion targetRotation;
    
    void Update()
    {
        if (target == null)
            return;
        targetPos = target.position + target.forward * offsetPos.z;
        targetPos += Vector3.up * offsetPos.y;
        targetPos += target.right * offsetPos.x;
        this.transform.position = Vector3.Lerp(this.transform.position, targetPos, moveSpeed * Time.deltaTime);

        targetRotation = Quaternion.LookRotation(target.position + Vector3.up * bodyHeight - this.transform.position);
        this.transform.rotation = Quaternion.Slerp(this.transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    public void SetTarget(Transform player)
    {
        target = player;
    }
}
