using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Media;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ognjen_Lubarda_Projekat
{
    public partial class GlavniEkran : Form
    {
        public GlavniEkran()
        {
            InitializeComponent();
        }
        float sirina, visina;
        float sirinaTacke = 12;
        float visinaTacke = 12;
        int offset = 16;
        bool igracNaRedu=true;
        int poeniPrvi=0;
        int poeniDrugi=0;
        bool incijalizacija = true;
        int indeks = 0;
        bool provjeraTacke=true;
        bool napravljenKvdrt = false;
        bool nijeZadnjaTacka = true;
        bool mute = false;
        Graphics graphics;
        ToolTip tool = new ToolTip();
        SoundPlayer ding = new SoundPlayer(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "/Muzika/Discord Notification - Sound Effect.wav");
        SoundPlayer zvonce = new SoundPlayer(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "/Muzika/Ding Sound Effect.wav");
        SoundPlayer kvak = new SoundPlayer(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "/Muzika/Quack Sound Effect.wav");
        SoundPlayer wah = new SoundPlayer(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "/Muzika/wah wah sound.wav");
        SoundPlayer fanfare = new SoundPlayer(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "/Muzika/Fanfare sound effects.wav");
        Form form = Form.ActiveForm;
        List<int> listaDrugogIgraca = new List<int>();
        List<RectangleF> listaPravougaonika = new List<RectangleF>();
        List<Tacke> listaPoteza = new List<Tacke>();
        List<int> listaPrvogIgraca = new List<int>();
        List<int> listaIskoristenihKvadrata = new List<int>();

        public GlavniEkran(Image image1,Image image2, string ime1, string ime2, int sirina, int visina)
        {
            InitializeComponent();
            this.Icon = Icon.FromHandle(Properties.Resources.grid_dot_icon.GetHicon());
            this.pictureBox1.Image = image1;
           this.pictureBox2.Image = image2;
            label1.Text = ime1;
            label2.Text = ime2;
            this.sirina = (float)(sirina);
            this.visina = (float)visina;
            okrugloDugme2.BackgroundImage = Properties.Resources.speaker;
           
        }

        private void GlavniEkran_Resize(object sender, EventArgs e)
        {
            panel1.Invalidate();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
        bool prviKlik = false;
        RectangleF rectangle = new RectangleF();
        private void panel1_Click(object sender, EventArgs e)
        {
            //Popunjavanje liste koja pokazuje koji kvadrati su zatvoreni
            if(incijalizacija)
            {
                for(int i=0;i<listaPravougaonika.Count;i++)
                {
                    listaIskoristenihKvadrata.Add(i);
                }
                incijalizacija = false;
            }
            nijeZadnjaTacka = true;
           
            //Klik na prvu tacku
            if (provjeraTacke)
            {
                foreach (RectangleF x in listaPravougaonika)
                {
                    //Provjera da li je kliknuto na tacku
                    if (x.Contains(panel1.PointToClient(Cursor.Position)))
                    {
                        Graphics g = panel1.CreateGraphics();
                        prviKlik = true;
                        rectangle = x;
                        if (ding != null) { ding.Play(); }
                        indeks = listaPravougaonika.IndexOf(x);
                        g.FillEllipse(new SolidBrush(Color.Green), x);
                        provjeraTacke = false;
                       
                        break;
                    }
                    else
                    {

                        if (kvak != null) { kvak.Play(); }
                    }
                }

            }
            //klik na drugu tacku
            else
            {
                foreach (RectangleF x in listaPravougaonika)
                {
                    
                    if (x.Contains(panel1.PointToClient(Cursor.Position)))
                    {
                       for(int i=1;i<listaPravougaonika.Count;i++)
                        {
                            if((indeks==(int)((visina+1)*i)-1 && listaPravougaonika.IndexOf(x)==(int)((visina+1)*i)) || (indeks == (int)((visina + 1) * i) && listaPravougaonika.IndexOf(x) == (int)((visina + 1) * i-1)))
                            {
                                nijeZadnjaTacka = false;
                                break;
                            }
                           
                        }
                        //Kad klikne na validnu tacku
                        if ((indeks == listaPravougaonika.IndexOf(x) - 1 ||
                            (indeks == listaPravougaonika.IndexOf(x) + 1)
                            || indeks == listaPravougaonika.IndexOf(x) - visina - 1 ||
                            indeks == listaPravougaonika.IndexOf(x) + visina + 1) &&
                            nijeZadnjaTacka == true)
                        {
                            provjeraTacke = true;
                            //U koliko vec nije odigran taj potez
                            if (listaPoteza.Contains(new Tacke(indeks, listaPravougaonika.IndexOf(x))) == false)
                            {
                                
                                listaPoteza.Add(new Tacke()
                                {
                                    tacka1 = indeks,
                                    tacka2 = listaPravougaonika.IndexOf(x),
                                });
                         napravljenKvdrt = KvadratNapravljen();
                                if (!napravljenKvdrt)
                                {

                                    if (ding != null) { ding.Play(); }
                                    igracNaRedu = !igracNaRedu;
                                }
                                else
                                {

                                    if (zvonce != null) { zvonce.Play(); }
                                }
                            }
                            
                            panel1.Invalidate();
                            
                            

                            break;
                        }
                        //Kad klikne dva puta na istu tacku
                        else if(indeks == listaPravougaonika.IndexOf(x))
                        {
                            panel1.Invalidate();
                            provjeraTacke = true;
                        }
                        //Kad klikne na tacku koja nije validna
                        else
                        {

                            if (kvak != null) { kvak.Play(); }
                        }
                    }
                    else
                    {

                        if (kvak != null) { kvak.Play(); }
                    }
                }
            }


            if (igracNaRedu)
            {
                panel2.Show();
                panel3.Hide();
            }
            else
            {
                panel3.Show();
                panel2.Hide();
            }


        }

       
        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            graphics = e.Graphics;
            
            listaPravougaonika.Clear();

           

            for (int i = 0; i <= sirina; i++)
            {
                for (int j = 0; j <= visina; j++)
                {
                    listaPravougaonika.Add(new RectangleF(i * ((panel1.Width - offset) / sirina) - 1, j * ((panel1.Height - offset) / visina) - 1, sirinaTacke + 2, visinaTacke + 2));
                    
                }
            }
            IscrtajPravougaonik(graphics);
            IscrtajLinije(graphics);
            for (int i = 0; i <= sirina; i++)
            {
                for (int j = 0; j <= visina; j++)
                {
                    graphics.FillEllipse(new SolidBrush(Color.FloralWhite), i * ((panel1.Width - offset) / sirina), j * ((panel1.Height - offset) / visina), sirinaTacke, visinaTacke);
                    graphics.DrawEllipse(new Pen(Color.Black, 3f), i * ((panel1.Width - offset) / sirina), j * ((panel1.Height - offset) / visina), sirinaTacke, visinaTacke);

                }
            }
         
            ProvjeraPobjednika();
            
        }

        void IscrtajPravougaonik(Graphics graphics)
        {
            foreach (int point in listaPrvogIgraca)
            {
                graphics.FillRectangle(new SolidBrush(Color.FromArgb(223, 87, 79)),listaPravougaonika.ElementAt(point).X + (sirinaTacke + 2) / 2, listaPravougaonika.ElementAt(point).Y + (visinaTacke + 2) / 2, (panel1.Width - offset) / sirina, (panel1.Height - offset) / visina);
            }


            foreach (int point in listaDrugogIgraca)
            {
                graphics.FillRectangle(new SolidBrush(Color.FromArgb(82, 160, 210)), listaPravougaonika.ElementAt(point).X + (sirinaTacke + 2) / 2, listaPravougaonika.ElementAt(point).Y + (visinaTacke + 2) / 2, (panel1.Width - offset) / sirina, (panel1.Height - offset) / visina);
            }
           
        }
        void IscrtajLinije(Graphics graphics)
        {
            foreach (Tacke x in listaPoteza)
            {
                graphics.DrawLine(new Pen(Color.FromArgb(192, 192, 192), sirinaTacke-4), listaPravougaonika.ElementAt(x.tacka1).X + (sirinaTacke + 2) / 2, listaPravougaonika.ElementAt(x.tacka1).Y + (visinaTacke + 2) / 2, listaPravougaonika.ElementAt(x.tacka2).X + (sirinaTacke + 2) / 2, listaPravougaonika.ElementAt(x.tacka2).Y + (visinaTacke + 2) / 2);
            }
        }
        //Provjera da li je kvadrat formiran
     bool  KvadratNapravljen()
        {
            bool napravljenKvadrat = false;
           
            for(int i=0; i<=listaPravougaonika.Count; i++)
            {
                //ukoliko jeste
                if (listaPoteza.Contains(new Tacke(i, i + 1)) &&
                listaPoteza.Contains(new Tacke(i, i + 1 + (int)visina)) &&
                listaPoteza.Contains(new Tacke(1 + i, i + 2 + (int)(visina))) &&
                listaPoteza.Contains(new Tacke(i + 1 + (int)visina, i + 2 + (int)visina)) &&
                listaIskoristenihKvadrata.Contains(i) == true)
                {
                    napravljenKvadrat = true;
                    listaIskoristenihKvadrata.Remove(i);
                    if (igracNaRedu)
                    {
                        listaPrvogIgraca.Add(i);
                        poeniPrvi++;
                        okrugloDugme1.Text = poeniPrvi.ToString();
                        igracNaRedu = true;
                    }
                    else
                    {
                        listaDrugogIgraca.Add(i);
                        poeniDrugi++;
                        okrugloDugme5.Text = poeniDrugi.ToString();
                        igracNaRedu = false;
                    }
                }
              
               
             }
          
            return napravljenKvadrat;
                   
            
        }
        
        //MUTE dugme
        private void okrugloDugme2_Click(object sender, EventArgs e)
        {
            if (mute == false)
            {
                okrugloDugme2.BackColor = Color.FromArgb(215, 53, 53);
                
                kvak = null;
                wah = null;
                fanfare = null;
                ding = null;
                mute = true;
                zvonce = null;
                okrugloDugme2.BackgroundImage = Properties.Resources.mute;
            }
            else
            {
                mute = false;
                okrugloDugme2.BackgroundImage = Properties.Resources.speaker;
                okrugloDugme2.BackColor = Color.FromArgb(39, 142, 101);
                 ding = new SoundPlayer(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "/Muzika/Discord Notification - Sound Effect.wav");
                 zvonce = new SoundPlayer(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "/Muzika/Ding Sound Effect.wav");
                 kvak = new SoundPlayer(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "/Muzika/Quack Sound Effect.wav");
                 wah = new SoundPlayer(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "/Muzika/wah wah sound.wav");
                 fanfare = new SoundPlayer(Directory.GetParent(Environment.CurrentDirectory).Parent.FullName + "/Muzika/Fanfare sound effects.wav");
                if (ding != null) { ding.Play(); }
            }
        }

        private void okrugloDugme4_Click(object sender, EventArgs e)
        {

            if (ding != null) { ding.Play(); }
            MessageBox.Show("Prvi igrač povlači vodoravnu ili uspravnu liniju između dvije susjedne tačke. Igrač koji prvi zatvori kvadrat između susjedne četiri tačke dobija bod i ponovno je na potezu.Igra završava kad se nemože nacrtati niti jedna crta.Pobjednik je igrač sa više osvojenih bodova.", "Pravila igre");

        }

        private void okrugloDugme3_Click(object sender, EventArgs e)
        {

            if (ding != null) { ding.Play(); }
            this.Close();
        }

        private void okrugloDugme6_MouseHover(object sender, EventArgs e)
        {
            tool.Show("Sačuvaj snimak ekrana.", okrugloDugme6, -20, -20, 3000);
        }

        private void okrugloDugme4_MouseHover(object sender, EventArgs e)
        {
            tool.Show("Pravila igre.", okrugloDugme4, -20, -20, 2000);
        }

        private void okrugloDugme2_MouseHover(object sender, EventArgs e)
        {

            tool.Show("Uključi/Isključi zvuk.", okrugloDugme2, -20, -20, 2000);
        }

        private void okrugloDugme3_MouseHover(object sender, EventArgs e)
        {
            tool.Show("Vrati se na početni ekran.", okrugloDugme3, -20, -20, 2000);

        }

        private void okrugloDugme6_Click(object sender, EventArgs e)
        {
           
            SaveFileDialog sf = new SaveFileDialog();
            sf.Filter = "BMP(*.bmp)|*.bmp|JPG(*.jpg)|*.jpg|PNG(*.png)|*.png|GIF(*.gif)|*.gif";
         
            Bitmap bitmapSave = new Bitmap(this.Width, this.Height);
            this.DrawToBitmap(bitmapSave, new Rectangle(0, 0, bitmapSave.Width, bitmapSave.Height));

            if (sf.ShowDialog() == DialogResult.OK)
            {
               
                bitmapSave.Save(sf.FileName);
            }
        }

        void ProvjeraPobjednika()
             {
            DialogResult dialogResult;
            if(listaIskoristenihKvadrata.Count==((visina+1)*(sirina+1))-(visina*sirina))
            {
                if(poeniPrvi>poeniDrugi)
                {
                    if (fanfare != null) { fanfare.Play(); }
                    dialogResult =MessageBox.Show("Pobijedio je " + label1.Text + "\nRezultat: \n" + label1.Text + ": " + poeniPrvi.ToString()+"\n"+label2.Text+": " + poeniDrugi.ToString());
                  
                }
            else if (poeniPrvi < poeniDrugi)
                {
                    if (fanfare != null) { fanfare.Play(); }
                    dialogResult =MessageBox.Show("Pobijedio je " + label2.Text + "\nRezultat: \n" + label1.Text + ": " + poeniPrvi.ToString() + "\n" + label2.Text + ": " + poeniDrugi.ToString());
                    
                }
                else
                {
                    if (wah != null) { wah.Play(); }
                    dialogResult =MessageBox.Show("Neriješeno je! " + "\nRezultat: \n" + label1.Text + ": " + poeniPrvi.ToString() + "\n" + label2.Text + ": " + poeniDrugi.ToString());
                    
                }
                if(dialogResult==DialogResult.OK)
                {
                    DialogResult dr;
                    dr = MessageBox.Show("Da li želite da počnete novu igru sa istim podešavanjima?", "Nova igra", MessageBoxButtons.YesNo);
                    if(dr==DialogResult.Yes)
                    {
                        this.Hide();
                        if (fanfare != null) { fanfare.Stop(); }
                        GlavniEkran glavniEkran = new GlavniEkran(pictureBox1.Image, pictureBox2.Image, label1.Text, label2.Text, (int)sirina, (int)visina);
                        glavniEkran.StartPosition = FormStartPosition.CenterScreen;
                        glavniEkran.ShowDialog();
                    }
                    if(dr==DialogResult.No)
                    {
                        if (fanfare != null) { fanfare.Stop(); }
                        Close();
                    }
                }
            }
        }
    }
}
