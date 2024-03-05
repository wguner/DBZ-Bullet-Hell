using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using BHSTG.Enemies;
using BHSTG.Constants;

namespace BHSTG
{
    class PlayerMovement : Movement
    {
       
        double tempSpeed;
        public PlayerMovement(Vector2 target, double minCompletionTime, double speed) : base(target, minCompletionTime, speed)
        {
         
        }

        
        public new Vector2 Move(Vector2 currentPosition, float updateTime, TimeSpan currentTime)
        {
            Vector2 newPos = currentPosition;
            
            var keyUp = Keyboard.GetState().IsKeyDown(Keys.Up);
            var keyDown = Keyboard.GetState().IsKeyDown(Keys.Down);
            var keyLeft = Keyboard.GetState().IsKeyDown(Keys.Left);
            var keyRight = Keyboard.GetState().IsKeyDown(Keys.Right);

            if (Keyboard.GetState().IsKeyDown(Keys.X))
            {
                tempSpeed = base.getSpeed() / 2;
            }
            else
            {
                tempSpeed = base.getSpeed();
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
