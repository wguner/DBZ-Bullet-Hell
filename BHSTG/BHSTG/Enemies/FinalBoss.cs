using BHSTG.Constants;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

namespace BHSTG.Enemies
{
    /// <summary>
    /// The most basic version of a final boss.
    /// </summary>
    internal class FinalBoss : IEnemy
    {
        int stateCounter;

        public FinalBoss(
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

            // This determines the rate at which this unit fires.
            /*
            if (updatesSinceLastFired >= attackSpeed)
            {
                FullCircleAttack();

                if (stateCounter % 2 == 0)
                {
                    ArrowAttack();
                }
                else
                {
                    ShotgunAttack();
                }
                updatesSinceLastFired = 0;
            }
            else
            {
                updatesSinceLastFired++;
            }*/
            //attack.attack(this);
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
        }

        private void FullCircleAttack()
        {
            Vector2 center = position;
            center.X += texture.Width / 2;
            center.Y += texture.Height / 2;

            projController.CircleAttack(center, Numbers.DEFAULT_PROJECTILE_SPEED, isEnemy: true);
        }

        private void ArrowAttack()
        {
            Vector2 center = position;
            center.X += texture.Width / 2;
            center.Y += texture.Height / 2;

            projController.ArrowAttack(center, Numbers.DEFAULT_PROJECTILE_SPEED);
        }

        private void ShotgunAttack()
        {
            Vector2 center = position;
            center.X += texture.Width / 2;
            center.Y += texture.Height / 2;

            projController.ShotGunAttack(center, Numbers.DEFAULT_PROJECTILE_SPEED);
        }
    }
}
