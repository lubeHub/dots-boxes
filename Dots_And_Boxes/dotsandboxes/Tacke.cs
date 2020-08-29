using System;
using System.Drawing.Drawing2D;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace Ognjen_Lubarda_Projekat
{
    class Tacke
    {
        public int tacka1;
       public  int tacka2;
        

        public Tacke(int tacka11, int tacka21)
        {
            this.tacka1 = tacka11;
            this.tacka2 = tacka21;
        }
        public Tacke()
        {
            
        }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            if (ReferenceEquals(this, obj))
                return true;

            Tacke other = obj as Tacke;

            if (other == null)
                return false;

            return (tacka1 == other.tacka1 && tacka2 == other.tacka2) ||
                   (tacka1 == other.tacka2 && tacka2 == other.tacka1);
        }

        public override int GetHashCode()
        {
   
            return tacka1 + tacka2;
        }

        bool ProvjeriKvadrat(bool prvi, bool drugi, bool treci, bool cetvrti)
        {
            if(prvi==true || drugi==true || treci == true ||cetvrti == true)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
