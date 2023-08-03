// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.OpenPgp.PgpPbeEncryptedData
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.IO;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.IO;
using System.IO;

namespace Org.BouncyCastle.Bcpg.OpenPgp
{
    public class PgpPbeEncryptedData : PgpEncryptedData
    {
        private readonly SymmetricKeyEncSessionPacket keyData;

        internal PgpPbeEncryptedData( SymmetricKeyEncSessionPacket keyData, InputStreamPacket encData )
          : base( encData )
        {
            this.keyData = keyData;
        }

        public override Stream GetInputStream() => this.encData.GetInputStream();

        public Stream GetDataStream( char[] passPhrase ) => this.DoGetDataStream( PgpUtilities.EncodePassPhrase( passPhrase, false ), true );

        public Stream GetDataStreamUtf8( char[] passPhrase ) => this.DoGetDataStream( PgpUtilities.EncodePassPhrase( passPhrase, true ), true );

        public Stream GetDataStreamRaw( byte[] rawPassPhrase ) => this.DoGetDataStream( rawPassPhrase, false );

        internal Stream DoGetDataStream( byte[] rawPassPhrase, bool clearPassPhrase )
        {
            try
            {
                SymmetricKeyAlgorithmTag encAlgorithm = this.keyData.EncAlgorithm;
                KeyParameter parameters = PgpUtilities.DoMakeKeyFromPassPhrase( encAlgorithm, this.keyData.S2k, rawPassPhrase, clearPassPhrase );
                byte[] secKeyData = this.keyData.GetSecKeyData();
                if (secKeyData != null && secKeyData.Length > 0)
                {
                    IBufferedCipher cipher = CipherUtilities.GetCipher( PgpUtilities.GetSymmetricCipherName( encAlgorithm ) + "/CFB/NoPadding" );
                    cipher.Init( false, new ParametersWithIV( parameters, new byte[cipher.GetBlockSize()] ) );
                    byte[] keyBytes = cipher.DoFinal( secKeyData );
                    encAlgorithm = (SymmetricKeyAlgorithmTag)keyBytes[0];
                    parameters = ParameterUtilities.CreateKeyParameter( PgpUtilities.GetSymmetricCipherName( encAlgorithm ), keyBytes, 1, keyBytes.Length - 1 );
                }
                IBufferedCipher streamCipher = this.CreateStreamCipher( encAlgorithm );
                byte[] numArray = new byte[streamCipher.GetBlockSize()];
                streamCipher.Init( false, new ParametersWithIV( parameters, numArray ) );
                this.encStream = BcpgInputStream.Wrap( new CipherStream( this.encData.GetInputStream(), streamCipher, null ) );
                if (this.encData is SymmetricEncIntegrityPacket)
                {
                    this.truncStream = new PgpEncryptedData.TruncatedStream( this.encStream );
                    this.encStream = new DigestStream( truncStream, DigestUtilities.GetDigest( PgpUtilities.GetDigestName( HashAlgorithmTag.Sha1 ) ), null );
                }
                if (Streams.ReadFully( this.encStream, numArray, 0, numArray.Length ) < numArray.Length)
                    throw new EndOfStreamException( "unexpected end of stream." );
                int num1 = this.encStream.ReadByte();
                int num2 = this.encStream.ReadByte();
                if (num1 < 0 || num2 < 0)
                    throw new EndOfStreamException( "unexpected end of stream." );
                bool flag1 = numArray[numArray.Length - 2] == (byte)num1 && numArray[numArray.Length - 1] == (byte)num2;
                bool flag2 = num1 == 0 && num2 == 0;
                if (!flag1 && !flag2)
                    throw new PgpDataValidationException( "quick check failed." );
                return this.encStream;
            }
            catch (PgpException ex)
            {
                throw ex;
            }
            catch (Exception ex)
            {
                throw new PgpException( "Exception creating cipher", ex );
            }
        }

        private IBufferedCipher CreateStreamCipher( SymmetricKeyAlgorithmTag keyAlgorithm )
        {
            string str = this.encData is SymmetricEncIntegrityPacket ? "CFB" : "OpenPGPCFB";
            return CipherUtilities.GetCipher( PgpUtilities.GetSymmetricCipherName( keyAlgorithm ) + "/" + str + "/NoPadding" );
        }
    }
}
