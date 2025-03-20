using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TestAmanotes
{
    public class Note : MonoBehaviour, IPoolable
    {
        [SerializeField] private GameObject[] _childNoteTiles;
        public HashSet<Vector3> GetAllPosition()
        {
          var data=  _childNoteTiles.Select(a => a.transform.position).ToHashSet();
          return data;
        }

        public void OnObjectSpawn()
        {
            
        }
    }

}
