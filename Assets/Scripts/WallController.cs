using UnityEngine;

public class WallController : MonoBehaviour
{
    [SerializeField] float _moveSpeed;   
      
    
    
    void Update()
    {
        transform.Translate(Vector3.back * _moveSpeed * Time.deltaTime);
    }
}
