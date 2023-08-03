// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.BerOctetString
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Asn1
{
    public class BerOctetString : DerOctetString, IEnumerable
    {
        private const int MaxLength = 1000;
        private readonly IEnumerable octs;

        public static BerOctetString FromSequence( Asn1Sequence seq )
        {
            IList arrayList = Platform.CreateArrayList();
            foreach (Asn1Encodable asn1Encodable in seq)
                arrayList.Add( asn1Encodable );
            return new BerOctetString( arrayList );
        }

        private static byte[] ToBytes( IEnumerable octs )
        {
            MemoryStream memoryStream = new();
            foreach (Asn1OctetString oct in octs)
            {
                byte[] octets = oct.GetOctets();
                memoryStream.Write( octets, 0, octets.Length );
            }
            return memoryStream.ToArray();
        }

        public BerOctetString( byte[] str )
          : base( str )
        {
        }

        public BerOctetString( IEnumerable octets )
          : base( ToBytes( octets ) )
        {
            this.octs = octets;
        }

        public BerOctetString( Asn1Object obj )
          : base( obj )
        {
        }

        public BerOctetString( Asn1Encodable obj )
          : base( obj.ToAsn1Object() )
        {
        }

        public override byte[] GetOctets() => this.str;

        public IEnumerator GetEnumerator() => this.octs == null ? this.GenerateOcts().GetEnumerator() : this.octs.GetEnumerator();

        [Obsolete( "Use GetEnumerator() instead" )]
        public IEnumerator GetObjects() => this.GetEnumerator();

        private IList GenerateOcts()
        {
            IList arrayList = Platform.CreateArrayList();
            for (int sourceIndex = 0; sourceIndex < this.str.Length; sourceIndex += 1000)
            {
                byte[] numArray = new byte[System.Math.Min( this.str.Length, sourceIndex + 1000 ) - sourceIndex];
                Array.Copy( str, sourceIndex, numArray, 0, numArray.Length );
                arrayList.Add( new DerOctetString( numArray ) );
            }
            return arrayList;
        }

        internal override void Encode( DerOutputStream derOut )
        {
            switch (derOut)
            {
                case Asn1OutputStream _:
                case BerOutputStream _:
                    derOut.WriteByte( 36 );
                    derOut.WriteByte( 128 );
                    foreach (DerOctetString derOctetString in this)
                        derOut.WriteObject( derOctetString );
                    derOut.WriteByte( 0 );
                    derOut.WriteByte( 0 );
                    break;
                default:
                    base.Encode( derOut );
                    break;
            }
        }
    }
}
