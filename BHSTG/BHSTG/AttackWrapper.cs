using BHSTG.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;
using BHSTG.Constants;
using BHSTG.Enemies;

namespace BHSTG
{
    public abstract class AttackWrapper
    {
        public abstract void attack(IEnemy firingUnit);
    }

    public class straightShot : AttackWrapper
    {
        public override void attack(IEnemy firingUnit)
        {
            Vector2 bottomMiddle = firingUnit.position;
            bottomMiddle.X += firingUnit.texture.Width / 2;
            bottomMiddle.Y += firingUnit.texture.Height;

            Vector2 downwards = new Vector2(bottomMiddle.X, bottomMiddle.Y + Numbers.WINDOW_HEIGHT);

            // Fire a single projectile downwards
            firingUnit.getProjectileController().LinearAttack(bottomMiddle, downwards, Numbers.DEFAULT_PROJECTILE_SPEED, isEnemy: true);
        }
    }

    public class trackingShot : AttackWrapper
    {
        public override void attack(IEnemy firingUnit)
        {
            // Calculate starting position of projectile.
            Vector2 bottomMiddle = firingUnit.position;
            bottomMiddle.X += firingUnit.texture.Width / 2;
            bottomMiddle.Y += firingUnit.texture.Height;

            firingUnit.getProjectileController().LinearAttackTargetingPlayer(bottomMiddle, Numbers.DEFAULT_PROJECTILE_SPEED);
        }
    }

    public class MidBossAttack : AttackWrapper
    {
        int stateCounter = 0;
        public override void attack(IEnemy firingUnit)
        {
            if (firingUnit.getUpdatesSinceLastFired() >= firingUnit.getAttackSpeed())
            {
                if (stateCounter % 2 == 0)
                {
                    Vector2 center = new Vector2(firingUnit.position.X, firingUnit.position.Y);
                    center.X += firingUnit.texture.Width / 2;
                    center.Y += firingUnit.texture.Height / 2;
                    firingUnit.getProjectileController().HalfCircleAttack(firingUnit.position, Numbers.DEFAULT_PROJECTILE_SPEED, isEnemy: true);
                }
                else
                {

                    firingUnit.getProjectileController().CircleAttack(firingUnit.position, Numbers.DEFAULT_PROJECTILE_SPEED, isEnemy: true);
                }
                firingUnit.setUpdatesSinceLastFired(0);
            }
            else
            {
               firingUnit.setUpdatesSinceLastFired(firingUnit.getUpdatesSinceLastFired()+1);
            }
        }
    }

    public class FinalBossAttack : AttackWrapper
    {
        int stateCounter = 0;
        public override void attack(IEnemy firingUnit)
        {
            if (firingUnit.getUpdatesSinceLastFired()>= firingUnit.getAttackSpeed())
            {

                Vector2 center = firingUnit.position;
                center.X += firingUnit.texture.Width / 2;
                center.Y += firingUnit.texture.Height / 2;

                firingUnit.getProjectileController().CircleAttack(center, Numbers.DEFAULT_PROJECTILE_SPEED, isEnemy: true);

                if (stateCounter % 2 == 0)
                {
                    center = firingUnit.position;
                    center.X += firingUnit.texture.Width / 2;
                    center.Y += firingUnit.texture.Height / 2;

                    firingUnit.getProjectileController().ArrowAttack(center, Numbers.DEFAULT_PROJECTILE_SPEED);
                }
                else
                {
                    center = firingUnit.position;
                    center.X += firingUnit.texture.Width / 2;
                    center.Y += firingUnit.texture.Height / 2;

                    firingUnit.getProjectileController().ShotGunAttack(center, Numbers.DEFAULT_PROJECTILE_SPEED);
                }
                firingUnit.setUpdatesSinceLastFired(0);
            }
            else
            {
                firingUnit.setUpdatesSinceLastFired(firingUnit.getUpdatesSinceLastFired() +1);
            }
        }
    }

    public class PlayerAttack : AttackWrapper
    {
        double updatesSinceAttack = 0;
        public override void attack(IEnemy firingUnit)
        {
           
            if (Keyboard.GetState().IsKeyDown(Numbers.fire))
            {
                if (updatesSinceAttack >= firingUnit.getAttackSpeed())
                {
                    // Calculate starting position of projectile.
                    Vector2 topMiddle = firingUnit.position;
                    topMiddle.X += firingUnit.texture.Width / 2;

                    Vector2 upwards = new Vector2(topMiddle.X, topMiddle.Y - Numbers.WINDOW_HEIGHT);

                    // Fire a single projectile downwards
                    firingUnit.getProjectileController().LinearAttack(topMiddle, upwards, Numbers.DEFAULT_PROJECTILE_SPEED * 3, isEnemy: false);
                    firingUnit.setUpdatesSinceLastFired(0);
                    updatesSinceAttack = 0;
                }
            }
            updatesSinceAttack++;
        }
    }
}
