using BHSTG.Constants;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using BHSTG.Player;

namespace BHSTG.Projectiles
{
    public abstract class IProjectile
    {
        // Current position of projectile
        public Vector2 position;
        public Vector2 Velocity; //Added
        public Texture2D texture;
        protected List<Movement> movements;

        public Rectangle Rectangle
        {
            get
            {
                return new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            }
        }

        /// <summary>
        /// Creates a projectile.
        /// </summary>
        /// <param name="iPos">Initial position</param>
        /// <param name="iTarget">Initial target.</param>
        /// <param name="iSpeed"></param>
        /// <param name="textureFile"></param>
        /// <param name="own"></param>
        public IProjectile(ref Texture2D textureFile, Vector2 iPos, List<Movement> movements)
        {
            texture = textureFile;
            position = iPos;
            this.movements = movements;
        }

        /// <summary>
        /// Moves the projectile along its path.
        /// More complex projectiles (seeking, exploding, etc) could implement
        /// additional functionality here.
        /// </summary>
        /// <param name="updateTime"></param>
        public virtual void Update(float updateTime, TimeSpan currentTime)
        {
            if (movements.Count > 0)
            {
                position = movements[0].Move(position, updateTime, currentTime);
                if (movements[0].IsFinished(position, currentTime))
                {
                    movements.RemoveAt(0);
                }
            }
        }

        //Added
        #region Collision
        public bool IsTouchingLeft(PlayerClass sprite)
        {
            return this.Rectangle.Right + this.position.X > sprite.Rectangle.Left &&
            this.Rectangle.Left < sprite.Rectangle.Left &&
            this.Rectangle.Bottom > sprite.Rectangle.Top &&
            this.Rectangle.Top < sprite.Rectangle.Bottom;
        }
        public bool IsTouchingRight(PlayerClass sprite)
        {
            return this.Rectangle.Left + this.position.X < sprite.Rectangle.Right &&
            this.Rectangle.Right > sprite.Rectangle.Right &&
            this.Rectangle.Bottom > sprite.Rectangle.Top &&
            this.Rectangle.Top < sprite.Rectangle.Bottom;
        }
        public bool IsTouchingTop(PlayerClass sprite)
        {
            return this.Rectangle.Bottom + this.position.Y > sprite.Rectangle.Top &&
            this.Rectangle.Top < sprite.Rectangle.Top &&
            this.Rectangle.Right > sprite.Rectangle.Left &&
            this.Rectangle.Left < sprite.Rectangle.Right;
        }
        public bool IsTouchingBottom(PlayerClass sprite)
        {
            return this.Rectangle.Top + this.position.Y < sprite.Rectangle.Bottom &&
            this.Rectangle.Bottom > sprite.Rectangle.Bottom &&
            this.Rectangle.Right > sprite.Rectangle.Left &&
            this.Rectangle.Left < sprite.Rectangle.Right;
        }
        #endregion
    }
}
