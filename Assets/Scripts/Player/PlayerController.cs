using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   [Range(0, 30)][SerializeField] private float _moveSpeed = 10f;
   [Range(0, 30)][SerializeField] private float _rotationSpeed = 10f;

   private Rigidbody _rb;



   void Awake() => Init();
     void Update() => ControlPlayer();


     private void Init()
     {
         _rb = GetComponent<Rigidbody>();
     }
     
     
     
    private void ControlPlayer()
    {
        //입력값(움직이는 방향) 가져오기
        Vector3 moveDirection = GetInputDirection();
        
        // 이동 처리
        SetMove(moveDirection);
        
        //회전 처리
        SetRotation(moveDirection);
        
    }


    private void SetMove(Vector3 direction)
    {
        
        //이동처리
        Vector3 velocity = _rb.velocity;
        velocity.x = direction.x * _moveSpeed;
        velocity.z = direction.z * _moveSpeed;
        _rb.velocity = velocity;
        
    }

    private void SetRotation(Vector3 direction)
    {
        //direction을 Quaternion으로 변환
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        
        //보간 처리
        _rb.rotation = Quaternion.Lerp(_rb.rotation, targetRotation, Time.deltaTime*_rotationSpeed);
    }
    

    private Vector3 GetInputDirection()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        return new Vector3(h, 0, v).normalized;
    }
    
    
}
