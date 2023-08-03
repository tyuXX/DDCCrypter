// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cmp.PkiStatusEncodable
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.Asn1.Cmp
{
    public class PkiStatusEncodable : Asn1Encodable
    {
        public static readonly PkiStatusEncodable granted = new PkiStatusEncodable( PkiStatus.Granted );
        public static readonly PkiStatusEncodable grantedWithMods = new PkiStatusEncodable( PkiStatus.GrantedWithMods );
        public static readonly PkiStatusEncodable rejection = new PkiStatusEncodable( PkiStatus.Rejection );
        public static readonly PkiStatusEncodable waiting = new PkiStatusEncodable( PkiStatus.Waiting );
        public static readonly PkiStatusEncodable revocationWarning = new PkiStatusEncodable( PkiStatus.RevocationWarning );
        public static readonly PkiStatusEncodable revocationNotification = new PkiStatusEncodable( PkiStatus.RevocationNotification );
        public static readonly PkiStatusEncodable keyUpdateWaiting = new PkiStatusEncodable( PkiStatus.KeyUpdateWarning );
        private readonly DerInteger status;

        private PkiStatusEncodable( PkiStatus status )
          : this( new DerInteger( (int)status ) )
        {
        }

        private PkiStatusEncodable( DerInteger status ) => this.status = status;

        public static PkiStatusEncodable GetInstance( object obj )
        {
            switch (obj)
            {
                case PkiStatusEncodable _:
                    return (PkiStatusEncodable)obj;
                case DerInteger _:
                    return new PkiStatusEncodable( (DerInteger)obj );
                default:
                    throw new ArgumentException( "Invalid object: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public virtual BigInteger Value => this.status.Value;

        public override Asn1Object ToAsn1Object() => status;
    }
}
