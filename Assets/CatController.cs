using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

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

    }

    // Update is called once per frame
    void Update()
    {
        SwitchLane();
        Jump();
        Slide();
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow) && !isMoving)
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
        if (Input.GetKeyDown(KeyCode.DownArrow) && !isMoving)
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

    void SwitchLane()
    {

        if (!isMoving && Input.GetKeyDown(KeyCode.LeftArrow) && charState != CharacterState.Left)
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

            // "move" the list items to the right
            Vector3 tempvec = positions[2];
            positions.RemoveAt(2);
            positions.Insert(0, tempvec);
        }
        else if (!isMoving && Input.GetKeyDown(KeyCode.RightArrow) && charState != CharacterState.Right)
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
