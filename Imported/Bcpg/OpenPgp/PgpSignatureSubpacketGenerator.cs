// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.OpenPgp.PgpSignatureSubpacketGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Bcpg.Sig;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections;

namespace Org.BouncyCastle.Bcpg.OpenPgp
{
    public class PgpSignatureSubpacketGenerator
    {
        private IList list = Platform.CreateArrayList();

        public void SetRevocable( bool isCritical, bool isRevocable ) => this.list.Add( new Revocable( isCritical, isRevocable ) );

        public void SetExportable( bool isCritical, bool isExportable ) => this.list.Add( new Exportable( isCritical, isExportable ) );

        public void SetFeature( bool isCritical, byte feature ) => this.list.Add( new Features( isCritical, feature ) );

        public void SetTrust( bool isCritical, int depth, int trustAmount ) => this.list.Add( new TrustSignature( isCritical, depth, trustAmount ) );

        public void SetKeyExpirationTime( bool isCritical, long seconds ) => this.list.Add( new KeyExpirationTime( isCritical, seconds ) );

        public void SetSignatureExpirationTime( bool isCritical, long seconds ) => this.list.Add( new SignatureExpirationTime( isCritical, seconds ) );

        public void SetSignatureCreationTime( bool isCritical, DateTime date ) => this.list.Add( new SignatureCreationTime( isCritical, date ) );

        public void SetPreferredHashAlgorithms( bool isCritical, int[] algorithms ) => this.list.Add( new PreferredAlgorithms( SignatureSubpacketTag.PreferredHashAlgorithms, isCritical, algorithms ) );

        public void SetPreferredSymmetricAlgorithms( bool isCritical, int[] algorithms ) => this.list.Add( new PreferredAlgorithms( SignatureSubpacketTag.PreferredSymmetricAlgorithms, isCritical, algorithms ) );

        public void SetPreferredCompressionAlgorithms( bool isCritical, int[] algorithms ) => this.list.Add( new PreferredAlgorithms( SignatureSubpacketTag.PreferredCompressionAlgorithms, isCritical, algorithms ) );

        public void SetKeyFlags( bool isCritical, int flags ) => this.list.Add( new KeyFlags( isCritical, flags ) );

        public void SetSignerUserId( bool isCritical, string userId )
        {
            if (userId == null)
                throw new ArgumentNullException( nameof( userId ) );
            this.list.Add( new SignerUserId( isCritical, userId ) );
        }

        public void SetSignerUserId( bool isCritical, byte[] rawUserId )
        {
            if (rawUserId == null)
                throw new ArgumentNullException( nameof( rawUserId ) );
            this.list.Add( new SignerUserId( isCritical, false, rawUserId ) );
        }

        public void SetEmbeddedSignature( bool isCritical, PgpSignature pgpSignature )
        {
            byte[] encoded = pgpSignature.GetEncoded();
            byte[] numArray = encoded.Length - 1 <= 256 ? new byte[encoded.Length - 2] : new byte[encoded.Length - 3];
            Array.Copy( encoded, encoded.Length - numArray.Length, numArray, 0, numArray.Length );
            this.list.Add( new EmbeddedSignature( isCritical, false, numArray ) );
        }

        public void SetPrimaryUserId( bool isCritical, bool isPrimaryUserId ) => this.list.Add( new PrimaryUserId( isCritical, isPrimaryUserId ) );

        public void SetNotationData(
          bool isCritical,
          bool isHumanReadable,
          string notationName,
          string notationValue )
        {
            this.list.Add( new NotationData( isCritical, isHumanReadable, notationName, notationValue ) );
        }

        public void SetRevocationReason(
          bool isCritical,
          RevocationReasonTag reason,
          string description )
        {
            this.list.Add( new RevocationReason( isCritical, reason, description ) );
        }

        public void SetRevocationKey(
          bool isCritical,
          PublicKeyAlgorithmTag keyAlgorithm,
          byte[] fingerprint )
        {
            this.list.Add( new RevocationKey( isCritical, RevocationKeyTag.ClassDefault, keyAlgorithm, fingerprint ) );
        }

        public void SetIssuerKeyID( bool isCritical, long keyID ) => this.list.Add( new IssuerKeyId( isCritical, keyID ) );

        public PgpSignatureSubpacketVector Generate()
        {
            SignatureSubpacket[] packets = new SignatureSubpacket[this.list.Count];
            for (int index = 0; index < this.list.Count; ++index)
                packets[index] = (SignatureSubpacket)this.list[index];
            return new PgpSignatureSubpacketVector( packets );
        }
    }
}
