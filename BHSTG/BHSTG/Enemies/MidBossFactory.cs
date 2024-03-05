using BHSTG.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace BHSTG.Enemies
{
    class MidBossFactory : IEnemyFactory
    {
        public MidBossFactory(
            ref Texture2D enemyTexture,
            ref List<IEnemy> enemies,
            ref ProjectileController projectileController,
            Vector2 position,
            int HP,
            double atkSpeed,
            int requestedUnits,
            double spawnInterval,
            double minCompletionTime,
            MovementWrapper movement,
            AttackWrapper attack) : base(
                ref enemyTexture,
                ref enemies,
                ref projectileController,
                position,
                HP,
                atkSpeed,
                requestedUnits,
                spawnInterval,
                minCompletionTime,
                movement,
                attack)
        {
        }

        /// <summary>
        /// Use this overload if you wish to use the default values specified in the Factory constructor.
        /// This way, fewer values need to be specified.
        /// </summary>
        /// <param name="direction">Which direction to move initially.</param>
        /// <param name="factories">Projectile factories for this unit to use.</param>
        /// <returns>An enemy object.</returns>
        protected override IEnemy CreateEnemy()
        {
            enemiesSpawned++;
            return new MidBoss(defaultPosition, ref enemyTexture, defaultHP, defaultAttackSpeed, ref projController, movement, attack);
        }

        // if # of enemies created == number of requested units
        // AND minCompletionTime is met (totalElapsedTime > minCompletionTime)
        // return true so this factory can be deleted
        public override bool IsFinished(TimeSpan currentTime)
        {
            return enemiesSpawned >= totalRequestedUnits &&
                (currentTime - startTime).TotalSeconds >= minCompletionTime;
        }

        public override void Update(float updateTime, TimeSpan currentTime)
        {
            // Start time is not initialized until the factory is at the front of the 
            // list of factories and begins spawning enemies.
            if (startTime == TimeSpan.Zero)
            {
                startTime = currentTime;
            }
            // Timing for creating enemies
            // Spawn a new enemy when time since last spawn is >= spawn interval
            // update a counter for number of enemies created
            timeSinceLastSpawn += updateTime;
            if (enemiesSpawned < totalRequestedUnits && timeSinceLastSpawn >= spawnInterval)
            {
                enemies.Add(CreateEnemy());
                timeSinceLastSpawn = 0;
            }
        }

        //public MidBossFactory(ref Texture2D defTexture, ref List<IProjectile> projectiles, ref Vector2 playerPos) : base(ref defTexture, ref projectiles, ref playerPos)
        //{
        //    // Set default values here. They can be whatever we choose
        //    // for a particular Enemy type.
        //    defaultPosition = new Vector2(0, 200);
        //    defaultHP = 1;
        //    defaultSpeed = 200f;
        //    defaultAttackSpeed = 50;
        //    // Starts out moving straight across the screen.
        //    defaultDirection = defaultPosition + new Vector2(10, 0);
        //}

        ///// <summary>
        ///// Use this overload if you wish to use the default values specified in the Factory constructor.
        ///// This way, fewer values need to be specified.
        ///// </summary>
        ///// <param name="direction">Which direction to move initially.</param>
        ///// <param name="factories">Projectile factories for this unit to use.</param>
        ///// <returns>An enemy object.</returns>
        //public override IEnemy CreateEnemy(Vector2 direction, List<IProjectileFactory> factories)
        //{
        //    return new MidBoss(defaultPosition, ref enemyTexture, direction, defaultHP, defaultSpeed, defaultAttackSpeed, factories, ref defaultProjectilesList, ref playerPosition);
        //}

        ///// <summary>
        ///// Use this overload for more control over the unit attributes.
        ///// In general this should be unnecessary, and creating a new Factory
        ///// with a different set of default values would be preferred.
        ///// </summary>
        ///// <param name="pos">Initial position.</param>
        ///// <param name="tex">Texture to use.</param>
        ///// <param name="direction">Which direction to move initially.</param>
        ///// <param name="HP">Number of hit points.</param>
        ///// <param name="movementSpeed">Movement speed.</param>
        ///// <param name="atkSpeed">Rate of attack.</param>
        ///// <param name="factories">Projectile factories for this unit to use.</param>
        ///// <param name="projectiles">Master list of projectiles.</param>
        ///// <returns>An enemy object.</returns>
        //public override IEnemy CreateEnemy(Vector2 pos, Texture2D tex, Vector2 direction, int HP, double movementSpeed, double atkSpeed, List<IProjectileFactory> factories, ref List<IProjectile> projectiles)
        //{
        //    return new MidBoss(pos, ref tex, direction, HP, movementSpeed, atkSpeed, factories, ref projectiles, ref playerPosition);
        //}
    }
}
