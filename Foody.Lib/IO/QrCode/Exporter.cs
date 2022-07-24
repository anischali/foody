// Decompiled with JetBrains decompiler
// Type: Foody.IO.QrCode.Exporter
// Assembly: Foody.Lib, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: B84D7D6A-CC40-4F70-B447-25F27D08A110
// Assembly location: C:\Users\Virtio\Desktop\Foody\Foody\Foody.Lib.dll

using Foody.Generic;
using System.Drawing;
using ZXing;
using ZXing.Common;
using ZXing.QrCode;

namespace Foody.IO.QrCode
{
  public static class Exporter
  {
    public static Bitmap GetQrCodeVersion<Type>(Type data, int width, int height)
    {
      QRCodeWriter qrCodeWriter = new QRCodeWriter();
      QrCodeEncodingOptions codeEncodingOptions1 = new QrCodeEncodingOptions();
      codeEncodingOptions1.DisableECI = true;
      codeEncodingOptions1.CharacterSet = "UTF-8";
      codeEncodingOptions1.PureBarcode = true;
      QrCodeEncodingOptions codeEncodingOptions2 = codeEncodingOptions1;
      string json = Tools.ConvertToJSON<Type>(data);
      BarcodeWriter barcodeWriter = new BarcodeWriter();
      barcodeWriter.Format = BarcodeFormat.QR_CODE;
      barcodeWriter.Options = (EncodingOptions) codeEncodingOptions2;
      return barcodeWriter.Write(qrCodeWriter.encode(json, BarcodeFormat.QR_CODE, width, height));
    }
  }
}
