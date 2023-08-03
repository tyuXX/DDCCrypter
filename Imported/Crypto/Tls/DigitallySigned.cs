// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.DigitallySigned
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
    public class DigitallySigned
    {
        protected readonly SignatureAndHashAlgorithm mAlgorithm;
        protected readonly byte[] mSignature;

        public DigitallySigned( SignatureAndHashAlgorithm algorithm, byte[] signature )
        {
            if (signature == null)
                throw new ArgumentNullException( nameof( signature ) );
            this.mAlgorithm = algorithm;
            this.mSignature = signature;
        }

        public virtual SignatureAndHashAlgorithm Algorithm => this.mAlgorithm;

        public virtual byte[] Signature => this.mSignature;

        public virtual void Encode( Stream output )
        {
            if (this.mAlgorithm != null)
                this.mAlgorithm.Encode( output );
            TlsUtilities.WriteOpaque16( this.mSignature, output );
        }

        public static DigitallySigned Parse( TlsContext context, Stream input )
        {
            SignatureAndHashAlgorithm algorithm = null;
            if (TlsUtilities.IsTlsV12( context ))
                algorithm = SignatureAndHashAlgorithm.Parse( input );
            byte[] signature = TlsUtilities.ReadOpaque16( input );
            return new DigitallySigned( algorithm, signature );
        }
    }
}
