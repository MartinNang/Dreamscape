using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static PlayerScript;

public class WallScript : MonoBehaviour
{
    public MoveDirection moveDirection;
    public static float moveDistance = 5;
    private bool previousMoving = false;
    public static float speed = 120;
    private Vector3 targetPos, targetRot;
    private bool moving = false;
    public static bool isPlayerMoving;
    Rigidbody m_Rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        targetPos = transform.position;
        targetRot = new Vector3(720, 0, 0);
        isPlayerMoving = false;
        // m_Rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerMoving && !previousMoving)
        {
            switch(moveDirection)
            {
                case (MoveDirection.UP):
                    targetPos.x -= moveDistance;
                    break;
                case (MoveDirection.DOWN):
                    targetPos.x += moveDistance;
                    break;
                case MoveDirection.LEFT:
                    targetPos.z -= moveDistance;
                    break;
                case MoveDirection.RIGHT:
                    targetPos.z += moveDistance;
                    break;
            }

            // if (targetPos.x < -17.5f || targetPos.x > 17.5f || targetPos.z < -17.5f || targetPos.z > 17.5f) Destroy(this.gameObject);
            if (targetPos.x < -17.5f)
            {
                transform.position = new Vector3(17.5f, transform.position.y, transform.position.z);
                targetPos = transform.position;
            }
            if (targetPos.x > 17.5f)
            {
                transform.position = new Vector3(-17.5f, transform.position.y, transform.position.z);
                targetPos = transform.position;
            }
            if (targetPos.z < -17.5f)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, 17.5f);
                targetPos = transform.position;
            }
            if (targetPos.z > 17.5f)
            {
                transform.position = new Vector3(transform.position.x, transform.position.y, -17.5f);
                targetPos = transform.position;
            }
            // targetRot.x += 30;
            moving = true;
        }
        if (moving)
        {
            // if (currentSpeed > speed * 0.5) currentSpeed *= friction;
            var step = speed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
            // transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, speed * Time.deltaTime);
            // transform.localRotation = Quaternion.RotateTowards(transform.localRotation, );
            /*Quaternion deltaRotation = Quaternion.Euler(targetRot * Time.fixedDeltaTime);
            m_Rigidbody.MoveRotation(m_Rigidbody.rotation * deltaRotation);*/
            if (transform.position == targetPos) moving = false;
        }

        previousMoving = isPlayerMoving;      
        
    }

    public static void setIsPlayerMoving(bool val)
    {
        isPlayerMoving = val;
    }
}
