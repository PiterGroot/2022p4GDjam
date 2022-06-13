using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace GameJam.Game
{
    public class GameRenderer : IDisposable
    {
        private bool canWin = true;
        public bool wonGame;
        private readonly GameContext context;
        private float frametime;
        private readonly Image image;
        private Font font = new Font("Monospace", 16);
        private Font tinyFont = new Font("Monospace", 10);
        private SolidBrush p1Brush = new SolidBrush(Color.Red);
        private SolidBrush p2Brush = new SolidBrush(Color.Blue);
        private SolidBrush Brush = new SolidBrush(Color.White);

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
                foreach (RenderObject powerUps in context.powerUps)
                {
                    RenderObject(g, powerUps);
                }
                RenderObject(g, context.player);
                RenderObject(g, context.player1);
                RenderObject(g, context.p1Heart);
                RenderObject(g, context.p2Heart);
                g.DrawString($"{context.p1BombCount}", tinyFont, Brush, -16, 0);
                g.DrawString($"{context.p2BombCount}", tinyFont, Brush, 272, 224);
            }
            else if(wonGame && canWin)
            {
                canWin = false;

                AudioManager.PlayMusic();
            }
            else if (wonGame)
            {
                if (context.winner == "Player 1")
                {
                    g.DrawString($"{context.winner} won!!!", font, p1Brush, (RenderForm.AppClientSize.Width / 2), 100);

                }
                else
                {
                    g.DrawString($"{context.winner} won!!!", font, p2Brush, (RenderForm.AppClientSize.Width / 2), 100);
                }
                g.DrawString($"press esc to retry", font, Brush, (RenderForm.AppClientSize.Width / 2), 120);
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


