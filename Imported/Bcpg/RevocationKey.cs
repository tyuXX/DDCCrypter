// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.RevocationKey
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;

namespace Org.BouncyCastle.Bcpg
{
    public class RevocationKey : SignatureSubpacket
    {
        public RevocationKey( bool isCritical, bool isLongLength, byte[] data )
          : base( SignatureSubpacketTag.RevocationKey, isCritical, isLongLength, data )
        {
        }

        public RevocationKey(
          bool isCritical,
          RevocationKeyTag signatureClass,
          PublicKeyAlgorithmTag keyAlgorithm,
          byte[] fingerprint )
          : base( SignatureSubpacketTag.RevocationKey, isCritical, false, CreateData( signatureClass, keyAlgorithm, fingerprint ) )
        {
        }

        private static byte[] CreateData(
          RevocationKeyTag signatureClass,
          PublicKeyAlgorithmTag keyAlgorithm,
          byte[] fingerprint )
        {
            byte[] destinationArray = new byte[2 + fingerprint.Length];
            destinationArray[0] = (byte)signatureClass;
            destinationArray[1] = (byte)keyAlgorithm;
            Array.Copy( fingerprint, 0, destinationArray, 2, fingerprint.Length );
            return destinationArray;
        }

        public virtual RevocationKeyTag SignatureClass => (RevocationKeyTag)this.GetData()[0];

        public virtual PublicKeyAlgorithmTag Algorithm => (PublicKeyAlgorithmTag)this.GetData()[1];

        public virtual byte[] GetFingerprint()
        {
            byte[] data = this.GetData();
            byte[] destinationArray = new byte[data.Length - 2];
            Array.Copy( data, 2, destinationArray, 0, destinationArray.Length );
            return destinationArray;
        }
    }
}
