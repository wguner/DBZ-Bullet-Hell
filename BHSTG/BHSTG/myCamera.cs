using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace BHSTG
{
    class myCamera
    {
        private Matrix flip;
        private Viewport viewport;
        private Vector2 center;
        private float zoomValue = 1;
        private float rotateValue = 0;

        public Matrix Flip
        {
            get { return flip; }
        }

        public float X
        {
            get { return center.X; }
            set { center.X = value; }
        }

        public float Y
        {
            get { return center.Y; }
            set { center.Y = value; }
        }

        public float ZoomValue
        {
            get { return zoomValue; }
            set { zoomValue = value; if (zoomValue < 0.1f) zoomValue = 0.1f; }
        }

        public float RotateValue
        {
            get { return rotateValue; }
            set { rotateValue = value; }
        }

        public myCamera(Viewport newViewport)
        {
            viewport = newViewport;
        }

        public void Update() //public void Update(Vector2 pos)
        {
            int width = viewport.Width;
            int height = viewport.Height;
            Vector2 pos = Vector2.Zero;
            //center = new Vector2(pos.X, pos.Y);
            center = new Vector2(width / 2f, height / 2f);
            flip = Matrix.CreateTranslation(new Vector3(-center.X, -center.Y, 0f))/*position of player*/ * Matrix.CreateRotationZ(rotateValue)/*rotates as you press rotate keys*/ * Matrix.CreateScale(new Vector3(ZoomValue, ZoomValue, 0))/*make it small depending on if your zoom in/out*/ * Matrix.CreateTranslation(new Vector3(viewport.Width / 2, viewport.Height / 2, 0));/*create translation to set it to center of screen*/
        }
    }
}