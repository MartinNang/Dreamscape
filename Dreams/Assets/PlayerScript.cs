using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    public int stepLength = 5;
    public int steps = 1;
    public float moveCooldownSeconds = 0;
    public float moveCooldownRemaining = 0;
    public float speed = 100;
    private float currentSpeed = 0;
    private Vector3 targetPos;
    public static bool moving = false;
    public float friction = 0.8f;
    private Queue<MoveDirection> moveQueue = new Queue<MoveDirection>();

    // Start is called before the first frame update
    void Start()
    {
        moveCooldownRemaining = moveCooldownSeconds;
        targetPos = transform.position;        
    }

    // Update is called once per frame
    void Update()
    {
        bool up = Input.GetKeyDown(KeyCode.W);
        bool down = Input.GetKeyDown(KeyCode.S);
        bool left = Input.GetKeyDown(KeyCode.A);
        bool right = Input.GetKeyDown(KeyCode.D);

        float currentX = transform.position.x;
        float currentZ = transform.position.z;

        if ((up ^ down ^ left ^ right))
        {
            if (!moving)
            {
                moving = true;
                // TODO: fix the queue
                if (moveQueue.Count > 0)
                {
                    MoveDirection currentMove = moveQueue.Dequeue();
                    switch (currentMove)
                    {
                        case MoveDirection.UP:
                            currentX += (up ? -1 : 0) * stepLength;
                            break;
                        case MoveDirection.DOWN:
                            currentX += (down ? 1 : 0) * stepLength;
                            break;
                        case MoveDirection.LEFT:
                            currentZ += (left ? -1 : 0) * stepLength;
                            break;
                        case MoveDirection.RIGHT:
                            currentZ += (right ? 1 : 0) * stepLength;
                            break;
                    }                    
                } 
                else
                {
                    currentX += (up ? -1 : 0) * stepLength + (down ? 1 : 0) * stepLength;
                    currentZ += (left ? -1 : 0) * stepLength + (right ? 1 : 0) * stepLength;
                                        
                }
                currentX = Mathf.Clamp(currentX, -8, 8);
                currentZ = Mathf.Clamp(currentZ, -8, 8);
                targetPos = new Vector3(currentX, transform.position.y, currentZ);
                Debug.Log("targetPos: " + targetPos);

                currentSpeed = speed;
            } 
            else
            {
                addMove(getDirection(up, down, left, right));
            }
            
        }
        Debug.Log("current speed: " + currentSpeed);
        // if (moveCooldownRemaining > 0) moveCooldownRemaining -= Time.deltaTime;
        moveTowardsTargetPosition();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Wall"))
        {
            SceneManager.LoadScene(0);
        }
    }

        private void popMove()
    {
        
    }

    private MoveDirection getDirection(bool up, bool down, bool left, bool right)
    {
        if (up) return MoveDirection.UP;
        else if (down) return MoveDirection.DOWN;
        else if (left) return MoveDirection.LEFT;
        else if (right) return MoveDirection.RIGHT;
        else throw new Exception("Incorrect usage of getDirection(): one directional input must be true");
    }

    private void addMove(MoveDirection move)
    {
        moveQueue.Enqueue(move);
    }

    private void moveTowardsTargetPosition()
    {
        if (moving)
        {
            if (currentSpeed > speed * 0.5) currentSpeed *=  friction;
            var step = currentSpeed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
            if (transform.position == targetPos)
            {
                moving = false;
            }
        } 
        else if (moveQueue.Count != 0)
        {

        }
    }

    public enum MoveDirection {
        UP, 
        DOWN, 
        LEFT,
        RIGHT
    };
}
