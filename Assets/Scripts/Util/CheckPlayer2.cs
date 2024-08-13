using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPlayer2 : MonoBehaviour
{
    [SerializeField] private Transform _leftPoint;
    [SerializeField] private Transform _rightPoint;

    [SerializeField] private LayerMask _player2Layer;


    private void Update()
    {
        CheckPlayer2InThisRay();
    }


    private void OnEnable()
    {
        SpiderController.Instance?.EnableNormalMove(true);
    }


    private void OnDisable()
    {
        SpiderController.Instance?.EnableNormalMove(false);
    }


    private void CheckPlayer2InThisRay()
    {
        RaycastHit2D hit = Physics2D.Raycast(_leftPoint.position, (_rightPoint.position - _leftPoint.position).normalized,
            Vector3.Distance(_leftPoint.position, _rightPoint.position), _player2Layer);

        if (hit.collider != null)
        {
            SpiderController.Instance.EnableNormalMove(true);
        }
        else
        {
            SpiderController.Instance.EnableNormalMove(false);
        }
    }
}
