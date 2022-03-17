
using libQrCodeGenerator.SwissQRBill.Generator;
using libQrCodeGenerator.SwissQRBill.PixelCanvas;


namespace libQrCodeGenerator
{


    public class Tests
    {


        // Core/Payments.cs
        // private static readonly int[] Mod10 = { 0, 9, 4, 6, 8, 2, 7, 1, 3, 5 };

        // int[] checkDigits = new int[] { 0, 9, 8, 7, 6, 5, 4, 3, 2, 1 };
        //private static int[][] carryOvers = new int[][]
        //{
        //    new int[] { 0, 9, 4, 6, 8, 2, 7, 1, 3, 5 },
        //    new int[] { 9, 4, 6, 8, 2, 7, 1, 3, 5, 0 },
        //    new int[] { 4, 6, 8, 2, 7, 1, 3, 5, 0, 9 },
        //    new int[] { 6, 8, 2, 7, 1, 3, 5, 0, 9, 4 },
        //    new int[] { 8, 2, 7, 1, 3, 5, 0, 9, 4, 6 },
        //    new int[] { 2, 7, 1, 3, 5, 0, 9, 4, 6, 8 },
        //    new int[] { 7, 1, 3, 5, 0, 9, 4, 6, 8, 2 },
        //    new int[] { 1, 3, 5, 0, 9, 4, 6, 8, 2, 7 },
        //    new int[] { 3, 5, 0, 9, 4, 6, 8, 2, 7, 1 },
        //    new int[] { 5, 0, 9, 4, 6, 8, 2, 7, 1, 3 }
        //};


        public static string FormatQrReferenceNumber(string refNo)
        {
            string ret = null;
            int len = refNo.Length;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            int t = 0;
            while (t < len)
            {
                int n = t + (len - t - 1) % 5 + 1;
                if (t != 0)
                {
                    sb.Append(" ");
                } // End if (t != 0) 

                sb.Append(refNo, t, n - t);
                t = n;
            } // Whend 

            ret = sb.ToString();
            sb.Clear();
            sb = null;

            return ret;
        } // End Function FormatQrReferenceNumber 


        public static System.Collections.Generic.List<string> FormatReferenceNumber(string input)
        {
            System.Collections.Generic.List<string> ls = new System.Collections.Generic.List<string>();

            if (string.IsNullOrEmpty(input))
                return ls;

            input = input.Replace(" ", "").Replace("\t", "")
    // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/
    // https://csharpindepth.com/articles/Strings
    .Replace("\0", "") // Unicode character 0
    .Replace("\x00A0", "") // ASCII 0xA0 (160: non-breaking space) 
    .Replace("\uFEFF", "") // Unicode Character 'ZERO WIDTH NO-BREAK SPACE' (U+FEFF)
    .Replace("\a", "") // alert
    .Replace("\b", "") // backspace 
    .Replace("\v", "").Replace("\f", "")
    .Replace("\r", "").Replace("\n", "");


            if (input.Length < 27)
                input = input.PadLeft(27, '0');

            if (input.Length > 27)
                throw new System.ArgumentException("Invalid length");

            while (input.Length > 5)
            {
                string extract = input.Substring(input.Length - 5, 5);
                ls.Insert(0, extract);
                input = input.Substring(0, input.Length - 5);
            } // Whend 

            if (!string.IsNullOrEmpty(input))
                ls.Insert(0, input);

            return ls;
        } // End Function FormatReferenceNumber 


        public static bool IsNumeric(string input)
        {
            foreach (char ch in input)
            {
                if (ch < '0' || ch > '9')
                    return false;
            } // Next ch 

            return true;
        } // End Function IsNumeric 


        public static bool ValidateReferenceNumber(string input)
        {
            int[] Mod10 = { 0, 9, 4, 6, 8, 2, 7, 1, 3, 5 };
            // System.Collections.Generic.List<string> readable = FormatReferenceNumber(input); System.Console.WriteLine(readable);
            input = input.Replace(" ", "").Replace("\t", "")
                // https://docs.microsoft.com/en-us/dotnet/csharp/programming-guide/strings/
                // https://csharpindepth.com/articles/Strings
                .Replace("\0", "") // Unicode character 0
                .Replace("\x00A0", "") // ASCII 0xA0 (160: non-breaking space) 
                .Replace("\uFEFF", "") // Unicode Character 'ZERO WIDTH NO-BREAK SPACE' (U+FEFF)
                .Replace("\a", "") // alert
                .Replace("\b", "") // backspace 
                .Replace("\v", "").Replace("\f", "")
                .Replace("\r", "").Replace("\n", "");

            if (!IsNumeric(input))
                throw new System.ArgumentException("Invalid character in reference (digits allowed only)");

            if (input.Length < 27)
                input = input.PadLeft(27, '0');

            if (input.Length > 27)
                throw new System.ArgumentException("Reference number is too long");

            int checkDigit = input[input.Length - 1] - '0';
            input = input.Substring(0, input.Length - 1);

            int carryOver = 0;

            foreach (char currentCharacter in input)
            {
                int num = currentCharacter - '0';
                carryOver = Mod10[(num + carryOver) % 10];
            } // Next currentCharacter 

            // int[] checkDigits = new int[] { 0, 9, 8, 7, 6, 5, 4, 3, 2, 1 };
            // int check = checkDigits[carryOver];

            int check = (10 - carryOver) % 10;
            return (check == checkDigit);
        } // End Function ValidateReferenceNumber 


        // https://www.codecrete.net/qrbill/bill
        public static void Validate()
        {
            bool isValid = ValidateReferenceNumber("21 00000 00003 13947 14300 09017");
            isValid = ValidateReferenceNumber("21 00000 00003 13947 14300 09025");
            isValid = ValidateReferenceNumber("21 00000 00003 13947 14300 09030");
            isValid = ValidateReferenceNumber("55000 00003 13947 14300 09038");

            System.Console.WriteLine(isValid);
        } // End Sub Validate 


        private static Bill IssueBill(string account, decimal amount, string currency
            , string creditor_name, string creditor_address_1, string creditor_address_2, string creditor_country
            , string debtor_name, string debtor_address_1, string debtor_address_2, string debtor_country
            , string unstructured_message)
        {
            // MODULUS 97-10
            // https://usersite.datalab.eu/pantheonusermanual/tabid/316/language/en-us/topic/calculation-of-check-digits-according-to-modulus-97-10/htmlid/2261/default.aspx

            if(!string.IsNullOrEmpty(unstructured_message))
                unstructured_message = unstructured_message // "Rechnungs-Nr. 2022_00000_123\r\nKOA 8300 9901/PC DL-IT\r\nCopycenter" // Optional
                .Replace("\r\n", "\n")
                .Replace("\n\r", "\n").Replace("\r", "\n") // Patrick-IQ-compensation 
                .TrimEnd('\n')
                .Replace("\n", "; ");


            // create bill data
            Bill bill = new Bill
            {
                // creditor data
                Account = account, // Account = "CH4431999123000889012",

                // payment data
                Amount = amount, // Amount = 199.95m,
                Currency = currency, // Currency = "CHF",

                // Creditor: Kreditor/Gläubiger/Kreditgeber 
                Creditor = new Address
                {
                    Name = creditor_name, // "Swiss Life AG", // Not-optional 
                    AddressLine1 = creditor_address_1,  // "Postfach 2831", // Optional, null allowed 
                    AddressLine2 = creditor_address_2, // "8022 Zürich", // Must-have 
                    CountryCode = creditor_country // "CH"
                },




                // debtor data
                // Debtor: Debitor/Schuldner/Kreditnehmer
                Debtor = new Address
                {
                    Name = debtor_name, // "Pia-Maria Rutschmann-Schnyder", // Not optional
                    AddressLine1 = debtor_address_1, // "Grosse Marktgasse 28", // optional, null allowed 
                    AddressLine2 = debtor_address_2, // "9400 Rorschach", // Not optional
                    CountryCode = debtor_country // "CH"
                },


                // more payment data
                // Reference = "210000000003139471430009017",
                // UnstructuredMessage = "Abonnement für 2020" // 1. Linie "Additional Information"
                // BillInformation="//S1/10/10201409/11/190512/20/1400.000-53/30/106017086/31/180508/32/7.7/40/2:10;0:30", // 2. Linie "Additional Information"

                UnstructuredMessage = unstructured_message
            };

            return bill;
        } // End Function IssueBill 


        // GenerateQrBill("de, "CH4431999123000889012", 199.95m, "CHF"
        // , "Swiss Life AG", "Postfach 2831", "8022 Zürich", "CH" 
        // , "Pia-Maria Rutschmann-Schnyder",  "Grosse Marktgasse 28", "9400 Rorschach", "CH" 
        // ,"Rechnungs-Nr. 2022_00000_123\r\nKOA 8300 9901/PC DL-IT\r\nCopycenter");
        public static byte[] GenerateQrBill(object obj_sprache
            , object obj_account, object obj_amount, object obj_currency
            , object obj_creditor_name, object obj_creditor_address_1, object obj_creditor_address_2, object obj_creditor_country
            , object obj_debtor_name, object obj_debtor_address_1, object obj_debtor_address_2, object obj_debtor_country
            , object obj_unstructured_message
        )
        {
            byte[] png;

            if (obj_amount == System.DBNull.Value)
                obj_amount = null;


            string sprache = System.Convert.ToString(obj_sprache, System.Globalization.CultureInfo.InvariantCulture);
            string account = System.Convert.ToString(obj_account, System.Globalization.CultureInfo.InvariantCulture);
            decimal amount = System.Convert.ToDecimal(obj_amount, System.Globalization.CultureInfo.InvariantCulture);
            string currency = System.Convert.ToString(obj_currency, System.Globalization.CultureInfo.InvariantCulture);

            string creditor_name = System.Convert.ToString(obj_creditor_name, System.Globalization.CultureInfo.InvariantCulture);
            string creditor_address_1 = System.Convert.ToString(obj_creditor_address_1, System.Globalization.CultureInfo.InvariantCulture);
            string creditor_address_2 = System.Convert.ToString(obj_creditor_address_2, System.Globalization.CultureInfo.InvariantCulture);
            string creditor_country = System.Convert.ToString(obj_creditor_country, System.Globalization.CultureInfo.InvariantCulture);

            string debtor_name = System.Convert.ToString(obj_debtor_name, System.Globalization.CultureInfo.InvariantCulture);
            string debtor_address_1 = System.Convert.ToString(obj_debtor_address_1, System.Globalization.CultureInfo.InvariantCulture);
            string debtor_address_2 = System.Convert.ToString(obj_debtor_address_2, System.Globalization.CultureInfo.InvariantCulture);
            string debtor_country = System.Convert.ToString(obj_debtor_country, System.Globalization.CultureInfo.InvariantCulture);

            string unstructured_message = System.Convert.ToString(obj_unstructured_message, System.Globalization.CultureInfo.InvariantCulture);


            Bill bill = IssueBill(account, amount, currency
                , creditor_name, creditor_address_1, creditor_address_2, creditor_country
                , debtor_name, debtor_address_1, debtor_address_2, debtor_country
                , unstructured_message
            );


            if ("fr".Equals(sprache, System.StringComparison.InvariantCultureIgnoreCase))
                bill.Format.Language = Language.FR;
            else if ("it".Equals(sprache, System.StringComparison.InvariantCultureIgnoreCase))
                bill.Format.Language = Language.IT;
            else if ("en".Equals(sprache, System.StringComparison.InvariantCultureIgnoreCase))
                bill.Format.Language = Language.EN;
            else
                bill.Format.Language = Language.DE;


            // using (PNGCanvas canvas = new PNGCanvas(QRBill.QrBillWidth, QRBill.QrBillHeight, 300, "\"Liberation Sans\",Arial, Helvetica"))
            // const int dpi = 192;
            // using (PNGCanvas canvas = new PNGCanvas(QRBill.QrCodeWidth/25.4* dpi, QRBill.QrCodeHeight/25.4* dpi, dpi, "Arial"))
            // using (PNGCanvas canvas = new PNGCanvas(QRBill.QrBillWidth / 25.4 * dpi, QRBill.QrBillHeight / 25.4 * dpi, dpi, "Arial"))
            // using (PNGCanvas canvas = new PNGCanvas(QRBill.QrBillWidth, QRBill.QrBillHeight, 192, "Arial"))


            float displayResolution = 192; 

            using (System.Drawing.Image img = new System.Drawing.Bitmap(1, 1))
            {
                // to set the actual width, multiply img.Resolution by width_in_mm/QRBill.QrCodeWidth 
                displayResolution = img.HorizontalResolution; 
                displayResolution *= 2; 
            } // End Using img 

#if true 
            // int width = (int)(QRBill.QrCodeWidth / 25.4 * displayResolution);
            // int height = (int)(QRBill.QrCodeHeight / 25.4 * displayResolution);

            int width = (int)(QRBill.QrCodeWidth);
            int height = (int)(QRBill.QrCodeHeight);

            bill.Format.OutputSize = OutputSize.QrCodeOnly; 
#else

            // int width = (int)(QRBill.QrBillWidth / 25.4 * displayResolution); 
            // int height = (int)(QRBill.QrBillHeight / 25.4 * displayResolution);

            int width = (int)QRBill.QrBillWidth; 
            int height = (int)QRBill.QrBillHeight; 
            bill.Format.OutputSize = OutputSize.QrBillOnly; 
#endif 


            try
            {

                using (PNGCanvas canvas = new PNGCanvas(width, height, displayResolution, "Arial"))
                {
                    QRBill.Draw(bill, canvas); 
                    png = canvas.ToByteArray(); 
                } // End Using canvas 

            }
            catch (System.Exception ex)
            {

                using (System.Drawing.Image draw = new System.Drawing.Bitmap(width, height))
                {

                    using (System.Drawing.Graphics g = System.Drawing.Graphics.FromImage(draw))
                    {
                        g.Clear(System.Drawing.Color.Crimson);

                        using (System.Drawing.Font font = new System.Drawing.Font("Arial", 11, System.Drawing.FontStyle.Bold))
                        {
                            System.Drawing.StringFormat sf = new System.Drawing.StringFormat();
                            sf.LineAlignment = System.Drawing.StringAlignment.Center;
                            sf.Alignment = System.Drawing.StringAlignment.Center;

                            // g.DrawString(ex.Message, font, System.Drawing.Brushes.White, new System.Drawing.Rectangle(0, 0, width, height), sf);
                            g.DrawString("BAD DATA", font, System.Drawing.Brushes.White, new System.Drawing.Rectangle(0, 0, width, height), sf);
                        } // End Using font 

                    } // End Using g 


                    using (System.IO.MemoryStream stream = new System.IO.MemoryStream())
                    {
                        draw.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                        png = stream.ToArray();
                    } // End Using stream 

                } // End Using draw 

            } // End Catch 

            return png;
        } // End Function GenerateQrBill 


    } // End Class Tests 


} // End Namespace libQrCodeGenerator 
