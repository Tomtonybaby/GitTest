using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f; // 子弹速度
    private Vector2 direction;
    private LineRenderer lineRenderer;
    private EdgeCollider2D edgeCollider;
    private bool hasHitWall = false; // 标记是否已碰撞到墙壁

    // 子弹的生存时间
    public float timeToLive = 60f;

    // 初始化子弹方向
    public void Initialize(Vector2 shootDirection)
    {
        direction = shootDirection.normalized;
    }




    //------------节点预制体------------
    [SerializeField] private GameObject _nodeLinePrefab;



    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        //edgeCollider = gameObject.AddComponent<EdgeCollider2D>();

        lineRenderer.positionCount = 1;
        lineRenderer.SetPosition(0, transform.position);

        // 设置刚体的检测模式
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.collisionDetectionMode = CollisionDetectionMode2D.Continuous;
        }

        Destroy(gameObject, timeToLive);
    }

    void Update()
    {
        if (!hasHitWall)
        {
            // 更新子弹位置
            transform.position += (Vector3)(direction * speed * Time.deltaTime);

            // 更新轨迹线
            lineRenderer.positionCount++;
            lineRenderer.SetPosition(lineRenderer.positionCount - 1, transform.position);

            // 更新碰撞体
            //UpdateCollider();
        }
    }

    public Vector2 colliderPoint;
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            print("碰撞到了墙");
            // 子弹碰到墙壁时停止运动
            hasHitWall = true;

            // 停止子弹的运动
            direction = Vector2.zero;
            //把碰撞传出来给外部使用
            //colliderPoint = collision
            print(transform.position);
            //碰撞点的位置到开火点的位置距离
            //Vector2 disLine = (transform.position - SpiderController.Instance.lastPoint.position).normalized;
            float disLine = Vector2.Distance(transform.position, SpiderController.Instance.lastPoint.position);
            print(disLine);
            //Vector2 startPoint = SpiderController.Instance.lastPoint.position;
            


            // 设置刚体为静态
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Static;
            }


            //-------------碰到墙后在当前位置放一个尾节点，然后再最后发射的地方放一个头节点-------------
            GameObject lgo = Instantiate(_nodeLinePrefab, transform.position, Quaternion.identity);
            GameObject tgo = lgo.GetComponentInChildren<NodeTrailControl>().gameObject;
            GameObject hgo = lgo.GetComponentInChildren<NodeHeadControl>().gameObject;
            tgo.transform.position = transform.position;
            hgo.transform.position = SpiderController.Instance.lastPoint.position;
            lgo.transform.parent = transform;
            Destroy(lgo, timeToLive-1);



            //-------------可以正常移动了-------------
            SpiderController.Instance.ResetMove();

        }
        else if (collision.gameObject.CompareTag("Enemy"))
        {
            print("射到敌人了");
            //执行敌人的逻辑

            // 子弹碰到墙壁时停止运动
            hasHitWall = true;

            // 停止子弹的运动
            direction = Vector2.zero;
            //把碰撞传出来给外部使用


            // 设置刚体为静态
            Rigidbody2D rb = GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.bodyType = RigidbodyType2D.Static;
            }


            //-------------可以正常移动了-------------
            SpiderController.Instance.ResetMove();

        }
    }



    /// <summary>
    /// 收集后消除子弹
    /// </summary>
    public void DestroySelfForCollected()
    {
        Destroy(gameObject, 0.1f);
    }

   

}
