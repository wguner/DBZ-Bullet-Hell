using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace BHSTG.Projectiles
{
    /// <summary>
    /// The most basic version of a projectile.
    /// Additional versions should copy this format and also create
    /// a matching Factory class.
    /// </summary>
    internal class BasicProjectile : IProjectile
    {
        public BasicProjectile(ref Texture2D texture, Vector2 iPos, List<Movement> movements) : base(ref texture, iPos, movements)
        {

        }
    }
}
