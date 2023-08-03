// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.Cmp.PkiStatusEncodable
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Asn1.Cmp
{
    public class PkiStatusEncodable : Asn1Encodable
    {
        public static readonly PkiStatusEncodable granted = new( PkiStatus.Granted );
        public static readonly PkiStatusEncodable grantedWithMods = new( PkiStatus.GrantedWithMods );
        public static readonly PkiStatusEncodable rejection = new( PkiStatus.Rejection );
        public static readonly PkiStatusEncodable waiting = new( PkiStatus.Waiting );
        public static readonly PkiStatusEncodable revocationWarning = new( PkiStatus.RevocationWarning );
        public static readonly PkiStatusEncodable revocationNotification = new( PkiStatus.RevocationNotification );
        public static readonly PkiStatusEncodable keyUpdateWaiting = new( PkiStatus.KeyUpdateWarning );
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
