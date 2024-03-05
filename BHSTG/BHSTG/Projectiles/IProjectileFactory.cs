using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BHSTG.Projectiles
{
    // Need to remove all usage of these factories.
    // Move CreatProjectile to ProjectileController.
    public abstract class IProjectileFactory
    {
        // List of 'default' values. Their values will be different for each Projectile class.
        // These should be set in the constructor of each class that inherits from this one.
        protected Texture2D defaultTexture;
        protected double defaultSpeed;

        public IProjectileFactory(ref Texture2D def)
        {
            defaultTexture = def;
        }

        /// <summary>
        /// This overload requires fewer arguments by making use of the default values set in the constructor.
        /// This one is preferred.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="direction"></param>
        /// <param name="speed"></param>
        /// <returns></returns>
        public abstract IProjectile CreateProjectile(Vector2 position, Vector2 direction);

        /// <summary>
        /// Creates a projectile of the requested type.
        /// Caller is responsible for adding the new Projectile to the appropriate list.
        /// 
        /// This overload requires all arguments.
        /// </summary>
        /// <param name="position">Starting position of projectile.</param>
        /// <param name="direction">Initial movement direction.</param>
        /// <param name="speed">Initial speed. May remove this and delegate control to Projectile classes.</param>
        /// <param name="texFile">Name of texture file. May remove this and delegate control to Projectile classes.</param>
        /// <param name="owner">Projectile owner, for collision handling.</param>
        /// <returns>IProjectile.</returns>
        public abstract IProjectile CreateProjectile(ref Texture2D texture, Vector2 position, Vector2 direction, double speed);
    }
}
