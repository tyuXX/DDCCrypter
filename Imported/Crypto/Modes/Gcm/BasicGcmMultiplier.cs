// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Modes.Gcm.BasicGcmMultiplier
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Crypto.Modes.Gcm
{
    public class BasicGcmMultiplier : IGcmMultiplier
    {
        private uint[] H;

        public void Init( byte[] H ) => this.H = GcmUtilities.AsUints( H );

        public void MultiplyH( byte[] x )
        {
            uint[] x1 = GcmUtilities.AsUints( x );
            GcmUtilities.Multiply( x1, this.H );
            GcmUtilities.AsBytes( x1, x );
        }
    }
}
