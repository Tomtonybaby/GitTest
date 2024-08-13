using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CheckRayManager : MonoBehaviour
{
    [SerializeField] private List<CheckPlayer2> checkPlayer2Lists;

    private Transform _player2;

    private void Awake()
    {
        _player2 = GameObject.FindWithTag("Player2").transform;
    }


    private void Update()
    {
        foreach(var e in checkPlayer2Lists)
        {
            float dis = Mathf.Abs(_player2.transform.position.y - e.transform.position.y);

            //print(e.name + ":" + dis);

            if(dis < 1.3f)
            {
                e.gameObject.SetActive(true);
            }
            else
            {
                e.gameObject.SetActive(false);
            }
        }
    }
}
