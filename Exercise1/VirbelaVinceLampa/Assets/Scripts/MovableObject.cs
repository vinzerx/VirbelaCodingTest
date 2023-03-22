using UnityEngine;

namespace VirbelaTest
{
    public abstract class MovableObject : MonoBehaviour
    {
        protected Vector3 currentPosition;
        
        protected virtual void Update()
        {
            if (currentPosition != transform.position)
            {
                Manager.Instance.ReportMovableMoved(this);
                currentPosition = transform.position;
            }
        }

        protected void OnDestroy()
        {
            Manager.Instance.UnregisterMovable(this);
        }
    }
}
