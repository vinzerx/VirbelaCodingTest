using UnityEngine;

namespace VirbelaTest
{
    /// <summary>
    /// Script for Item instances in the scene.
    /// </summary>
    public class Item : MovableObject
    {
        [SerializeField] private Renderer myRenderer;
        
        /// <summary>
        /// Sets the color for the Item instance.
        /// </summary>
        /// <param name="newColor">Color to change to.</param>
        public void SetColor(Color newColor)
        {
            myRenderer.material.color = newColor;
        }
        
        void Awake()
        {
            //let manager know of the new item created
            Manager.Instance.RegisterItem(this);
        }
    }
}
