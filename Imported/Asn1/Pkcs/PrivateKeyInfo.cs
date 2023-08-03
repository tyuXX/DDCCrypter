// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Pkcs.PrivateKeyInfo
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Math;
using System;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Asn1.Pkcs
{
    public class PrivateKeyInfo : Asn1Encodable
    {
        private readonly Asn1OctetString privKey;
        private readonly AlgorithmIdentifier algID;
        private readonly Asn1Set attributes;

        public static PrivateKeyInfo GetInstance( Asn1TaggedObject obj, bool explicitly ) => GetInstance( Asn1Sequence.GetInstance( obj, explicitly ) );

        public static PrivateKeyInfo GetInstance( object obj )
        {
            if (obj == null)
                return null;
            return obj is PrivateKeyInfo ? (PrivateKeyInfo)obj : new PrivateKeyInfo( Asn1Sequence.GetInstance( obj ) );
        }

        public PrivateKeyInfo( AlgorithmIdentifier algID, Asn1Encodable privateKey )
          : this( algID, privateKey, null )
        {
        }

        public PrivateKeyInfo( AlgorithmIdentifier algID, Asn1Encodable privateKey, Asn1Set attributes )
        {
            this.algID = algID;
            this.privKey = new DerOctetString( privateKey.GetEncoded( "DER" ) );
            this.attributes = attributes;
        }

        private PrivateKeyInfo( Asn1Sequence seq )
        {
            IEnumerator enumerator = seq.GetEnumerator();
            enumerator.MoveNext();
            BigInteger bigInteger = ((DerInteger)enumerator.Current).Value;
            if (bigInteger.IntValue != 0)
                throw new ArgumentException( "wrong version for private key info: " + bigInteger.IntValue );
            enumerator.MoveNext();
            this.algID = AlgorithmIdentifier.GetInstance( enumerator.Current );
            enumerator.MoveNext();
            this.privKey = Asn1OctetString.GetInstance( enumerator.Current );
            if (!enumerator.MoveNext())
                return;
            this.attributes = Asn1Set.GetInstance( (Asn1TaggedObject)enumerator.Current, false );
        }

        public virtual AlgorithmIdentifier PrivateKeyAlgorithm => this.algID;

        [Obsolete( "Use 'PrivateKeyAlgorithm' property instead" )]
        public virtual AlgorithmIdentifier AlgorithmID => this.algID;

        public virtual Asn1Object ParsePrivateKey() => Asn1Object.FromByteArray( this.privKey.GetOctets() );

        [Obsolete( "Use 'ParsePrivateKey' instead" )]
        public virtual Asn1Object PrivateKey
        {
            get
            {
                try
                {
                    return this.ParsePrivateKey();
                }
                catch (IOException ex)
                {
                    throw new InvalidOperationException( "unable to parse private key" );
                }
            }
        }

        public virtual Asn1Set Attributes => this.attributes;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[3]
            {
         new DerInteger(0),
         algID,
         privKey
            } );
            if (this.attributes != null)
                v.Add( new DerTaggedObject( false, 0, attributes ) );
            return new DerSequence( v );
        }
    }
}
