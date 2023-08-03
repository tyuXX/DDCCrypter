// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.OpenPgp.PgpUtilities
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.IO;
using System.Text;

namespace Org.BouncyCastle.Bcpg.OpenPgp
{
    public sealed class PgpUtilities
    {
        private const int ReadAhead = 60;

        private PgpUtilities()
        {
        }

        public static MPInteger[] DsaSigToMpi( byte[] encoding )
        {
            DerInteger derInteger1;
            DerInteger derInteger2;
            try
            {
                Asn1Sequence asn1Sequence = (Asn1Sequence)Asn1Object.FromByteArray( encoding );
                derInteger1 = (DerInteger)asn1Sequence[0];
                derInteger2 = (DerInteger)asn1Sequence[1];
            }
            catch (IOException ex)
            {
                throw new PgpException( "exception encoding signature", ex );
            }
            return new MPInteger[2]
            {
        new MPInteger(derInteger1.Value),
        new MPInteger(derInteger2.Value)
            };
        }

        public static MPInteger[] RsaSigToMpi( byte[] encoding ) => new MPInteger[1]
        {
      new MPInteger(new BigInteger(1, encoding))
        };

        public static string GetDigestName( HashAlgorithmTag hashAlgorithm )
        {
            switch (hashAlgorithm)
            {
                case HashAlgorithmTag.MD5:
                    return "MD5";
                case HashAlgorithmTag.Sha1:
                    return "SHA1";
                case HashAlgorithmTag.RipeMD160:
                    return "RIPEMD160";
                case HashAlgorithmTag.MD2:
                    return "MD2";
                case HashAlgorithmTag.Sha256:
                    return "SHA256";
                case HashAlgorithmTag.Sha384:
                    return "SHA384";
                case HashAlgorithmTag.Sha512:
                    return "SHA512";
                case HashAlgorithmTag.Sha224:
                    return "SHA224";
                default:
                    throw new PgpException( "unknown hash algorithm tag in GetDigestName: " + hashAlgorithm );
            }
        }

        public static string GetSignatureName(
          PublicKeyAlgorithmTag keyAlgorithm,
          HashAlgorithmTag hashAlgorithm )
        {
            string str;
            switch (keyAlgorithm)
            {
                case PublicKeyAlgorithmTag.RsaGeneral:
                case PublicKeyAlgorithmTag.RsaSign:
                    str = "RSA";
                    break;
                case PublicKeyAlgorithmTag.ElGamalEncrypt:
                case PublicKeyAlgorithmTag.ElGamalGeneral:
                    str = "ElGamal";
                    break;
                case PublicKeyAlgorithmTag.Dsa:
                    str = "DSA";
                    break;
                case PublicKeyAlgorithmTag.EC:
                    str = "ECDH";
                    break;
                case PublicKeyAlgorithmTag.ECDsa:
                    str = "ECDSA";
                    break;
                default:
                    throw new PgpException( "unknown algorithm tag in signature:" + keyAlgorithm );
            }
            return GetDigestName( hashAlgorithm ) + "with" + str;
        }

        public static string GetSymmetricCipherName( SymmetricKeyAlgorithmTag algorithm )
        {
            switch (algorithm)
            {
                case SymmetricKeyAlgorithmTag.Null:
                    return null;
                case SymmetricKeyAlgorithmTag.Idea:
                    return "IDEA";
                case SymmetricKeyAlgorithmTag.TripleDes:
                    return "DESEDE";
                case SymmetricKeyAlgorithmTag.Cast5:
                    return "CAST5";
                case SymmetricKeyAlgorithmTag.Blowfish:
                    return "Blowfish";
                case SymmetricKeyAlgorithmTag.Safer:
                    return "SAFER";
                case SymmetricKeyAlgorithmTag.Des:
                    return "DES";
                case SymmetricKeyAlgorithmTag.Aes128:
                    return "AES";
                case SymmetricKeyAlgorithmTag.Aes192:
                    return "AES";
                case SymmetricKeyAlgorithmTag.Aes256:
                    return "AES";
                case SymmetricKeyAlgorithmTag.Twofish:
                    return "Twofish";
                case SymmetricKeyAlgorithmTag.Camellia128:
                    return "Camellia";
                case SymmetricKeyAlgorithmTag.Camellia192:
                    return "Camellia";
                case SymmetricKeyAlgorithmTag.Camellia256:
                    return "Camellia";
                default:
                    throw new PgpException( "unknown symmetric algorithm: " + algorithm );
            }
        }

        public static int GetKeySize( SymmetricKeyAlgorithmTag algorithm )
        {
            switch (algorithm)
            {
                case SymmetricKeyAlgorithmTag.Idea:
                case SymmetricKeyAlgorithmTag.Cast5:
                case SymmetricKeyAlgorithmTag.Blowfish:
                case SymmetricKeyAlgorithmTag.Safer:
                case SymmetricKeyAlgorithmTag.Aes128:
                case SymmetricKeyAlgorithmTag.Camellia128:
                    return 128;
                case SymmetricKeyAlgorithmTag.TripleDes:
                case SymmetricKeyAlgorithmTag.Aes192:
                case SymmetricKeyAlgorithmTag.Camellia192:
                    return 192;
                case SymmetricKeyAlgorithmTag.Des:
                    return 64;
                case SymmetricKeyAlgorithmTag.Aes256:
                case SymmetricKeyAlgorithmTag.Twofish:
                case SymmetricKeyAlgorithmTag.Camellia256:
                    return 256;
                default:
                    throw new PgpException( "unknown symmetric algorithm: " + algorithm );
            }
        }

        public static KeyParameter MakeKey( SymmetricKeyAlgorithmTag algorithm, byte[] keyBytes ) => ParameterUtilities.CreateKeyParameter( GetSymmetricCipherName( algorithm ), keyBytes );

        public static KeyParameter MakeRandomKey(
          SymmetricKeyAlgorithmTag algorithm,
          SecureRandom random )
        {
            byte[] numArray = new byte[(GetKeySize( algorithm ) + 7) / 8];
            random.NextBytes( numArray );
            return MakeKey( algorithm, numArray );
        }

        internal static byte[] EncodePassPhrase( char[] passPhrase, bool utf8 )
        {
            if (passPhrase == null)
                return null;
            return !utf8 ? Strings.ToByteArray( passPhrase ) : Encoding.UTF8.GetBytes( passPhrase );
        }

        public static KeyParameter MakeKeyFromPassPhrase(
          SymmetricKeyAlgorithmTag algorithm,
          S2k s2k,
          char[] passPhrase )
        {
            return DoMakeKeyFromPassPhrase( algorithm, s2k, EncodePassPhrase( passPhrase, false ), true );
        }

        public static KeyParameter MakeKeyFromPassPhraseUtf8(
          SymmetricKeyAlgorithmTag algorithm,
          S2k s2k,
          char[] passPhrase )
        {
            return DoMakeKeyFromPassPhrase( algorithm, s2k, EncodePassPhrase( passPhrase, true ), true );
        }

        public static KeyParameter MakeKeyFromPassPhraseRaw(
          SymmetricKeyAlgorithmTag algorithm,
          S2k s2k,
          byte[] rawPassPhrase )
        {
            return DoMakeKeyFromPassPhrase( algorithm, s2k, rawPassPhrase, false );
        }

        internal static KeyParameter DoMakeKeyFromPassPhrase(
          SymmetricKeyAlgorithmTag algorithm,
          S2k s2k,
          byte[] rawPassPhrase,
          bool clearPassPhrase )
        {
            int keySize = GetKeySize( algorithm );
            byte[] input = rawPassPhrase;
            byte[] numArray = new byte[(keySize + 7) / 8];
            int destinationIndex = 0;
            int num = 0;
            while (destinationIndex < numArray.Length)
            {
                IDigest digest;
                if (s2k != null)
                {
                    string digestName = GetDigestName( s2k.HashAlgorithm );
                    try
                    {
                        digest = DigestUtilities.GetDigest( digestName );
                    }
                    catch (Exception ex)
                    {
                        throw new PgpException( "can't find S2k digest", ex );
                    }
                    for (int index = 0; index != num; ++index)
                        digest.Update( 0 );
                    byte[] iv = s2k.GetIV();
                    switch (s2k.Type)
                    {
                        case 0:
                            digest.BlockUpdate( input, 0, input.Length );
                            break;
                        case 1:
                            digest.BlockUpdate( iv, 0, iv.Length );
                            digest.BlockUpdate( input, 0, input.Length );
                            break;
                        case 3:
                            long iterationCount = s2k.IterationCount;
                            digest.BlockUpdate( iv, 0, iv.Length );
                            digest.BlockUpdate( input, 0, input.Length );
                            long length1 = iterationCount - (iv.Length + input.Length);
                            while (length1 > 0L)
                            {
                                if (length1 < iv.Length)
                                {
                                    digest.BlockUpdate( iv, 0, (int)length1 );
                                    break;
                                }
                                digest.BlockUpdate( iv, 0, iv.Length );
                                long length2 = length1 - iv.Length;
                                if (length2 < input.Length)
                                {
                                    digest.BlockUpdate( input, 0, (int)length2 );
                                    length1 = 0L;
                                }
                                else
                                {
                                    digest.BlockUpdate( input, 0, input.Length );
                                    length1 = length2 - input.Length;
                                }
                            }
                            break;
                        default:
                            throw new PgpException( "unknown S2k type: " + s2k.Type );
                    }
                }
                else
                {
                    try
                    {
                        digest = DigestUtilities.GetDigest( "MD5" );
                        for (int index = 0; index != num; ++index)
                            digest.Update( 0 );
                        digest.BlockUpdate( input, 0, input.Length );
                    }
                    catch (Exception ex)
                    {
                        throw new PgpException( "can't find MD5 digest", ex );
                    }
                }
                byte[] sourceArray = DigestUtilities.DoFinal( digest );
                if (sourceArray.Length > numArray.Length - destinationIndex)
                    Array.Copy( sourceArray, 0, numArray, destinationIndex, numArray.Length - destinationIndex );
                else
                    Array.Copy( sourceArray, 0, numArray, destinationIndex, sourceArray.Length );
                destinationIndex += sourceArray.Length;
                ++num;
            }
            if (clearPassPhrase && rawPassPhrase != null)
                Array.Clear( rawPassPhrase, 0, rawPassPhrase.Length );
            return MakeKey( algorithm, numArray );
        }

        public static void WriteFileToLiteralData( Stream output, char fileType, FileInfo file )
        {
            Stream pOut = new PgpLiteralDataGenerator().Open( output, fileType, file.Name, file.Length, file.LastWriteTime );
            PipeFileContents( file, pOut, 4096 );
        }

        public static void WriteFileToLiteralData(
          Stream output,
          char fileType,
          FileInfo file,
          byte[] buffer )
        {
            Stream pOut = new PgpLiteralDataGenerator().Open( output, fileType, file.Name, file.LastWriteTime, buffer );
            PipeFileContents( file, pOut, buffer.Length );
        }

        private static void PipeFileContents( FileInfo file, Stream pOut, int bufSize )
        {
            FileStream s = file.OpenRead();
            byte[] buffer = new byte[bufSize];
            int count;
            while ((count = s.Read( buffer, 0, buffer.Length )) > 0)
                pOut.Write( buffer, 0, count );
            Platform.Dispose( pOut );
            Platform.Dispose( s );
        }

        private static bool IsPossiblyBase64( int ch ) => (ch >= 65 && ch <= 90) || (ch >= 97 && ch <= 122) || (ch >= 48 && ch <= 57) || ch == 43 || ch == 47 || ch == 13 || ch == 10;

        public static Stream GetDecoderStream( Stream inputStream )
        {
            long num1 = inputStream.CanSeek ? inputStream.Position : throw new ArgumentException( "inputStream must be seek-able", nameof( inputStream ) );
            int ch1 = inputStream.ReadByte();
            if ((ch1 & 128) != 0)
            {
                inputStream.Position = num1;
                return inputStream;
            }
            if (!IsPossiblyBase64( ch1 ))
            {
                inputStream.Position = num1;
                return new ArmoredInputStream( inputStream );
            }
            byte[] sourceArray = new byte[60];
            int num2 = 1;
            int num3 = 1;
            sourceArray[0] = (byte)ch1;
            int ch2;
            for (; num2 != 60 && (ch2 = inputStream.ReadByte()) >= 0; ++num2)
            {
                if (!IsPossiblyBase64( ch2 ))
                {
                    inputStream.Position = num1;
                    return new ArmoredInputStream( inputStream );
                }
                if (ch2 != 10 && ch2 != 13)
                    sourceArray[num3++] = (byte)ch2;
            }
            inputStream.Position = num1;
            if (num2 < 4)
                return new ArmoredInputStream( inputStream );
            byte[] numArray = new byte[8];
            Array.Copy( sourceArray, 0, numArray, 0, numArray.Length );
            bool hasHeaders = (Base64.Decode( numArray )[0] & 128) == 0;
            return new ArmoredInputStream( inputStream, hasHeaders );
        }

        internal static IWrapper CreateWrapper( SymmetricKeyAlgorithmTag encAlgorithm )
        {
            switch (encAlgorithm)
            {
                case SymmetricKeyAlgorithmTag.Aes128:
                case SymmetricKeyAlgorithmTag.Aes192:
                case SymmetricKeyAlgorithmTag.Aes256:
                    return WrapperUtilities.GetWrapper( "AESWRAP" );
                case SymmetricKeyAlgorithmTag.Camellia128:
                case SymmetricKeyAlgorithmTag.Camellia192:
                case SymmetricKeyAlgorithmTag.Camellia256:
                    return WrapperUtilities.GetWrapper( "CAMELLIAWRAP" );
                default:
                    throw new PgpException( "unknown wrap algorithm: " + encAlgorithm );
            }
        }

        internal static byte[] GenerateIV( int length, SecureRandom random )
        {
            byte[] buffer = new byte[length];
            random.NextBytes( buffer );
            return buffer;
        }

        internal static S2k GenerateS2k(
          HashAlgorithmTag hashAlgorithm,
          int s2kCount,
          SecureRandom random )
        {
            byte[] iv = GenerateIV( 8, random );
            return new S2k( hashAlgorithm, iv, s2kCount );
        }
    }
}
