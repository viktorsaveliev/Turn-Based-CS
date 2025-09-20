using Echobay.GridSystem;
using Echobay.InteractSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Echobay
{
    public class ObjectMover : MonoBehaviour, ICellMoveableOccupant
    {
        public event Action OnPathCompleted;
        public GridCell CurrentCell { get; set; }

        [SerializeField] private float _moveSpeed = 3f;

        private Coroutine _moveRoutine;

        public void MoveAlongPath(List<GridCell> path)
        {
            if (_moveRoutine != null)
            {
                StopCoroutine(_moveRoutine);
            }

            _moveRoutine = StartCoroutine(MoveRoutine(path));
        }

        private IEnumerator MoveRoutine(List<GridCell> path)
        {
            foreach (var cell in path)
            {
                Vector3 target = cell.transform.position;

                while (Vector3.Distance(transform.position, target) > 0.05f)
                {
                    Vector3 direction = (target - transform.position).normalized;
                    transform.position += _moveSpeed * Time.deltaTime * direction;
                    yield return null;
                }

                transform.position = target;
                CurrentCell = cell;

                yield return null;
            }

            _moveRoutine = null;
            OnPathCompleted?.Invoke();
        }
    }

}
