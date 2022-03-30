using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LernProject
{
    public class BulletJHON : MonoBehaviour
    {
        private Transform _target;
        private float _speed;
        [SerializeField] private float _damage = 3;
        [SerializeField] private float _force = 3; // сила
        private Rigidbody _rigidbody;


        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();
        }

        public void Init(Transform target, float lifeTime, float speed)
        {
            _target = target;
            _speed = speed;
            Destroy(gameObject, lifeTime);

            _rigidbody.AddForce(transform.forward * _force);// движение пули вперед с силой
        }

        //void FixedUpdate()
        //{
        //    //transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed); \\ самонаводящиеся пули
        //    transform.position += transform.forward * _speed * Time.fixedDeltaTime;
        //}

        private void OnCollisionEnter(Collision collision) // точка соприкосновения
        {

            if (collision.gameObject.TryGetComponent(out ITakeDamage takeDamage)) // нахождение конкретного компонента и взаимодейстиве с ним
            {
                Debug.Log("Hit!");
                takeDamage.Hit(_damage);
            }
        }
    }

}

