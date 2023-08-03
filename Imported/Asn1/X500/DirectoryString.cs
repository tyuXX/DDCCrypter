// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X500.DirectoryString
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.X500
{
    public class DirectoryString : Asn1Encodable, IAsn1Choice, IAsn1String
    {
        private readonly DerStringBase str;

        public static DirectoryString GetInstance( object obj )
        {
            switch (obj)
            {
                case DirectoryString _:
                    return (DirectoryString)obj;
                case DerStringBase _:
                    if (obj is DerT61String || obj is DerPrintableString || obj is DerUniversalString || obj is DerUtf8String || obj is DerBmpString)
                        return new DirectoryString( (DerStringBase)obj );
                    break;
            }
            throw new ArgumentException( "unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
        }

        public static DirectoryString GetInstance( Asn1TaggedObject obj, bool isExplicit )
        {
            if (!isExplicit)
                throw new ArgumentException( "choice item must be explicitly tagged" );
            return GetInstance( obj.GetObject() );
        }

        private DirectoryString( DerStringBase str ) => this.str = str;

        public DirectoryString( string str ) => this.str = new DerUtf8String( str );

        public string GetString() => this.str.GetString();

        public override Asn1Object ToAsn1Object() => this.str.ToAsn1Object();
    }
}
