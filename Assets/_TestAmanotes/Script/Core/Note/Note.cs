using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TestAmanotes
{
    public class Note : MonoBehaviour, IPoolable
    {
        [SerializeField] private GameObject[] _childNoteTiles;
        public float fallSpeed = 5f; // Speed at which the object falls

        private bool _runable;
        public HashSet<Vector3> GetAllPosition()
        {
          var data=  _childNoteTiles.Select(a => a.transform.position).ToHashSet();
          return data;
        }

        public void OnObjectSpawn()
        {
            _runable = true;
        }

        public void OnObjectDisabled()
        {
            _runable = false;
        }

        private void FixedUpdate()
        {
            transform.position += Vector3.down * fallSpeed * Time.deltaTime;
        }
    }

}
