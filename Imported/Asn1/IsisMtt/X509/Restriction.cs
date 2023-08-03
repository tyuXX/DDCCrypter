// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.IsisMtt.X509.Restriction
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X500;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.IsisMtt.X509
{
    public class Restriction : Asn1Encodable
    {
        private readonly DirectoryString restriction;

        public static Restriction GetInstance( object obj )
        {
            switch (obj)
            {
                case Restriction _:
                    return (Restriction)obj;
                case IAsn1String _:
                    return new Restriction( DirectoryString.GetInstance( obj ) );
                default:
                    throw new ArgumentException( "Unknown object in GetInstance: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        private Restriction( DirectoryString restriction ) => this.restriction = restriction;

        public Restriction( string restriction ) => this.restriction = new DirectoryString( restriction );

        public virtual DirectoryString RestrictionString => this.restriction;

        public override Asn1Object ToAsn1Object() => this.restriction.ToAsn1Object();
    }
}
