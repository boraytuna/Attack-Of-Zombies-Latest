using UnityEngine;

public class PlayerInputManager : MonoBehaviour
{
    public static PlayerInputManager Instance { get; private set; }

    private Vector3 moveVector;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void UpdateMoveVector(Vector3 move)
    {
        moveVector = move;
    }

    public Vector3 MoveVector
    {
        get { return moveVector; }
    }
}
