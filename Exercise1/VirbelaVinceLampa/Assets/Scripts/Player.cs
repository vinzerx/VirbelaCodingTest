namespace VirbelaTest
{
    public class Player : MovableObject
    {
        private void Awake()
        {
            currentPosition = transform.position;
            Manager.Instance.RegisterPlayer(this);
        }
    }
}
