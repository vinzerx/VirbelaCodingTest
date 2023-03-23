namespace VirbelaTest
{
    /// <summary>
    /// Script for the Player instance in the scene.
    /// </summary>
    public class Player : MovableObject
    {
        private void Awake()
        {
            currentPosition = transform.position;
            
            //let manager know which object is the player
            Manager.Instance.RegisterPlayer(this);
        }
    }
}
