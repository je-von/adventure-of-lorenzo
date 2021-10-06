using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KyleController : MonoBehaviour
{
    public Kyle kyle;
    private CharacterController controller;

    public SpriteRenderer coreItem;
    // Start is called before the first frame update
    void Start()
    {
        kyle = new Kyle();
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("-----" + this.gameObject);
        if (kyle.healthPoints <= 0)
        {
            Destroy(this.gameObject);
            Vector3 pos = this.transform.position;
            pos.y = 1;
            var c = Instantiate(coreItem, pos, Quaternion.identity);
            //c.a
        }

        //var direction = Vector3.forward;
        //float targetAngle = Mathf.Atan2(direction.x, direction.z);
        //Vector3 moveDirection = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;

        //controller.Move(moveDirection.normalized * 3f * Time.deltaTime);
    }
}
