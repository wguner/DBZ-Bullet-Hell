using BHSTG.Constants;
using BHSTG.Projectiles;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Text;

namespace BHSTG
{
    public abstract class MovementWrapper
    {
        public List<Movement> movements;

        public abstract Vector2 Move(Vector2 pos, float updateTime, TimeSpan currentTime);

        public void removeMovement()
        {
            movements.RemoveAt(0);
        }

        public bool movementIsEmpty()
        {
            return movements.Count == 0;
        }

        public bool movementIsFinished(Vector2 position, TimeSpan currentTime)
        {
            return movements[0].IsFinished(position, currentTime);
        }
    }

    public class linearMove : MovementWrapper
    {
        public linearMove(Vector2 target, int minTime, double speed) : base()
        {
            movements = MovementFactory.CreateLinearMovement(target, minTime, speed);
        }

        public override Vector2 Move(Vector2 pos, float updateTime, TimeSpan currentTime)
        {
            return movements[0].Move(pos, updateTime, currentTime);
        }
    }

    public class zigZagMove : MovementWrapper
    {
        public zigZagMove(Vector2 start, int minTime, double speed) : base()
        {
            Vector2 target = new Vector2(start.X, start.Y);
            target.X += 100;
            target.Y += 100;
            movements = MovementFactory.CreateLinearMovement(new Vector2(target.X, target.Y), 0, speed);
            target.X += 100;
            target.Y -= 190;
            movements.AddRange(MovementFactory.CreateLinearMovement(new Vector2(target.X, target.Y), 0, speed));
            target.X += 100;
            target.Y += 190;
            movements.AddRange(MovementFactory.CreateLinearMovement(new Vector2(target.X, target.Y), 0, speed));
            target.X += 100;
            target.Y -= 190;
            movements.AddRange(MovementFactory.CreateLinearMovement(new Vector2(target.X, target.Y), 0, speed));
            target.X += 100;
            target.Y += 190;
            movements.AddRange(MovementFactory.CreateLinearMovement(new Vector2(target.X, target.Y), 0, speed));
            target.X += 100;
            target.Y -= 190;
            movements.AddRange(MovementFactory.CreateLinearMovement(new Vector2(target.X, target.Y), 0, speed));
            target.X += 100;
            target.Y += 190;
            movements.AddRange(MovementFactory.CreateLinearMovement(new Vector2(target.X, target.Y), 0, speed));
            target.X += 100;
            target.Y -= 190;
            movements.AddRange(MovementFactory.CreateLinearMovement(new Vector2(target.X, target.Y), 0, speed));

        }
        public override Vector2 Move(Vector2 pos, float updateTime, TimeSpan currentTime)
        {
            return movements[0].Move(pos, updateTime, currentTime);
        }
    }

    public class MidBossMovement : MovementWrapper
    {
        public MidBossMovement() : base()
        {
            double speed = 150;
            double minCompletionTime = 3.5;
            Vector2 startPosition = new Vector2(0, 200);
            Vector2 offScreen = new Vector2(-100, -100);
            Vector2 position1 = new Vector2(100, 100);
            Vector2 position2 = new Vector2(600, 150);

            movements = MovementFactory.CreateLinearMovement(startPosition, 0, speed);
            movements.AddRange(MovementFactory.CreateLinearMovement(position1, minCompletionTime, speed));
            movements.AddRange(MovementFactory.CreateLinearMovement(position2, minCompletionTime, speed));
            movements.AddRange(MovementFactory.CreateLinearMovement(position1, minCompletionTime, speed));
            movements.AddRange(MovementFactory.CreateLinearMovement(position2, minCompletionTime, speed));
            movements.AddRange(MovementFactory.CreateLinearMovement(position1, minCompletionTime, speed));
            movements.AddRange(MovementFactory.CreateLinearMovement(position2, minCompletionTime, speed));
            movements.AddRange(MovementFactory.CreateLinearMovement(offScreen, minCompletionTime, speed));

        }

        public override Vector2 Move(Vector2 pos, float updateTime, TimeSpan currentTime)
        {
            return movements[0].Move(pos, updateTime, currentTime);
        }
    }

    public class FinalBossMovement : MovementWrapper
    {
        public FinalBossMovement () : base()
        {
            Random rnd = new Random();
            Vector2 startPosition = new Vector2(750, 90);
            double speed = 200;
            double minCompletionTime = 3;
            List<Vector2> positions = new List<Vector2>()
            {
                new Vector2(150, 50),
                new Vector2(150, 150),
                new Vector2(550, 50),
                new Vector2(550, 150)
            };
            base.movements = MovementFactory.CreateLinearMovement(startPosition, 0, speed);
            for (int i = 0; i < 10; i++)
            {
                base.movements.AddRange(MovementFactory.CreateLinearMovement(positions[rnd.Next(0, 4)], minCompletionTime, speed));
            }
            base.movements.AddRange(MovementFactory.CreateLinearMovement(new Vector2(-100, -100), minCompletionTime, speed));

        }
        public override Vector2 Move(Vector2 pos, float updateTime, TimeSpan currentTime)
        {
            return movements[0].Move(pos, updateTime, currentTime);
        }
    }

    public class MovePlayer : MovementWrapper
    {
        double speed;

        public MovePlayer(double speed) : base()
        {
            this.speed = speed;
        }

        public override Vector2 Move(Vector2 pos, float updateTime, TimeSpan currentTime)
        {
            Vector2 newPos = pos;
            double tempSpeed;

            bool keyUp = Keyboard.GetState().IsKeyDown(Numbers.up);
            bool keyDown = Keyboard.GetState().IsKeyDown(Numbers.down);
            bool keyLeft = Keyboard.GetState().IsKeyDown(Numbers.left);
            bool keyRight = Keyboard.GetState().IsKeyDown(Numbers.right);

            if (Keyboard.GetState().IsKeyDown(Numbers.slow))
            {
                tempSpeed = this.speed/ 2;
            }
            else
            {
                tempSpeed = this.speed;
            }
            if (keyUp)
            {
                newPos.Y -= (int)tempSpeed;
            }
            if (keyDown)
            {
                newPos.Y += (int)tempSpeed;
            }
            if (keyLeft)
            {
                newPos.X -= (int)tempSpeed;
            }
            if (keyRight)
            {
                newPos.X += (int)tempSpeed;
            }

            return newPos;
        }

    }

}
