using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using V.Tool.JuicyFeeling;

namespace V.TowerDefense
{
    public class SquashAndStretchChange : MonoBehaviour
    {
        [SerializeField] private SquashStretchSO[] squashStretchSOs;
        private SquashAndStretch _squashAndStretch;

        private void Start() 
        {
            _squashAndStretch = GetComponent<SquashAndStretch>();    
        }
        public void ChangeConfig()
        {
            int randomIndex = Random.Range(0, squashStretchSOs.Length);

            _squashAndStretch.config = squashStretchSOs[randomIndex];
        }
    }
}
