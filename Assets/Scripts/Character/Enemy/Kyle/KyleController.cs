using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KyleController : MonoBehaviour
{
    protected CharacterController controller;
    // Start is called before the first frame update
    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        var direction = Vector3.forward;
        float targetAngle = Mathf.Atan2(direction.x, direction.z);
        Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

        controller.Move(moveDirection.normalized * 3f * Time.deltaTime);
    }
}
