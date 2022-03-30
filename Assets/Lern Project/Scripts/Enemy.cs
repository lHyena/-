using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.AI;
using UnityEngine.Events;

namespace LernProject

{
    public enum OffMeshLinkMoveMethod
    {
        Teleport,
        NormalSpeed,
        Parabola
    }

    [RequireComponent(typeof(NavMeshAgent))]
    public class Enemy : MonoBehaviour , ITakeDamage
    {
        [SerializeField] private Player _player; // нахождение игрока
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private Transform _spawnPosition;
        private Vector3 _direction; // направление врага *
        public float speed = 1.5f; // скорость врага
        [SerializeField] private float _durability = 1f; //*
        [SerializeField] private float _cooldown;
        [SerializeField] private bool _isFire;

        private NavMeshAgent _agent; // искусственный интеллект
        public OffMeshLinkMoveMethod method = OffMeshLinkMoveMethod.Parabola;
        

        [SerializeField] private float _speedRotate;
        private bool _look;
        private bool _isSpawnBool;

        void Awake()
        {
            _player = FindObjectOfType<Player>();// это нежелательный метод, тк при большом кол-ве игроков, может заглючить игру
            _agent = GetComponent<NavMeshAgent>();

        }

        IEnumerator Start()
        {
            _agent.SetDestination(_player.transform.position);
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            agent.autoTraverseOffMeshLink = false;
            while (true)
            {
                if (agent.isOnOffMeshLink) // если противник встал на точку прыжка(Линк)
                {
                    if (method == OffMeshLinkMoveMethod.NormalSpeed)
                        yield return StartCoroutine(NormalSpeed(agent));
                    else if (method == OffMeshLinkMoveMethod.Parabola)
                        yield return StartCoroutine(Parabola(agent, 2.0f, 0.5f));
                    agent.CompleteOffMeshLink();
                }
                yield return null;
            }
        }
        IEnumerator NormalSpeed(NavMeshAgent agent)
        {
            OffMeshLinkData data = agent.currentOffMeshLinkData;
            Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
            while (agent.transform.position != endPos)
            {
                agent.transform.position = Vector3.MoveTowards(agent.transform.position, endPos, agent.speed * Time.deltaTime);
                yield return null;
            }
        }

        IEnumerator Parabola(NavMeshAgent agent, float height, float duration)
        {
            OffMeshLinkData data = agent.currentOffMeshLinkData;
            Vector3 startPos = agent.transform.position;
            Vector3 endPos = data.endPos + Vector3.up * agent.baseOffset;
            float normalizedTime = 0.0f;
            while (normalizedTime < 1.0f)
            {
                float yOffset = height * 4.0f * (normalizedTime - normalizedTime * normalizedTime);
                agent.transform.position = Vector3.Lerp(startPos, endPos, normalizedTime) + yOffset * Vector3.up;
                normalizedTime += Time.deltaTime / duration;
                yield return null;
            }
        }

        void FixedUpdate() // проверка значений
        {

            if (Vector3.Distance(transform.position, _player.transform.position) < 6)
            {
                _look = false;
                Look();
            }

        }

        private void Update()
        {
            /*transform.LookAt(_player.transform);*/ // поворот противника (персонажа)на игрока

            transform.Translate(Vector3.forward * Time.deltaTime * speed); // изменение на позиции. движение.

            Ray ray = new Ray(_spawnPosition.position, transform.forward); // определение местонахождение игрока лучем

            if (Physics.Raycast(ray, out RaycastHit hit, 6))//Raycast - нахождение первого предмета 
            {
                Debug.DrawRay(_spawnPosition.position, transform.forward * hit.distance, Color.blue); // проверка луча

                if (hit.collider.CompareTag("Player")) // если луч(ray), выпущенный из направления(_spawnPosition.position)
                {                                      // на дистанцию 6, столкнулся(hit), с collider.CompareTag("Player"), только тогда Fire().
                    if (_isFire)
                        Fire();

                }

                //_look = true;

            }

            if (Vector3.Distance(transform.position, _player.transform.position) < 6)
            {
                _look = true;
            }

            //из позиции (_agent.transform.position), радиусом 0.2f во всех областях (navMeshHit.AllAreas) найти данные
            // и посметить их (out NawMeshHit) на navMeshHit
            if (NavMesh.SamplePosition(_agent.transform.position, out NavMeshHit navMeshHit, 0.2f, NavMesh.AllAreas))
            {
                print(NavMesh.GetAreaCost((int)Mathf.Log(navMeshHit.mask, 2)));
            }

        }

        private void Fire()
        {
            _isFire = false;
            var shieldObj = Instantiate(_bulletPrefab, _spawnPosition.position, _spawnPosition.rotation);
            var shield = shieldObj.GetComponent<Bullet>(); // получем ссылку на экземпляр класса ( щита)
            shield.Init(_player.transform, 10, 0.6f);

           Invoke(nameof(Reloading), _cooldown);

        }
        private void Reloading() // перезарядка стрельбы
        {
            _isFire = true;
        }

        public void Init(float durability) //*
        {
            _durability = durability;

            Destroy(gameObject, t: 1f);
        }

        public void Hit(float damage) //Уничтожение *
        {
            _durability -= damage;

            if (_durability <= 0)
            {
                Destroy(gameObject);
            }
        }
        

        private void Look()
        {
            transform.LookAt(_player.transform); // поворот противника (персонажа)на игрока

            var direction = _player.transform.position - transform.position;
            var stepRotate = Vector3.RotateTowards(transform.forward, direction, _speedRotate * Time.fixedDeltaTime, 0f);// (указываем текущее направление взгляда, указываем точку взгляда


            transform.rotation = Quaternion.LookRotation(stepRotate);// указание направления
        }


    }
}




