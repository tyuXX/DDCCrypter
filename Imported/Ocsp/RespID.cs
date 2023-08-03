// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Ocsp.RespID
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Ocsp;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.X509;
using System;

namespace Org.BouncyCastle.Ocsp
{
    public class RespID
    {
        internal readonly ResponderID id;

        public RespID( ResponderID id ) => this.id = id;

        public RespID( X509Name name ) => this.id = new ResponderID( name );

        public RespID( AsymmetricKeyParameter publicKey )
        {
            try
            {
                this.id = new ResponderID( new DerOctetString( DigestUtilities.CalculateDigest( "SHA1", SubjectPublicKeyInfoFactory.CreateSubjectPublicKeyInfo( publicKey ).PublicKeyData.GetBytes() ) ) );
            }
            catch (Exception ex)
            {
                throw new OcspException( "problem creating ID: " + ex, ex );
            }
        }

        public ResponderID ToAsn1Object() => this.id;

        public override bool Equals( object obj )
        {
            if (obj == this)
                return true;
            return obj is RespID respId && this.id.Equals( respId.id );
        }

        public override int GetHashCode() => this.id.GetHashCode();
    }
}
