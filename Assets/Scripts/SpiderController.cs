using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SpiderController : MonoBehaviour
{
    private static SpiderController instance;
    public static SpiderController Instance => instance;
    public GameObject bulletPrefab; // 子弹的Prefab
    public Transform firePoint; // 子弹发射点

    public float moveSpeed=5;

    public Transform lastPoint;


    /*---------------------*/

    float x, y = 0;
    //角色移动方向
    private Vector2 _direction = Vector2.zero;

    //角色移动速度
    [SerializeField] private float _speed = 3f;

    //角色移动速度
    [SerializeField] private float _speciaSpeed = 1f;

    [SerializeField] bool _canMove; //可以正常左右动
    [SerializeField] bool _canNormalMove; //可以正常左右动

    private Rigidbody2D rgd;


    //地面检测
    [SerializeField] private Transform _groundCheckPoint;
    [SerializeField] private float _checkRadius;




    private void Awake()
    {
        instance = this;

        rgd = GetComponent<Rigidbody2D>();

        //----------------一开始可以正常移动----------------
        _canMove = true;
        _canNormalMove = true;
    }


    void Update()
    {
        // 检测鼠标左键点击
        if (Input.GetMouseButtonDown(0))
        {
            Shoot();
        }

        //正常移动
        Player2Movement();

        //特殊移动
        Player2SpeciaMovement();

        //朝向
        Player2Direction();

        //重力设置
        ResetGravity();
    }


    /// <summary>
    /// 正常移动
    /// </summary>
    private void Player2Movement()
    {
        if (!_canMove) return;

        if (!_canNormalMove) return;

        x = GameInputManager.Instance.Movement2.x;

        _direction = new Vector2(x, y);


        //控制角色朝着方向移动
        transform.Translate(_direction * _speed * Time.deltaTime);
    }


    /// <summary>
    /// 能否特殊移动
    /// </summary>
    /// <returns></returns>
    private bool CanSpeciaMove()
    {
        return NodeManager.Instance.CanSpeciaMoveInCurrentNode();
    }


    /// <summary>
    /// 特殊移动
    /// </summary>
    private void Player2SpeciaMovement()
    {
        if (!CanSpeciaMove()) return;
        if (!_canMove) return;

        _direction = NodeManager.Instance.CurrentNodeTail().position - NodeManager.Instance.CurrentNodeHead().position;

        if (Input.GetKey(KeyCode.UpArrow))
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


    /// <summary>
    /// 朝向控制
    /// </summary>
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







    void Shoot()
    {
        //----------------一射就不给动-------------------
        _canMove = false;

       


        // 获取鼠标在世界坐标中的位置
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePosition.z = 0; 

        // 计算从发射点到鼠标位置的方向
        Vector2 direction = (mousePosition - firePoint.position).normalized;

        // 实例化子弹并设置其初始位置
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        lastPoint = firePoint;

        // 初始化子弹方向
        bullet.GetComponent<Bullet>().Initialize(direction);

        // 使子弹朝向移动方向
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        bullet.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

        //开火点与子弹碰撞点的距离  制作蛛丝

    }



    /// <summary>
    /// 恢复移动 在射到墙壁或虫子后
    /// </summary>
    public void ResetMove()
    {
        _canMove = true;
    }



    /// <summary>
    /// 恢复左右移动
    /// </summary>
    public void EnableNormalMove(bool enable)
    {
        _canNormalMove = enable;
    }




    public bool Player2IsInSlide()
    {
        if (Mathf.Abs(GameInputManager.Instance.Movement2.y) > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    /// <summary>
    /// 启用重力
    /// </summary>
    /// <param name="enable"></param>
    public void EnableGravity(bool enable)
    {
        if(enable)
        {
            rgd.gravityScale = 1;
        }
        else
        {
            rgd.gravityScale = 0;
        }
        
    }


    private bool IsOnGround()
    {
        Collider2D hit = Physics2D.OverlapCircle(_groundCheckPoint.position, _checkRadius);

        if (hit != null && hit.tag == "Wall")
        {
            Debug.Log(hit.name);
            return true;
        }
        else
        {
            return false;
        }
    }



    private void ResetGravity()
    {
        if(IsOnGround())
        {
            EnableGravity(false);
        }
    }


    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_groundCheckPoint.position, _checkRadius);
    }

}
