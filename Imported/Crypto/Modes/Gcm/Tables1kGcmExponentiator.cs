// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Modes.Gcm.Tables1kGcmExponentiator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System.Collections;

namespace Org.BouncyCastle.Crypto.Modes.Gcm
{
    public class Tables1kGcmExponentiator : IGcmExponentiator
    {
        private IList lookupPowX2;

        public void Init( byte[] x )
        {
            uint[] a = GcmUtilities.AsUints( x );
            if (this.lookupPowX2 != null && Arrays.AreEqual( a, (uint[])this.lookupPowX2[0] ))
                return;
            this.lookupPowX2 = Platform.CreateArrayList( 8 );
            this.lookupPowX2.Add( a );
        }

        public void ExponentiateX( long pow, byte[] output )
        {
            uint[] x = GcmUtilities.OneAsUints();
            int num = 0;
            for (; pow > 0L; pow >>= 1)
            {
                if ((pow & 1L) != 0L)
                {
                    this.EnsureAvailable( num );
                    GcmUtilities.Multiply( x, (uint[])this.lookupPowX2[num] );
                }
                ++num;
            }
            GcmUtilities.AsBytes( x, output );
        }

        private void EnsureAvailable( int bit )
        {
            int count = this.lookupPowX2.Count;
            if (count > bit)
                return;
            uint[] numArray = (uint[])this.lookupPowX2[count - 1];
            do
            {
                numArray = Arrays.Clone( numArray );
                GcmUtilities.Multiply( numArray, numArray );
                this.lookupPowX2.Add( numArray );
            }
            while (++count <= bit);
        }
    }
}
