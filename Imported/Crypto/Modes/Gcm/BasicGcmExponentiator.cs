// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Modes.Gcm.BasicGcmExponentiator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Modes.Gcm
{
    public class BasicGcmExponentiator : IGcmExponentiator
    {
        private uint[] x;

        public void Init( byte[] x ) => this.x = GcmUtilities.AsUints( x );

        public void ExponentiateX( long pow, byte[] output )
        {
            uint[] x = GcmUtilities.OneAsUints();
            if (pow > 0L)
            {
                uint[] numArray = Arrays.Clone( this.x );
                do
                {
                    if ((pow & 1L) != 0L)
                        GcmUtilities.Multiply( x, numArray );
                    GcmUtilities.Multiply( numArray, numArray );
                    pow >>= 1;
                }
                while (pow > 0L);
            }
            GcmUtilities.AsBytes( x, output );
        }
    }
}
