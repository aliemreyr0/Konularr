using Azure.Storage.Blobs;
//Nuget ile Azure.Storage.Blobs y�klenmelidir.
namespace DosyayukleAzure
{
    public partial class Form1 : Form
    {
        /*
        Bir dosyay�, Azure'daki bir depolama hesab�na aktarmay� ger�ekle�tirir. 
        Gereklilikler:
        Ad�m 1: Azure Hesab�na Giri�. ��renci hesab� ile giri� yap�labilir. (@kocaeli.edu.tr)
        https://portal.azure.com adresine giri� yap�l�r.
        Ad�m 2: Storage Account olu�tur
        Sol men�den "Storage accounts (Depolama Hesaplar�)" > + Create
        Resource Group: (Yeni olu�tur veya var olan� se�)
        Storage account name: ogrencikayita (k���k harf, benzersiz olmal�)
        Region: (En yak�n b�lge se�ilir, �rn. West Europe)
        Performance: Standard
        Redundancy: LRS (ucuz ve yeterli)
        Ad�m 3: Depolama hesaplar�ndan "Kapsay�c�lar" se�ilir.
        ogrencidosyalari ad�nda bir Blob depo olu�turulur (Dosya y�kleme i�in)
        Ad�m 4: Depolama hesab�ndaki key1 alt�ndaki Connection string�i kopyalan�r. (Arama k�sm�na "Eri�im Anahtarlar�" yaz�l�r)
        Ad�m 5: Projeye, Nuget ile Azure.Storage.Blobs y�klenmelidir.
        */
        public Form1()
        {
            InitializeComponent();
        }
        AzureBlobServisi blobServis = new AzureBlobServisi();
        string secilenDosya = "";

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dyukle = new OpenFileDialog();//Dosya y�kleme penceresi
            if (dyukle.ShowDialog() == DialogResult.OK)
            {
                secilenDosya = dyukle.FileName;
                textBox1.Text = secilenDosya;
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            label2.Text = "";
        }

        private async void button2_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(secilenDosya) || string.IsNullOrEmpty(textBox1.Text))
            {
                label2.Text = "L�tfen ad�n�z� ve dosya se�imini yap�n.";
                return;
            }
            label2.Text = "Y�kleniyor...";
            string url = await blobServis.DosyaYukleAsync(secilenDosya);
            label2.Text = $"Y�klendi! URL:\n{url}";
            Clipboard.SetText(label1.Text);
            MessageBox.Show("Ba�lant� panoya kopyaland�. Azure Portal'a giri� yaparak dosya kontrol edilebilir.");
        }
    }

    public class AzureBlobServisi
    {
        //C�mle, yukar�daki Ad�m 4 ile elde edilecek.
        private string baglanticumlesi = "DefaultEndpointsProtocol=https;AccountName=ogrencikayita;AccountKey=n7t7P9ug1pfXn/d3xMl5XK04aDueyalvoTZO6LD+i0UwHYhBqk4NGFfICk4wIszGluHr4LxB6dA8+AStcPxnCA==;EndpointSuffix=core.windows.net";
        private string kapsayici = "ogrencidosyalari";
        //Dosya y�kleme i�lemini yapar. Asenkron yapar
        public async Task<string> DosyaYukleAsync(string dosyaYolu)
        {
            string dosyaAdi = Path.GetFileName(dosyaYolu);
            var blobClient = new BlobServiceClient(baglanticumlesi);
            var container = blobClient.GetBlobContainerClient(kapsayici);
            await container.CreateIfNotExistsAsync();

            var blob = container.GetBlobClient(dosyaAdi);
            using var dosya = File.OpenRead(dosyaYolu);
            await blob.UploadAsync(dosya, overwrite: true);
            return blob.Uri.ToString();
        }
    }
}
