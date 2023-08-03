// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.ExtendedKeyUsage
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;
using System.Collections;

namespace Org.BouncyCastle.Asn1.X509
{
    public class ExtendedKeyUsage : Asn1Encodable
    {
        internal readonly IDictionary usageTable = Platform.CreateHashtable();
        internal readonly Asn1Sequence seq;

        public static ExtendedKeyUsage GetInstance( Asn1TaggedObject obj, bool explicitly ) => GetInstance( Asn1Sequence.GetInstance( obj, explicitly ) );

        public static ExtendedKeyUsage GetInstance( object obj )
        {
            switch (obj)
            {
                case ExtendedKeyUsage _:
                    return (ExtendedKeyUsage)obj;
                case Asn1Sequence _:
                    return new ExtendedKeyUsage( (Asn1Sequence)obj );
                case X509Extension _:
                    return GetInstance( X509Extension.ConvertValueToObject( (X509Extension)obj ) );
                default:
                    throw new ArgumentException( "Invalid ExtendedKeyUsage: " + Platform.GetTypeName( obj ) );
            }
        }

        private ExtendedKeyUsage( Asn1Sequence seq )
        {
            this.seq = seq;
            foreach (object key in seq)
                this.usageTable[key] = key is DerObjectIdentifier ? key : throw new ArgumentException( "Only DerObjectIdentifier instances allowed in ExtendedKeyUsage." );
        }

        public ExtendedKeyUsage( params KeyPurposeID[] usages )
        {
            this.seq = new DerSequence( usages );
            foreach (KeyPurposeID usage in usages)
                this.usageTable[usage] = usage;
        }

        [Obsolete]
        public ExtendedKeyUsage( ArrayList usages )
          : this( (IEnumerable)usages )
        {
        }

        public ExtendedKeyUsage( IEnumerable usages )
        {
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[0] );
            foreach (object usage in usages)
            {
                Asn1Encodable instance = DerObjectIdentifier.GetInstance( usage );
                v.Add( instance );
                this.usageTable[instance] = instance;
            }
            this.seq = new DerSequence( v );
        }

        public bool HasKeyPurposeId( KeyPurposeID keyPurposeId ) => this.usageTable.Contains( keyPurposeId );

        [Obsolete( "Use 'GetAllUsages'" )]
        public ArrayList GetUsages() => new ArrayList( this.usageTable.Values );

        public IList GetAllUsages() => Platform.CreateArrayList( this.usageTable.Values );

        public int Count => this.usageTable.Count;

        public override Asn1Object ToAsn1Object() => seq;
    }
}
