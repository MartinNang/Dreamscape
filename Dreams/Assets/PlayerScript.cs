using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerScript : MonoBehaviour
{
    public Rigidbody rb;
    public float stepLength = 5;
    public int steps = 1;
    public int rows = 4;
    public int columns = 4;
    public float moveCooldownSeconds = 0;
    public float moveCooldownRemaining = 0;
    public float speed = 100;
    private float currentSpeed = 0;
    private Vector3 targetPos;
    private static bool isMoving = false;
    public float friction = 0.8f;
    private Queue<MoveDirection> moveQueue = new Queue<MoveDirection>();
    private bool neutral;
    private bool isFalling = false;

    // Start is called before the first frame update
    void Start()
    {
        moveCooldownRemaining = moveCooldownSeconds;
        targetPos = transform.position;     
        neutral = false;
    }

    // Update is called once per frame
    void Update()
    {
        bool up = Input.GetAxisRaw("Vertical") > 0 && neutral;
        if (up) Debug.Log("up");
        bool down = Input.GetAxisRaw("Vertical") < 0 && neutral;
        if (down) Debug.Log("down");
        bool left = Input.GetAxisRaw("Horizontal") < 0 && neutral;
        if (left) Debug.Log("left");
        bool right = Input.GetAxisRaw("Horizontal") > 0 && neutral;
        if (right) Debug.Log("right");
        neutral = Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0; // bezieht sich auf letzte frame

        float currentX = transform.position.x;
        float currentZ = transform.position.z;

        if ((up ^ down ^ left ^ right))
        {
            if (!isMoving && !isFalling) // TODO fix falling
            {
                setIsMoving(true);
                // TODO: fix the queue
                if (moveQueue.Count > 0)
                {
                    MoveDirection currentMove = moveQueue.Dequeue();
                    switch (currentMove)
                    {
                        case MoveDirection.UP:
                            currentZ += (up ? -1 : 0) * stepLength;
                            break;
                        case MoveDirection.DOWN:
                            currentZ += (down ? 1 : 0) * stepLength;
                            break;
                        case MoveDirection.LEFT:
                            currentX += (left ? -1 : 0) * stepLength;
                            break;
                        case MoveDirection.RIGHT:
                            currentX += (right ? 1 : 0) * stepLength;
                            break;
                    }
                } 
                else
                {
                    currentX += (up ? -1 : 0) * stepLength + (down ? 1 : 0) * stepLength;
                    currentZ += (left ? -1 : 0) * stepLength + (right ? 1 : 0) * stepLength;
                                        
                }
                currentX = Mathf.Clamp(currentX, -((rows - 1) * stepLength / 2), (rows-1) * stepLength /2);
                currentZ = Mathf.Clamp(currentZ, -((rows - 1) * stepLength / 2), (rows-1) * stepLength / 2);
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

    private void FixedUpdate()
    {
        // Vector3 dir = targetPos - transform.position;
        // rb.AddForce(0,0,0);
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag.Equals("Level"))
        {
            isFalling = true;
        }
    }

    private void setIsMoving(bool value)
    {
        isMoving = value;
        WallScript.setIsPlayerMoving(value);
    }

    public static bool getIsMoving()
    {
        return isMoving;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Wall"))
        {
            SceneManager.LoadScene(0);
        }
        else if (other.gameObject.tag.Equals("Level"))
        {
            isFalling = false;
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
        if (getIsMoving())
        {
            // if (currentSpeed > speed * 0.5) currentSpeed *=  friction;
            var step = currentSpeed * Time.deltaTime; // calculate distance to move
            transform.position = Vector3.MoveTowards(transform.position, targetPos, step);
            if (transform.position == targetPos)
            {
                setIsMoving(false);
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
