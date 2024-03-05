using BHSTG.Player;
using Microsoft.Xna.Framework.Graphics;


namespace BHSTG.Controller
{
    public class LivesController
    {
        PlayerClass player;
        public Texture2D WinImage, LoseImage;
        int lives;
        public bool win = false, lose = false;
        float winTime;


        public LivesController(ref PlayerClass player, int lives, float winTime, ref Texture2D win, ref Texture2D lose)
        {
            this.player = player;
            this.lives = lives;
            this.WinImage = win;
            this.LoseImage = lose;
            this.winTime = winTime;
        }
        public void Update(float updateTime, bool finalBossIsDead)
        {
            if ((updateTime >= winTime || finalBossIsDead) && !lose)
            {
                win = true;
            }
        }

        public int ResetPlayer()
        {
            player.resetPosition();
            player.resetHealth();
            player.invincible = true;
            lives--;
            if(lives <= 0)
            {
                lose = true;
            }
            return lives;
        }

        public int getLives()
        {
            return lives;
        }
        
        
    }
}
