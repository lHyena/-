using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace LernProject
{
    public class Boomb : MonoBehaviour
    {
        [SerializeField] private float _damage = 100000f;

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

