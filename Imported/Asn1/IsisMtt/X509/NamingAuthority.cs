// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.IsisMtt.X509.NamingAuthority
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1.X500;
using Org.BouncyCastle.Utilities;
using System.Collections;

namespace Org.BouncyCastle.Asn1.IsisMtt.X509
{
    public class NamingAuthority : Asn1Encodable
    {
        public static readonly DerObjectIdentifier IdIsisMttATNamingAuthoritiesRechtWirtschaftSteuern = new( IsisMttObjectIdentifiers.IdIsisMttATNamingAuthorities.ToString() + ".1" );
        private readonly DerObjectIdentifier namingAuthorityID;
        private readonly string namingAuthorityUrl;
        private readonly DirectoryString namingAuthorityText;

        public static NamingAuthority GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case NamingAuthority _:
                    return (NamingAuthority)obj;
                case Asn1Sequence _:
                    return new NamingAuthority( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public static NamingAuthority GetInstance( Asn1TaggedObject obj, bool isExplicit ) => GetInstance( Asn1Sequence.GetInstance( obj, isExplicit ) );

        private NamingAuthority( Asn1Sequence seq )
        {
            IEnumerator enumerator = seq.Count <= 3 ? seq.GetEnumerator() : throw new ArgumentException( "Bad sequence size: " + seq.Count );
            if (enumerator.MoveNext())
            {
                Asn1Encodable current = (Asn1Encodable)enumerator.Current;
                switch (current)
                {
                    case DerObjectIdentifier _:
                        this.namingAuthorityID = (DerObjectIdentifier)current;
                        break;
                    case DerIA5String _:
                        this.namingAuthorityUrl = DerIA5String.GetInstance( current ).GetString();
                        break;
                    case IAsn1String _:
                        this.namingAuthorityText = DirectoryString.GetInstance( current );
                        break;
                    default:
                        throw new ArgumentException( "Bad object encountered: " + Platform.GetTypeName( current ) );
                }
            }
            if (enumerator.MoveNext())
            {
                Asn1Encodable current = (Asn1Encodable)enumerator.Current;
                switch (current)
                {
                    case DerIA5String _:
                        this.namingAuthorityUrl = DerIA5String.GetInstance( current ).GetString();
                        break;
                    case IAsn1String _:
                        this.namingAuthorityText = DirectoryString.GetInstance( current );
                        break;
                    default:
                        throw new ArgumentException( "Bad object encountered: " + Platform.GetTypeName( current ) );
                }
            }
            if (!enumerator.MoveNext())
                return;
            Asn1Encodable current1 = (Asn1Encodable)enumerator.Current;
            this.namingAuthorityText = current1 is IAsn1String ? DirectoryString.GetInstance( current1 ) : throw new ArgumentException( "Bad object encountered: " + Platform.GetTypeName( current1 ) );
        }

        public virtual DerObjectIdentifier NamingAuthorityID => this.namingAuthorityID;

        public virtual DirectoryString NamingAuthorityText => this.namingAuthorityText;

        public virtual string NamingAuthorityUrl => this.namingAuthorityUrl;

        public NamingAuthority(
          DerObjectIdentifier namingAuthorityID,
          string namingAuthorityUrl,
          DirectoryString namingAuthorityText )
        {
            this.namingAuthorityID = namingAuthorityID;
            this.namingAuthorityUrl = namingAuthorityUrl;
            this.namingAuthorityText = namingAuthorityText;
        }

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new( new Asn1Encodable[0] );
            if (this.namingAuthorityID != null)
                v.Add( namingAuthorityID );
            if (this.namingAuthorityUrl != null)
                v.Add( new DerIA5String( this.namingAuthorityUrl, true ) );
            if (this.namingAuthorityText != null)
                v.Add( namingAuthorityText );
            return new DerSequence( v );
        }
    }
}
