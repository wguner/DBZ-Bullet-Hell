using BHSTG.Constants;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

// Need to remove all usage of these factories.
// Move CreatProjectile to ProjectileController.
namespace BHSTG.Projectiles
{
    /// <summary>
    /// Used for creating a BasicProjectile. 
    /// </summary>
    //internal class BasicProjectileFactory : IProjectileFactory
    //{
    //    public BasicProjectileFactory(ref Texture2D defaultTex) : base(ref defaultTex)
    //    {
    //        // set defaults here
    //        defaultSpeed = 100f;
    //    }

    //    public override IProjectile CreateProjectile(Vector2 position, Vector2 direction)
    //    {
    //        return new BasicProjectile(ref defaultTexture, position, direction, defaultSpeed);
    //    }

    //    public override IProjectile CreateProjectile(ref Texture2D texture, Vector2 position, Vector2 direction, double speed)
    //    {
    //        return new BasicProjectile(ref texture, position, direction, speed);
    //    }
    //}
}
