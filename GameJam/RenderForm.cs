using GameJam.Game;
using GameJam.Tools;
using System;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.Media;
using System.IO;
using System.Collections.Generic;
using MathNet;

namespace GameJam
{

    public partial class RenderForm : Form
    {
        private Vector2 p1Pos;
        private Vector2 p2Pos;

        private LevelLoader levelLoader;
        private float frametime;
        private GameRenderer renderer;
        private readonly GameContext gc = new GameContext();
        public RenderForm()
        {
            InitializeComponent();

            DoubleBuffered = true;
            ResizeRedraw = true;

            WindowState = FormWindowState.Normal;
            FormBorderStyle = FormBorderStyle.None;
            Bounds = Screen.PrimaryScreen.Bounds;

            KeyDown += RenderForm_KeyDown;
            FormClosing += Form1_FormClosing;
            Load += RenderForm_Load;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e) {
            renderer.Dispose();
        }
        private void RenderForm_Load(object sender, EventArgs e)
        {
            levelLoader = new LevelLoader(gc.tileSize, new FileLevelDataSource());
            levelLoader.LoadRooms(gc.spriteMap.GetMap());

            renderer = new GameRenderer(gc);

            gc.room = levelLoader.GetRoom(0, 0);
            InstantiateRenderObjects();
            ClientSize =
                 new Size(

                    (gc.tileSize * gc.room.tiles[0].Length) * gc.scaleunit,
                    (gc.tileSize * gc.room.tiles.Length) * gc.scaleunit
                    );
        }

        private void InstantiateRenderObjects()
        {
            p1Pos = new Vector2(432, 224);
            p2Pos = new Vector2(16, 16);

            gc.player = new RenderObject()
            {
                frames = gc.spriteMap.GetPlayerFrames(),
                rectangle = new Rectangle(16, 16, gc.tileSize, gc.tileSize),
            };

            gc.player1 = new RenderObject()
            {
                frames = gc.spriteMap.GetPlayer1Frames(),
                rectangle = new Rectangle(432, 224, gc.tileSize, gc.tileSize),
            };

            gc.p1Heart = new RenderObject()
            {
                frames = gc.GetSingeFrameArray('O'),
                rectangle = new Rectangle(0, 0, gc.tileSize, gc.tileSize),
            };
            gc.p2Heart = new RenderObject()
            {
                frames = gc.GetSingeFrameArray('O'),
                rectangle = new Rectangle(164, 0, gc.tileSize, gc.tileSize),
            };

        }

        private void RenderForm_KeyDown(object sender, KeyEventArgs e)
        {
            //first player controls
            if (e.KeyCode == Keys.W) { 
                MovePlayer(0, -1);
            }
            else if (e.KeyCode == Keys.S)
            {
                MovePlayer(0, 1);
            }
            else if (e.KeyCode == Keys.A)
            {
                MovePlayer(-1, 0);
            }
            else if (e.KeyCode == Keys.D)
            {
                MovePlayer(1, 0);
            }
            else if (e.KeyCode == Keys.Space)
            {
                new Bomb(gc, 2500, p2Pos, true);
            }

            //second player controls
            else if (e.KeyCode == Keys.Up) {
                MovePlayer1(0, -1);
            }
            else if (e.KeyCode == Keys.Right) {
                MovePlayer1(1, 0);
            }
            else if (e.KeyCode == Keys.Down) {
                MovePlayer1(0, 1);
            }
            else if (e.KeyCode == Keys.Left) {
                MovePlayer1(-1, 0);
            }   

            else if (e.KeyCode == Keys.Enter) {
                new Bomb(gc, 2500, p1Pos, false);
            }

            else if (e.KeyCode == Keys.Subtract)
            {
                gc.SetRenderScale(-1);
            }
            else if (e.KeyCode == Keys.Add)
            {
                gc.SetRenderScale(1);
            }
        }



        private void MovePlayer(int x, int y)
        {
            RenderObject player = gc.player;
            float newx = player.rectangle.X + (x * gc.tileSize);
            float newy = player.rectangle.Y + (y * gc.tileSize);

            Tile next = gc.room.tiles.SelectMany(ty => ty.Where(tx => tx.rectangle.Contains((int)newx, (int)newy))).FirstOrDefault();
            
            if (next != null)
            {
                foreach (RenderObject renderObject in gc.bombs)
                {
                    if(next.rectangle.X == (int)renderObject.rectangle.X && next.rectangle.Y == (int)renderObject.rectangle.X)
                    {
                        return;
                    }
                }
                if (next.graphic != '#' && next.graphic != ',')
                {
                    player.rectangle.X = newx;
                    player.rectangle.Y = newy;
                    p2Pos = new Vector2(newx, newy);
                }

            }
        }
        private void MovePlayer1(int x, int y) {
            RenderObject player = gc.player1;
            float newx = player.rectangle.X + (x * gc.tileSize);
            float newy = player.rectangle.Y + (y * gc.tileSize);

            Tile next = gc.room.tiles.SelectMany(ty => ty.Where(tx => tx.rectangle.Contains((int)newx, (int)newy))).FirstOrDefault();

            if (next != null) {
                if (next.graphic == 'D') {
                    gc.room = levelLoader.GetRoom(gc.room.roomx + x, gc.room.roomy + y);

                    if (y != 0) {
                        player.rectangle.Y += -y * ((gc.room.tiles.Length - 2) * gc.tileSize);
                    }
                    else {
                        player.rectangle.X += -x * ((gc.room.tiles[0].Length - 2) * gc.tileSize);
                    }
                }

                else if (next.graphic != '#' && next.graphic != ',') {
                    player.rectangle.X = newx;
                    player.rectangle.Y = newy;

                    p1Pos = new Vector2(newx, newy);
                }
            }
        }

        public void Logic(float frametime)
        {
            this.frametime = frametime;
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            renderer.Render(e, frametime);
        }
    }

}


