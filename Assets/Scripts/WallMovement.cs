using UnityEngine;

public class WallMovement : MonoBehaviour
{
    [SerializeField] float _moveSpeed = 1f;
    [SerializeField] GameManager _gameManager;

    void Update()
    {
        transform.Translate(Vector3.back * _moveSpeed * Time.deltaTime);
    }

    public void Stop()
    {
        _moveSpeed = 0;
        _gameManager.Gameover();
    }
}
