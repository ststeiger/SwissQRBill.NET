
using libQrCodeGenerator.SwissQRBill.Generator;
using libQrCodeGenerator.SwissQRBill.PixelCanvas;


namespace libQrCodeGenerator
{


    public class Tests
    {


        // Payments.cs
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


        private static Bill CreateExample1(string schuldner)
        {
            // MODULUS 97-10
            // https://usersite.datalab.eu/pantheonusermanual/tabid/316/language/en-us/topic/calculation-of-check-digits-according-to-modulus-97-10/htmlid/2261/default.aspx

            // create bill data
            Bill bill = new Bill
            {
                // creditor data
                Account = "CH4431999123000889012",

                // Creditor: Kreditor/Gläubiger/Kreditgeber 
                Creditor = new Address
                {
                    Name = "Robert Schneider AG",
                    AddressLine1 = "Rue du Lac 1268/2/22",
                    AddressLine2 = "2501 Biel",
                    CountryCode = "CH"
                },

                // payment data
                Amount = 199.95m,
                Currency = "CHF",

                // debtor data
                // Debtor: Debitor/Schuldner/Kreditnehmer
                Debtor = new Address
                {
                    Name = "Pia-Maria Rutschmann-Schnyder",
                    AddressLine1 = "Grosse Marktgasse 28",
                    AddressLine2 = "9400 Rorschach",
                    CountryCode = "CH"
                },



                // more payment data
                Reference = "210000000003139471430009017",
                UnstructuredMessage = "Abonnement für 2020" // 1. Linie "Additional Information"
                // BillInformation="//S1/10/10201409/11/190512/20/1400.000-53/30/106017086/31/180508/32/7.7/40/2:10;0:30", // 2. Linie "Additional Information"



                
            };

            return bill;
        }


        public static byte[] GenerateQrBill(string schuldner, string sprache)
        {
            byte[] png;

            Bill bill = CreateExample1(schuldner);


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
                displayResolution = img.HorizontalResolution * 2;
            }


            using (PNGCanvas canvas = new PNGCanvas(QRBill.QrCodeWidth, QRBill.QrCodeHeight, displayResolution, "Arial"))
            {
                // bill.Format.OutputSize = OutputSize.QrBillOnly;
                bill.Format.OutputSize = OutputSize.QrCodeOnly;
                QRBill.Draw(bill, canvas);
                png = canvas.ToByteArray();
            }

            return png;
        }


    }


}
