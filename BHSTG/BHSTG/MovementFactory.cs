using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Text;
using BHSTG.Enemies;

namespace BHSTG
{
    public static class MovementFactory
    {
        public static List<Movement> CreateLinearMovement(Vector2 target, double minCompletionTime, double speed)
        {
            return new List<Movement>() { new Movement(target, minCompletionTime, speed) };
        }

        public static List<Movement> CreateCircularMovement()
        {
            throw new NotImplementedException();
        }

        public static List<Movement> CreatePlayerMovement( Vector2 target, double minCompletionTime, double speed)
        {
            return new List<Movement>() { new PlayerMovement(target, minCompletionTime, speed) };
        }
    }
}
