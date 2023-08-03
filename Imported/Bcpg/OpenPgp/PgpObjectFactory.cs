// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.OpenPgp.PgpObjectFactory
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Bcpg.OpenPgp
{
    public class PgpObjectFactory
    {
        private readonly BcpgInputStream bcpgIn;

        public PgpObjectFactory( Stream inputStream ) => this.bcpgIn = BcpgInputStream.Wrap( inputStream );

        public PgpObjectFactory( byte[] bytes )
          : this( new MemoryStream( bytes, false ) )
        {
        }

        public PgpObject NextPgpObject()
        {
            PacketTag packetTag = this.bcpgIn.NextPacketTag();
            if (packetTag == ~PacketTag.Reserved)
                return null;
            switch (packetTag)
            {
                case PacketTag.PublicKeyEncryptedSession:
                case PacketTag.SymmetricKeyEncryptedSessionKey:
                    return new PgpEncryptedDataList( this.bcpgIn );
                case PacketTag.Signature:
                    IList arrayList1 = Platform.CreateArrayList();
                    while (this.bcpgIn.NextPacketTag() == PacketTag.Signature)
                    {
                        try
                        {
                            arrayList1.Add( new PgpSignature( this.bcpgIn ) );
                        }
                        catch (PgpException ex)
                        {
                            throw new IOException( "can't create signature object: " + ex );
                        }
                    }
                    PgpSignature[] sigs1 = new PgpSignature[arrayList1.Count];
                    for (int index = 0; index < arrayList1.Count; ++index)
                        sigs1[index] = (PgpSignature)arrayList1[index];
                    return new PgpSignatureList( sigs1 );
                case PacketTag.OnePassSignature:
                    IList arrayList2 = Platform.CreateArrayList();
                    while (this.bcpgIn.NextPacketTag() == PacketTag.OnePassSignature)
                    {
                        try
                        {
                            arrayList2.Add( new PgpOnePassSignature( this.bcpgIn ) );
                        }
                        catch (PgpException ex)
                        {
                            throw new IOException( "can't create one pass signature object: " + ex );
                        }
                    }
                    PgpOnePassSignature[] sigs2 = new PgpOnePassSignature[arrayList2.Count];
                    for (int index = 0; index < arrayList2.Count; ++index)
                        sigs2[index] = (PgpOnePassSignature)arrayList2[index];
                    return new PgpOnePassSignatureList( sigs2 );
                case PacketTag.SecretKey:
                    try
                    {
                        return new PgpSecretKeyRing( bcpgIn );
                    }
                    catch (PgpException ex)
                    {
                        throw new IOException( "can't create secret key object: " + ex );
                    }
                case PacketTag.PublicKey:
                    return new PgpPublicKeyRing( bcpgIn );
                case PacketTag.CompressedData:
                    return new PgpCompressedData( this.bcpgIn );
                case PacketTag.Marker:
                    return new PgpMarker( this.bcpgIn );
                case PacketTag.LiteralData:
                    return new PgpLiteralData( this.bcpgIn );
                case PacketTag.Experimental1:
                case PacketTag.Experimental2:
                case PacketTag.Experimental3:
                case PacketTag.Experimental4:
                    return new PgpExperimental( this.bcpgIn );
                default:
                    throw new IOException( "unknown object in stream " + this.bcpgIn.NextPacketTag() );
            }
        }

        [Obsolete( "Use NextPgpObject() instead" )]
        public object NextObject() => this.NextPgpObject();

        public IList AllPgpObjects()
        {
            IList arrayList = Platform.CreateArrayList();
            PgpObject pgpObject;
            while ((pgpObject = this.NextPgpObject()) != null)
                arrayList.Add( pgpObject );
            return arrayList;
        }
    }
}
