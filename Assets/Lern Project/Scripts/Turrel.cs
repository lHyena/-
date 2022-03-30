using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LernProject
{
    public class Turrel : MonoBehaviour
    {
        [SerializeField] private Player _player; // нахождение игрока
        [SerializeField] private float _speedRotate;
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private Transform _spawnPosition;
        [SerializeField] private float _cooldown;
        [SerializeField] private bool _isFire;
        private bool _isSpawnBool;
        private bool _look;
        
             
           

        void Start()
        {
            _player = FindObjectOfType<Player>();// это нежелательный метод, тк при большом кол-ве игроков, может заглючить игру
            //_player = GameObject.FindWithTag("Player").transform;

        }

        void FixedUpdate() // проверка значений
        {
            if (Vector3.Distance(transform.position, _player.transform.position) < 6)
            {
                _isSpawnBool = false;
                if (_isFire)
                {
                    Fire();
                }
                    
            }

            if (Vector3.Distance(transform.position, _player.transform.position) < 6)
            {
                _look = false;
                Look();
            }

        }

        private void Update()//*transform.LookAt(_player.transform);
        {
            
            if (Vector3.Distance(transform.position, _player.transform.position) < 6)
            {
                _look = true;
            }
        }

        private void Look()
        {
            transform.LookAt(_player.transform); // поворот противника (персонажа)на игрока

            var direction = _player.transform.position - transform.position;
            var stepRotate = Vector3.RotateTowards(transform.forward, direction, _speedRotate * Time.fixedDeltaTime, 0f);// (указываем текущее направление взгляда, указываем точку взгляда


            transform.rotation = Quaternion.LookRotation(stepRotate);// указание направления
        }

        private void Fire() // метод стрельбы по игроку *
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


    }

}


