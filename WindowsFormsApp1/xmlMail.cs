using System;
using System.Data;
using System.Windows.Forms;
using System.Xml;
using System.Net.Mail;


namespace WindowsFormsApp1
{
    public partial class xmlMail : Form
    {
        XmlReader _xmlReader;
        public xmlMail()
        {
            InitializeComponent();
        }
        void xmlList()
        {
            /// <summary>
            /// Bu method xml dosyasından verileri çekip gridControllde listeleme işlemini yapan kod bölümü
            /// </summary>
            try
            {
                XmlDocument xmlDocument = new XmlDocument();
                DataSet dataSet = new DataSet();
                XmlReader xmlFile;
                xmlFile = XmlReader.Create(@"xmlMail.xml", new XmlReaderSettings());
                dataSet.ReadXml(xmlFile);
                gridControl1.DataSource = dataSet.Tables[0];
                xmlFile.Close();
            }//Hata alırsak çalışacak kod parçacıgı
            catch (Exception error)
            {
                MessageBox.Show(error.Message);
            }
        }
        void sendMail()
        {
            /// <summary>
            // Xml dosyası bin/debug klasörünün içindedir.
            // Burada da o dosyanın okuma işlemini yapıyorum.
            /// </summary>
            XmlTextReader xmlRead = new XmlTextReader("xmlMail.xml");             
            string meslek = "", mail = "";
            int i = 0;

            while (xmlRead.Read())
            {
                if (xmlRead.NodeType == XmlNodeType.Element) // Xml dosyasında ki nodeları okuma sorgusu
                {
                    if (xmlRead.Name == "meslek") // Tag adı "meslek" olanları seçme işlemi
                    {
                        xmlRead.Read();// Okuma 
                        meslek = xmlRead.Value; // Meslek değeri atama
                    }
                    if (xmlRead.Name == "mail")//Tag adı "mail" olanları seçme işlemi
                    {
                        //NodeType ile şuanda okunan elemanın tipi kontrol edilir.
                        xmlRead.Read();
                        mail = xmlRead.Value;
                        try
                        {
                            if (meslek == "asd") // Mesleği mimar olanları seçme işlemi
                            {
                                i++;
                                MailMessage mesajim = new MailMessage();//MailMessage nesnesi tanımlama
                                SmtpClient istemci = new SmtpClient(); //istemci nesnesi
                                //istemcinin kimiği ve ağ kimliği
                                istemci.Credentials = new System.Net.NetworkCredential("ttheyzc@gmail.com", "Theyzc1903."); 
                                istemci.Port = 587; // Türkiyede kullanılan mail adresi port
                                istemci.Host = "smtp.gmail.com"; // istersek gmail yapabiliriz.
                                istemci.EnableSsl = true;
                                mesajim.To.Add(mail);
                                mesajim.From = new MailAddress(mail); //kime gönderilecek
                                mesajim.Subject = "UYARI!!!!!";
                                mesajim.Body = "Gürülüyüz ulaaaa.";
                                istemci.Send(mesajim);// Mesajı gönderme
                                MessageBox.Show("Bu mail" + i + ". mail gönderim");
                            }
                        }
                        catch (Exception)
                        {
                            MessageBox.Show("Mail gönderilemedi. Sistemsel bir hata oluştu.", "Bilgi", 
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        }
                    }
                }
            }
        }
        private void XmlMail_Load(object sender, EventArgs e)
        {
            xmlList();
        }
        private void Timer1_Tick(object sender, EventArgs e)
        {//Timer ile 15 sn de bir xml dosyasını kontrol ettirme
            timer1.Start();
            sendMail();
            timer1.Stop();
        }
    }
}