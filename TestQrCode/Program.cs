
namespace TestQrCode
{


    static class Program
    {


        public static void ChangeNamespaces()
        {
            string basePath = @"D:\username\Documents\Visual Studio 2017\Projects\SwissQRBill.NET\libQrCodeGenerator";

            System.Collections.Generic.List<string> files = BasicFolderHelper.EnumFolders(basePath);

            foreach (string file in files)
            {
                System.Console.WriteLine(file);
                string content = System.IO.File.ReadAllText(file, System.Text.Encoding.UTF8);
                // content = content.Replace("namespace Net.Codecrete.", "namespace libQrCodeGenerator.");
                // content = content.Replace("namespace Codecrete.SwissQRBill.Generator", "namespace libQrCodeGenerator.SwissQRBill.Generator");
                // content = content.Replace("using Codecrete.SwissQRBill.Generator", "using libQrCodeGenerator.SwissQRBill.Generator");
                // content = content.Replace("using static Codecrete.SwissQRBill.Generator.", "using static libQrCodeGenerator.SwissQRBill.Generator.");
                content = content.Replace("namespace Codecrete.SwissQRBill.PixelCanvas", "namespace libQrCodeGenerator.SwissQRBill.PixelCanvas");

                System.IO.File.WriteAllText(file, content, System.Text.Encoding.UTF8);
            } // Next file 

        } // End Sub ChangeNamespaces 



        public static byte[] GetQrBill(object obj_sprache
          , object obj_account, object obj_amount, object obj_currency
          , object obj_creditor_name, object obj_creditor_address_1, object obj_creditor_address_2, object obj_creditor_country
          , object obj_debtor_name, object obj_debtor_address_1, object obj_debtor_address_2, object obj_debtor_country
          , object obj_unstructured_message
      )
        {
            return libQrCodeGenerator.Tests.GenerateQrBill(obj_sprache
                 , obj_account, obj_amount, obj_currency
                 , obj_creditor_name, obj_creditor_address_1, obj_creditor_address_2, obj_creditor_country
                 , obj_debtor_name, obj_debtor_address_1, obj_debtor_address_2, obj_debtor_country
                 , obj_unstructured_message
            );
        } // End Function GetQrBill 


        // No more needed, possible errors now in QR-Code output. 
        public static string GetQrBill2(object obj_sprache
            , object obj_account, object obj_amount, object obj_currency
            , object obj_creditor_name, object obj_creditor_address_1, object obj_creditor_address_2, object obj_creditor_country
            , object obj_debtor_name, object obj_debtor_address_1, object obj_debtor_address_2, object obj_debtor_country
            , object obj_unstructured_message
        )
        {
            byte[] png = null;

            try
            {
                png = libQrCodeGenerator.Tests.GenerateQrBill(obj_sprache
                     , obj_account, obj_amount, obj_currency
                     , obj_creditor_name, obj_creditor_address_1, obj_creditor_address_2, obj_creditor_country
                     , obj_debtor_name, obj_debtor_address_1, obj_debtor_address_2, obj_debtor_country
                     , obj_unstructured_message
                );
            }
            catch (System.Exception ex)
            {
                return ex.Message + System.Environment.NewLine + ex.StackTrace;
            }

            return System.Convert.ToBase64String(png);
        } // End Function GetQrBill2 

        // In Report, Register System.Drawing (NET40), libQrCodeGenerator.dll


        // Public Function GetQrBill(ByVal obj_sprache As Object, ByVal obj_account As Object, ByVal obj_amount As Object, ByVal obj_currency As Object, ByVal obj_creditor_name As Object, ByVal obj_creditor_address_1 As Object, ByVal obj_creditor_address_2 As Object, ByVal obj_creditor_country As Object, ByVal obj_debtor_name As Object, ByVal obj_debtor_address_1 As Object, ByVal obj_debtor_address_2 As Object, ByVal obj_debtor_country As Object, ByVal obj_unstructured_message As Object) As Byte()
        //     Return libQrCodeGenerator.Tests.GenerateQrBill(obj_sprache, obj_account, obj_amount, obj_currency, obj_creditor_name, obj_creditor_address_1, obj_creditor_address_2, obj_creditor_country, obj_debtor_name, obj_debtor_address_1, obj_debtor_address_2, obj_debtor_country, obj_unstructured_message)
        // End Function

        // Public Function GetQrBill2(ByVal obj_sprache As Object, ByVal obj_account As Object, ByVal obj_amount As Object, ByVal obj_currency As Object, ByVal obj_creditor_name As Object, ByVal obj_creditor_address_1 As Object, ByVal obj_creditor_address_2 As Object, ByVal obj_creditor_country As Object, ByVal obj_debtor_name As Object, ByVal obj_debtor_address_1 As Object, ByVal obj_debtor_address_2 As Object, ByVal obj_debtor_country As Object, ByVal obj_unstructured_message As Object) As String
        //     Dim png As Byte() = Nothing
        //     
        //     Try
        //         png = libQrCodeGenerator.Tests.GenerateQrBill(obj_sprache, obj_account, obj_amount, obj_currency, obj_creditor_name, obj_creditor_address_1, obj_creditor_address_2, obj_creditor_country, obj_debtor_name, obj_debtor_address_1, obj_debtor_address_2, obj_debtor_country, obj_unstructured_message)
        //     Catch ex As System.Exception
        //         Return ex.Message & System.Environment.NewLine & ex.StackTrace
        //     End Try
        // 
        //     Return System.Convert.ToBase64String(png)
        // End Function




        // Copy CorQrCode.dll, libQrCodeGenerator.dll to both: 
        // C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\IDE\CommonExtensions\Microsoft\SSRS
        // C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\IDE\PublicAssemblies


        /// <summary>
        /// Der Haupteinstiegspunkt für die Anwendung.
        /// </summary>
        [System.STAThread]
        static void Main()
        {

#if false
            System.Windows.Forms.Application.EnableVisualStyles();
            System.Windows.Forms.Application.SetCompatibleTextRenderingDefault(false);
            System.Windows.Forms.Application.Run(new Form1());
#endif

            string RPT_ES_Zweck = "Rechnungs-Nr. 2022-02-21-133\r\nKunde-Nr. 1234";


            string rechnungsempfänger = "Pia-Maria Rutschmann-Schnyder";
            string adresse1 = "Grosse Marktgasse 28";
            string plz_ort = "9400 Rorschach";
            string country = "CH";

            decimal RPT_Kosten = 20m;


            libQrCodeGenerator.Tests.Validate();
            // byte[] png = GetQrBill("de", "CH4431999123000889012", 199.95m, "CHF" // QR-IBAN 
            // Dim png As Byte() = GetQrBill("de", "CH93 0900 0000 8000 0209 2", RPT_Kosten, "CHF", "Swiss Life AG", "Postfach 2831", "8022 Zürich", "CH", rechnungsempfänger, adresse1, plz_ort, country, RPT_ES_Zweck)
            byte[] png = GetQrBill("de", "IBAN_WITOUT_REFERENCE", RPT_Kosten, "CHF"
                 , "Utopia Planetia AG", "Postfach 1234", "8000 Zürich", "CH"
                 , rechnungsempfänger, adresse1, plz_ort, country
                 , RPT_ES_Zweck
            );

            System.IO.File.WriteAllBytes(@"D:\QrBill.png", png);


            System.Console.WriteLine(System.Environment.NewLine);
            System.Console.WriteLine(" --- Press any key to continue --- ");
            System.Console.ReadKey();
        } // End Sub Main 


    } // End Class Program 


} // End Namespace TestQrCode 
