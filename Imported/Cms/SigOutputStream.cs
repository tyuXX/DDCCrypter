// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Cms.SigOutputStream
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.IO;

namespace Org.BouncyCastle.Cms
{
    internal class SigOutputStream : BaseOutputStream
    {
        private readonly ISigner sig;

        internal SigOutputStream( ISigner sig ) => this.sig = sig;

        public override void WriteByte( byte b )
        {
            try
            {
                this.sig.Update( b );
            }
            catch (SignatureException ex)
            {
                throw new CmsStreamException( "signature problem: " + ex );
            }
        }

        public override void Write( byte[] b, int off, int len )
        {
            try
            {
                this.sig.BlockUpdate( b, off, len );
            }
            catch (SignatureException ex)
            {
                throw new CmsStreamException( "signature problem: " + ex );
            }
        }
    }
}
