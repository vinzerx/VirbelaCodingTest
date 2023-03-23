using UnityEngine;

namespace VirbelaTest
{
    /// <summary>
    /// Superclass for all movable objects in the scene.
    /// </summary>
    /// <seealso cref="Bot">Bot</seealso>
    /// <seealso cref="Item">Item</seealso>
    /// <seealso cref="Player">Player</seealso>
    public abstract class MovableObject : MonoBehaviour
    {
        protected Vector3 currentPosition;
        
        protected virtual void Update()
        {
            //checks if the movable object has moved; ideally this would be handled as an event
            //instead of checking on the update loop
            if (currentPosition != transform.position)
            {
                Manager.Instance.ReportMovableMoved(this);
                currentPosition = transform.position;
            }
        }

        protected void OnDestroy()
        {
            //ensure object is not checked by the manager anymore when destroyed
            Manager.Instance.UnregisterMovable(this);
        }
    }
}
