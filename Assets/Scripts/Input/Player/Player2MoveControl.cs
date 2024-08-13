using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Player2MoveControl : MonoBehaviour
{
    //角色移动方向
    private Vector2 _direction = Vector2.zero;

    //角色移动速度
    [SerializeField] private float _speed = 3f;

    //角色移动速度
    [SerializeField] private float _speciaSpeed = 1f;

    float x, y = 0;



    private void FixedUpdate()
    {
        Player2Movement();
        Player2SpeciaMovement();
        Player2Direction();
    }



    private void Player2Movement()
    {
        x = GameInputManager.Instance.Movement2.x;

        _direction = new Vector2(x, y);


        //控制角色朝着方向移动
        transform.Translate(_direction * _speed * Time.deltaTime);
    }


    private bool CanSpeciaMove()
    {
        return NodeManager.Instance.CanSpeciaMoveInCurrentNode();
    }


    private void Player2SpeciaMovement()
    {
        if (!CanSpeciaMove()) return;

        
        _direction = NodeManager.Instance.CurrentNodeTail().position - NodeManager.Instance.CurrentNodeHead().position;

        if(Input.GetKey(KeyCode.UpArrow))
        {
            //控制角色朝着方向移动
            transform.Translate(_direction * _speciaSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            //控制角色朝着方向移动
            transform.Translate(-_direction * _speciaSpeed * Time.deltaTime);
        }
    }


    private void Player2Direction()
    {
        print(x);


        if (x < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else if (x > 0)
        {
            transform.localScale = new Vector3(1, 1, 1);
        }
    }

}
