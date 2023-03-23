using UnityEngine;

namespace VirbelaTest
{
    /// <summary>
    /// Script for Bot instances in the scene.
    /// </summary>
    public class Bot : MovableObject
    {
        [SerializeField] private Renderer myRenderer;
        
        /// <summary>
        /// Sets the color for the Bot instance.
        /// </summary>
        /// <param name="newColor">Color to change to.</param>
        public void SetColor(Color newColor)
        {
            myRenderer.material.color = newColor;
        }
        
        void Awake()
        {
            //let manager know of the new bot created
            Manager.Instance.RegisterBot(this);
        }
    }
}
