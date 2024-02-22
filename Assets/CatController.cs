using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class CatController : MonoBehaviour
{
    public float moveDistance;
    public Vector3 centerPos;
    public CharacterState charState;
    [SerializeField] private float moveDuration;
    [SerializeField] private Ease moveEase;
    public Animator anim;
    public Transform coll;

    [Header("Jump params")]

    public float collMinPos;
    public float collMaxPos;
    public float collAnimDuration;
    public Ease jumpUpEase;
    public Ease jumpDownEase;
    public float jumpSwipeThreshold;


    [Header("Slide params")]
    public float collSlideMin;
    public float collSlideAnimDuration;
    public Ease slideDownEase;
    public Ease slideUpEase;


    public List<Vector3> positions = new List<Vector3>();


    // Start is called before the first frame update
    void Start()
    {
        charState = CharacterState.Center;
        transform.position = centerPos;

        positions.Clear();
        positions.Add(centerPos + Vector3.left * moveDistance);
        positions.Add(centerPos);
        positions.Add(centerPos + Vector3.right * moveDistance);

        // Cursor.visible = false;
        // Cursor.lockState = CursorLockMode.None;
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
        SwitchLane();
        Jump();
        Slide();
    }

    private void HandleInput()
    {
        touchedLeft = false;
        touchedRight = false;
        swipedUp = false;
        swipedDown = false;

#if UNITY_ANDROID
        /*         if (Input.touchCount > 0)
                {
                    Touch touch = Input.GetTouch(0);
                    if (touch.phase == TouchPhase.Began)
                    {
                        startYPos = touch.position.y;
                    }
                    else if (touch.phase == TouchPhase.Moved)
                    {
                        float newYPos = touch.position.y;
                        float yDelta = newYPos - startYPos;

                        if (yDelta / Screen.height > jumpSwipeThreshold)
                        {
                            swipedUp = true;
                        }
                        else if (Mathf.Abs(yDelta / Screen.height) > jumpSwipeThreshold)
                        {
                            swipedDown = true;
                        }
                    }
                    else if (touch.phase == TouchPhase.Ended)
                    {
                        float xPos = touch.position.x;
                        if (xPos < Screen.width / 2)
                        {
                            // player touched on the left side of the screen
                            touchedLeft = true;
                        }
                        else
                        {
                            // player touched on the right side of the screen
                            touchedRight = true;
                        }
                    }
                }
         */


#endif
#if UNITY_EDITOR

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            touchedLeft = true;
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            touchedRight = true;
        }
        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            swipedUp = true;
        }
        else if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            swipedDown = true;
        }
#endif

        if (SimpleInput.GetButtonDown("MoveLeft"))
        {
            touchedLeft = true;
        }
        else if (SimpleInput.GetButtonDown("MoveRight"))
        {
            touchedRight = true;
        }
        else if (SimpleInput.GetButtonDown("Jump"))
        {
            swipedUp = true;
        }
        else if (SimpleInput.GetButtonDown("Slide"))
        {
            swipedDown = true;
        }
    }

    float startYPos = 0;
    bool swipedUp = false;
    bool swipedDown = false;
    private void Jump()
    {

        if (swipedUp && !isMoving)
        {
            isMoving = true;
            // play anim
            anim.ResetTrigger("Jump");
            anim.SetTrigger("Jump");

            // animate collider
            coll.DOLocalMoveY(collMaxPos, collAnimDuration / 2).SetEase(jumpUpEase).OnComplete(
                () =>
                {
                    Debug.Log("Jumping reached highest point");
                    coll.DOLocalMoveY(collMinPos, collAnimDuration / 2).SetEase(jumpDownEase).OnComplete(
                        () =>
                        {
                            isMoving = false;
                        }
                    );
                }
             );
        }
    }

    void Slide()
    {
        if (swipedDown && !isMoving)
        {
            isMoving = true;
            // play anim
            anim.ResetTrigger("Slide");
            anim.SetTrigger("Slide");

            // animate collider
            coll.DOLocalMoveY(collSlideMin, collSlideAnimDuration / 2).SetEase(slideDownEase).OnComplete(
                () =>
                {
                    coll.DOLocalMoveY(collMinPos, collSlideAnimDuration / 2).SetEase(slideUpEase).OnComplete(
                        () =>
                        {
                            isMoving = false;
                        }
                    );
                }
             );
        }
    }

    private bool isMoving;
    private bool touchedLeft;
    private bool touchedRight;
    void SwitchLane()
    {
        if (!isMoving && touchedLeft && charState != CharacterState.Left)
        {
            isMoving = true;

            if (charState == CharacterState.Center)
            {
                charState = CharacterState.Left;
            }
            else
            {
                charState = CharacterState.Center;
            }

            transform.DOMove(positions[0], moveDuration, false).SetEase(moveEase).OnComplete(
                () =>
                {
                    isMoving = false;
                }
            );
            transform.DOLocalRotate(Vector3.up * -90, moveDuration / 2).OnComplete(
                () =>
                {
                    transform.DOLocalRotate(Vector3.zero, moveDuration / 2);
                }
            );

            // "move" the list items to the right
            Vector3 tempvec = positions[2];
            positions.RemoveAt(2);
            positions.Insert(0, tempvec);
        }
        else if (!isMoving && touchedRight && charState != CharacterState.Right)
        {
            isMoving = true;

            if (charState == CharacterState.Center)
            {
                charState = CharacterState.Right;
            }
            else
            {
                charState = CharacterState.Center;
            }

            transform.DOMove(positions[2], moveDuration, false).SetEase(moveEase).OnComplete(
                () =>
                {
                    isMoving = false;
                }
            );
            transform.DOLocalRotate(Vector3.up * 90, moveDuration / 2).OnComplete(
                () =>
                {
                    transform.DOLocalRotate(Vector3.zero, moveDuration / 2);
                }
            );

            // "move" the list items to the left
            Vector3 tempvec = positions[0];
            positions.RemoveAt(0);
            positions.Add(tempvec);
        }

    }
}

public enum CharacterState
{
    Left,
    Right,
    Center
}
