using System;
using Microsoft.Xna.Framework;

namespace BHSTG
{
    // Basically finished
    // If minCompletionTime = 0, Movement.IsFinished() returns true when
    // unit is at target position
    // Else, IsFinished() returns true when unit is at target position AND
    // when elapsedTime > minCompletionTime
    // Call Movement.Move to get new position for unit

    public abstract class baseMovement{
        public abstract Vector2 Move(Vector2 currentPosition, float updateTime, TimeSpan currentTime);
    }
    public class Movement : baseMovement
    {
        Vector2 target;
        TimeSpan startTime;
        double minCompletionTime;
        double speed;

        public Movement(Vector2 target, TimeSpan startTime, double minCompletionTime, double speed)
        {
            this.target = target;
            this.startTime = startTime;
            this.minCompletionTime = minCompletionTime;
            this.speed = speed;
        }

        public Movement(Vector2 target, double minCompletionTime, double speed)
        {
            this.target = target;
            this.minCompletionTime = minCompletionTime;
            this.speed = speed;
        }

        //add another class (finalbossmovement.cs to this movement) ability to change the movements easier
        public override Vector2 Move(Vector2 currentPosition, float updateTime, TimeSpan currentTime)
        {
            if (startTime == TimeSpan.Zero)
            {
                startTime = currentTime;
            }
            Vector2 newPosition = currentPosition;
            Vector2 velocity = CalculateVelocity(currentPosition, target, (float)speed);
            if (!ApproximatelyEqual(currentPosition, target))
            {
                newPosition.X = currentPosition.X + velocity.X * updateTime;
                newPosition.Y = currentPosition.Y + velocity.Y * updateTime;
            }
            return newPosition;
        }

        public bool IsFinished(Vector2 currentPosition, TimeSpan currentTime)
        {
            return ApproximatelyEqual(currentPosition, target) && (currentTime - startTime).TotalSeconds >= minCompletionTime;
        }

        private bool ApproximatelyEqual(Vector2 x1, Vector2 x2)
        {
            return Math.Abs(x1.X - x2.X) < 5 && Math.Abs(x1.Y - x2.Y) < 5;
        }
        public double getSpeed()
        {
            return speed;
        }
        protected Vector2 CalculateVelocity(Vector2 position, Vector2 target, float speed)
        {
            Vector2 direction = target - position;
            direction.Normalize();
            direction.X *= speed;
            direction.Y *= speed;
            return direction;
        }
    }
}
