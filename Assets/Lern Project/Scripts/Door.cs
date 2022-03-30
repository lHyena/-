using UnityEngine;


public class Door : MonoBehaviour
    {
        [SerializeField] private Transform _rotatePoint;

        private bool _isStopped;


        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !_isStopped)
            {
                _rotatePoint.Rotate(Vector3.up, 90);
            }
        }


        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag("Player") && !_isStopped)
            {
                _rotatePoint.Rotate(Vector3.up, -90);
            }

        }
        private void OnTriggerStay(Collider other)
        {
            if (other.CompareTag("Player"))
            {
                if (Input.GetKeyDown(KeyCode.E))
                    _isStopped = true;
            }

        }

    }


    






