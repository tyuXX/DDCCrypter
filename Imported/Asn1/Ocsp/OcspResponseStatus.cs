// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Ocsp.OcspResponseStatus
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Asn1.Ocsp
{
    public class OcspResponseStatus : DerEnumerated
    {
        public const int Successful = 0;
        public const int MalformedRequest = 1;
        public const int InternalError = 2;
        public const int TryLater = 3;
        public const int SignatureRequired = 5;
        public const int Unauthorized = 6;

        public OcspResponseStatus( int value )
          : base( value )
        {
        }

        public OcspResponseStatus( DerEnumerated value )
          : base( value.Value.IntValue )
        {
        }
    }
}
