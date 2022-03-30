using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LernProject
{
    public class Player : MonoBehaviour
    {
        public GameObject ShieldPrefab;
        public Transform SpawnPosition;
             
        
        private bool _isSpawnShield;
        [HideInInspector] public int level = 1;// HideInInspector скрывает уровень игрока в инспекторе но не в самой игре


        public float speed = 2f; // Скорость движения, а в дальнейшем ускорение
        private Vector3 _direction; // Направление движения
        public float speedRotate = 20f;
        private bool _isSprint;
        [SerializeField] private float _jumpForce = 50f;
        private float _cfSpeed = 1f;
        [SerializeField] private Rigidbody _rb;

        [SerializeField] private Enemy _enemy; // нахождение противника
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private Transform _spawnPosition;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
        }
                      

                      
        void Update()
        {
            if (Input.GetMouseButtonDown(1))
                _isSpawnShield = true; //(1) проверка, создал ли игрок щит, нажал ли кнопку

            if (Input.GetMouseButtonDown(0))
            {
                Fire();
            }
                                               
        }

        private void FixedUpdate() // Движения лучше делать в FixeUpdate
        {
            _direction.x = Input.GetAxis("Horizontal");// перемещение игрока
            _direction.z = Input.GetAxis("Vertical");// перемещение игрока
            
            float sprint = (_isSprint) ? 2f : 1f; // увеличение скорости

            _direction = transform.TransformDirection(_direction);//перевод из локальной координаты в глобальную
            _rb.MovePosition(transform.position + _direction.normalized * speed * sprint * _cfSpeed * Time.fixedDeltaTime); // перемещение тв.тела(игрока)

            Vector3 rotate = new Vector3(0f, Input.GetAxis("Mouse X") * speedRotate * Time.fixedDeltaTime, 0f);
            //_rb.MoveRotation(_rb.rotation * Quaternion.Euler(rotate));

            transform.Rotate(new Vector3(0, Input.GetAxis("Mouse X") * speedRotate * Time.fixedDeltaTime, 0)); // позволяет врящать персонажа

            if (Input.GetKeyDown(KeyCode.Space)) // прыжок
                GetComponent<Rigidbody>().AddForce(Vector3.up * _jumpForce, ForceMode.Impulse);
            
            if (_isSpawnShield)
            {
                _isSpawnShield = false;//(3) проверяет успел ли нанестись урон по игроку, если да, то щит не сработает
                SpawnShield(); //(2) создает щит
            }

            //Move(Time.fixedDeltaTime);
                       
        }

        
        private void SpawnShield()
        {
            var shieldObj = Instantiate(ShieldPrefab, SpawnPosition.position, SpawnPosition.rotation);
            var shield = shieldObj.GetComponent<Shield>(); // получем ссылку на экземпляр класса ( щита)
            shield.Init(10 * level);
            shield.transform.SetParent(SpawnPosition);// постоянное нахождение с игроком
        }

        private void Fire() // метод стрельбы по противнику *
        {
            var enemyObj = Instantiate(_bulletPrefab, _spawnPosition.position, _spawnPosition.rotation);
            var enemy = enemyObj.GetComponent<BulletJHON>(); // получем ссылку на экземпляр класса (гостя )
            enemy.Init(_enemy.transform, 10, 0.6f);

        }

        //private void Move(float delta) // привод движения
        //{
        //    var fixedDirection = transform.TransformDirection(_direction.normalized); // игрок двигается в куазанном вращении
        //    transform.position += fixedDirection * (_isSprint ? speed * 2 : speed) * delta; // delta делит движение, чтобы не было скачков // transform.position - текущая позиция
        //    // normalized делает одинаковую скорость как при движении по горизонтали, так и по диагоналям
        //}
    }

}
