using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public static NodeManager Instance { get; private set; }

    [SerializeField] private Transform _currentNodeHead;
    [SerializeField] private Transform _currentNodeTail;

    [SerializeField] private Transform _Player2;

    public Transform CurrentNodeHead() => _currentNodeHead;
    public Transform CurrentNodeTail() => _currentNodeTail;

    [SerializeField] private LayerMask _player2Layer;


    private void Awake()
    {
        Instance = this;
    }


    private void Start()
    {
        _Player2 = GameObject.FindWithTag("Player2").transform;
    }



    /*切换当前节点*/

    public void ChangeCurrentNodes(Transform nh,Transform nt)
    {
        if (SpiderController.Instance.Player2IsInSlide()) return;

        _currentNodeHead = nh;
        _currentNodeTail = nt;
    }



    /*给Player2调用*/
    public bool CanSpeciaMoveInCurrentNode()
    {
        if (_currentNodeHead == null || _currentNodeTail == null) return false;
        RaycastHit2D hit = Physics2D.Raycast(_currentNodeHead.position, (_currentNodeTail.position - _currentNodeHead.position).normalized,
            Vector3.Distance(_currentNodeHead.position, _currentNodeTail.position), _player2Layer);

        if (hit.collider != null)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //private void OnDrawGizmos()
    //{
    //    Gizmos.DrawLine(_currentNodeTail.position, _currentNodeHead.position);
    //}

}
