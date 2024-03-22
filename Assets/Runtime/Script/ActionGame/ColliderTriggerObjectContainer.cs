using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.ActionGame
{
    /// <summary>
    /// あたり判定範囲内のオブジェクト
    /// </summary>
    public class ColliderTriggerObjectContainer : MonoBehaviour
    {
        private List<GameObject> list = new List<GameObject>();
        public IReadOnlyList<GameObject> List => list;
        [SerializeField] private LayerMask layerMask;

        public void ManualRemoveElementFromList(GameObject[] removeObjects)
        {
            
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (1 << other.gameObject.layer == layerMask.value)
            {
                if(!list.Contains(other.gameObject)) list.Add(other.gameObject);
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (1 << other.gameObject.layer == layerMask.value)
            {
                if(list.Contains(other.gameObject)) list.Remove(other.gameObject);
            }
        }
    }
}
