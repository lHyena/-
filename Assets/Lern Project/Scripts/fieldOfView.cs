using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fieldOfView : MonoBehaviour
{
    public float radius;
    [Range(0, 360)]
    public float angle;

    public GameObject playerRef;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer;

    private void Start()
    {
        playerRef = GameObject.FindGameObjectWithTag("Player");
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);//задержка поиска

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            if (Vector3.Angle(transform.forward, directionToTarget) < angle / 2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);

                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask))
                    canSeePlayer = true;
                else
                    canSeePlayer = false;
            }
            else
                canSeePlayer = false;
        }
        else if (canSeePlayer)
            canSeePlayer = false;
    }
}

//public class fieldOfView : MonoBehaviour
//{
//    public float radius;
//    [Range(0,360)]
//    public float angle;

//    public GameObject player;

//    public LayerMask targetMask; // маска мишень
//    public LayerMask obstructionMask; // защитная маска

//    public bool canSeePlayer;

//    private void Start()
//    {
//        player = GameObject.FindGameObjectWithTag("Player");
//        StartCoroutine(FOVRoutine());

//    }

//    private IEnumerator FOVRoutine()// поиск игрока
//    {
//        WaitForSeconds wait = new WaitForSeconds(0.2f);//задержка поиска


//        while (true)
//        {
//            yield return wait;
//            fieldOfViewCheck();
//        }
//    }

//    private void fieldOfViewCheck()// проверка поля зрения
//    {
//        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);// проверка дальности

//        if(rangeChecks.Length != 0) //проверка нашел ли противник игрока
//        {
//            Transform target = rangeChecks[0].transform;//rangeChecks[0] - возвращаемое значение в OverlapSphere, к-рая возвращает знаечение в  Collider[]

//            Vector3 directionToTarget = (target.position - transform.position).normalized;//дистанция до игрока

//            if(Vector3.Angle(transform.forward, directionToTarget) < angle / 2)//проверка угла обзора
//            {
//                float distanceToTarget = Vector3.Distance(transform.position, target.position);//растояние на котором находится игрок от текущего угла обзора

//                // запуск луча(Physics.Raycats) из центра врага(transform.position)
//                // на игрока(directionToTarget) с ограниченным расстоянием(distanceToTarget)
//                // ограничение луча если перед нами стена (obstructionMask)
//                if (!Physics.Raycast(transform.position, directionToTarget, distanceToTarget, obstructionMask)) 
//                    canSeePlayer = true;  
//                else
//                    canSeePlayer = false;
//            }
//            else
//                canSeePlayer = false;
//        }
//        else if (canSeePlayer)//если противник не нашел игрока
//        {
//            canSeePlayer = false;
//        }


//    }

//}
