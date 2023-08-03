// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X9.X9IntegerConverter
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;
using Org.BouncyCastle.Math.EC;
using System;

namespace Org.BouncyCastle.Asn1.X9
{
    public abstract class X9IntegerConverter
    {
        public static int GetByteLength( ECFieldElement fe ) => (fe.FieldSize + 7) / 8;

        public static int GetByteLength( ECCurve c ) => (c.FieldSize + 7) / 8;

        public static byte[] IntegerToBytes( BigInteger s, int qLength )
        {
            byte[] byteArrayUnsigned = s.ToByteArrayUnsigned();
            if (qLength < byteArrayUnsigned.Length)
            {
                byte[] destinationArray = new byte[qLength];
                Array.Copy( byteArrayUnsigned, byteArrayUnsigned.Length - destinationArray.Length, destinationArray, 0, destinationArray.Length );
                return destinationArray;
            }
            if (qLength <= byteArrayUnsigned.Length)
                return byteArrayUnsigned;
            byte[] destinationArray1 = new byte[qLength];
            Array.Copy( byteArrayUnsigned, 0, destinationArray1, destinationArray1.Length - byteArrayUnsigned.Length, byteArrayUnsigned.Length );
            return destinationArray1;
        }
    }
}
