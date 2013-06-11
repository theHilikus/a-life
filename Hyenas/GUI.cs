using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace Hyenas {
    public partial class GUI : Form {
        
        private World theWorld;

        public GUI() {
            InitializeComponent();

           
            theWorld = new World(this);
        }

        internal void report(string msg) {
            statusBox.AppendText(msg+"\n");
            
        }

        internal void addRepresentation(PictureBox pictureBox, double x, double y, Directions orientation) {
            //rescale frame of reference
            pictureBox.Location = new Point((int)Math.Round((decimal)x * mainArea.Width - pictureBox.Width / 2), (int)Math.Round((decimal)y * mainArea.Height - pictureBox.Height / 2));
            switch (orientation) {
                case Directions.WEST:
                    pictureBox.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
                    break;
                case Directions.SOUTH:
                    pictureBox.Image.RotateFlip(RotateFlipType.Rotate180FlipNone);
                    break;
                case Directions.EAST:
                    pictureBox.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
                    break;

            }
            mainArea.Controls.Add(pictureBox);
            pictureBox.Click += new EventHandler(agent_Click);
        }

        private void button1_Click(object sender, EventArgs e) {
            theWorld.start();
            
        }

        private void button2_Click(object sender, EventArgs e) {
            theWorld.stop();
        }

        internal void remove(PictureBox pictureBox) {
            mainArea.Controls.RemoveByKey(pictureBox.Name);
        }

        internal void refreshPosition(PictureBox pictureBox, double x, double y) {
            //pictureBox.t = 30;
            pictureBox.Location = new Point((int)Math.Round((decimal)x * mainArea.Width - pictureBox.Width / 2), (int)Math.Round((decimal)y * mainArea.Height - -pictureBox.Height / 2));            
        }

        internal void flipRepresentation(PictureBox pictureBox, bool right) {
            if (right) {
                pictureBox.Image.RotateFlip(RotateFlipType.Rotate90FlipNone);
            } else {
                pictureBox.Image.RotateFlip(RotateFlipType.Rotate270FlipNone);
            }
            pictureBox.Invalidate();
        }

        private void stepButton_Click(object sender, EventArgs e) {
            theWorld.update();
        }

        private void restartButton_Click(object sender, EventArgs e) {
            reset();
            theWorld = new World(this);
            this.Refresh();
            
        }

        internal void reset() {
            mainArea.Controls.Clear();
            if (theWorld.Running) {
                theWorld.stop();
            }
        }

        private void agent_Click(object sender, EventArgs e) {
            Agent clickedAgent = (Agent)theWorld.Population[((PictureBox)sender).Name];
            AgentDialog props = new AgentDialog(clickedAgent, theWorld.Population, this);
            props.Show();
            //mainArea.Visible = false;
            //draw the vision polygon
            Graphics graph = mainArea.CreateGraphics();
            Pen redPen = new Pen(Color.Red, 2);

            Point[] area = clickedAgent.VisionPoly;
            int count=0;
            while (count < area.Length) {
                 PointF temp = theWorld.normalizePoint(area[count]);
                 area[count].X = (int)Math.Round(temp.X * mainArea.Width);
                 area[count].Y = (int)Math.Round(temp.Y * mainArea.Height);
                 count++;
            }
            graph.DrawPolygon(redPen, area) ;
            //mainArea.Refresh();
            //mainArea.Invalidate();
            
        }
    }

    

}