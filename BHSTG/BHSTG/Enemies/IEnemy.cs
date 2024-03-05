using BHSTG.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

// Enemies will no longer user projectile factories
// No longer move themselves in Update
// No longer hold functions for attacking
// Will now use ProjectileController for attacking
// Still controls rate of fire (attackSpeed),
// which is basically how often it calls ProjectileController.Attack()
// Probably doesn't need CalculateVelocity or MoveTowardsTarget anymore.
namespace BHSTG.Enemies
{
    /// <summary>
    /// Interface for Enemy classes.
    /// </summary>
    public abstract class IEnemy
    {

        public bool invincible = false;
        public bool hard = false;
        public Vector2 position;
        public Vector2 Velocity; //Added
        public Texture2D texture;
        protected int hp;
        protected double attackSpeed;
        protected ProjectileController projController;
        protected MovementWrapper movement;
        protected AttackWrapper attack;
        // Counter for controlling rate of fire in Update().
        protected int updatesSinceLastFired;

        public Rectangle Rectangle
        {
            get 
            {
                return new Rectangle((int)position.X, (int)position.Y, texture.Width, texture.Height);
            }
        }

        public IEnemy(
            Vector2 pos,
            ref Texture2D tex,
            int health,
            double atkSpeed,
            ref ProjectileController projList,
            MovementWrapper movement,
            AttackWrapper attack)
        {
            position = pos;
            texture = tex;
            hp = health;
            attackSpeed = atkSpeed;
            projController = projList;
            this.movement = movement;
            this.attack = attack;
        }

        /// <summary>
        /// Contains instructions for unit lifetime.
        /// On every Update call, unit should decide whether to move and how far,
        /// as well as shoot if able and desired.
        /// 
        /// This will contain the majority of the code for each Enemy, since it will
        /// decide the pattern of movement and attacks.
        /// </summary>
        public abstract void Update(float updateTime, TimeSpan currentTime);

        public bool enteredHitboxEnemy(Vector2 position1)
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

        public bool isDead()
        {
            return hp <= 0;
        }

        public void TakeDamage(int dmg)
        {
            hp -= dmg;
        }

        public int getUpdatesSinceLastFired()
        {
            return updatesSinceLastFired;
        }

        public void setUpdatesSinceLastFired(int num)
        {
            updatesSinceLastFired = num;
        }

        public void resetUpdatesSinceLastFired()
        {
            updatesSinceLastFired = 0;
        }

        public double getAttackSpeed()
        {
            return attackSpeed;
        }

        public ref ProjectileController getProjectileController()
        {
            return ref projController;
        }

    }
}
