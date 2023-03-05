using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Tetris
{
    public partial class Form1 : Form
    {
        private const int ROWS = 20;
        private const int COLUMNS = 10;
        private const int BLOCK_SIZE = 20;

        private int[,] board;
        private int[,] currentPiece;
        private Point currentPiecePosition;

        public Form1()
        {
            InitializeComponent();

            board = new int[ROWS, COLUMNS];

            currentPiece = new int[4, 4] {
                { 0, 0, 0, 0 },
                { 0, 0, 0, 0 },
                { 1, 1, 1, 1 },
                { 0, 0, 0, 0 }
            };

            currentPiecePosition = new Point(3, 0);

            GameTimer = new Timer();
            GameTimer.Interval = 500;
            GameTimer.Tick += GameTimer_Tick;
            GameTimer.Start();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            MovePieceDown();
            Invalidate();
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Left:
                    MovePieceLeft();
                    break;
                case Keys.Right:
                    MovePieceRight();
                    break;
                case Keys.Down:
                    MovePieceDown();
                    break;
                case Keys.Up:
                    RotatePiece();
                    break;
            }

            Invalidate();
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            for (int row = 0; row < ROWS; row++)
            {
                for (int column = 0; column < COLUMNS; column++)
                {
                    int block = board[row, column];
                    if (block != 0)
                    {
                        Brush brush = new SolidBrush(Color.Red);
                        e.Graphics.FillRectangle(brush, column * BLOCK_SIZE, row * BLOCK_SIZE, BLOCK_SIZE, BLOCK_SIZE);
                    }
                }
            }

            for (int row = 0; row < 4; row++)
            {
                for (int column = 0; column < 4; column++)
                {
                    int block = currentPiece[row, column];
                    if (block != 0)
                    {
                        Brush brush = new SolidBrush(Color.Blue);
                        e.Graphics.FillRectangle(brush, (currentPiecePosition.X + column) * BLOCK_SIZE, (currentPiecePosition.Y + row) * BLOCK_SIZE, BLOCK_SIZE, BLOCK_SIZE);
                    }
                }
            }
        }
        private void MovePieceLeft()
        {
            if (!Collision(-1, 0, currentPiece, currentPiecePosition))
            {
                currentPiecePosition.X--;
            }
        }

        private void MovePieceRight()
        {
            if (!Collision(1, 0, currentPiece, currentPiecePosition))
            {
                currentPiecePosition.X++;
            }
        }

        private void MovePieceDown()
        {
            if (!Collision(0, 1, currentPiece, currentPiecePosition))
            {
                currentPiecePosition.Y++;
            }
            else
            {
                AddPieceToBoard(currentPiece, currentPiecePosition);
                currentPiecePosition = new Point(3, 0);
                currentPiece = GenerateRandomPiece();
            }
        }
        private void RotatePiece()
        {
            int[,] rotatedPiece = Rotate(currentPiece);
            if (!Collision(0, 0, rotatedPiece, currentPiecePosition))
            {
                currentPiece = rotatedPiece;
            }
        }

        private bool Collision(int offsetX, int offsetY, int[,] piece, Point position)
        {
            for (int row = 0; row < 4; row++)
            {
                for (int column = 0; column < 4; column++)
                {
                    if (piece[row, column] != 0)
                    {
                        int x = position.X + column + offsetX;
                        int y = position.Y + row + offsetY;

                        if (x < 0 || x >= COLUMNS || y >= ROWS)
                        {
                            return true;
                        }

                        if (y >= 0 && board[y, x] != 0)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private int[,] GenerateRandomPiece()
        {
            int[,] piece = new int[4, 4];

            Random random = new Random();
            int index = random.Next(0, 7);

            switch (index)
            {
                case 0:
                    piece = new int[4, 4] {
                    { 0, 0, 0, 0 },
                    { 1, 1, 1, 1 },
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 0 }
                };
                    break;
                case 1:
                    piece = new int[4, 4] {
                    { 1, 1, 0, 0 },
                    { 1, 1, 0, 0 },
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 0 }
                };
                    break;
                case 2:
                    piece = new int[4, 4] {
                    { 1, 0, 0, 0 },
                    { 1, 1, 1, 0 },
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 0 }
                };
                    break;
                case 3:
                    piece = new int[4, 4] {
                    { 0, 0, 1, 0 },
                    { 1, 1, 1, 0 },
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 0 }
                };
                    break;
                case 4:
                    piece = new int[4, 4] {
                    { 0, 1, 1, 0 },
                    { 1, 1, 0, 0 },
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 0 }
                };
                    break;

                case 5:
                    piece = new int[4, 4] {
                    { 1, 1, 0, 0 },
                    { 0, 1, 1, 0 },
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 0 }
                };
                    break;
                case 6:
                    piece = new int[4, 4] {
                    { 0, 1, 0, 0 },
                    { 1, 1, 1, 0 },
                    { 0, 0, 0, 0 },
                    { 0, 0, 0, 0 }
                };
                    break;
            }
            return piece;
        }

        private int[,] Rotate(int[,] piece)
        {
            int[,] rotatedPiece = new int[4, 4];

            for (int row = 0; row < 4; row++)
            {
                for (int column = 0; column < 4; column++)
                {
                    rotatedPiece[row, column] = piece[3 - column, row];
                }
            }

            return rotatedPiece;
        }

        private void AddPieceToBoard(int[,] piece, Point position)
        {
            for (int row = 0; row < 4; row++)
            {
                for (int column = 0; column < 4; column++)
                {
                    if (piece[row, column] != 0)
                    {
                        int x = position.X + column;
                        int y = position.Y + row;

                        board[y, x] = piece[row, column];
                    }
                }
            }
        }

    }

}
