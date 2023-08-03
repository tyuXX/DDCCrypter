// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Asn1Object
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;
using System.IO;

namespace Org.BouncyCastle.Asn1
{
    public abstract class Asn1Object : Asn1Encodable
    {
        public static Asn1Object FromByteArray( byte[] data )
        {
            try
            {
                MemoryStream inputStream = new MemoryStream( data, false );
                Asn1Object asn1Object = new Asn1InputStream( inputStream, data.Length ).ReadObject();
                if (inputStream.Position != inputStream.Length)
                    throw new IOException( "extra data found after object" );
                return asn1Object;
            }
            catch (InvalidCastException ex)
            {
                throw new IOException( "cannot recognise object in byte array" );
            }
        }

        public static Asn1Object FromStream( Stream inStr )
        {
            try
            {
                return new Asn1InputStream( inStr ).ReadObject();
            }
            catch (InvalidCastException ex)
            {
                throw new IOException( "cannot recognise object in stream" );
            }
        }

        public override sealed Asn1Object ToAsn1Object() => this;

        internal abstract void Encode( DerOutputStream derOut );

        protected abstract bool Asn1Equals( Asn1Object asn1Object );

        protected abstract int Asn1GetHashCode();

        internal bool CallAsn1Equals( Asn1Object obj ) => this.Asn1Equals( obj );

        internal int CallAsn1GetHashCode() => this.Asn1GetHashCode();
    }
}
