using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviourPun
{
   [Range(0, 30)][SerializeField] private float _moveSpeed = 10f;

   
   //----- compoent ----- //
   private CharacterController _characterController;
   private Animator _animator;

   
   //----- constant --- //
   private float _gravity = -9.81f;
   private static readonly int VelocityHash = Animator.StringToHash("Velocity");
   


   void Awake() => Init();
     void Update()
     {
         // 내 플레이어에 대해서만 움직이도록 처리
         if(photonView.IsMine)
             ControlPlayer();
     }


     private void Init()
     {
         _characterController = GetComponent<CharacterController>();
         _animator = GetComponent<Animator>();
         
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
        if (direction.magnitude >= 0.1f)
        {
            _characterController.Move(_moveSpeed * Time.deltaTime * direction);
            _animator.SetFloat(VelocityHash, direction.magnitude);   // 애니메이션 처리
        }
        else
        {
            _animator.SetFloat(VelocityHash, 0);
        }
 
    }

    private void SetRotation(Vector3 direction)
    {
        if(direction.magnitude < 0.1f) return;
            
        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;  //역탄젠트를 써서 회전값(Degree) 구하기
        transform.rotation = Quaternion.Euler(0, targetAngle, 0);   // y축에 대해서만 rotation 처리
    }
    

    private Vector3 GetInputDirection()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        return new Vector3(h, 0, v).normalized;
    }
    
    
}
