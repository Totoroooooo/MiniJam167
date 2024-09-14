using System;
using UnityEngine;

namespace MiniJam167.Utility
{
    [Serializable] public struct VisualPart
    {
        public SpriteRenderer Renderer;
        public Sprite NormalSprite;
        public Sprite CorruptedSprite;
    }
}