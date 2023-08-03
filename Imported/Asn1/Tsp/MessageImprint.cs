// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Tsp.MessageImprint
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Tsp
{
    public class MessageImprint : Asn1Encodable
    {
        private readonly AlgorithmIdentifier hashAlgorithm;
        private readonly byte[] hashedMessage;

        public static MessageImprint GetInstance( object o )
        {
            switch (o)
            {
                case null:
                case MessageImprint _:
                    return (MessageImprint)o;
                case Asn1Sequence _:
                    return new MessageImprint( (Asn1Sequence)o );
                default:
                    throw new ArgumentException( "Unknown object in 'MessageImprint' factory: " + Platform.GetTypeName( o ) );
            }
        }

        private MessageImprint( Asn1Sequence seq )
        {
            this.hashAlgorithm = seq.Count == 2 ? AlgorithmIdentifier.GetInstance( seq[0] ) : throw new ArgumentException( "Wrong number of elements in sequence", nameof( seq ) );
            this.hashedMessage = Asn1OctetString.GetInstance( seq[1] ).GetOctets();
        }

        public MessageImprint( AlgorithmIdentifier hashAlgorithm, byte[] hashedMessage )
        {
            this.hashAlgorithm = hashAlgorithm;
            this.hashedMessage = hashedMessage;
        }

        public AlgorithmIdentifier HashAlgorithm => this.hashAlgorithm;

        public byte[] GetHashedMessage() => this.hashedMessage;

        public override Asn1Object ToAsn1Object() => new DerSequence( new Asn1Encodable[2]
        {
       hashAlgorithm,
       new DerOctetString(this.hashedMessage)
        } );
    }
}
