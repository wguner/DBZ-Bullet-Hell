using BHSTG.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace BHSTG.Enemies
{
    /// <summary>
    /// Factory to go along with the BasicEnemy class.
    /// </summary>
    internal class AvgBearFactory : IEnemyFactory
    {
        public AvgBearFactory(
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
            return new AvgBear(defaultPosition, ref enemyTexture, defaultHP, defaultAttackSpeed, ref projController, movement, attack);
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
            if (timeSinceLastSpawn >= spawnInterval)
            {
                enemies.Add(CreateEnemy());
                timeSinceLastSpawn = 0;
            }
        }
    }
}
