using System.Reflection;
using System.Runtime.Intrinsics.Arm;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace LinqVeriOrnek
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        IList<Ogrenci> ogrenciler = new List<Ogrenci>();//Genel nesne
        IList<Urun> urunler = new List<Urun>();
        IList<Kitap> kitaplar = new List<Kitap>();
        IList<Ders> dersler = new List<Ders>();
        IList<Kisiler> kisiler = new List<Kisiler>();

        private void button1_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            //��renci listesini olu�tur. Yeni ��rencileri ekleme           
            ogrenciler.Add(new Ogrenci() { Kimlik = 1, Ad = "Ali", Yas = 15 });
            ogrenciler.Add(new Ogrenci() { Kimlik = 2, Ad = "Kemal", Yas = 20 });
            ogrenciler.Add(new Ogrenci() { Kimlik = 3, Ad = "Selami", Yas = 22 });
            ogrenciler.Add(new Ogrenci() { Kimlik = 4, Ad = "Zeki", Yas = 18 });
            ogrenciler.Add(new Ogrenci() { Kimlik = 5, Ad = "Murat", Yas = 23 });
            ogrenciler.Add(new Ogrenci() { Kimlik = 6, Ad = "Seher", Yas = 42 });
            ogrenciler.Add(new Ogrenci() { Kimlik = 7, Ad = "Ay�e", Yas = 28 });

            //Linq sat�r� (Sorgu yaz�m t�r�nde)
            //�sminde "a" ge�enler
            var sonuc1 = from ogr in ogrenciler where ogr.Ad.Contains("a") select ogr;//Sql tarz�
            //Ya�� 15 ile 20 aras�nda olanlar
            var sonuc2 = from ogr in ogrenciler where (ogr.Yas >= 15 && ogr.Yas <= 20) orderby ogr.Ad select ogr;
            listBox1.Items.Add("*�sminde \"a\" ge�enler");
            foreach (Ogrenci eleman in sonuc1)
            {
                listBox1.Items.Add("Kimlik:" + eleman.Kimlik + " Ad�:" + eleman.Ad + " Ya�:" + eleman.Yas);
            }
            listBox1.Items.Add("*Ya�� 15 ile 20 aras�nda olanlar");
            foreach (Ogrenci eleman in sonuc2)
            {
                listBox1.Items.Add("Kimlik:" + eleman.Kimlik + " Ad�:" + eleman.Ad + " Ya�:" + eleman.Yas);
            }



        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            //�r�n listesini olu�tur. Yeni �r�n ekleme
            urunler.Add(new Urun() { Kimlik = 1, Ad = "Defter", Fiyat = 5.0 });
            urunler.Add(new Urun() { Kimlik = 2, Ad = "Kitap", Fiyat = 6.0 });
            urunler.Add(new Urun() { Kimlik = 3, Ad = "Kalem", Fiyat = 4.0 });
            urunler.Add(new Urun() { Kimlik = 4, Ad = "Silgi", Fiyat = 3.0 });
            //Linq sat�r� (Metot yaz�m �eklinde)
            //�r�n isminde "a" ge�enler
            var sonuc1 = urunler.Where(urunbilgi => urunbilgi.Ad.Contains("a"));
            //�r�n fiyat�na g�re s�ralan�r.
            var sonuc2 = urunler.OrderBy(urunbilgi => urunbilgi.Fiyat);
            //�r�n isminde "a" ge�enlerin fiyat bilgisine g�re s�ralanmas�
            var sonuc3 = urunler.Where(urunbilgi => urunbilgi.Ad.Contains("a")).OrderBy(urunbilgi => urunbilgi.Fiyat);
            listBox1.Items.Add("*�r�n ad�nda \"a\" ge�enler");
            foreach (Urun eleman in sonuc1)
            {
                listBox1.Items.Add("�r�n No:" + eleman.Kimlik + " �r�n Ad�:" + eleman.Ad + " Fiyat�:" + eleman.Fiyat);
            }
            listBox1.Items.Add("*�r�n fiyat�na g�re s�ral� liste");
            foreach (Urun eleman in sonuc2)
            {
                listBox1.Items.Add("�r�n No:" + eleman.Kimlik + " �r�n Ad�:" + eleman.Ad + " Fiyat�:" + eleman.Fiyat);
            }
            listBox1.Items.Add("*�r�n isminde \"a\" ge�enlerin fiyat bilgisine g�re s�ralanmas�");
            foreach (Urun eleman in sonuc3)
            {
                listBox1.Items.Add("�r�n No:" + eleman.Kimlik + " �r�n Ad�:" + eleman.Ad + " Fiyat�:" + eleman.Fiyat);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            //Verileri girelim. Yeni kitap ekleme
            kitaplar.Add(new Kitap() { Kimlik = 1, Ad = "C#", Yazar = "Ali Demir", Yayinevi = "Alfa" });
            kitaplar.Add(new Kitap() { Kimlik = 2, Ad = "C++", Yazar = "Ahmet Deniz", Yayinevi = "Beta" });
            kitaplar.Add(new Kitap() { Kimlik = 3, Ad = "Delphi", Yazar = "Kemal Ak", Yayinevi = "Gama" });
            kitaplar.Add(new Kitap() { Kimlik = 4, Ad = "C Programlama", Yazar = "Burak �ak�r", Yayinevi = "Detay" });
            kitaplar.Add(new Kitap() { Kimlik = 5, Ad = "Flutter ile Programlama", Yazar = "Mustafa OF", Yayinevi = "Alfa" });
            //Linq Sorgusu
            //S�ral� liste
            var sonuc = from s in kitaplar orderby s.Yazar select s;
            //Yay�nevi, 4 karakterden uzun olanlar� yazar ad�na g�re s�ral� listele
            var sonuc2 = from s in kitaplar where s.Yayinevi.Length > 4 orderby s.Yazar select s;
            //1. S�ralama alan� yazar, 2. s�ralama alan� ad (Kitap�n ad�)
            var sonuc3 = kitaplar.OrderBy(s => s.Yazar).ThenBy(s => s.Ad);//B�y�kten k����e do�ru s�ralama i�in ThenByDescending kullan�l�r

            listBox1.Items.Add("*Kitaplar (Yazara G�re S�ral�)");
            foreach (Kitap eleman in sonuc)
            {
                listBox1.Items.Add("Kitap No:" + eleman.Kimlik + " Kitap Ad�:" + eleman.Ad + " Yazar�:"
                    + eleman.Yazar + " Yay�nevi:" + eleman.Yayinevi);
            }
            listBox1.Items.Add("*Yay�nevi, 4 karakterden uzun olanlar� yazar ad�na g�re s�ral� listele");
            foreach (Kitap eleman in sonuc2)
            {
                listBox1.Items.Add("Kitap No:" + eleman.Kimlik + " Kitap Ad�:" + eleman.Ad + " Yazar�:"
                    + eleman.Yazar + " Yay�nevi:" + eleman.Yayinevi);
            }
            listBox1.Items.Add("*1. S�ralama alan� yazar, 2. s�ralama alan� ad (Kitap�n ad�)");
            foreach (Kitap eleman in sonuc3)
            {
                listBox1.Items.Add("Kitap No:" + eleman.Kimlik + " Kitap Ad�:" + eleman.Ad + " Yazar�:"
                    + eleman.Yazar + " Yay�nevi:" + eleman.Yayinevi);
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            //��renci listesini olu�tur            
            ogrenciler.Add(new Ogrenci() { Kimlik = 1, Ad = "Ali", Yas = 15 });
            ogrenciler.Add(new Ogrenci() { Kimlik = 2, Ad = "Kemal", Yas = 20 });
            ogrenciler.Add(new Ogrenci() { Kimlik = 3, Ad = "Selami", Yas = 22 });
            ogrenciler.Add(new Ogrenci() { Kimlik = 4, Ad = "Zeki", Yas = 18 });
            ogrenciler.Add(new Ogrenci() { Kimlik = 5, Ad = "Murat", Yas = 23 });
            ogrenciler.Add(new Ogrenci() { Kimlik = 6, Ad = "Selami", Yas = 23 });
            //Ya��na g�re ��rencileri gruplad�
            var grupsonucu1 = from s in ogrenciler group s by s.Yas;
            //Metoda g�re
            var grupsonuc2 = ogrenciler.GroupBy(s => s.Yas);

            foreach (var yasgrubu in grupsonuc2)
            {
                listBox1.Items.Add("Ya� Grubu:" + yasgrubu.Key);
                foreach (Ogrenci eleman in yasgrubu)
                    listBox1.Items.Add("Kimlik: " + eleman.Kimlik + " Ad�:" + eleman.Ad + " Ya��:" + eleman.Yas);
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            //SelectMany metodu, bir koleksiyondaki her ��e �zerinde i�lem yaparak,
            //her ��eden bir koleksiyon olu�turan ve bu koleksiyonlar� tek bir koleksiyonda birle�tiren bir i�lemdir.
            listBox1.Items.Clear();
            List<string> kelimeler = ["bug�n hava �ok g�zel", "h�zl� ve atak biri"];
            //Dizi elemanlar�n� bo�luklardan kelimelere ay�r�r.
            var sonuc = kelimeler.SelectMany(kelimeler => kelimeler.Split(' '));

            foreach (string s in sonuc)
            {
                listBox1.Items.Add(s);
            }

            List<string> metin = ["Ali Aslan;5;Bilgisayar"];
            var sonuc2 = metin.SelectMany(metin => metin.Split(";"));
            foreach (string s in sonuc2)
            {
                listBox1.Items.Add(s);
            }


        }

        private void button6_Click(object sender, EventArgs e)
        {
            //Join
            listBox1.Items.Clear();
            IList<string> liste1 = new List<string>();
            liste1.Add("Ali");
            liste1.Add("Ahmet");
            liste1.Add("Ay�e");
            liste1.Add("Kemal");
            IList<string> liste2 = new List<string>();
            liste2.Add("Ali");
            liste2.Add("Ahmet");
            liste2.Add("�smail");
            liste2.Add("Ali");
            //liste1 de olan liste2 de olmayan 
            var sonuc1 = liste1.Join(liste2, //Outer liste
                s1 => s1, //Outer anahtar�
                s2 => s2, //Inner anahtar�
                (s1, s2) => s1);
            foreach (string eleman in sonuc1)
            {
                listBox1.Items.Add(eleman);
            }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            //�ki koleksiyondaki verilerin birle�tirilmesi
            listBox1.Items.Clear();
            //��renci listesini olu�tur            
            ogrenciler.Add(new Ogrenci() { Kimlik = 1, Ad = "Ali", Yas = 15 });
            ogrenciler.Add(new Ogrenci() { Kimlik = 2, Ad = "Kemal", Yas = 20 });
            ogrenciler.Add(new Ogrenci() { Kimlik = 3, Ad = "Selami", Yas = 22 });
            ogrenciler.Add(new Ogrenci() { Kimlik = 4, Ad = "Zeki", Yas = 18 });
            ogrenciler.Add(new Ogrenci() { Kimlik = 5, Ad = "Murat", Yas = 23 });
            ogrenciler.Add(new Ogrenci() { Kimlik = 6, Ad = "Selami", Yas = 23 });
            //Ders listesini olu�tur
            dersler.Add(new Ders() { Ogrid = 1, Dersadi = "C#", Hocasi = "Mustafa OF" });
            dersler.Add(new Ders() { Ogrid = 1, Dersadi = "Asp.Core", Hocasi = "Mustafa OF" });
            dersler.Add(new Ders() { Ogrid = 2, Dersadi = "Matematik", Hocasi = "Nevin Antar" });
            dersler.Add(new Ders() { Ogrid = 2, Dersadi = "C++", Hocasi = "Burak �ak�r" });
            //Inner Join kural�m
            var sonuc1 = from ogr in ogrenciler //Outer
                         join drs in dersler //inner
                         on ogr.Kimlik equals drs.Ogrid //Ba�lant�
                         select new
                         {
                             Ogrid = ogr.Kimlik,
                             Ogradi = ogr.Ad,
                             Dersi = drs.Dersadi,
                             Hocasi = drs.Hocasi
                         };
            foreach (var eleman in sonuc1)
            {
                listBox1.Items.Add("��renci Id:" + eleman.Ogrid + " Ad�:" + eleman.Ogradi + " Dersi:" + eleman.Dersi + " Hocas�:" + eleman.Hocasi);
            }

        }

        private void button8_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            //�r�n listesini olu�tur 
            urunler.Add(new Urun() { Kimlik = 1, Ad = "Defter", Fiyat = 5.0 });
            urunler.Add(new Urun() { Kimlik = 2, Ad = "Kitap", Fiyat = 6.0 });
            urunler.Add(new Urun() { Kimlik = 3, Ad = "Kalem", Fiyat = 4.0 });
            urunler.Add(new Urun() { Kimlik = 4, Ad = "Silgi", Fiyat = 4.0 });
            //Fiyatlara g�re grupla
            var sonuc1 = from urun1 in urunler group urun1 by urun1.Fiyat;
            //Metoda g�re sat�r ��yle olabilir
            var sonuc2 = urunler.GroupBy(urun1 => urun1.Fiyat);
            foreach (var fiyatgrup in sonuc1)
            {
                listBox1.Items.Add("Fiyatlara g�re grupland�. Alan ismi:" + fiyatgrup.Key);
                foreach (Urun eleman in fiyatgrup)
                    listBox1.Items.Add("Kimlik:" + eleman.Kimlik + " Ad:" + eleman.Ad + " Fiyat:" + eleman.Fiyat);
            }

        }

        private void button9_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            //��renci listesini olu�tur            
            ogrenciler.Add(new Ogrenci() { Kimlik = 1, Ad = "Ali", Yas = 15 });
            ogrenciler.Add(new Ogrenci() { Kimlik = 2, Ad = "Kemal", Yas = 20 });
            ogrenciler.Add(new Ogrenci() { Kimlik = 3, Ad = "Selami", Yas = 22 });
            ogrenciler.Add(new Ogrenci() { Kimlik = 4, Ad = "Zeki", Yas = 18 });
            ogrenciler.Add(new Ogrenci() { Kimlik = 5, Ad = "Murat", Yas = 23 });
            var sonuc1 = ogrenciler.Count(ogr => ogr.Yas >= 15 && ogr.Yas <= 20);//Say�s�
            var sonuc2 = ogrenciler.Average(ogr => ogr.Yas); //Ortalamas�
            var sonuc3 = ogrenciler.Max(ogr => ogr.Yas);//En b�y���
            var sonuc4 = ogrenciler.Min(ogr => ogr.Yas);//En k�����
            var sonuc5 = ogrenciler.Sum(ogr => ogr.Yas);//Toplam�
            listBox1.Items.Add("Ya�� 15-20 aras�nda olan ��renci say�s�:" + sonuc1.ToString());
            listBox1.Items.Add("��rencilerin ya� ortalamas�:" + sonuc2.ToString());
            listBox1.Items.Add("��rencilerin en b�y�k ya� de�eri:" + sonuc3.ToString());
            listBox1.Items.Add("��rencilerin en k���k ya� de�eri:" + sonuc4.ToString());
            listBox1.Items.Add("��rencilerin ya�lar�n�n toplam�:" + sonuc5.ToString());
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            //Ki�iler listesini olu�tur. Yeni ki�i ekleme
            kisiler.Add(new Kisiler() { Id = 1, Adsoyad = "Ali Aslan", Telefon = 3494351, Eposta = "aliaslan@ali.com" });
            kisiler.Add(new Kisiler() { Id = 2, Adsoyad = "Ahmet Ak", Telefon = 3494356, Eposta = "ahmetak@ahmetak.com" });
            kisiler.Add(new Kisiler() { Id = 3, Adsoyad = "Mustafa OF", Telefon = 3494350, Eposta = "mustafaof@hotmail.com" });
            //S�ral� liste
            var sonuc = from s in kisiler orderby s.Adsoyad select s;
            listBox1.Items.Add("*Ki�iler (Ad Soyada G�re S�ral�)");
            //Sonu� k�mesini listele
            foreach (Kisiler eleman in sonuc)
            {
                listBox1.Items.Add("Id:" + eleman.Id + " Ad� Soyad�:" + eleman.Adsoyad + " Telefonu:"
                    + eleman.Telefon + " Eposta:" + eleman.Eposta);
            }
            //Ad� soyad� i�erisinde 'a' ge�enler
            var sonuc2 = kisiler.Where(s => s.Adsoyad.Contains("a"));
            listBox1.Items.Add("*Ad� soyad� i�erisinde 'a' ge�enler");
            //Sonu� k�mesini listele
            foreach (Kisiler eleman in sonuc2)
            {
                listBox1.Items.Add("Id:" + eleman.Id + " Ad� Soyad�:" + eleman.Adsoyad + " Telefonu:"
                    + eleman.Telefon + " Eposta:" + eleman.Eposta);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            //SelectMany metodu, bir koleksiyondaki her ��e �zerinde i�lem yaparak,
            //her ��eden bir koleksiyon olu�turan ve bu koleksiyonlar� tek bir koleksiyonda birle�tiren bir i�lemdir.
            listBox1.Items.Clear();
            //Verileri girelim
            var siniflar = new List<List<Ogrenci>>
            {
            // 1. S�n�f ��rencileri
            new List<Ogrenci>
            {
                new Ogrenci { Kimlik = 1, Ad = "Ali", Yas = 8 },
                new Ogrenci { Kimlik = 2, Ad = "Veli", Yas = 7 }
            },
            // 2. S�n�f ��rencileri
            new List<Ogrenci>
            {
                new Ogrenci { Kimlik = 3, Ad = "Ay�e", Yas = 9 },
                new Ogrenci { Kimlik = 4, Ad = "Fatma", Yas = 8 }
            }
            };
            // SelectMany kullanarak t�m ��rencileri tek bir listeye al�yoruz
            var tumOgrenciler = siniflar.SelectMany(sinif => sinif).ToList();  // Her s�n�ftaki ��rencileri birle�tiriyoruz

            // Sonu�lar� ekrana yazd�ral�m
            foreach (var ogrenci in tumOgrenciler)
            {
                listBox1.Items.Add($"��renci Kimlik: {ogrenci.Kimlik}, Ad: {ogrenci.Ad}, Ya�: {ogrenci.Yas}");
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            // ��renciler
            var ogrenciler = new List<Ogrenci>
            {
            new Ogrenci { Kimlik = 1, Ad = "Ali", Yas = 8 },
            new Ogrenci { Kimlik = 2, Ad = "Veli", Yas = 7 },
            new Ogrenci { Kimlik = 3, Ad = "Ay�e", Yas = 9 }
            };
            //Dersler
            var dersler1 = new List<Ders>
            {
            new Ders { Dersadi = "Asp.Core", Hocasi = "��r.G�r.Dr. Mustafa OF" },
            new Ders { Dersadi = "C++", Hocasi = "��r.G�r. Burak �ak�r" }
            };
            var dersler2 = new List<Ders>
            {
            new Ders { Dersadi = "Matematik", Hocasi = "��r.G�r. Nevin Antar" },
            new Ders { Dersadi = "Veri Yap�lar�", Hocasi = "��r.G�r. Burak �ak�r" }
            };
            // ��renciler ve dersleri birbirine ba�lay�p t�m dersleri birle�tiriyoruz
            var ogrenciDersleri = ogrenciler.Zip(new List<List<Ders>> { dersler1, dersler2 },
                (ogrenci, dersler) => new { Ogrenci = ogrenci, Dersler = dersler })
                .SelectMany(ogrenciDers => ogrenciDers.Dersler,
                    (ogrenciDers, ders) => new { ogrenciDers.Ogrenci.Ad, ogrenciDers.Ogrenci.Kimlik, ders.Dersadi, ders.Hocasi })
                .ToList();
            // Sonu�lar� ekrana yazd�ral�m
            foreach (var ogrenciDers in ogrenciDersleri)
            {
                listBox1.Items.Add($"��renci Ad�: {ogrenciDers.Ad}, Kimlik: {ogrenciDers.Kimlik}, Ders: {ogrenciDers.Dersadi}, E�itimci: {ogrenciDers.Hocasi}");
            }
        }

        private void button13_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            // Personel listesi. Yeni personel ekleme
            var personeller = new List<Personel>
            {
                new Personel { Id = 1, Adsoyad = "Ahmet Ak", Birimi = "IT", Maas = 5000 },
                new Personel { Id = 2, Adsoyad = "Mehmet Deniz", Birimi = "Pazarlama", Maas = 4500 },
                new Personel { Id = 3, Adsoyad = "Ay�e Karaca", Birimi = "Muhasebe", Maas = 3500 },
                new Personel { Id = 4, Adsoyad = "Fatma Demir", Birimi = "IT", Maas = 6000 },
                new Personel { Id = 5, Adsoyad = "Ali K�l��", Birimi = "Pazarlama", Maas = 4700 }
            };
            // 1. Toplam maa�
            var toplamMaas = personeller.Sum(p => p.Maas);
            listBox1.Items.Add($"Toplam Maa�: {toplamMaas} TL");
            // 2. Ortalama maa�
            var ortalamaMaas = personeller.Average(p => p.Maas);
            listBox1.Items.Add($"Ortalama Maa�: {ortalamaMaas} TL");
            // 3. En y�ksek maa�
            var enYuksekMaas = personeller.Max(p => p.Maas);
            listBox1.Items.Add($"En Y�ksek Maa�: {enYuksekMaas} TL");
            // 4. En d���k maa�
            var enDusukMaas = personeller.Min(p => p.Maas);
            listBox1.Items.Add($"En D���k Maa�: {enDusukMaas} TL");
            // 5. Maa�� 5000 TL'den fazla olan personel say�s�
            var maasiYuksekOlanlar = personeller.Count(p => p.Maas > 5000);
            listBox1.Items.Add($"Maa�� 5000 TL'den fazla olan personel say�s�: {maasiYuksekOlanlar}");
            // 6. Personelleri birimlerine g�re grupland�rarak her birimin ortalama maa��n� g�sterelim
            var birimGruplari = personeller.GroupBy(p => p.Birimi)
            .Select(grup => new
            {
                Birim = grup.Key,
                OrtalamaMaas = grup.Average(p => p.Maas)
            });
            listBox1.Items.Add("\nBirimlere g�re ortalama maa�lar:");
            foreach (var grup in birimGruplari)
            {
                listBox1.Items.Add($"{grup.Birim}: {grup.OrtalamaMaas} TL");
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            //First ve FirstOrDefault metotlar�, LINQ ile koleksiyonlar �zerinde ilk ��eyi bulmak i�in kullan�lan metotlard�r.
            //Ancak, ikisi aras�nda �nemli bir fark vard�r:
            //First: E�er koleksiyon bo�sa veya �artlara uyan bir ��e bulunmazsa, hata verir 
            //irstOrDefault: E�er koleksiyon bo�sa veya �artlara uyan bir ��e bulunmazsa, null d�ner 
            listBox1.Items.Clear();
            // Personel listesi. Yeni personel ekleme
            var personeller = new List<Personel>
            {
                new Personel { Id = 1, Adsoyad = "Ahmet Ak", Birimi = "IT", Maas = 5000 },
                new Personel { Id = 2, Adsoyad = "Mehmet Deniz", Birimi = "Pazarlama", Maas = 4500 },
                new Personel { Id = 3, Adsoyad = "Ay�e Karaca", Birimi = "Muhasebe", Maas = 3500 },
                new Personel { Id = 4, Adsoyad = "Fatma Demir", Birimi = "IT", Maas = 6000 },
                new Personel { Id = 5, Adsoyad = "Ali K�l��", Birimi = "Pazarlama", Maas = 4700 }
            };
            // First() kullan�m�: Maa�� 4500 olan ilk personeli bulal�m
            var personelFirst = personeller.First(p => p.Maas == 4500);
            listBox1.Items.Add("First() - Personel: " + personelFirst.Adsoyad + ", Birimi: " + personelFirst.Birimi);
            // FirstOrDefault() kullan�m�: Maa�� 6000 olan ilk personeli bulal�m
            var personelFirstOrDefault = personeller.FirstOrDefault(p => p.Maas == 6000);
            if (personelFirstOrDefault != null)
            {
                listBox1.Items.Add("FirstOrDefault() - Personel: " + personelFirstOrDefault.Adsoyad + ", Birimi: " + personelFirstOrDefault.Birimi);
            }
            else
            {
                listBox1.Items.Add("FirstOrDefault() - �artlar� sa�layan personel bulunamad�!");
            }
            // FirstOrDefault() kullan�m�: Maa�� 10000 olan ilk personeli bulal�m (bulunamayacak)
            var personel10bin = personeller.FirstOrDefault(p => p.Maas == 10000);
            if (personel10bin != null)
            {
                listBox1.Items.Add("FirstOrDefault() - Personel: " + personel10bin.Adsoyad + ", Birimi: " + personel10bin.Birimi);
            }
            else
            {
                listBox1.Items.Add("FirstOrDefault() - Maa�� 10000 olan personel bulunamad�!");
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            //ElementAt() metodu, LINQ'de belirli bir indeksin kar��l�k geldi�i ��eyi almak i�in kullan�l�r.
            //Bu metodun bir di�er benzer fonksiyonu ElementAtOrDefault()'tur,
            //ancak ElementAtOrDefault() metodu belirtilen indekste ��e yoksa, varsay�lan de�er (�rne�in null veya s�f�r) d�ner.
            // Personel listesi
            var personeller = new List<Personel>
            {
                new Personel { Id = 1, Adsoyad = "Ahmet Y�lmaz", Birimi = "IT", Maas = 5000 },
                new Personel { Id = 2, Adsoyad = "Mehmet Demir", Birimi = "Pazarlama", Maas = 4500 },
                new Personel { Id = 3, Adsoyad = "Ay�e Karaca", Birimi = "Muhasebe", Maas = 3500 },
                new Personel { Id = 4, Adsoyad = "Fatma Y�lmaz", Birimi = "IT", Maas = 6000 },
                new Personel { Id = 5, Adsoyad = "Ali K�l��", Birimi = "Pazarlama", Maas = 4700 }
            };
            // ElementAt() kullan�m�: 2. indeksteki personeli alal�m (indeks 2, 3. personeli verir)
            var personelElementAt = personeller.ElementAt(2); // 3. personel, ��nk� indeks 0'dan ba�lar
            listBox1.Items.Add("ElementAt() - Personel: " + personelElementAt.Adsoyad + ", Birimi: " + personelElementAt.Birimi);
            // ElementAtOrDefault() kullan�m�: 10. indeksteki personeli alal�m (bulunamayacak)
            var personelElementAtOrDefault = personeller.ElementAtOrDefault(10);
            if (personelElementAtOrDefault != null)
            {
                listBox1.Items.Add("ElementAtOrDefault() - Personel: " + personelElementAtOrDefault.Adsoyad + ", Birimi: " + personelElementAtOrDefault.Birimi);
            }
            else
            {
                listBox1.Items.Add("ElementAtOrDefault() - 10. indekste personel bulunamad�!");
            }

        }

        private void button16_Click(object sender, EventArgs e)
        {
            //Last() ve LastOrDefault() metotlar�, LINQ'de koleksiyondaki son ��eyi almak i�in kullan�l�r.
            //Bu metotlar�n birbirinden fark� �udur:
            //Last(): E�er koleksiyon bo�sa veya ko�ula uyan ��e bulunamazsa, InvalidOperationException hatas� f�rlat�r.
            //LastOrDefault(): E�er koleksiyon bo�sa veya ko�ula uyan ��e bulunamazsa, null (referans tipleri i�in) veya varsay�lan de�er (de�er tipleri i�in) d�ner.
            listBox1.Items.Clear();
            // Personel listesi
            var personeller = new List<Personel>
            {
                new Personel { Id = 1, Adsoyad = "Ahmet Y�lmaz", Birimi = "IT", Maas = 5000 },
                new Personel { Id = 2, Adsoyad = "Mehmet Demir", Birimi = "Pazarlama", Maas = 4500 },
                new Personel { Id = 3, Adsoyad = "Ay�e Karaca", Birimi = "Muhasebe", Maas = 3500 },
                new Personel { Id = 4, Adsoyad = "Fatma Y�lmaz", Birimi = "IT", Maas = 6000 },
                new Personel { Id = 5, Adsoyad = "Ali K�l��", Birimi = "Pazarlama", Maas = 4700 }
            };
            // Last() kullan�m�: Maa�� 4500'den fazla olan son personeli alal�m
            var personelLast = personeller.Last(p => p.Maas >= 4500);
            listBox1.Items.Add("Last() - Personel: " + personelLast.Adsoyad + ", Birimi: " + personelLast.Birimi);
            // LastOrDefault() kullan�m�: Maa�� 4500'den fazla olan son personeli alal�m
            var personelLastOrDefault = personeller.LastOrDefault(p => p.Maas >= 4500);
            if (personelLastOrDefault != null)
            {
                Console.WriteLine("LastOrDefault() - Personel: " + personelLastOrDefault.Adsoyad + ", Birimi: " + personelLastOrDefault.Birimi);
            }
            else
            {
                Console.WriteLine("LastOrDefault() - Maa�� 4500 TL veya daha fazla olan personel bulunamad�!");
            }
            // LastOrDefault() kullan�m�: Maa�� 10000 TL olan son personeli alal�m (bulunamayacak)
            var personelNotFound = personeller.LastOrDefault(p => p.Maas == 10000);
            if (personelNotFound != null)
            {
                Console.WriteLine("LastOrDefault() - Personel: " + personelNotFound.Adsoyad + ", Birimi: " + personelNotFound.Birimi);
            }
            else
            {
                Console.WriteLine("LastOrDefault() - Maa�� 10000 TL olan personel bulunamad�!");
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            //Single() ve SingleOrDefault() metotlar�, LINQ'de tek bir ��e aramak i�in kullan�l�r.
            //Bu metotlar�n kullan�m farklar�, koleksiyonda birden fazla ��e bulunup bulunmad���na g�re farkl� davran��lar sergilemeleridir:
            //Single(): Ko�ula uyan **tam olarak bir ��e olmas� gerekti�ini varsayar. E�er koleksiyonda birden fazla ��e varsa veya hi� ��e yoksa,
            //InvalidOperationException hatas� f�rlat�r.
            //SingleOrDefault(): Ko�ula uyan s�f�r veya bir ��e olmal�d�r. E�er ��e yoksa null (referans tipleri i�in) veya varsay�lan de�er (de�er tipleri i�in) d�ner.
            //E�er birden fazla ��e varsa, InvalidOperationException hatas� f�rlat�l�r.
            listBox1.Items.Clear();
            // Personel listesi
            var personeller = new List<Personel>
            {
                new Personel { Id = 1, Adsoyad = "Ahmet Y�lmaz", Maas = 5000 },
                new Personel { Id = 2, Adsoyad = "Mehmet Demir", Maas = 4500 },
                new Personel { Id = 3, Adsoyad = "Ay�e Karaca", Maas = 3500 },
                new Personel { Id = 4, Adsoyad = "Ahmet Y�lmaz", Maas = 6000 },  // Ayn� isimli 2. personel
                new Personel { Id = 5, Adsoyad = "Ali K�l��", Maas = 4700 }
            };
            // Single() kullan�m�: "Ahmet Y�lmaz" ad�nda sadece bir personel olmal�            
            var personelSingle = personeller.Single(p => p.Adsoyad == "Ahmet Y�lmaz");

            listBox1.Items.Add("Single() - Personel: " + personelSingle.Adsoyad + ", Maa�: " + personelSingle.Maas);
            // SingleOrDefault() kullan�m�: "Ahmet Y�lmaz" ad�nda sadece bir personel olmal�
            var personelSingleOrDefault = personeller.SingleOrDefault(p => p.Adsoyad == "Ahmet Y�lmaz");
            if (personelSingleOrDefault != null)
            {
                listBox1.Items.Add("SingleOrDefault() - Personel: " + personelSingleOrDefault.Adsoyad + ", Maa�: " + personelSingleOrDefault.Maas);
            }
            else
            {
                listBox1.Items.Add("SingleOrDefault() - 'Ahmet Y�lmaz' ad�nda personel bulunamad�!");
            }
            // SingleOrDefault() kullan�m�: "Ali Veli" ad�nda bir personel var m�? (bulunamayacak)
            var personelNotFound = personeller.SingleOrDefault(p => p.Adsoyad == "Ali Veli");
            if (personelNotFound != null)
            {
                listBox1.Items.Add("SingleOrDefault() - Personel: " + personelNotFound.Adsoyad + ", Maa�: " + personelNotFound.Maas);
            }
            else
            {
                listBox1.Items.Add("SingleOrDefault() - 'Ali Veli' ad�nda personel bulunamad�!");
            }
        }
    }

    class Kisiler
    {
        //Kisiler class t�r�
        public int Id { get; set; }
        public String Adsoyad { get; set; }
        public int Telefon { get; set; }
        public String Eposta { get; set; }
    }
}
