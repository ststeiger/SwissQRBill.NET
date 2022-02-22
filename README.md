# QR Code Generator for Microsoft SSRS (using .NET 4.0)

Open-source library for generating Swiss QR-Bill QR-codes in SSRS.

This library is built for .NET Framwork 4.0 and builds on [SwissQRBill.NET](https://github.com/manuelbl/SwissQRBill.NET). 


## Examples

**Code in Report**

```vblang
Public Function GetQrBill(ByVal obj_sprache As Object, ByVal obj_account As Object, ByVal obj_amount As Object, ByVal obj_currency As Object, ByVal obj_creditor_name As Object, ByVal obj_creditor_address_1 As Object, ByVal obj_creditor_address_2 As Object, ByVal obj_creditor_country As Object, ByVal obj_debtor_name As Object, ByVal obj_debtor_address_1 As Object, ByVal obj_debtor_address_2 As Object, ByVal obj_debtor_country As Object, ByVal obj_unstructured_message As Object) As Byte()
     Return libQrCodeGenerator.Tests.GenerateQrBill(obj_sprache, obj_account, obj_amount, obj_currency, obj_creditor_name, obj_creditor_address_1, obj_creditor_address_2, obj_creditor_country, obj_debtor_name, obj_debtor_address_1, obj_debtor_address_2, obj_debtor_country, obj_unstructured_message)
 End Function
```

**Usage in Report**

```vblang
=Code.GetQrBill("de", "IBAN", Fields!RPT_KZ_Total.Value, "CHF", "Utopia Planetia AG", "Postfach 9876", "8000 Zürich", "CH", Fields!RPT_ES_ADR_Zeile1.Value, Fields!RPT_ES_ADR_Zeile2.Value, Fields!RPT_ES_ADR_Zeile3.Value, "CH", Fields!RPT_ES_Zweck.Value)
```


## Requirements

You need to reference System.Drawing and libQrCodeGenerator in Extension -> Report -> Report properties - References

- System.Drawing, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a
- libQrCodeGenerator, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null

Also, for Visual Studio 2019, you need to copy libQrCodeGenerator to <br />
`C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\IDE\CommonExtensions\Microsoft\SSRS`<br />
and <br />
`C:\Program Files (x86)\Microsoft Visual Studio\2019\Community\Common7\IDE\PublicAssemblies`

Probably dito for Visual Studio 2022, just replace 2019 with 2022. 

Replace Community with Enterprise, if you use Visual Studio Enterprise. 
