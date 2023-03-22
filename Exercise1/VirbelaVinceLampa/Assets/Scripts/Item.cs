using System.Collections;
using System.Collections.Generic;
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
            myRenderer.material = Instantiate(Resources.Load("ItemMat") as Material);
            Manager.Instance.RegisterItem(this);
        }
    }
}
