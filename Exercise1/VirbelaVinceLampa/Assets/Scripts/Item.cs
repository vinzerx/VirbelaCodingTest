using UnityEngine;

namespace VirbelaTest
{
    public class Item : MovableObject
    {
        [SerializeField] private Renderer myRenderer;
        
        public void SetColor(Color newColor)
        {
            myRenderer.material.color = newColor;
        }
        
        void Awake()
        {
            Manager.Instance.RegisterItem(this);
        }
    }
}
