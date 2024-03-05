using BHSTG.Constants;
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
    /// The most basic version of an enemy.
    /// Additional versions should copy this format and also create
    /// a matching Factory class.
    /// 
    /// See comments on Update and Attack for specifics on functionality.
    /// </summary>
    internal class AvgBear : IEnemy
    {
        public AvgBear(
            Vector2 pos,
            ref Texture2D tex,
            int health,
            double atkSpeed,
            ref ProjectileController projList,
            MovementWrapper movement,
            AttackWrapper attack
            ) : base(
                pos,
                ref tex,
                health,
                atkSpeed,
                ref projList,
                movement,
                attack)
        {
        }

        public override void Update(float updateTime, TimeSpan currentTime)
        {
            /*if (movements.Count == 0)
            {
                return;
            }
            if (movements[0].IsFinished(position, currentTime))
            {
                movements.RemoveAt(0);
            }
            position = movements[0].Move(position, updateTime, currentTime);*/

            if (movement.movementIsEmpty())
            {
                return;
            }
            if (movement.movementIsFinished(position, currentTime))
            {
                movement.removeMovement();
            }
            position = movement.Move(position, updateTime, currentTime);

            // This determines the rate at which this unit fires.
            if (this.hard == false)
            {
                this.attackSpeed = 80;
                if (updatesSinceLastFired >= attackSpeed)
                {
                    Attack();
                    updatesSinceLastFired = 0;
                }
                else
                {
                    updatesSinceLastFired++;
                }
            }
            else
            {
                this.attackSpeed = 23; //default value is 80
                if (updatesSinceLastFired >= attackSpeed)
                {
                    Attack();
                    updatesSinceLastFired = 0;
                }
                else
                {
                    updatesSinceLastFired++;
                }
            }
        }

        /// <summary>
        /// Performs a simple attack, firing one projectile.
        /// Multiple versions of this function could be created with
        /// varying sets of projectiles being spawned, particularly for bosses.
        /// 
        /// Enemies attack by simply creating a projectile with the desired attributes and
        /// adding it to the master list owned by Game1. The Update function in Game1 then
        /// handles the projectiles completely and Enemy no longer controls them.
        /// </summary>
        private void Attack()
        {
            /* // Calculate starting position of projectile.
             Vector2 bottomMiddle = position;
             bottomMiddle.X += texture.Width / 2;
             bottomMiddle.Y += texture.Height;

             Vector2 downwards = new Vector2(bottomMiddle.X, bottomMiddle.Y + Numbers.WINDOW_HEIGHT);

             // Fire a single projectile downwards
             projController.LinearAttackTargetingPlayer(bottomMiddle, Numbers.DEFAULT_PROJECTILE_SPEED);*/
            attack.attack(this);
        }

    }
}
