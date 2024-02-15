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
        else if (!isMoving && Input.GetKeyDown(KeyCode.RightArrow) &&  charState != CharacterState.Right)
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
