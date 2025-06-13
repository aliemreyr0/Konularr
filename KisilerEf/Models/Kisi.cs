using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KisilerEf.Models
{
    public class Kisi //public olmalı
    {
        //Özellikler / Alanlar
        public int Id { get; set; }
        public string Adsoyad { get; set; }
        public string Eposta { get; set; }
        public int Yas { get; set; }
    }
}
