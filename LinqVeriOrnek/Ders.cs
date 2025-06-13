using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinqVeriOrnek
{
    internal class Ders
    {
        //Ogrenci class tipi ile bağlantılıdır. Ogrenci.Kimlik <==> Dersler.Ogrid
        public int Ogrid { get; set; }
        public string Dersadi { get; set; }
        public string Hocasi { get; set; }

    }
}
