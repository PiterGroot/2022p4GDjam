using GameJam.Game;
using GameJam.Tools;
using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using SharpDX.XInput;

namespace GameJam
{
    public partial class RenderForm : Form
    {
        public Gamepad gamepad;
        private Vector2 p1Pos;
        private Vector2 p2Pos;

        public static Size AppClientSize;
        private LevelLoader levelLoader;
        private float frametime;
        public GameRenderer renderer;
        private readonly GameContext gc = new GameContext();
        private const int CONTROLLER_RUMBLE = 65000;
        public RenderForm()
        {
            InitializeComponent();

            DoubleBuffered = true;
            ResizeRedraw = true;
            
            WindowState = FormWindowState.Normal;
            FormBorderStyle = FormBorderStyle.None;
            Bounds = Screen.PrimaryScreen.Bounds;

            //setting up controller
            gc.controller = new Controller(UserIndex.One);
            gc.vibrationLeftMotorSpeed = CONTROLLER_RUMBLE;
            gc.vibration.RightMotorSpeed = (ushort)gc.vibrationLeftMotorSpeed;
            gc.vibration.LeftMotorSpeed = (ushort)gc.vibrationLeftMotorSpeed;

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
            levelLoader.LoadRooms(gc.spriteMap);

            renderer = new GameRenderer(gc);

            gc.renderer = renderer;
            gc.room = levelLoader.GetRoom(0, 0);
            InstantiateRenderObjects();
            ClientSize =
                 new Size(

                    (gc.tileSize * gc.room.tiles[0].Length) * gc.scaleunit * 3, 
                    (gc.tileSize * gc.room.tiles.Length) * gc.scaleunit * 3
                    );
        }

        private void InstantiateRenderObjects()
        {
            gc.ReloadBombs();
            
            p1Pos = new Vector2(240, 208);
            p2Pos = new Vector2(16, 16);

            gc.player = new RenderObject()
            {
                frames = gc.spriteMap.GetPlayerFrames(),
                rectangle = new Rectangle(16, 16, gc.tileSize, gc.tileSize),
            };

            gc.player1 = new RenderObject()
            {
                frames = gc.spriteMap.GetPlayer1Frames(),
                rectangle = new Rectangle(240, 208, gc.tileSize, gc.tileSize),
            };

            gc.p1Heart = new RenderObject()
            {
                frames = gc.GetSingeFrameArray('O'),
                rectangle = new Rectangle(0, 0, gc.tileSize, gc.tileSize),
            };
            gc.p2Heart = new RenderObject()
            {
                frames = gc.GetSingeFrameArray('O'),
                rectangle = new Rectangle(256, 224, gc.tileSize, gc.tileSize),
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
                if(gc.p1BombCount >= 1 && !renderer.wonGame)
                {
                    gc.p1BombCount--;
                    new Bomb(gc, 2500, p2Pos, true);
                    Console.WriteLine("BOMB placed at : " + p2Pos.x + " " + p2Pos.y);
                }
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
                if(gc.p2BombCount >= 1 && !renderer.wonGame)
                {
                    gc.p2BombCount--;
                    new Bomb(gc, 2500, p1Pos, false);
                    Console.WriteLine("BOMB placed at : " + p1Pos.x + " " + p1Pos.y);
                }
            }

            else if (e.KeyCode == Keys.Subtract)
            {
                gc.SetRenderScale(-1);
            }
            else if (e.KeyCode == Keys.Add)
            {
                gc.SetRenderScale(1);
            }

            if(e.KeyCode == Keys.Escape)
            {
                Application.Restart();
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
                    if(newx == (int)renderObject.rectangle.X && newy == (int)renderObject.rectangle.Y)
                    {
                        return;
                    }
                }
                if (next.graphic != '#' && next.graphic != ',' && next.graphic != 'W')
                {
                    foreach (RenderObject renderObject in gc.explosionTiles)
                    {
                        if (newx == (int)renderObject.rectangle.X && newy == (int)renderObject.rectangle.Y && !renderer.wonGame)
                        {
                            Console.WriteLine("Player 1 is dead");
                            gc.KillPlayer(true);
                        }
                    }
                    for (int i = gc.powerUps.Count -1; i >= 0; i--)
                    {
                        RenderObject obj = gc.powerUps[i];
                        if (newx == (int)obj.rectangle.X && newy == (int)obj.rectangle.Y && !renderer.wonGame)
                        {
                            Console.WriteLine("Player 1 touched powerup");
                            gc.powerUps.Remove(obj);
                            OnPowerUpPickup(obj, true);
                        }
                    }
                    player.rectangle.X = newx;
                    player.rectangle.Y = newy;
                    p2Pos = new Vector2(newx, newy);
                }

            }
        }

        private void OnPowerUpPickup(RenderObject renderObject, bool wichPlayer)
        {
            if (renderObject.frames[0] == gc.GetSingeFrameArray('S')[0])
            {
                Console.WriteLine("Found shield");
            }
            else if (renderObject.frames[0] == gc.GetSingeFrameArray('N')[0])
            {
                Console.WriteLine("Found nuke");
                new Nuke(gc, this);
            }
            else if (renderObject.frames[0] == gc.GetSingeFrameArray('/')[0])
            {
                Console.WriteLine("Found steal");
                if (wichPlayer)
                {
                    gc.p2BombCount = 0; //p1 
                    if (gc.maxBombP2 >= 1) gc.maxBombP2--;
                }
                else
                {
                    gc.p1BombCount = 0; //p2
                    if (gc.maxBombP1 >= 1) gc.maxBombP1--;
                }
            }
            else if (renderObject.frames[0] == gc.GetSingeFrameArray('B')[0])
            {
                Console.WriteLine("Found bomb");
                if (wichPlayer) gc.maxBombP1++; //p1 
                else gc.maxBombP2++; //p2
            }
            else if (renderObject.frames[0] == gc.GetSingeFrameArray('J')[0])
            {
                Console.WriteLine("Found jump");
            }
        }

        private void MovePlayer1(int x, int y) {
            RenderObject player = gc.player1;
            float newx = player.rectangle.X + (x * gc.tileSize);
            float newy = player.rectangle.Y + (y * gc.tileSize);

            Tile next = gc.room.tiles.SelectMany(ty => ty.Where(tx => tx.rectangle.Contains((int)newx, (int)newy))).FirstOrDefault();

            if (next != null) {
                foreach (RenderObject renderObject in gc.bombs)
                {
                    if (newx == (int)renderObject.rectangle.X && newy == (int)renderObject.rectangle.Y)
                    {
                        return;
                    }
                }
                if (next.graphic != '#' && next.graphic != ',' && next.graphic != 'W') {
                    foreach (RenderObject renderObject in gc.explosionTiles)
                    {
                        if (newx == (int)renderObject.rectangle.X && newy == (int)renderObject.rectangle.Y && !renderer.wonGame)
                        {
                            Console.WriteLine("Player 2 is dead");
                            gc.KillPlayer(false);
                        }
                    }
                    for (int i = gc.powerUps.Count - 1; i >= 0; i--)
                    {
                        RenderObject obj = gc.powerUps[i];
                        if (newx == (int)obj.rectangle.X && newy == (int)obj.rectangle.Y && !renderer.wonGame)
                        {
                            Console.WriteLine("Player 2 touched powerup");
                            gc.powerUps.Remove(obj);
                            OnPowerUpPickup(obj, false);
                        }
                    }
                    player.rectangle.X = newx;
                    player.rectangle.Y = newy;

                    p1Pos = new Vector2(newx, newy);
                }
            }
        }
        private void CheckDamagep1()
        {
            Tile current = gc.room.tiles.SelectMany(ty => ty.Where(tx => tx.rectangle.Contains((int)gc.player.rectangle.X, (int)gc.player.rectangle.Y))).FirstOrDefault();
            if (current.graphic != '#' && current.graphic != ',')
            {
                foreach (RenderObject renderObject in gc.explosionTiles)
                {
                    if (gc.player.rectangle.X == (int)renderObject.rectangle.X && gc.player.rectangle.Y == (int)renderObject.rectangle.Y && !renderer.wonGame)
                    {
                        Console.WriteLine("Player 1 is dead!");
                        gc.KillPlayer(true);
                    }
                }
                if(current.graphic == 'K') gc.KillPlayer(true);
            }
        }
        private void CheckDamagep2()
        {
            Tile current = gc.room.tiles.SelectMany(ty => ty.Where(tx => tx.rectangle.Contains((int)gc.player1.rectangle.X, (int)gc.player1.rectangle.Y))).FirstOrDefault();
            if (current.graphic != '#' && current.graphic != ',')
            {
                foreach (RenderObject renderObject in gc.explosionTiles)
                {
                    if (gc.player1.rectangle.X == (int)renderObject.rectangle.X && gc.player1.rectangle.Y == (int)renderObject.rectangle.Y && !renderer.wonGame)
                    {
                        Console.WriteLine("Player 2 is dead!");
                        gc.KillPlayer(false);
                    }
                }
                if (current.graphic == 'K') gc.KillPlayer(false);
            }
        }
        public void Logic(float frametime)
        {
            CheckDamagep1();
            CheckDamagep2();

            if(gc.controllerMode && !renderer.wonGame && gc.controller.IsConnected) UpdateControllerInput();

            this.frametime = frametime;
            AppClientSize = new Size(

                   (gc.tileSize * gc.room.tiles[0].Length) /2,
                   (gc.tileSize * gc.room.tiles.Length) /2
                   );
        }

        bool lastKeyPressUp;
        bool lastKeyPressDown;
        bool lastKeyPressLeft;
        bool lastKeyPressRight;
        bool lastAButton;
        bool lastBButton;
        bool lastXButton;
        bool lastYButton;
        private void UpdateControllerInput(){
            gamepad = gc.controller.GetState().Gamepad;
            if (gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadRight) && lastKeyPressRight == false)
            {
                MovePlayer1(1, 0);
                lastKeyPressRight = true;
            }
            else
            {
                lastKeyPressRight = gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadRight);
            }
            if (gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadLeft) && lastKeyPressLeft == false)
            {
                MovePlayer1(-1, 0);
                lastKeyPressLeft = true;
            }
            else
            {
                lastKeyPressLeft = gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadLeft);
            }
            if (gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadUp) && lastKeyPressUp == false)
            {
                MovePlayer1(0, -1);
                lastKeyPressUp = true;
            }
            else
            {
                lastKeyPressUp = gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadUp);
            }
            if (gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadDown) && lastKeyPressDown == false)
            {
                MovePlayer1(0, 1);
                lastKeyPressDown = true;
            }
            else
            {
                lastKeyPressDown = gamepad.Buttons.HasFlag(GamepadButtonFlags.DPadDown);
            }
            if (gamepad.Buttons.HasFlag(GamepadButtonFlags.A) && lastAButton == false)
            {
                if (gc.p2BombCount >= 1)
                {
                    gc.SetControllerVibration(CONTROLLER_RUMBLE);
                   
                    gc.p2BombCount--;
                    new Bomb(gc, 2500, p1Pos, false);
                    lastAButton = true;
                }
            }
            else
            {
                lastAButton = gamepad.Buttons.HasFlag(GamepadButtonFlags.A);
            }
            if (gamepad.Buttons.HasFlag(GamepadButtonFlags.B) && lastBButton == false)
            {
                if (gc.p2BombCount >= 1)
                {
                    gc.SetControllerVibration(CONTROLLER_RUMBLE);

                    gc.p2BombCount--;
                    new Bomb(gc, 2500, p1Pos, false);
                    lastBButton = true;
                }
            }
            else
            {
                lastBButton = gamepad.Buttons.HasFlag(GamepadButtonFlags.B);
            }
            if (gamepad.Buttons.HasFlag(GamepadButtonFlags.X) && lastXButton == false)
            {
                if (gc.p2BombCount >= 1)
                {
                    gc.SetControllerVibration(CONTROLLER_RUMBLE);

                    gc.p2BombCount--;
                    new Bomb(gc, 2500, p1Pos, false);
                    lastXButton = true;
                }
            }
            else
            {
                lastXButton = gamepad.Buttons.HasFlag(GamepadButtonFlags.X);
            }
            if (gamepad.Buttons.HasFlag(GamepadButtonFlags.Y) && lastYButton == false)
            {
                if (gc.p2BombCount >= 1)
                {
                    gc.SetControllerVibration(CONTROLLER_RUMBLE);

                    gc.p2BombCount--;
                    new Bomb(gc, 2500, p1Pos, false);
                    lastYButton = true;
                }
            }
            else
            {
                lastYButton = gamepad.Buttons.HasFlag(GamepadButtonFlags.Y);
            }
        }

        protected override void OnPaint(PaintEventArgs e){
            base.OnPaint(e);
            renderer.Render(e, frametime);
        }
    }

}


