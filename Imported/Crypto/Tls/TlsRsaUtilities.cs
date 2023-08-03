// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.TlsRsaUtilities
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Encodings;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
    public abstract class TlsRsaUtilities
    {
        public static byte[] GenerateEncryptedPreMasterSecret(
          TlsContext context,
          RsaKeyParameters rsaServerPublicKey,
          Stream output )
        {
            byte[] encryptedPreMasterSecret = new byte[48];
            context.SecureRandom.NextBytes( encryptedPreMasterSecret );
            TlsUtilities.WriteVersion( context.ClientVersion, encryptedPreMasterSecret, 0 );
            Pkcs1Encoding pkcs1Encoding = new( new RsaBlindedEngine() );
            pkcs1Encoding.Init( true, new ParametersWithRandom( rsaServerPublicKey, context.SecureRandom ) );
            try
            {
                byte[] numArray = pkcs1Encoding.ProcessBlock( encryptedPreMasterSecret, 0, encryptedPreMasterSecret.Length );
                if (TlsUtilities.IsSsl( context ))
                    output.Write( numArray, 0, numArray.Length );
                else
                    TlsUtilities.WriteOpaque16( numArray, output );
            }
            catch (InvalidCipherTextException ex)
            {
                throw new TlsFatalAlert( 80, ex );
            }
            return encryptedPreMasterSecret;
        }

        public static byte[] SafeDecryptPreMasterSecret(
          TlsContext context,
          RsaKeyParameters rsaServerPrivateKey,
          byte[] encryptedPreMasterSecret )
        {
            ProtocolVersion clientVersion = context.ClientVersion;
            bool flag = false;
            byte[] numArray1 = new byte[48];
            context.SecureRandom.NextBytes( numArray1 );
            byte[] numArray2 = Arrays.Clone( numArray1 );
            try
            {
                Pkcs1Encoding pkcs1Encoding = new( new RsaBlindedEngine(), numArray1 );
                pkcs1Encoding.Init( false, new ParametersWithRandom( rsaServerPrivateKey, context.SecureRandom ) );
                numArray2 = pkcs1Encoding.ProcessBlock( encryptedPreMasterSecret, 0, encryptedPreMasterSecret.Length );
            }
            catch (Exception ex)
            {
            }
            if (!flag || !clientVersion.IsEqualOrEarlierVersionOf( ProtocolVersion.TLSv10 ))
            {
                int num1 = (clientVersion.MajorVersion ^ (numArray2[0] & byte.MaxValue)) | (clientVersion.MinorVersion ^ (numArray2[1] & byte.MaxValue));
                int num2 = num1 | (num1 >> 1);
                int num3 = num2 | (num2 >> 2);
                int num4 = ~(((num3 | (num3 >> 4)) & 1) - 1);
                for (int index = 0; index < 48; ++index)
                    numArray2[index] = (byte)((numArray2[index] & ~num4) | (numArray1[index] & num4));
            }
            return numArray2;
        }
    }
}
