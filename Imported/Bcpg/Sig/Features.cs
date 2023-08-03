// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.Sig.Features
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Bcpg.Sig
{
    public class Features : SignatureSubpacket
    {
        public static readonly byte FEATURE_MODIFICATION_DETECTION = 1;

        private static byte[] FeatureToByteArray( byte feature ) => new byte[1]
        {
      feature
        };

        public Features( bool critical, bool isLongLength, byte[] data )
          : base( SignatureSubpacketTag.Features, critical, isLongLength, data )
        {
        }

        public Features( bool critical, byte feature )
          : base( SignatureSubpacketTag.Features, critical, false, FeatureToByteArray( feature ) )
        {
        }

        public bool SupportsModificationDetection => this.SupportsFeature( FEATURE_MODIFICATION_DETECTION );

        public bool SupportsFeature( byte feature ) => Array.IndexOf( data, (object)feature ) >= 0;

        private void SetSupportsFeature( byte feature, bool support )
        {
            int num = feature != 0 ? Array.IndexOf( data, (object)feature ) : throw new ArgumentException( "cannot be 0", nameof( feature ) );
            if ((num >= 0) == support)
                return;
            if (support)
            {
                this.data = Arrays.Append( this.data, feature );
            }
            else
            {
                byte[] destinationArray = new byte[this.data.Length - 1];
                Array.Copy( data, 0, destinationArray, 0, num );
                Array.Copy( data, num + 1, destinationArray, num, destinationArray.Length - num );
                this.data = destinationArray;
            }
        }
    }
}
