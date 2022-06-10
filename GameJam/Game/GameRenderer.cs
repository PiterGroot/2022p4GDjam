using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GameJam.Game
{
    public class GameRenderer : IDisposable
    {
        public bool wonGame;
        private readonly GameContext context;
        private float frametime;
        private readonly Image image;
        private Font font = new Font("Monospace", 16);
        private SolidBrush drawBrush = new SolidBrush(Color.White);

        public GameRenderer(GameContext context)
        {
            this.context = context;
            image = Bitmap.FromFile("sprites.png");

        }
        private Graphics InitGraphics(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            //make nice pixels
            g.SmoothingMode = SmoothingMode.None;
            g.InterpolationMode = InterpolationMode.NearestNeighbor;


            g.Transform = new Matrix();
            g.ScaleTransform(context.scaleunit, context.scaleunit);
            //there will be some tearing between tiles, a solution to that is to render to a bitmap then draw that bitmap, fun challenge?
            g.Clear(Color.Black);
            return g;
        }
        internal void Render(PaintEventArgs e, float frametime)
        {
            this.frametime = frametime;

            Graphics g = InitGraphics(e);
            if (!wonGame)
            {
                //  Console.WriteLine(RenderForm.AppClientSize.Width /2);
                g.TranslateTransform(RenderForm.AppClientSize.Width / 2, 25);
                RenderFloor(g);
                foreach (RenderObject explosions in context.explosionTiles)
                {
                    RenderObject(g, explosions);
                }
                RenderWalls(g);
                foreach (RenderObject bomb in context.bombs)
                {
                    RenderObject(g, bomb);
                }
                RenderObject(g, context.player);
                RenderObject(g, context.player1);
                RenderObject(g, context.p1Heart);
                RenderObject(g, context.p2Heart);

            }
            else
            {
                g.DrawString($"{context.winner} won!!!", font, drawBrush, (RenderForm.AppClientSize.Width / 2), 100);
            }
        }

        private void RenderFloor(Graphics g)
        {
            foreach (Tile[] row in context.room.tiles)
            {
                foreach (Tile t in row)
                {
                    if(t.graphic == '.')
                    {
                        g.DrawImage(image, t.rectangle, t.sprite, GraphicsUnit.Pixel);
                    }
                }
            }
        }
        private void RenderWalls(Graphics g)
        {
            foreach (Tile[] row in context.room.tiles)
            {
                foreach (Tile t in row)
                {
                    if (t.graphic == '#' || t.graphic == ',')
                    {
                        g.DrawImage(image, t.rectangle, t.sprite, GraphicsUnit.Pixel);
                    }
                }
            }
        }

        private void RenderObject(Graphics g, RenderObject renderObject)
        {
            g.DrawImage(image, renderObject.rectangle, renderObject.frames[(int)renderObject.frame], GraphicsUnit.Pixel);
            renderObject.MoveFrame(frametime);
        }

        public void Dispose()
        {
            image.Dispose();
        }
    }

}


