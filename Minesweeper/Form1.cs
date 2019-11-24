using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Media;
using MineSweeper;

namespace Minesweeper
{
    public partial class Form1 : Form
    {

        const int EASY = 10;
        const int MEDIUM = 20;
        const int HARD = 30;

        int skillLevel = MEDIUM;

        const int BOARD_WIDTH = 20;
        const int BOARD_HEIGHT = 10;
        const int TILE_SIZE = 30;

        Square[,] board = new Square[BOARD_WIDTH, BOARD_HEIGHT];
        SoundPlayer boom = new SoundPlayer(Properties.Resources.Explosion_02);
        Random r = new Random();

        bool gameOver = false;
		int time = 0;


        public Form1()
        {
            InitializeComponent();

            for (int i = 0; i < BOARD_WIDTH; i++)
            {
                for (int j = 0; j < BOARD_HEIGHT; j++)
                {
                    board[i, j] = new Square();
                }
            }

            NewGame();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            for (int i = 0; i < BOARD_WIDTH; i++)
            {
                for (int j = 0; j < BOARD_HEIGHT; j++)
                {
                    board[i, j].Draw(e.Graphics , i * TILE_SIZE, j * TILE_SIZE);
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            MouseEventArgs mea = e as MouseEventArgs;

        }

        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {
			timer1.Start();
            int x = e.Location.X / TILE_SIZE;
            int y = e.Location.Y / TILE_SIZE;

            if (e.Button == MouseButtons.Left)
            {
				ClickSquare(x, y);

                pictureBox1.Invalidate();
            } else if (e.Button == MouseButtons.Right)
            {
                board[x, y].ToggleFlag();
                pictureBox1.Invalidate();
            }

			if (gameOver)
			{
				timer1.Stop();
				MessageBox.Show("You Lost! Click New Game to Try Again");
			}
			if (IsGameOver())
			{
				timer1.Stop();
				MessageBox.Show("Congrats, you won!");

			}
		}

        private void easyToolStripMenuItem_Click(object sender, EventArgs e)
        {
            skillLevel = EASY;
        }

        private void mediumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            skillLevel = MEDIUM;
        }

        private void hardToolStripMenuItem_Click(object sender, EventArgs e)
        {
            skillLevel = HARD;
        }

		void ClearMines()
		{
			foreach (Square s in board)
			{
				s.Reset();
			}
		}

		void PlaceMines()
        {
            for (int i = 0; i < skillLevel; i++)
            {
                int x = 0, y = 0;
                do
                {
                    x = r.Next(BOARD_WIDTH);
                    y = r.Next(BOARD_HEIGHT);
                }
                while (board[x,y].HasMine);


                board[x,y].HasMine = true;

            }
        }

        int HasMine(int x, int y) 
        {
            if (x < 0 || x > BOARD_WIDTH - 1|| y < 0 || y > BOARD_HEIGHT - 1) 
                return 0;

            if (board[x,y].HasMine) 
                return 1; 
            else
                return 0;
			
        }

		void SetMineCounts()
		{
			for (int i = 0; i < BOARD_WIDTH; i++)
			{
				for (int j = 0; j < BOARD_HEIGHT; j++)
				{
					board[i, j].AdjacentMines = CountAdjacentMines(i, j);
				}
			}
		}

		void NewGame()
		{
			gameOver = false;
			ClearMines();
			PlaceMines();
			SetMineCounts();
			pictureBox1.Invalidate();
			time = 0;
		}

		public void Draw(Graphics graphics)
		{
			for (int i = 0; i < BOARD_WIDTH; i++)
			{
				for (int j = 0; j < BOARD_HEIGHT; j++)
				{

					board[i, j].Draw(graphics, i * TILE_SIZE, j * TILE_SIZE);
				}
			}
		}

		bool IsGameOver()
		{
			int count = 0;
			foreach (Square s in board)
			{
				if (s.HasMine || s.Revealed )
				{
					count++;
				}
			}

			if (count > 0)
				return false;
			else
				return true;
			
		}

		int CountAdjacentMines(int x, int y)
		{
			int count = 0;
			count += HasMine(x-1, y-1);
			count += HasMine(x, y - 1);
			count += HasMine(x + 1, y - 1);
			count += HasMine(x - 1, y);
			count += HasMine(x + 1, y);
			count += HasMine(x - 1, y + 1);
			count += HasMine(x, y + 1);
			count += HasMine(x + 1, y + 1);

			board[x, y].AdjacentMines = count;

			return count;
		}

		void ToggleFlag(int x, int y)
		{
			board[x, y].ToggleFlag();
		}

		void RevealSquare(int x, int y)
		{
			if (x < 0 || x > BOARD_WIDTH - 1 || y < 0 || y > BOARD_HEIGHT - 1 || board[x,y].Revealed)
				return;


			if (!board[x,y].Revealed)
				board[x, y].Revealed = true;

			if (board[x,y].AdjacentMines == 0)
			{
				RevealSquare(x - 1, y - 1);
				RevealSquare(x, y - 1);
				RevealSquare(x + 1, y - 1);
				RevealSquare(x - 1, y);
				RevealSquare(x + 1, y);
				RevealSquare(x - 1, y + 1);
				RevealSquare(x, y + 1);
				RevealSquare(x + 1, y + 1);
			}

			return;
		}

		void ClickSquare(int x, int y)
		{
			if (gameOver || board[x, y].Revealed == true || board[x,y].Flagged)
				return;

			RevealSquare(x, y);

			if (board[x,y].HasMine)
			{
				ShowBadFlag();
				gameOver = true;
			}

			pictureBox1.Invalidate();

		}

		void ShowBadFlag()
		{
			foreach (Square s in board)
			{
				s.CheckFlag();
			}

		}

		private void newGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            NewGame();
        }

		private void timer1_Tick(object sender, EventArgs e)
		{
			time++;
			textBox1.Text = time.ToString();
			textBox1.Invalidate();
			if (time == 999)
			{
				gameOver = true;
				MessageBox.Show("You took too long! Try again~");
				timer1.Stop();

			}
		}
	}
}
