using BHSTG.Constants;
using BHSTG.Projectiles;
using BHSTG.Enemies;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace BHSTG.Player
{
    /// <summary>
    /// Interface for Enemy classes.
    /// </summary>
    public class PlayerClass : IEnemy
    {
        public Vector2 OriginalPosition;
        // speed of the character
        protected float speed;
        private float tempSpeed;
        protected int maxHealth;
        protected KeyboardState key;
        protected KeyboardState lastKey;

        Controller.LivesController Lives;

        public PlayerClass(
            Vector2 pos,
            ref Texture2D tex,
            int health,
            float mSpeed,
            double atkSpeed,
            ref ProjectileController projectileController,
            MovementWrapper movement,
            AttackWrapper attack,
             Controller.LivesController Lives) : base(
                pos,
                ref tex,
                health,
                atkSpeed,
                ref projectileController,
                movement,
                attack)
        {
                OriginalPosition = new Vector2(pos.X, pos.Y);
                maxHealth = health;
                speed = mSpeed;
                invincible = false;
                updatesSinceLastFired = 0;
            this.Lives = Lives;
        }

        /// <summary>
        /// Contains instructions for unit lifetime.
        /// On every Update call, unit should decide whether to move and how far,
        /// as well as shoot if able and desired.
        /// 
        /// This will contain the majority of the code for each Enemy, since it will
        /// decide the pattern of movement and attacks.
        /// </summary>
        public override void Update(float updateTime, TimeSpan currentTime)
        {
            updatesSinceLastFired++;
            move(updateTime, currentTime);
        }

        private void move(float updateTime, TimeSpan currentTime)
        {
            Vector2 newPos = movement.Move(position, updateTime, currentTime);
            double xMin = newPos.X;
            double yMin = newPos.Y;
            double xMax = newPos.X + texture.Width;
            double yMax = newPos.Y + texture.Height;

            if ( yMin > 0 && yMax < Numbers.WINDOW_HEIGHT && xMin > 0 && xMax < Numbers.WINDOW_WIDTH)
            {
                position = newPos;
            }

            attack.attack(this);
            if (Keyboard.GetState().IsKeyDown(Numbers.invincible))
            {
                invincible = true;
                abilityTimer(0);
            }
            if (Keyboard.GetState().IsKeyDown(Keys.H))
            {
                hard = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.E))
            {
                hard = false;
            }
        }

        public void resetHealth()
        {
            hp = maxHealth;
        }

        public void abilityTimer(double abilityTime)
        {
            if (abilityTime > 30.0)
            {
                invincible = false;
            }
        }

        public bool enteredHitboxPlayer(Vector2 position1)
        {
            bool collide = false;
            var x = position.X + texture.Width;
            var y = position.Y + texture.Height;

            if (position1.X > position.X && position1.X < x)
            {
                collide = true;
            }
            else if (position1.Y > position.Y && position1.Y < y)
            {
                collide = true;
            }

            return collide;
        }

        public void resetPosition()
        {
            position.X = OriginalPosition.X;
            position.Y = OriginalPosition.Y;
        }
        /// <summary>
        /// Calculates desired velocity of an object based on its position, target, and speed.
        /// </summary>
        /// <param name="position">Current position.</param>
        /// <param name="target">Object or point to head towards.</param>
        /// <param name="speed">Speed of object.</param>
        /// <returns>Velocity for use in Update() when moving the object.</returns>
        protected Vector2 CalculateVelocity(Vector2 position, Vector2 target, float speed)
        {
            Vector2 direction = target - position;
            direction.Normalize();
            direction.X *= speed;
            direction.Y *= speed;
            return direction;
        }

        public ref Vector2 GetPositionReference()
        {
            return ref position;
        }

        public new void TakeDamage(int dmg)
        {
            if (!invincible)
            {
                hp -= dmg;
                if (isDead())
                {
                    Lives.ResetPlayer();
                }
            }
        }

        public void setLivesController(Controller.LivesController newLives)
        {
            Lives = newLives;
        }
    }
}

