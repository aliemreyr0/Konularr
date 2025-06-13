using Azure.Storage.Blobs;
//Nuget ile Azure.Storage.Blobs yüklenmelidir.
namespace DosyayukleAzure
{
    public partial class Form1 : Form
    {
        /*
        Bir dosyayý, Azure'daki bir depolama hesabýna aktarmayý gerçekleþtirir. 
        Gereklilikler:
        Adým 1: Azure Hesabýna Giriþ. Öðrenci hesabý ile giriþ yapýlabilir. (@kocaeli.edu.tr)
        https://portal.azure.com adresine giriþ yapýlýr.
        Adým 2: Storage Account oluþtur
        Sol menüden "Storage accounts (Depolama Hesaplarý)" > + Create
        Resource Group: (Yeni oluþtur veya var olaný seç)
        Storage account name: ogrencikayita (küçük harf, benzersiz olmalý)
        Region: (En yakýn bölge seçilir, örn. West Europe)
        Performance: Standard
        Redundancy: LRS (ucuz ve yeterli)
        Adým 3: Depolama hesaplarýndan "Kapsayýcýlar" seçilir.
        ogrencidosyalari adýnda bir Blob depo oluþturulur (Dosya yükleme için)
        Adým 4: Depolama hesabýndaki key1 altýndaki Connection string’i kopyalanýr. (Arama kýsmýna "Eriþim Anahtarlarý" yazýlýr)
        Adým 5: Projeye, Nuget ile Azure.Storage.Blobs yüklenmelidir.
        */
        public Form1()
        {
            InitializeComponent();
        }
        AzureBlobServisi blobServis = new AzureBlobServisi();
        string secilenDosya = "";

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dyukle = new OpenFileDialog();//Dosya yükleme penceresi
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
                label2.Text = "Lütfen adýnýzý ve dosya seçimini yapýn.";
                return;
            }
            label2.Text = "Yükleniyor...";
            string url = await blobServis.DosyaYukleAsync(secilenDosya);
            label2.Text = $"Yüklendi! URL:\n{url}";
            Clipboard.SetText(label1.Text);
            MessageBox.Show("Baðlantý panoya kopyalandý. Azure Portal'a giriþ yaparak dosya kontrol edilebilir.");
        }
    }

    public class AzureBlobServisi
    {
        //Cümle, yukarýdaki Adým 4 ile elde edilecek.
        private string baglanticumlesi = "DefaultEndpointsProtocol=https;AccountName=ogrencikayita;AccountKey=n7t7P9ug1pfXn/d3xMl5XK04aDueyalvoTZO6LD+i0UwHYhBqk4NGFfICk4wIszGluHr4LxB6dA8+AStcPxnCA==;EndpointSuffix=core.windows.net";
        private string kapsayici = "ogrencidosyalari";
        //Dosya yükleme iþlemini yapar. Asenkron yapar
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
