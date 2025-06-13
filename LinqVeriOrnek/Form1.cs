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
            //Öðrenci listesini oluþtur. Yeni öðrencileri ekleme           
            ogrenciler.Add(new Ogrenci() { Kimlik = 1, Ad = "Ali", Yas = 15 });
            ogrenciler.Add(new Ogrenci() { Kimlik = 2, Ad = "Kemal", Yas = 20 });
            ogrenciler.Add(new Ogrenci() { Kimlik = 3, Ad = "Selami", Yas = 22 });
            ogrenciler.Add(new Ogrenci() { Kimlik = 4, Ad = "Zeki", Yas = 18 });
            ogrenciler.Add(new Ogrenci() { Kimlik = 5, Ad = "Murat", Yas = 23 });
            ogrenciler.Add(new Ogrenci() { Kimlik = 6, Ad = "Seher", Yas = 42 });
            ogrenciler.Add(new Ogrenci() { Kimlik = 7, Ad = "Ayþe", Yas = 28 });

            //Linq satýrý (Sorgu yazým türünde)
            //Ýsminde "a" geçenler
            var sonuc1 = from ogr in ogrenciler where ogr.Ad.Contains("a") select ogr;//Sql tarzý
            //Yaþý 15 ile 20 arasýnda olanlar
            var sonuc2 = from ogr in ogrenciler where (ogr.Yas >= 15 && ogr.Yas <= 20) orderby ogr.Ad select ogr;
            listBox1.Items.Add("*Ýsminde \"a\" geçenler");
            foreach (Ogrenci eleman in sonuc1)
            {
                listBox1.Items.Add("Kimlik:" + eleman.Kimlik + " Adý:" + eleman.Ad + " Yaþ:" + eleman.Yas);
            }
            listBox1.Items.Add("*Yaþý 15 ile 20 arasýnda olanlar");
            foreach (Ogrenci eleman in sonuc2)
            {
                listBox1.Items.Add("Kimlik:" + eleman.Kimlik + " Adý:" + eleman.Ad + " Yaþ:" + eleman.Yas);
            }



        }

        private void button2_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            //Ürün listesini oluþtur. Yeni ürün ekleme
            urunler.Add(new Urun() { Kimlik = 1, Ad = "Defter", Fiyat = 5.0 });
            urunler.Add(new Urun() { Kimlik = 2, Ad = "Kitap", Fiyat = 6.0 });
            urunler.Add(new Urun() { Kimlik = 3, Ad = "Kalem", Fiyat = 4.0 });
            urunler.Add(new Urun() { Kimlik = 4, Ad = "Silgi", Fiyat = 3.0 });
            //Linq satýrý (Metot yazým þeklinde)
            //Ürün isminde "a" geçenler
            var sonuc1 = urunler.Where(urunbilgi => urunbilgi.Ad.Contains("a"));
            //Ürün fiyatýna göre sýralanýr.
            var sonuc2 = urunler.OrderBy(urunbilgi => urunbilgi.Fiyat);
            //Ürün isminde "a" geçenlerin fiyat bilgisine göre sýralanmasý
            var sonuc3 = urunler.Where(urunbilgi => urunbilgi.Ad.Contains("a")).OrderBy(urunbilgi => urunbilgi.Fiyat);
            listBox1.Items.Add("*Ürün adýnda \"a\" geçenler");
            foreach (Urun eleman in sonuc1)
            {
                listBox1.Items.Add("Ürün No:" + eleman.Kimlik + " Ürün Adý:" + eleman.Ad + " Fiyatý:" + eleman.Fiyat);
            }
            listBox1.Items.Add("*Ürün fiyatýna göre sýralý liste");
            foreach (Urun eleman in sonuc2)
            {
                listBox1.Items.Add("Ürün No:" + eleman.Kimlik + " Ürün Adý:" + eleman.Ad + " Fiyatý:" + eleman.Fiyat);
            }
            listBox1.Items.Add("*Ürün isminde \"a\" geçenlerin fiyat bilgisine göre sýralanmasý");
            foreach (Urun eleman in sonuc3)
            {
                listBox1.Items.Add("Ürün No:" + eleman.Kimlik + " Ürün Adý:" + eleman.Ad + " Fiyatý:" + eleman.Fiyat);
            }

        }

        private void button3_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            //Verileri girelim. Yeni kitap ekleme
            kitaplar.Add(new Kitap() { Kimlik = 1, Ad = "C#", Yazar = "Ali Demir", Yayinevi = "Alfa" });
            kitaplar.Add(new Kitap() { Kimlik = 2, Ad = "C++", Yazar = "Ahmet Deniz", Yayinevi = "Beta" });
            kitaplar.Add(new Kitap() { Kimlik = 3, Ad = "Delphi", Yazar = "Kemal Ak", Yayinevi = "Gama" });
            kitaplar.Add(new Kitap() { Kimlik = 4, Ad = "C Programlama", Yazar = "Burak Çakýr", Yayinevi = "Detay" });
            kitaplar.Add(new Kitap() { Kimlik = 5, Ad = "Flutter ile Programlama", Yazar = "Mustafa OF", Yayinevi = "Alfa" });
            //Linq Sorgusu
            //Sýralý liste
            var sonuc = from s in kitaplar orderby s.Yazar select s;
            //Yayýnevi, 4 karakterden uzun olanlarý yazar adýna göre sýralý listele
            var sonuc2 = from s in kitaplar where s.Yayinevi.Length > 4 orderby s.Yazar select s;
            //1. Sýralama alaný yazar, 2. sýralama alaný ad (Kitapýn adý)
            var sonuc3 = kitaplar.OrderBy(s => s.Yazar).ThenBy(s => s.Ad);//Büyükten küçüðe doðru sýralama için ThenByDescending kullanýlýr

            listBox1.Items.Add("*Kitaplar (Yazara Göre Sýralý)");
            foreach (Kitap eleman in sonuc)
            {
                listBox1.Items.Add("Kitap No:" + eleman.Kimlik + " Kitap Adý:" + eleman.Ad + " Yazarý:"
                    + eleman.Yazar + " Yayýnevi:" + eleman.Yayinevi);
            }
            listBox1.Items.Add("*Yayýnevi, 4 karakterden uzun olanlarý yazar adýna göre sýralý listele");
            foreach (Kitap eleman in sonuc2)
            {
                listBox1.Items.Add("Kitap No:" + eleman.Kimlik + " Kitap Adý:" + eleman.Ad + " Yazarý:"
                    + eleman.Yazar + " Yayýnevi:" + eleman.Yayinevi);
            }
            listBox1.Items.Add("*1. Sýralama alaný yazar, 2. sýralama alaný ad (Kitapýn adý)");
            foreach (Kitap eleman in sonuc3)
            {
                listBox1.Items.Add("Kitap No:" + eleman.Kimlik + " Kitap Adý:" + eleman.Ad + " Yazarý:"
                    + eleman.Yazar + " Yayýnevi:" + eleman.Yayinevi);
            }

        }

        private void button4_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            //Öðrenci listesini oluþtur            
            ogrenciler.Add(new Ogrenci() { Kimlik = 1, Ad = "Ali", Yas = 15 });
            ogrenciler.Add(new Ogrenci() { Kimlik = 2, Ad = "Kemal", Yas = 20 });
            ogrenciler.Add(new Ogrenci() { Kimlik = 3, Ad = "Selami", Yas = 22 });
            ogrenciler.Add(new Ogrenci() { Kimlik = 4, Ad = "Zeki", Yas = 18 });
            ogrenciler.Add(new Ogrenci() { Kimlik = 5, Ad = "Murat", Yas = 23 });
            ogrenciler.Add(new Ogrenci() { Kimlik = 6, Ad = "Selami", Yas = 23 });
            //Yaþýna göre öðrencileri grupladý
            var grupsonucu1 = from s in ogrenciler group s by s.Yas;
            //Metoda göre
            var grupsonuc2 = ogrenciler.GroupBy(s => s.Yas);

            foreach (var yasgrubu in grupsonuc2)
            {
                listBox1.Items.Add("Yaþ Grubu:" + yasgrubu.Key);
                foreach (Ogrenci eleman in yasgrubu)
                    listBox1.Items.Add("Kimlik: " + eleman.Kimlik + " Adý:" + eleman.Ad + " Yaþý:" + eleman.Yas);
            }

        }

        private void button5_Click(object sender, EventArgs e)
        {
            //SelectMany metodu, bir koleksiyondaki her öðe üzerinde iþlem yaparak,
            //her öðeden bir koleksiyon oluþturan ve bu koleksiyonlarý tek bir koleksiyonda birleþtiren bir iþlemdir.
            listBox1.Items.Clear();
            List<string> kelimeler = ["bugün hava çok güzel", "hýzlý ve atak biri"];
            //Dizi elemanlarýný boþluklardan kelimelere ayýrýr.
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
            liste1.Add("Ayþe");
            liste1.Add("Kemal");
            IList<string> liste2 = new List<string>();
            liste2.Add("Ali");
            liste2.Add("Ahmet");
            liste2.Add("Ýsmail");
            liste2.Add("Ali");
            //liste1 de olan liste2 de olmayan 
            var sonuc1 = liste1.Join(liste2, //Outer liste
                s1 => s1, //Outer anahtarý
                s2 => s2, //Inner anahtarý
                (s1, s2) => s1);
            foreach (string eleman in sonuc1)
            {
                listBox1.Items.Add(eleman);
            }

        }

        private void button7_Click(object sender, EventArgs e)
        {
            //Ýki koleksiyondaki verilerin birleþtirilmesi
            listBox1.Items.Clear();
            //Öðrenci listesini oluþtur            
            ogrenciler.Add(new Ogrenci() { Kimlik = 1, Ad = "Ali", Yas = 15 });
            ogrenciler.Add(new Ogrenci() { Kimlik = 2, Ad = "Kemal", Yas = 20 });
            ogrenciler.Add(new Ogrenci() { Kimlik = 3, Ad = "Selami", Yas = 22 });
            ogrenciler.Add(new Ogrenci() { Kimlik = 4, Ad = "Zeki", Yas = 18 });
            ogrenciler.Add(new Ogrenci() { Kimlik = 5, Ad = "Murat", Yas = 23 });
            ogrenciler.Add(new Ogrenci() { Kimlik = 6, Ad = "Selami", Yas = 23 });
            //Ders listesini oluþtur
            dersler.Add(new Ders() { Ogrid = 1, Dersadi = "C#", Hocasi = "Mustafa OF" });
            dersler.Add(new Ders() { Ogrid = 1, Dersadi = "Asp.Core", Hocasi = "Mustafa OF" });
            dersler.Add(new Ders() { Ogrid = 2, Dersadi = "Matematik", Hocasi = "Nevin Antar" });
            dersler.Add(new Ders() { Ogrid = 2, Dersadi = "C++", Hocasi = "Burak Çakýr" });
            //Inner Join kuralým
            var sonuc1 = from ogr in ogrenciler //Outer
                         join drs in dersler //inner
                         on ogr.Kimlik equals drs.Ogrid //Baðlantý
                         select new
                         {
                             Ogrid = ogr.Kimlik,
                             Ogradi = ogr.Ad,
                             Dersi = drs.Dersadi,
                             Hocasi = drs.Hocasi
                         };
            foreach (var eleman in sonuc1)
            {
                listBox1.Items.Add("Öðrenci Id:" + eleman.Ogrid + " Adý:" + eleman.Ogradi + " Dersi:" + eleman.Dersi + " Hocasý:" + eleman.Hocasi);
            }

        }

        private void button8_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            //Ürün listesini oluþtur 
            urunler.Add(new Urun() { Kimlik = 1, Ad = "Defter", Fiyat = 5.0 });
            urunler.Add(new Urun() { Kimlik = 2, Ad = "Kitap", Fiyat = 6.0 });
            urunler.Add(new Urun() { Kimlik = 3, Ad = "Kalem", Fiyat = 4.0 });
            urunler.Add(new Urun() { Kimlik = 4, Ad = "Silgi", Fiyat = 4.0 });
            //Fiyatlara göre grupla
            var sonuc1 = from urun1 in urunler group urun1 by urun1.Fiyat;
            //Metoda göre satýr þöyle olabilir
            var sonuc2 = urunler.GroupBy(urun1 => urun1.Fiyat);
            foreach (var fiyatgrup in sonuc1)
            {
                listBox1.Items.Add("Fiyatlara göre gruplandý. Alan ismi:" + fiyatgrup.Key);
                foreach (Urun eleman in fiyatgrup)
                    listBox1.Items.Add("Kimlik:" + eleman.Kimlik + " Ad:" + eleman.Ad + " Fiyat:" + eleman.Fiyat);
            }

        }

        private void button9_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            //Öðrenci listesini oluþtur            
            ogrenciler.Add(new Ogrenci() { Kimlik = 1, Ad = "Ali", Yas = 15 });
            ogrenciler.Add(new Ogrenci() { Kimlik = 2, Ad = "Kemal", Yas = 20 });
            ogrenciler.Add(new Ogrenci() { Kimlik = 3, Ad = "Selami", Yas = 22 });
            ogrenciler.Add(new Ogrenci() { Kimlik = 4, Ad = "Zeki", Yas = 18 });
            ogrenciler.Add(new Ogrenci() { Kimlik = 5, Ad = "Murat", Yas = 23 });
            var sonuc1 = ogrenciler.Count(ogr => ogr.Yas >= 15 && ogr.Yas <= 20);//Sayýsý
            var sonuc2 = ogrenciler.Average(ogr => ogr.Yas); //Ortalamasý
            var sonuc3 = ogrenciler.Max(ogr => ogr.Yas);//En büyüðü
            var sonuc4 = ogrenciler.Min(ogr => ogr.Yas);//En küçüðü
            var sonuc5 = ogrenciler.Sum(ogr => ogr.Yas);//Toplamý
            listBox1.Items.Add("Yaþý 15-20 arasýnda olan öðrenci sayýsý:" + sonuc1.ToString());
            listBox1.Items.Add("Öðrencilerin yaþ ortalamasý:" + sonuc2.ToString());
            listBox1.Items.Add("Öðrencilerin en büyük yaþ deðeri:" + sonuc3.ToString());
            listBox1.Items.Add("Öðrencilerin en küçük yaþ deðeri:" + sonuc4.ToString());
            listBox1.Items.Add("Öðrencilerin yaþlarýnýn toplamý:" + sonuc5.ToString());
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void button10_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            //Kiþiler listesini oluþtur. Yeni kiþi ekleme
            kisiler.Add(new Kisiler() { Id = 1, Adsoyad = "Ali Aslan", Telefon = 3494351, Eposta = "aliaslan@ali.com" });
            kisiler.Add(new Kisiler() { Id = 2, Adsoyad = "Ahmet Ak", Telefon = 3494356, Eposta = "ahmetak@ahmetak.com" });
            kisiler.Add(new Kisiler() { Id = 3, Adsoyad = "Mustafa OF", Telefon = 3494350, Eposta = "mustafaof@hotmail.com" });
            //Sýralý liste
            var sonuc = from s in kisiler orderby s.Adsoyad select s;
            listBox1.Items.Add("*Kiþiler (Ad Soyada Göre Sýralý)");
            //Sonuç kümesini listele
            foreach (Kisiler eleman in sonuc)
            {
                listBox1.Items.Add("Id:" + eleman.Id + " Adý Soyadý:" + eleman.Adsoyad + " Telefonu:"
                    + eleman.Telefon + " Eposta:" + eleman.Eposta);
            }
            //Adý soyadý içerisinde 'a' geçenler
            var sonuc2 = kisiler.Where(s => s.Adsoyad.Contains("a"));
            listBox1.Items.Add("*Adý soyadý içerisinde 'a' geçenler");
            //Sonuç kümesini listele
            foreach (Kisiler eleman in sonuc2)
            {
                listBox1.Items.Add("Id:" + eleman.Id + " Adý Soyadý:" + eleman.Adsoyad + " Telefonu:"
                    + eleman.Telefon + " Eposta:" + eleman.Eposta);
            }
        }

        private void button11_Click(object sender, EventArgs e)
        {
            //SelectMany metodu, bir koleksiyondaki her öðe üzerinde iþlem yaparak,
            //her öðeden bir koleksiyon oluþturan ve bu koleksiyonlarý tek bir koleksiyonda birleþtiren bir iþlemdir.
            listBox1.Items.Clear();
            //Verileri girelim
            var siniflar = new List<List<Ogrenci>>
            {
            // 1. Sýnýf Öðrencileri
            new List<Ogrenci>
            {
                new Ogrenci { Kimlik = 1, Ad = "Ali", Yas = 8 },
                new Ogrenci { Kimlik = 2, Ad = "Veli", Yas = 7 }
            },
            // 2. Sýnýf Öðrencileri
            new List<Ogrenci>
            {
                new Ogrenci { Kimlik = 3, Ad = "Ayþe", Yas = 9 },
                new Ogrenci { Kimlik = 4, Ad = "Fatma", Yas = 8 }
            }
            };
            // SelectMany kullanarak tüm öðrencileri tek bir listeye alýyoruz
            var tumOgrenciler = siniflar.SelectMany(sinif => sinif).ToList();  // Her sýnýftaki öðrencileri birleþtiriyoruz

            // Sonuçlarý ekrana yazdýralým
            foreach (var ogrenci in tumOgrenciler)
            {
                listBox1.Items.Add($"Öðrenci Kimlik: {ogrenci.Kimlik}, Ad: {ogrenci.Ad}, Yaþ: {ogrenci.Yas}");
            }
        }

        private void button12_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            // Öðrenciler
            var ogrenciler = new List<Ogrenci>
            {
            new Ogrenci { Kimlik = 1, Ad = "Ali", Yas = 8 },
            new Ogrenci { Kimlik = 2, Ad = "Veli", Yas = 7 },
            new Ogrenci { Kimlik = 3, Ad = "Ayþe", Yas = 9 }
            };
            //Dersler
            var dersler1 = new List<Ders>
            {
            new Ders { Dersadi = "Asp.Core", Hocasi = "Öðr.Gör.Dr. Mustafa OF" },
            new Ders { Dersadi = "C++", Hocasi = "Öðr.Gör. Burak Çakýr" }
            };
            var dersler2 = new List<Ders>
            {
            new Ders { Dersadi = "Matematik", Hocasi = "Öðr.Gör. Nevin Antar" },
            new Ders { Dersadi = "Veri Yapýlarý", Hocasi = "Öðr.Gör. Burak Çakýr" }
            };
            // Öðrenciler ve dersleri birbirine baðlayýp tüm dersleri birleþtiriyoruz
            var ogrenciDersleri = ogrenciler.Zip(new List<List<Ders>> { dersler1, dersler2 },
                (ogrenci, dersler) => new { Ogrenci = ogrenci, Dersler = dersler })
                .SelectMany(ogrenciDers => ogrenciDers.Dersler,
                    (ogrenciDers, ders) => new { ogrenciDers.Ogrenci.Ad, ogrenciDers.Ogrenci.Kimlik, ders.Dersadi, ders.Hocasi })
                .ToList();
            // Sonuçlarý ekrana yazdýralým
            foreach (var ogrenciDers in ogrenciDersleri)
            {
                listBox1.Items.Add($"Öðrenci Adý: {ogrenciDers.Ad}, Kimlik: {ogrenciDers.Kimlik}, Ders: {ogrenciDers.Dersadi}, Eðitimci: {ogrenciDers.Hocasi}");
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
                new Personel { Id = 3, Adsoyad = "Ayþe Karaca", Birimi = "Muhasebe", Maas = 3500 },
                new Personel { Id = 4, Adsoyad = "Fatma Demir", Birimi = "IT", Maas = 6000 },
                new Personel { Id = 5, Adsoyad = "Ali Kýlýç", Birimi = "Pazarlama", Maas = 4700 }
            };
            // 1. Toplam maaþ
            var toplamMaas = personeller.Sum(p => p.Maas);
            listBox1.Items.Add($"Toplam Maaþ: {toplamMaas} TL");
            // 2. Ortalama maaþ
            var ortalamaMaas = personeller.Average(p => p.Maas);
            listBox1.Items.Add($"Ortalama Maaþ: {ortalamaMaas} TL");
            // 3. En yüksek maaþ
            var enYuksekMaas = personeller.Max(p => p.Maas);
            listBox1.Items.Add($"En Yüksek Maaþ: {enYuksekMaas} TL");
            // 4. En düþük maaþ
            var enDusukMaas = personeller.Min(p => p.Maas);
            listBox1.Items.Add($"En Düþük Maaþ: {enDusukMaas} TL");
            // 5. Maaþý 5000 TL'den fazla olan personel sayýsý
            var maasiYuksekOlanlar = personeller.Count(p => p.Maas > 5000);
            listBox1.Items.Add($"Maaþý 5000 TL'den fazla olan personel sayýsý: {maasiYuksekOlanlar}");
            // 6. Personelleri birimlerine göre gruplandýrarak her birimin ortalama maaþýný gösterelim
            var birimGruplari = personeller.GroupBy(p => p.Birimi)
            .Select(grup => new
            {
                Birim = grup.Key,
                OrtalamaMaas = grup.Average(p => p.Maas)
            });
            listBox1.Items.Add("\nBirimlere göre ortalama maaþlar:");
            foreach (var grup in birimGruplari)
            {
                listBox1.Items.Add($"{grup.Birim}: {grup.OrtalamaMaas} TL");
            }
        }

        private void button14_Click(object sender, EventArgs e)
        {
            //First ve FirstOrDefault metotlarý, LINQ ile koleksiyonlar üzerinde ilk öðeyi bulmak için kullanýlan metotlardýr.
            //Ancak, ikisi arasýnda önemli bir fark vardýr:
            //First: Eðer koleksiyon boþsa veya þartlara uyan bir öðe bulunmazsa, hata verir 
            //irstOrDefault: Eðer koleksiyon boþsa veya þartlara uyan bir öðe bulunmazsa, null döner 
            listBox1.Items.Clear();
            // Personel listesi. Yeni personel ekleme
            var personeller = new List<Personel>
            {
                new Personel { Id = 1, Adsoyad = "Ahmet Ak", Birimi = "IT", Maas = 5000 },
                new Personel { Id = 2, Adsoyad = "Mehmet Deniz", Birimi = "Pazarlama", Maas = 4500 },
                new Personel { Id = 3, Adsoyad = "Ayþe Karaca", Birimi = "Muhasebe", Maas = 3500 },
                new Personel { Id = 4, Adsoyad = "Fatma Demir", Birimi = "IT", Maas = 6000 },
                new Personel { Id = 5, Adsoyad = "Ali Kýlýç", Birimi = "Pazarlama", Maas = 4700 }
            };
            // First() kullanýmý: Maaþý 4500 olan ilk personeli bulalým
            var personelFirst = personeller.First(p => p.Maas == 4500);
            listBox1.Items.Add("First() - Personel: " + personelFirst.Adsoyad + ", Birimi: " + personelFirst.Birimi);
            // FirstOrDefault() kullanýmý: Maaþý 6000 olan ilk personeli bulalým
            var personelFirstOrDefault = personeller.FirstOrDefault(p => p.Maas == 6000);
            if (personelFirstOrDefault != null)
            {
                listBox1.Items.Add("FirstOrDefault() - Personel: " + personelFirstOrDefault.Adsoyad + ", Birimi: " + personelFirstOrDefault.Birimi);
            }
            else
            {
                listBox1.Items.Add("FirstOrDefault() - Þartlarý saðlayan personel bulunamadý!");
            }
            // FirstOrDefault() kullanýmý: Maaþý 10000 olan ilk personeli bulalým (bulunamayacak)
            var personel10bin = personeller.FirstOrDefault(p => p.Maas == 10000);
            if (personel10bin != null)
            {
                listBox1.Items.Add("FirstOrDefault() - Personel: " + personel10bin.Adsoyad + ", Birimi: " + personel10bin.Birimi);
            }
            else
            {
                listBox1.Items.Add("FirstOrDefault() - Maaþý 10000 olan personel bulunamadý!");
            }
        }

        private void button15_Click(object sender, EventArgs e)
        {
            listBox1.Items.Clear();
            //ElementAt() metodu, LINQ'de belirli bir indeksin karþýlýk geldiði öðeyi almak için kullanýlýr.
            //Bu metodun bir diðer benzer fonksiyonu ElementAtOrDefault()'tur,
            //ancak ElementAtOrDefault() metodu belirtilen indekste öðe yoksa, varsayýlan deðer (örneðin null veya sýfýr) döner.
            // Personel listesi
            var personeller = new List<Personel>
            {
                new Personel { Id = 1, Adsoyad = "Ahmet Yýlmaz", Birimi = "IT", Maas = 5000 },
                new Personel { Id = 2, Adsoyad = "Mehmet Demir", Birimi = "Pazarlama", Maas = 4500 },
                new Personel { Id = 3, Adsoyad = "Ayþe Karaca", Birimi = "Muhasebe", Maas = 3500 },
                new Personel { Id = 4, Adsoyad = "Fatma Yýlmaz", Birimi = "IT", Maas = 6000 },
                new Personel { Id = 5, Adsoyad = "Ali Kýlýç", Birimi = "Pazarlama", Maas = 4700 }
            };
            // ElementAt() kullanýmý: 2. indeksteki personeli alalým (indeks 2, 3. personeli verir)
            var personelElementAt = personeller.ElementAt(2); // 3. personel, çünkü indeks 0'dan baþlar
            listBox1.Items.Add("ElementAt() - Personel: " + personelElementAt.Adsoyad + ", Birimi: " + personelElementAt.Birimi);
            // ElementAtOrDefault() kullanýmý: 10. indeksteki personeli alalým (bulunamayacak)
            var personelElementAtOrDefault = personeller.ElementAtOrDefault(10);
            if (personelElementAtOrDefault != null)
            {
                listBox1.Items.Add("ElementAtOrDefault() - Personel: " + personelElementAtOrDefault.Adsoyad + ", Birimi: " + personelElementAtOrDefault.Birimi);
            }
            else
            {
                listBox1.Items.Add("ElementAtOrDefault() - 10. indekste personel bulunamadý!");
            }

        }

        private void button16_Click(object sender, EventArgs e)
        {
            //Last() ve LastOrDefault() metotlarý, LINQ'de koleksiyondaki son öðeyi almak için kullanýlýr.
            //Bu metotlarýn birbirinden farký þudur:
            //Last(): Eðer koleksiyon boþsa veya koþula uyan öðe bulunamazsa, InvalidOperationException hatasý fýrlatýr.
            //LastOrDefault(): Eðer koleksiyon boþsa veya koþula uyan öðe bulunamazsa, null (referans tipleri için) veya varsayýlan deðer (deðer tipleri için) döner.
            listBox1.Items.Clear();
            // Personel listesi
            var personeller = new List<Personel>
            {
                new Personel { Id = 1, Adsoyad = "Ahmet Yýlmaz", Birimi = "IT", Maas = 5000 },
                new Personel { Id = 2, Adsoyad = "Mehmet Demir", Birimi = "Pazarlama", Maas = 4500 },
                new Personel { Id = 3, Adsoyad = "Ayþe Karaca", Birimi = "Muhasebe", Maas = 3500 },
                new Personel { Id = 4, Adsoyad = "Fatma Yýlmaz", Birimi = "IT", Maas = 6000 },
                new Personel { Id = 5, Adsoyad = "Ali Kýlýç", Birimi = "Pazarlama", Maas = 4700 }
            };
            // Last() kullanýmý: Maaþý 4500'den fazla olan son personeli alalým
            var personelLast = personeller.Last(p => p.Maas >= 4500);
            listBox1.Items.Add("Last() - Personel: " + personelLast.Adsoyad + ", Birimi: " + personelLast.Birimi);
            // LastOrDefault() kullanýmý: Maaþý 4500'den fazla olan son personeli alalým
            var personelLastOrDefault = personeller.LastOrDefault(p => p.Maas >= 4500);
            if (personelLastOrDefault != null)
            {
                Console.WriteLine("LastOrDefault() - Personel: " + personelLastOrDefault.Adsoyad + ", Birimi: " + personelLastOrDefault.Birimi);
            }
            else
            {
                Console.WriteLine("LastOrDefault() - Maaþý 4500 TL veya daha fazla olan personel bulunamadý!");
            }
            // LastOrDefault() kullanýmý: Maaþý 10000 TL olan son personeli alalým (bulunamayacak)
            var personelNotFound = personeller.LastOrDefault(p => p.Maas == 10000);
            if (personelNotFound != null)
            {
                Console.WriteLine("LastOrDefault() - Personel: " + personelNotFound.Adsoyad + ", Birimi: " + personelNotFound.Birimi);
            }
            else
            {
                Console.WriteLine("LastOrDefault() - Maaþý 10000 TL olan personel bulunamadý!");
            }
        }

        private void button17_Click(object sender, EventArgs e)
        {
            //Single() ve SingleOrDefault() metotlarý, LINQ'de tek bir öðe aramak için kullanýlýr.
            //Bu metotlarýn kullaným farklarý, koleksiyonda birden fazla öðe bulunup bulunmadýðýna göre farklý davranýþlar sergilemeleridir:
            //Single(): Koþula uyan **tam olarak bir öðe olmasý gerektiðini varsayar. Eðer koleksiyonda birden fazla öðe varsa veya hiç öðe yoksa,
            //InvalidOperationException hatasý fýrlatýr.
            //SingleOrDefault(): Koþula uyan sýfýr veya bir öðe olmalýdýr. Eðer öðe yoksa null (referans tipleri için) veya varsayýlan deðer (deðer tipleri için) döner.
            //Eðer birden fazla öðe varsa, InvalidOperationException hatasý fýrlatýlýr.
            listBox1.Items.Clear();
            // Personel listesi
            var personeller = new List<Personel>
            {
                new Personel { Id = 1, Adsoyad = "Ahmet Yýlmaz", Maas = 5000 },
                new Personel { Id = 2, Adsoyad = "Mehmet Demir", Maas = 4500 },
                new Personel { Id = 3, Adsoyad = "Ayþe Karaca", Maas = 3500 },
                new Personel { Id = 4, Adsoyad = "Ahmet Yýlmaz", Maas = 6000 },  // Ayný isimli 2. personel
                new Personel { Id = 5, Adsoyad = "Ali Kýlýç", Maas = 4700 }
            };
            // Single() kullanýmý: "Ahmet Yýlmaz" adýnda sadece bir personel olmalý            
            var personelSingle = personeller.Single(p => p.Adsoyad == "Ahmet Yýlmaz");

            listBox1.Items.Add("Single() - Personel: " + personelSingle.Adsoyad + ", Maaþ: " + personelSingle.Maas);
            // SingleOrDefault() kullanýmý: "Ahmet Yýlmaz" adýnda sadece bir personel olmalý
            var personelSingleOrDefault = personeller.SingleOrDefault(p => p.Adsoyad == "Ahmet Yýlmaz");
            if (personelSingleOrDefault != null)
            {
                listBox1.Items.Add("SingleOrDefault() - Personel: " + personelSingleOrDefault.Adsoyad + ", Maaþ: " + personelSingleOrDefault.Maas);
            }
            else
            {
                listBox1.Items.Add("SingleOrDefault() - 'Ahmet Yýlmaz' adýnda personel bulunamadý!");
            }
            // SingleOrDefault() kullanýmý: "Ali Veli" adýnda bir personel var mý? (bulunamayacak)
            var personelNotFound = personeller.SingleOrDefault(p => p.Adsoyad == "Ali Veli");
            if (personelNotFound != null)
            {
                listBox1.Items.Add("SingleOrDefault() - Personel: " + personelNotFound.Adsoyad + ", Maaþ: " + personelNotFound.Maas);
            }
            else
            {
                listBox1.Items.Add("SingleOrDefault() - 'Ali Veli' adýnda personel bulunamadý!");
            }
        }
    }

    class Kisiler
    {
        //Kisiler class türü
        public int Id { get; set; }
        public String Adsoyad { get; set; }
        public int Telefon { get; set; }
        public String Eposta { get; set; }
    }
}
