using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMoveControl2 : MonoBehaviour
{
    
    [SerializeField] private float _speed = 3f; //角色移动速度

    private Vector2 _direction = Vector2.zero; //角色移动方向

    float x, y = 0;



    private void FixedUpdate()
    {
        Player2Movement();
        Player2Direction();
    }



    private void Player2Movement()
    {
        x = GameInputManager.Instance.Movement1.x;
        y = GameInputManager.Instance.Movement1.y;

        _direction = new Vector2(x, y);


        //控制角色朝着方向移动
        transform.Translate(_direction * _speed * Time.deltaTime);
    }



    private void Player2Direction()
    {
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
