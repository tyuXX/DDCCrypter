// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.OpenSsl.PemUtilities
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using System;

namespace Org.BouncyCastle.OpenSsl
{
    internal sealed class PemUtilities
    {
        static PemUtilities()
        {
            ((PemUtilities.PemBaseAlg)Enums.GetArbitraryValue( typeof( PemUtilities.PemBaseAlg ) )).ToString();
            ((PemUtilities.PemMode)Enums.GetArbitraryValue( typeof( PemUtilities.PemMode ) )).ToString();
        }

        private static void ParseDekAlgName(
          string dekAlgName,
          out PemUtilities.PemBaseAlg baseAlg,
          out PemUtilities.PemMode mode )
        {
            try
            {
                mode = PemMode.ECB;
                if (dekAlgName == "DES-EDE" || dekAlgName == "DES-EDE3")
                {
                    baseAlg = (PemUtilities.PemBaseAlg)Enums.GetEnumValue( typeof( PemUtilities.PemBaseAlg ), dekAlgName );
                    return;
                }
                int length = dekAlgName.LastIndexOf( '-' );
                if (length >= 0)
                {
                    baseAlg = (PemUtilities.PemBaseAlg)Enums.GetEnumValue( typeof( PemUtilities.PemBaseAlg ), dekAlgName.Substring( 0, length ) );
                    mode = (PemUtilities.PemMode)Enums.GetEnumValue( typeof( PemUtilities.PemMode ), dekAlgName.Substring( length + 1 ) );
                    return;
                }
            }
            catch (ArgumentException ex)
            {
            }
            throw new EncryptionException( "Unknown DEK algorithm: " + dekAlgName );
        }

        internal static byte[] Crypt(
          bool encrypt,
          byte[] bytes,
          char[] password,
          string dekAlgName,
          byte[] iv )
        {
            PemUtilities.PemBaseAlg baseAlg;
            PemUtilities.PemMode mode;
            ParseDekAlgName( dekAlgName, out baseAlg, out mode );
            string str1;
            switch (mode)
            {
                case PemMode.CBC:
                case PemMode.ECB:
                    str1 = "PKCS5Padding";
                    break;
                case PemMode.CFB:
                case PemMode.OFB:
                    str1 = "NoPadding";
                    break;
                default:
                    throw new EncryptionException( "Unknown DEK algorithm: " + dekAlgName );
            }
            byte[] numArray = iv;
            string str2;
            switch (baseAlg)
            {
                case PemBaseAlg.AES_128:
                case PemBaseAlg.AES_192:
                case PemBaseAlg.AES_256:
                    str2 = "AES";
                    if (numArray.Length > 8)
                    {
                        numArray = new byte[8];
                        Array.Copy( iv, 0, numArray, 0, numArray.Length );
                        break;
                    }
                    break;
                case PemBaseAlg.BF:
                    str2 = "BLOWFISH";
                    break;
                case PemBaseAlg.DES:
                    str2 = "DES";
                    break;
                case PemBaseAlg.DES_EDE:
                case PemBaseAlg.DES_EDE3:
                    str2 = "DESede";
                    break;
                case PemBaseAlg.RC2:
                case PemBaseAlg.RC2_40:
                case PemBaseAlg.RC2_64:
                    str2 = "RC2";
                    break;
                default:
                    throw new EncryptionException( "Unknown DEK algorithm: " + dekAlgName );
            }
            IBufferedCipher cipher = CipherUtilities.GetCipher( str2 + "/" + mode + "/" + str1 );
            ICipherParameters parameters = GetCipherParameters( password, baseAlg, numArray );
            if (mode != PemMode.ECB)
                parameters = new ParametersWithIV( parameters, iv );
            cipher.Init( encrypt, parameters );
            return cipher.DoFinal( bytes );
        }

        private static ICipherParameters GetCipherParameters(
          char[] password,
          PemUtilities.PemBaseAlg baseAlg,
          byte[] salt )
        {
            int keySize;
            string algorithm;
            switch (baseAlg)
            {
                case PemBaseAlg.AES_128:
                    keySize = 128;
                    algorithm = "AES128";
                    break;
                case PemBaseAlg.AES_192:
                    keySize = 192;
                    algorithm = "AES192";
                    break;
                case PemBaseAlg.AES_256:
                    keySize = 256;
                    algorithm = "AES256";
                    break;
                case PemBaseAlg.BF:
                    keySize = 128;
                    algorithm = "BLOWFISH";
                    break;
                case PemBaseAlg.DES:
                    keySize = 64;
                    algorithm = "DES";
                    break;
                case PemBaseAlg.DES_EDE:
                    keySize = 128;
                    algorithm = "DESEDE";
                    break;
                case PemBaseAlg.DES_EDE3:
                    keySize = 192;
                    algorithm = "DESEDE3";
                    break;
                case PemBaseAlg.RC2:
                    keySize = 128;
                    algorithm = "RC2";
                    break;
                case PemBaseAlg.RC2_40:
                    keySize = 40;
                    algorithm = "RC2";
                    break;
                case PemBaseAlg.RC2_64:
                    keySize = 64;
                    algorithm = "RC2";
                    break;
                default:
                    return null;
            }
            OpenSslPbeParametersGenerator parametersGenerator = new OpenSslPbeParametersGenerator();
            parametersGenerator.Init( PbeParametersGenerator.Pkcs5PasswordToBytes( password ), salt );
            return parametersGenerator.GenerateDerivedParameters( algorithm, keySize );
        }

        private enum PemBaseAlg
        {
            AES_128,
            AES_192,
            AES_256,
            BF,
            DES,
            DES_EDE,
            DES_EDE3,
            RC2,
            RC2_40,
            RC2_64,
        }

        private enum PemMode
        {
            CBC,
            CFB,
            ECB,
            OFB,
        }
    }
}
