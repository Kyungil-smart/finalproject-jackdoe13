using UnityEngine;

public class TestMove : MonoBehaviour
{
    public PlayerMovement player;
    public Transform target;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            player.MoveTo(target);
        }
    }
}
