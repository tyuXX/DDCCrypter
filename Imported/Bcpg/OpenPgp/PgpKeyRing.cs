// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.OpenPgp.PgpKeyRing
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Bcpg.OpenPgp
{
    public abstract class PgpKeyRing : PgpObject
    {
        internal PgpKeyRing()
        {
        }

        internal static TrustPacket ReadOptionalTrustPacket( BcpgInputStream bcpgInput ) => bcpgInput.NextPacketTag() != PacketTag.Trust ? null : (TrustPacket)bcpgInput.ReadPacket();

        internal static IList ReadSignaturesAndTrust( BcpgInputStream bcpgInput )
        {
            try
            {
                IList arrayList = Platform.CreateArrayList();
                while (bcpgInput.NextPacketTag() == PacketTag.Signature)
                {
                    SignaturePacket sigPacket = (SignaturePacket)bcpgInput.ReadPacket();
                    TrustPacket trustPacket = ReadOptionalTrustPacket( bcpgInput );
                    arrayList.Add( new PgpSignature( sigPacket, trustPacket ) );
                }
                return arrayList;
            }
            catch (PgpException ex)
            {
                throw new IOException( "can't create signature object: " + ex.Message, ex );
            }
        }

        internal static void ReadUserIDs(
          BcpgInputStream bcpgInput,
          out IList ids,
          out IList idTrusts,
          out IList idSigs )
        {
            ids = Platform.CreateArrayList();
            idTrusts = Platform.CreateArrayList();
            idSigs = Platform.CreateArrayList();
            while (bcpgInput.NextPacketTag() == PacketTag.UserId || bcpgInput.NextPacketTag() == PacketTag.UserAttribute)
            {
                Packet packet = bcpgInput.ReadPacket();
                if (packet is UserIdPacket)
                {
                    UserIdPacket userIdPacket = (UserIdPacket)packet;
                    ids.Add( userIdPacket.GetId() );
                }
                else
                {
                    UserAttributePacket userAttributePacket = (UserAttributePacket)packet;
                    ids.Add( new PgpUserAttributeSubpacketVector( userAttributePacket.GetSubpackets() ) );
                }
                idTrusts.Add( ReadOptionalTrustPacket( bcpgInput ) );
                idSigs.Add( ReadSignaturesAndTrust( bcpgInput ) );
            }
        }
    }
}
