// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Pkcs.Pkcs12Utilities
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using System;
using System.IO;

namespace Org.BouncyCastle.Pkcs
{
    public class Pkcs12Utilities
    {
        public static byte[] ConvertToDefiniteLength( byte[] berPkcs12File ) => new Pfx( Asn1Sequence.GetInstance( Asn1Object.FromByteArray( berPkcs12File ) ) ).GetEncoded( "DER" );

        public static byte[] ConvertToDefiniteLength( byte[] berPkcs12File, char[] passwd )
        {
            Pfx pfx = new Pfx( Asn1Sequence.GetInstance( Asn1Object.FromByteArray( berPkcs12File ) ) );
            ContentInfo authSafe = pfx.AuthSafe;
            Asn1Object asn1Object = Asn1Object.FromByteArray( Asn1OctetString.GetInstance( authSafe.Content ).GetOctets() );
            ContentInfo contentInfo = new ContentInfo( authSafe.ContentType, new DerOctetString( asn1Object.GetEncoded( "DER" ) ) );
            MacData macData1 = pfx.MacData;
            MacData macData2;
            try
            {
                int intValue = macData1.IterationCount.IntValue;
                byte[] octets = Asn1OctetString.GetInstance( contentInfo.Content ).GetOctets();
                byte[] pbeMac = Pkcs12Store.CalculatePbeMac( macData1.Mac.AlgorithmID.Algorithm, macData1.GetSalt(), intValue, passwd, false, octets );
                macData2 = new MacData( new DigestInfo( new AlgorithmIdentifier( macData1.Mac.AlgorithmID.Algorithm, DerNull.Instance ), pbeMac ), macData1.GetSalt(), intValue );
            }
            catch (Exception ex)
            {
                throw new IOException( "error constructing MAC: " + ex.ToString() );
            }
            return new Pfx( contentInfo, macData2 ).GetEncoded( "DER" );
        }
    }
}
