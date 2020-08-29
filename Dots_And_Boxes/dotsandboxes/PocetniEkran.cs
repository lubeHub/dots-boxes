using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ognjen_Lubarda_Projekat
{
    public partial class PocetniEkran : Form
    {
        public PocetniEkran()
        {
            InitializeComponent();
            comboBox1.SelectedIndex = 0;
            comboBox2.SelectedIndex = 0;
            this.Icon = Icon.FromHandle(Properties.Resources.grid_dot_icon.GetHicon());
            SoundPlayer pozadina = new SoundPlayer(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "/Muzika/pozadina.wav");
            pozadina.Play();
        }
        ToolTip tool = new ToolTip();
        SoundPlayer pozadina = new SoundPlayer(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "/Muzika/pozadina.wav");
        bool mute = false;
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();

            opf.Filter = "PNG|*.png";
            opf.InitialDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "/Slike za avatar";
            if (opf.ShowDialog() == DialogResult.OK)
            {

                pictureBox1.Image = Image.FromFile(opf.FileName);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog opf = new OpenFileDialog();

            opf.Filter = "PNG|*.png";
            opf.InitialDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "/Slike za avatar";
            if (opf.ShowDialog() == DialogResult.OK)
            {

                pictureBox2.Image = Image.FromFile(opf.FileName);
            }
        }

        private void startDugme_Click(object sender, EventArgs e)
        {
            if (textBox1.Text != "" && textBox2.Text != "")
            {
                Hide();
                GlavniEkran glavniEkran = new GlavniEkran(pictureBox1.Image, pictureBox2.Image, textBox1.Text, textBox2.Text, int.Parse(comboBox1.SelectedItem.ToString()), int.Parse(comboBox2.SelectedItem.ToString()));
                glavniEkran.ShowDialog();
                Show();
            }
            else
            {
                MessageBox.Show("Unesite imena igrača!", "Greška", MessageBoxButtons.OK);
            }
        }

        private void okrugloDugme3_Click(object sender, EventArgs e)
        {

            if (textBox1.Text != "" && textBox2.Text != "")
            {
               
                Hide();
                if (pozadina != null) { pozadina.Stop(); }
                
                GlavniEkran glavniEkran = new GlavniEkran(pictureBox1.Image, pictureBox2.Image, textBox1.Text, textBox2.Text, int.Parse(comboBox1.SelectedItem.ToString()), int.Parse(comboBox2.SelectedItem.ToString()));
                glavniEkran.ShowDialog();
                Show();

                if (pozadina != null) { pozadina.PlayLooping(); }
            }
            else
            {
                MessageBox.Show("Unesite imena igrača!", "Greška", MessageBoxButtons.OK);
            }
        }

        private void okrugloDugme2_MouseHover(object sender, EventArgs e)
        {

            tool.Show("Sačuvaj snimak ekrana.", okrugloDugme2, -20, -20, 3000);
        }

        private void okrugloDugme3_MouseHover(object sender, EventArgs e)
        {

            tool.Show("Započni novu igru.", okrugloDugme3, -20, -20, 3000);
        }

        private void okrugloDugme1_MouseHover(object sender, EventArgs e)
        {
            tool.Show("Pravila igre.", okrugloDugme1, -20, -20, 2000);
        }

        private void okrugloDugme4_MouseHover(object sender, EventArgs e)
        {
            tool.Show("Uključi/Isključi zvuk.", okrugloDugme4, -20, -20, 2000);
        }

        private void button1_MouseHover(object sender, EventArgs e)
        {
            Button button = (Button)sender;
            tool.Show("Promijeni sliku.", button, -20, -20, 2000);
        }

        private void okrugloDugme2_Click(object sender, EventArgs e)
        {
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = "BMP(*.bmp)|*.bmp|JPG(*.jpg)|*.jpg|PNG(*.png)|*.png|GIF(*.gif)|*.gif";
            sf.InitialDirectory = Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "/Screenshot";
         
            Bitmap bitmapSave = new Bitmap(this.Width, this.Height);
            this.DrawToBitmap(bitmapSave, new Rectangle(0, 0, bitmapSave.Width, bitmapSave.Height));

            if (sf.ShowDialog() == DialogResult.OK)
            {

                bitmapSave.Save(sf.FileName);
            }


        }

 
        private void okrugloDugme4_Click(object sender, EventArgs e)
        {
            if(mute==false)
            {
                okrugloDugme4.BackColor = Color.FromArgb(215, 53, 53);
                pozadina.Stop();
                okrugloDugme4.BackgroundImage = Properties.Resources.mute;
                mute = true;
            }
            else
            {
                mute = false;
                okrugloDugme4.BackColor = Color.FromArgb(39, 142, 101);
                okrugloDugme4.BackgroundImage = Properties.Resources.speaker;
                pozadina.Play();
            }
        }

        private void okrugloDugme1_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Prvi igrač povlači vodoravnu ili uspravnu liniju između dvije susjedne tačke. Igrač koji prvi zatvori kvadrat između susjedne četiri tačke dobija bod i ponovno je na potezu.Igra završava kad se nemože nacrtati niti jedna crta.Pobjednik je igrač sa više osvojenih bodova.","Pravila igre");
        }
    }
}
