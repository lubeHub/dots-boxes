using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ognjen_Lubarda_Projekat
{
    class OkrugloDugme : Button
    {
        protected override void OnPaint(PaintEventArgs pevent)
        {
            GraphicsPath put = new GraphicsPath();
            put.AddEllipse(0, 0, ClientSize.Width, ClientSize.Height);
            this.Region = new System.Drawing.Region(put);
            base.OnPaint(pevent);
        }
    }
}
