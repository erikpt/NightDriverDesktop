using NightDriver;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.DirectoryServices.ActiveDirectory;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NightDriver
{
    internal class LEDVisualizer : Panel
    {
        int xSpacing = 1;
        int ySpacing = 1;
        int xMargin = 4;
        int yMargin = 4;
        int xSize = 8;
        int ySize = 8;

        // CalculateMaxSquareSize
        //
        // Figures out the largest square that can be used to represent this strip in the available
        // client area

        private void CalculateMaxSquareSize()
        {
            if (ColorData == null || ColorData.Length == 0)
                return;

            int totalSquares = ColorData.Length;
            int availableWidth = this.ClientRectangle.Width - xMargin * 2;
            int availableHeight = this.ClientRectangle.Height - yMargin * 2;

            // Start with a size guess
            int maxSize = Math.Min(availableWidth, availableHeight);

            while (maxSize > 0)
            {
                int squaresPerRow = (availableWidth + xSpacing) / (maxSize + xSpacing);
                int squaresPerColumn = (availableHeight + ySpacing) / (maxSize + ySpacing);

                if (squaresPerRow * squaresPerColumn >= totalSquares)
                {
                    // Found a suitable size
                    xSize = ySize = maxSize;
                    return;
                }

                maxSize--; // Decrease size guess and try again
            }
        }

        public LEDVisualizer()
        {
            // Activate double buffering
            this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint, true);
        }

        private CRGB[] _ColorData;
        public CRGB[] ColorData
        {
            get
            {
                return _ColorData;
            }
            set
            {
                _ColorData = value;
                CalculateMaxSquareSize();
            }
        }

        protected override void OnPaintBackground(PaintEventArgs e)
        {
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            xSize = Math.Clamp(xSize + e.Delta/120, 1, 32);
            ySize = Math.Clamp(ySize + e.Delta/120, 1, 32);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            e.Graphics.Clear(Color.Black);

            if (ColorData == null )
            {
                e.Graphics.DrawString("Select an active strip for visualization", this.Font, Brushes.White, 2, 2);
                return;
            }

            int availableWidth = this.ClientRectangle.Width - xMargin * 2;
            int availableXSlots = availableWidth / (xSize + xSpacing);
            int availableHeight = this.ClientRectangle.Height - yMargin * 2;
            int availableYSlots = availableHeight / (ySize + ySpacing);

            // The Draw thread will also lock the buffer, and that synchronization allows us to 
            // avoid showing a frame for visualization when the buffer is halfway through a render

            lock (ColorData)
            {
                int iSlot = 0;
                for (int y = 0; y < availableYSlots; y++)
                    for (int x = 0; x < availableXSlots; x++)
                        if (iSlot < ColorData.Length)
                            e.Graphics.FillRectangle(new SolidBrush(ColorData[iSlot++].GetColor()),
                                                     xMargin + x * (xSize + xSpacing),
                                                     yMargin + y * (ySize + ySpacing),
                                                     xSize,
                                                     ySize);
            }
        }
    }
}
