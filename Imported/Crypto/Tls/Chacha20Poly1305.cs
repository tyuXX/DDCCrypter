// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.Chacha20Poly1305
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Utilities;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Tls
{
    public class Chacha20Poly1305 : TlsCipher
    {
        protected readonly TlsContext context;
        protected readonly ChaChaEngine encryptCipher;
        protected readonly ChaChaEngine decryptCipher;

        public Chacha20Poly1305( TlsContext context )
        {
            this.context = TlsUtilities.IsTlsV12( context ) ? context : throw new TlsFatalAlert( 80 );
            byte[] keyBlock = TlsUtilities.CalculateKeyBlock( context, 64 );
            KeyParameter keyParameter1 = new( keyBlock, 0, 32 );
            KeyParameter keyParameter2 = new( keyBlock, 32, 32 );
            this.encryptCipher = new ChaChaEngine( 20 );
            this.decryptCipher = new ChaChaEngine( 20 );
            KeyParameter parameters1;
            KeyParameter parameters2;
            if (context.IsServer)
            {
                parameters1 = keyParameter2;
                parameters2 = keyParameter1;
            }
            else
            {
                parameters1 = keyParameter1;
                parameters2 = keyParameter2;
            }
            byte[] iv = new byte[8];
            this.encryptCipher.Init( true, new ParametersWithIV( parameters1, iv ) );
            this.decryptCipher.Init( false, new ParametersWithIV( parameters2, iv ) );
        }

        public virtual int GetPlaintextLimit( int ciphertextLimit ) => ciphertextLimit - 16;

        public virtual byte[] EncodePlaintext(
          long seqNo,
          byte type,
          byte[] plaintext,
          int offset,
          int len )
        {
            int length = len + 16;
            KeyParameter macKey = this.InitRecordMac( this.encryptCipher, true, seqNo );
            byte[] numArray = new byte[length];
            this.encryptCipher.ProcessBytes( plaintext, offset, len, numArray, 0 );
            byte[] additionalData = this.GetAdditionalData( seqNo, type, len );
            byte[] recordMac = this.CalculateRecordMac( macKey, additionalData, numArray, 0, len );
            Array.Copy( recordMac, 0, numArray, len, recordMac.Length );
            return numArray;
        }

        public virtual byte[] DecodeCiphertext(
          long seqNo,
          byte type,
          byte[] ciphertext,
          int offset,
          int len )
        {
            if (this.GetPlaintextLimit( len ) < 0)
                throw new TlsFatalAlert( 50 );
            int len1 = len - 16;
            byte[] b = Arrays.CopyOfRange( ciphertext, offset + len1, offset + len );
            if (!Arrays.ConstantTimeAreEqual( this.CalculateRecordMac( this.InitRecordMac( this.decryptCipher, false, seqNo ), this.GetAdditionalData( seqNo, type, len1 ), ciphertext, offset, len1 ), b ))
                throw new TlsFatalAlert( 20 );
            byte[] outBytes = new byte[len1];
            this.decryptCipher.ProcessBytes( ciphertext, offset, len1, outBytes, 0 );
            return outBytes;
        }

        protected virtual KeyParameter InitRecordMac(
          ChaChaEngine cipher,
          bool forEncryption,
          long seqNo )
        {
            byte[] numArray1 = new byte[8];
            TlsUtilities.WriteUint64( seqNo, numArray1, 0 );
            cipher.Init( forEncryption, new ParametersWithIV( null, numArray1 ) );
            byte[] numArray2 = new byte[64];
            cipher.ProcessBytes( numArray2, 0, numArray2.Length, numArray2, 0 );
            Array.Copy( numArray2, 0, numArray2, 32, 16 );
            KeyParameter keyParameter = new( numArray2, 16, 32 );
            Poly1305KeyGenerator.Clamp( keyParameter.GetKey() );
            return keyParameter;
        }

        protected virtual byte[] CalculateRecordMac(
          KeyParameter macKey,
          byte[] additionalData,
          byte[] buf,
          int off,
          int len )
        {
            IMac mac = new Poly1305();
            mac.Init( macKey );
            this.UpdateRecordMac( mac, additionalData, 0, additionalData.Length );
            this.UpdateRecordMac( mac, buf, off, len );
            return MacUtilities.DoFinal( mac );
        }

        protected virtual void UpdateRecordMac( IMac mac, byte[] buf, int off, int len )
        {
            mac.BlockUpdate( buf, off, len );
            byte[] le = Pack.UInt64_To_LE( (ulong)len );
            mac.BlockUpdate( le, 0, le.Length );
        }

        protected virtual byte[] GetAdditionalData( long seqNo, byte type, int len )
        {
            byte[] buf = new byte[13];
            TlsUtilities.WriteUint64( seqNo, buf, 0 );
            TlsUtilities.WriteUint8( type, buf, 8 );
            TlsUtilities.WriteVersion( this.context.ServerVersion, buf, 9 );
            TlsUtilities.WriteUint16( len, buf, 11 );
            return buf;
        }
    }
}
