// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.BasicConstraints
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.X509
{
    public class BasicConstraints : Asn1Encodable
    {
        private readonly DerBoolean cA;
        private readonly DerInteger pathLenConstraint;

        public static BasicConstraints GetInstance( Asn1TaggedObject obj, bool explicitly ) => GetInstance( Asn1Sequence.GetInstance( obj, explicitly ) );

        public static BasicConstraints GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case BasicConstraints _:
                    return (BasicConstraints)obj;
                case Asn1Sequence _:
                    return new BasicConstraints( (Asn1Sequence)obj );
                case X509Extension _:
                    return GetInstance( X509Extension.ConvertValueToObject( (X509Extension)obj ) );
                default:
                    throw new ArgumentException( "unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        private BasicConstraints( Asn1Sequence seq )
        {
            if (seq.Count <= 0)
                return;
            if (seq[0] is DerBoolean)
                this.cA = DerBoolean.GetInstance( seq[0] );
            else
                this.pathLenConstraint = DerInteger.GetInstance( seq[0] );
            if (seq.Count <= 1)
                return;
            if (this.cA == null)
                throw new ArgumentException( "wrong sequence in constructor", nameof( seq ) );
            this.pathLenConstraint = DerInteger.GetInstance( seq[1] );
        }

        public BasicConstraints( bool cA )
        {
            if (!cA)
                return;
            this.cA = DerBoolean.True;
        }

        public BasicConstraints( int pathLenConstraint )
        {
            this.cA = DerBoolean.True;
            this.pathLenConstraint = new DerInteger( pathLenConstraint );
        }

        public bool IsCA() => this.cA != null && this.cA.IsTrue;

        public BigInteger PathLenConstraint => this.pathLenConstraint != null ? this.pathLenConstraint.Value : null;

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[0] );
            if (this.cA != null)
                v.Add( cA );
            if (this.pathLenConstraint != null)
                v.Add( pathLenConstraint );
            return new DerSequence( v );
        }

        public override string ToString()
        {
            if (this.pathLenConstraint == null)
                return "BasicConstraints: isCa(" + this.IsCA() + ")";
            return "BasicConstraints: isCa(" + this.IsCA() + "), pathLenConstraint = " + pathLenConstraint.Value;
        }
    }
}
