/*
 * Starter code for C# MineSweeper Project
 * Evan Wright, 2/2019
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace MineSweeper
{
    class Square
    {

        public bool HasMine { get; set; }
        public bool Revealed { get; set; }
        public int AdjacentMines { get; set; }
        public bool Flagged { get; set; }
        public bool BadFlag { get; set; }

        static Image[] images = new Image[13];

        //constants for the images
        const int EMPTY = 0;
        const int SQUARE_1 = 1;
        const int SQUARE_2 = 2;
        const int SQUARE_3 = 3;
        const int SQUARE_4 = 4;
        const int SQUARE_5 = 5;
        const int SQUARE_6 = 6;
        const int SQUARE_7 = 7;
        const int SQUARE_8 = 8;
        const int MINE = 9;
        const int UNKNOWN = 10;
        const int FLAGGED = 11;
        const int BAD_FLAG = 12;

        //populate the image array
        static Square() {
            images[0] = Minesweeper.Properties.Resources.square_empty;
            images[1] = Minesweeper.Properties.Resources.square_1;
            images[2] = Minesweeper.Properties.Resources.square_2;
            images[3] = Minesweeper.Properties.Resources.square_3;
            images[4] = Minesweeper.Properties.Resources.square_4;
            images[5] = Minesweeper.Properties.Resources.square_5;
            images[6] = Minesweeper.Properties.Resources.square_6;
            images[7] = Minesweeper.Properties.Resources.square_7;
            images[8] = Minesweeper.Properties.Resources.square_mine;
            images[9] = Minesweeper.Properties.Resources.square_mine;
            images[10] = Minesweeper.Properties.Resources.square_unknown;
            images[11] = Minesweeper.Properties.Resources.square_flag;
            images[12] = Minesweeper.Properties.Resources.square_incorrect_flag;
        }

        public Square()
        {
            Reset();
        }

        public void Reset()
        {
            HasMine = false;
            Revealed = false;
            Flagged = false;
            BadFlag = false;
            AdjacentMines = 0;
        }

        public void CheckFlag()
        {
            if (!HasMine && Flagged) // shouldn't this make it not a badflag? (HasMine --> !HasMine)
                BadFlag = true;
        }

        public void ToggleFlag()
        {
            if (!Revealed)
            {
                Flagged = !Flagged;
            }
        }

        public void Draw(Graphics graphics, int x, int y)
        {

            if (BadFlag)
            {
                graphics.DrawImage(images[BAD_FLAG], x, y);
            }
            else if (Flagged)
            {
                graphics.DrawImage(images[FLAGGED], x, y);
            }
            else if (!Revealed)
            {
                graphics.DrawImage(images[UNKNOWN], x, y);
            }
            else if (HasMine)
            {
                graphics.DrawImage(images[MINE], x, y);
            }
            else
            {
                graphics.DrawImage(images[ AdjacentMines ], x, y);
            }
        }
    }
}
