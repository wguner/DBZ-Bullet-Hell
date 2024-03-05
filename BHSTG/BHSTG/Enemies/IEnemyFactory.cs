using BHSTG.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;

namespace BHSTG.Enemies
{
    public abstract class IEnemyFactory
    {
        // List of 'default' values. This will be different for each Enemy class.
        // These should be set in the constructor of each Enemy class that inherits from this one.
        protected Texture2D enemyTexture;
        protected Vector2 defaultPosition;
        protected int defaultHP;
        protected double defaultAttackSpeed;
        protected int totalRequestedUnits;
        protected double spawnInterval;
        protected double minCompletionTime;
        protected double timeSinceLastSpawn;
        protected int enemiesSpawned;
        protected TimeSpan startTime;
        protected List<IEnemy> enemies;
        protected ProjectileController projController;
        protected MovementWrapper movement;
        protected AttackWrapper attack;

        /// <summary>
        /// Constructor. These parameters are required because they must be references.
        /// </summary>
        /// <param name="defTexture">Texture to use by default for a given type of Enemy.</param>
        /// <param name="projectiles">Reference to the master list of projectiles.</param>
        public IEnemyFactory(
            ref Texture2D defTexture,
            ref List<IEnemy> enemies,
            ref ProjectileController projectileController,
            Vector2 position,
            int HP,
            double atkSpeed,
            int requestedUnits,
            double spawnInterval,
            double minCompletionTime,
            MovementWrapper movement,
            AttackWrapper attack
            )
        {
            enemyTexture = defTexture;
            this.enemies = enemies;
            projController = projectileController;
            defaultPosition = position;
            defaultHP = HP;
            defaultAttackSpeed = atkSpeed;
            totalRequestedUnits = requestedUnits;
            this.spawnInterval = spawnInterval;
            this.minCompletionTime = minCompletionTime;
            timeSinceLastSpawn = 0;
            enemiesSpawned = 0;
            startTime = TimeSpan.Zero;
            this.movement = movement;
            this.attack = attack;

        }

        protected abstract IEnemy CreateEnemy();

        public abstract bool IsFinished(TimeSpan currentTime);

        public abstract void Update(float updateTime, TimeSpan currentTime);
    }
}
