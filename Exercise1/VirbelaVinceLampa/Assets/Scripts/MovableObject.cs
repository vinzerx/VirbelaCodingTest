using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace VirbelaTest
{
    public abstract class MovableObject : MonoBehaviour
    {
        protected Vector3 currentPosition;
    
        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        protected virtual void Update()
        {
            if (currentPosition != transform.position)
            {
                Manager.Instance.ReportMovableMoved(this);
                currentPosition = transform.position;
            }
        }
    }
}
