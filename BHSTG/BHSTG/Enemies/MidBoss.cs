using BHSTG.Constants;
using BHSTG.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace BHSTG.Enemies
{
    /// <summary>
    /// Mid-boss derived from Enemy class. 
    /// </summary>
    internal class MidBoss : IEnemy
    {
        int stateCounter = 0;

        public MidBoss(
            Vector2 pos,
            ref Texture2D tex,
            int health,
            double atkSpeed,
            ref ProjectileController projList,
            MovementWrapper movement, 
            AttackWrapper attack) : base(
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
            /* if (movements.Count == 0)
             {
                 return;
             }
             if (movements[0].IsFinished(position, currentTime))
             {
                 movements.RemoveAt(0);
                 stateCounter++;
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

            if (this.hard == false)
            {
                if (updatesSinceLastFired - 50 >= attackSpeed)
                {
                    attack.attack(this);
                    updatesSinceLastFired = 0;
                }
                else
                {
                    updatesSinceLastFired++;
                }
            }
            else if (this.hard == true)
            {
                if (updatesSinceLastFired >= attackSpeed)
                {
                    attack.attack(this);
                    updatesSinceLastFired = 0;
                }
                else
                {
                    updatesSinceLastFired++;
                }
            }

            // This determines the rate at which this unit fires.
            /*
            if (updatesSinceLastFired >= attackSpeed)
            {
                if (stateCounter % 2 == 0)
                {
                    HalfCircleAttack();
                }
                else
                {
                    FullCircleAttack();
                }
                updatesSinceLastFired = 0;
            }
            else
            {
                updatesSinceLastFired++;
            }
            */

            //attack.attack(this); THIS WAS NOT COMMENTED
        }

        private void HalfCircleAttack()
        {
            Vector2 center = new Vector2(position.X, position.Y);
            center.X += texture.Width / 2;
            center.Y += texture.Height / 2;
            projController.HalfCircleAttack(position, Numbers.DEFAULT_PROJECTILE_SPEED, isEnemy: true);
        }

        private void FullCircleAttack()
        {
            projController.CircleAttack(position, Numbers.DEFAULT_PROJECTILE_SPEED, isEnemy: true);
        }
    }
}
