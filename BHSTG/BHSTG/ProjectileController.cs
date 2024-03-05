using BHSTG.Constants;
using BHSTG.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using maths = System.Numerics;

namespace BHSTG
{
    // Incomplete function definitions.
    public class ProjectileController
    {
        public List<IProjectile> enemyProjectiles;
        public List<IProjectile> playerProjectiles;
        Vector2 playerPosition;
        Texture2D projTex1;
        Texture2D projTex2;
        Texture2D projTex3;

        public ProjectileController(ref Texture2D texture1, ref Texture2D texture2, Vector2 playerPos)
        {
            enemyProjectiles = new List<IProjectile>();
            playerProjectiles = new List<IProjectile>();
            projTex1 = texture1;
            projTex2 = texture2;
            playerPosition = playerPos;
        }

        /********************************************************
         * 
         * ATTACK FUNCTIONS
         * 
         * ******************************************************/

        public void LinearAttack(Vector2 source, Vector2 target, double speed, bool isEnemy)
        {
            if (isEnemy)
            {
                enemyProjectiles.Add(CreateProjectile(source, target, ref projTex1, speed));
            }
            else
            {
                playerProjectiles.Add(CreateProjectile(source, target, ref projTex1, speed));
            }
        }

        public void LinearAttackTargetingPlayer(Vector2 source, double speed)
        {

            enemyProjectiles.Add(CreateProjectile(source, ShootPast(source, playerPosition), ref projTex2, speed));
        }

        public void HalfCircleAttack(Vector2 unitLocation, double speed, bool isEnemy)
        {
            int projectileCount = 10;
            for (int i = 0; i < projectileCount; i++)
            {
                // Starting point
                maths.Complex p1 = maths.Complex.FromPolarCoordinates(100, (179 + (i * 180 / projectileCount)) * Math.PI / 180);
                Vector2 projectileLocation = new Vector2(unitLocation.X, unitLocation.Y);
                projectileLocation.X += (float)p1.Real;
                projectileLocation.Y -= (float)p1.Imaginary;

                // Direction
                Vector2 target = ShootPast(unitLocation, projectileLocation);

                enemyProjectiles.Add(CreateProjectile(projectileLocation, target, ref projTex1, speed));
            }
        }

        public void CircleAttack(Vector2 unitLocation, double speed, bool isEnemy)
        {
            int projectileCount = 20;
            for (int i = 0; i < projectileCount; i++)
            {
                // Starting point
                maths.Complex p1 = maths.Complex.FromPolarCoordinates(100, (i * 179 / projectileCount) * 2*Math.PI / 180);
                Vector2 projectileLocation = new Vector2(unitLocation.X, unitLocation.Y);
                projectileLocation.X += (float)p1.Real;
                projectileLocation.Y -= (float)p1.Imaginary;

                // Direction
                Vector2 target = ShootPast(unitLocation, projectileLocation);

                enemyProjectiles.Add(CreateProjectile(projectileLocation, target, ref projTex1, speed));
            }
        }

        /*
         *  0
         *      0
         *  0       0   ->        X
         *      0
         *  0
         * 
         */
        public void ArrowAttack(Vector2 source, double speed)
        {
            float pointOffset = 75;
            float tailOffset = 40;
            float rowSpacing = 5;

            Vector2 diff = (playerPosition - source);
            diff.Normalize();
            Vector2 perpClockwise = PerpendicularClockwise(diff);
            perpClockwise.Normalize();
            Vector2 perpCounter = PerpendicularCounterClockwise(diff);
            perpCounter.Normalize();

            Vector2 p1StartingPoint = pointOffset * diff + source;
            Vector2 target = ShootPast(source, playerPosition);
            enemyProjectiles.Add(CreateProjectile(p1StartingPoint, target, ref projTex2, speed));

            Vector2 p2StartingPoint = tailOffset * diff + source;
            enemyProjectiles.Add(CreateProjectile(p2StartingPoint, target, ref projTex2, speed));

            Vector2 p3 = ((pointOffset + tailOffset) / 2) * diff + source;
            p3 += rowSpacing * perpClockwise;
            enemyProjectiles.Add(CreateProjectile(p3, target, ref projTex2, speed));

            Vector2 p4 = ((pointOffset + tailOffset) / 2) * diff + source;
            p4 += rowSpacing * perpCounter;
            enemyProjectiles.Add(CreateProjectile(p4, target, ref projTex2, speed));

            Vector2 p5 = tailOffset * diff + source;
            p5 += rowSpacing * 1.5f * perpClockwise;
            enemyProjectiles.Add(CreateProjectile(p5, target, ref projTex2, speed));

            Vector2 p6 = tailOffset * diff + source;
            p6 += rowSpacing * 1.5f * perpCounter;
            enemyProjectiles.Add(CreateProjectile(p6, target, ref projTex2, speed));
        }

        public void ShotGunAttack(Vector2 source, double speed)
        {

        }

        private IProjectile CreateProjectile(Vector2 position, Vector2 target, ref Texture2D texture, double speed)
        {
            return new BasicProjectile(ref texture, position, MovementFactory.CreateLinearMovement(target, minCompletionTime: 0, speed));
        }


        public void Update(float elapsedTime, TimeSpan currentTime, Vector2 newPlayerPos)
        {
            playerPosition = newPlayerPos;
            for (int i = 0; i < enemyProjectiles.Count; i++)
            {
                if (IsOutOfBounds(enemyProjectiles[i].position, enemyProjectiles[i].texture.Width, enemyProjectiles[i].texture.Height))
                {
                    enemyProjectiles.RemoveAt(i);
                }
                else
                {
                    enemyProjectiles[i].Update(elapsedTime, currentTime);
                }
            }
            for (int i = 0; i < playerProjectiles.Count; i++)
            {
                if (IsOutOfBounds(playerProjectiles[i].position, playerProjectiles[i].texture.Width, playerProjectiles[i].texture.Height))
                {
                    playerProjectiles.RemoveAt(i);
                }
                else
                {
                    playerProjectiles[i].Update(elapsedTime, currentTime);
                }
            }
        }

        private bool IsOutOfBounds(Vector2 position, double width, double height)
        {
            Vector2 midpoint = new Vector2((float)width / 2, (float)height / 2);
            midpoint += position;
            return midpoint.X < 0 || midpoint.X > Numbers.WINDOW_WIDTH || midpoint.Y < 0 || midpoint.Y > Numbers.WINDOW_HEIGHT;
        }

        // pedillo aqui
        // si tiran cuando el jugador esta justo abajo
        // del otro, tira parriba cuando debe ser pa bajo
        // si la ecuacion la tenemos bien, mejor solo multiplicar
        // la ubicacion del objetivo con cinco o diez. seria mas
        // confiable
        private Vector2 ShootPast(Vector2 position, Vector2 target)
        {
            Vector2 diff = target - position;
            if (Math.Abs(diff.X) < 2)
            {
                if (diff.X >= 0)
                {
                    diff.X += 1;
                }
                else
                {
                    diff.X -= 1;
                }
            }
            float slope = diff.Y / diff.X;
            float b = position.Y - (slope * position.X);
            float multiplyBy;
            if (diff.X > 0)
            {
                multiplyBy = 30;
            }
            else
            {
                multiplyBy = -30;
            }
            float testX = diff.X * 30;
            float testY = slope * testX + b;
            if (testX > Numbers.WINDOW_WIDTH || testX < 0 || testY > Numbers.WINDOW_WIDTH || testY < 0)
            {
                return new Vector2(testX, testY);
            }
            else
            {
                testY = diff.Y * 30;
                return new Vector2((testY - b) / slope, testY);
            }
        }

        private void GetPolarCoordinates(Vector2 point, out double r, out double t)
        {
            r = Math.Sqrt((point.X * point.X) + (point.Y * point.Y));
            t = Math.Atan2(point.Y, point.X);
        }

        private Vector2 PerpendicularClockwise(Vector2 vector)
        {
            return new Vector2(vector.Y, -vector.X);
        }

        private Vector2 PerpendicularCounterClockwise(Vector2 vector)
        {
            return new Vector2(-vector.Y, vector.X);
        }
    }
}
