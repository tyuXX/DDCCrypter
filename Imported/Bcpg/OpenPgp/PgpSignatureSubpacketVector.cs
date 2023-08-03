// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.OpenPgp.PgpSignatureSubpacketVector
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Bcpg.Sig;
using System;

namespace Org.BouncyCastle.Bcpg.OpenPgp
{
    public class PgpSignatureSubpacketVector
    {
        private readonly SignatureSubpacket[] packets;

        internal PgpSignatureSubpacketVector( SignatureSubpacket[] packets ) => this.packets = packets;

        public SignatureSubpacket GetSubpacket( SignatureSubpacketTag type )
        {
            for (int index = 0; index != this.packets.Length; ++index)
            {
                if (this.packets[index].SubpacketType == type)
                    return this.packets[index];
            }
            return null;
        }

        public bool HasSubpacket( SignatureSubpacketTag type ) => this.GetSubpacket( type ) != null;

        public SignatureSubpacket[] GetSubpackets( SignatureSubpacketTag type )
        {
            int length = 0;
            for (int index = 0; index < this.packets.Length; ++index)
            {
                if (this.packets[index].SubpacketType == type)
                    ++length;
            }
            SignatureSubpacket[] subpackets = new SignatureSubpacket[length];
            int num = 0;
            for (int index = 0; index < this.packets.Length; ++index)
            {
                if (this.packets[index].SubpacketType == type)
                    subpackets[num++] = this.packets[index];
            }
            return subpackets;
        }

        public NotationData[] GetNotationDataOccurences()
        {
            SignatureSubpacket[] subpackets = this.GetSubpackets( SignatureSubpacketTag.NotationData );
            NotationData[] notationDataOccurences = new NotationData[subpackets.Length];
            for (int index = 0; index < subpackets.Length; ++index)
                notationDataOccurences[index] = (NotationData)subpackets[index];
            return notationDataOccurences;
        }

        public long GetIssuerKeyId()
        {
            SignatureSubpacket subpacket = this.GetSubpacket( SignatureSubpacketTag.IssuerKeyId );
            return subpacket != null ? ((IssuerKeyId)subpacket).KeyId : 0L;
        }

        public bool HasSignatureCreationTime() => this.GetSubpacket( SignatureSubpacketTag.CreationTime ) != null;

        public DateTime GetSignatureCreationTime() => ((SignatureCreationTime)(this.GetSubpacket( SignatureSubpacketTag.CreationTime ) ?? throw new PgpException( "SignatureCreationTime not available" ))).GetTime();

        public long GetSignatureExpirationTime()
        {
            SignatureSubpacket subpacket = this.GetSubpacket( SignatureSubpacketTag.ExpireTime );
            return subpacket != null ? ((SignatureExpirationTime)subpacket).Time : 0L;
        }

        public long GetKeyExpirationTime()
        {
            SignatureSubpacket subpacket = this.GetSubpacket( SignatureSubpacketTag.KeyExpireTime );
            return subpacket != null ? ((KeyExpirationTime)subpacket).Time : 0L;
        }

        public int[] GetPreferredHashAlgorithms() => ((PreferredAlgorithms)this.GetSubpacket( SignatureSubpacketTag.PreferredHashAlgorithms ))?.GetPreferences();

        public int[] GetPreferredSymmetricAlgorithms() => ((PreferredAlgorithms)this.GetSubpacket( SignatureSubpacketTag.PreferredSymmetricAlgorithms ))?.GetPreferences();

        public int[] GetPreferredCompressionAlgorithms() => ((PreferredAlgorithms)this.GetSubpacket( SignatureSubpacketTag.PreferredCompressionAlgorithms ))?.GetPreferences();

        public int GetKeyFlags()
        {
            SignatureSubpacket subpacket = this.GetSubpacket( SignatureSubpacketTag.KeyFlags );
            return subpacket != null ? ((KeyFlags)subpacket).Flags : 0;
        }

        public string GetSignerUserId() => ((SignerUserId)this.GetSubpacket( SignatureSubpacketTag.SignerUserId ))?.GetId();

        public bool IsPrimaryUserId()
        {
            PrimaryUserId subpacket = (PrimaryUserId)this.GetSubpacket( SignatureSubpacketTag.PrimaryUserId );
            return subpacket != null && subpacket.IsPrimaryUserId();
        }

        public SignatureSubpacketTag[] GetCriticalTags()
        {
            int length = 0;
            for (int index = 0; index != this.packets.Length; ++index)
            {
                if (this.packets[index].IsCritical())
                    ++length;
            }
            SignatureSubpacketTag[] criticalTags = new SignatureSubpacketTag[length];
            int num = 0;
            for (int index = 0; index != this.packets.Length; ++index)
            {
                if (this.packets[index].IsCritical())
                    criticalTags[num++] = this.packets[index].SubpacketType;
            }
            return criticalTags;
        }

        public Features GetFeatures()
        {
            SignatureSubpacket subpacket = this.GetSubpacket( SignatureSubpacketTag.Features );
            return subpacket == null ? null : new Features( subpacket.IsCritical(), subpacket.IsLongLength(), subpacket.GetData() );
        }

        [Obsolete( "Use 'Count' property instead" )]
        public int Size => this.packets.Length;

        public int Count => this.packets.Length;

        internal SignatureSubpacket[] ToSubpacketArray() => this.packets;
    }
}
