// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Prng.Drbg.DrbgUtilities
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System.Collections;

namespace Org.BouncyCastle.Crypto.Prng.Drbg
{
    internal class DrbgUtilities
    {
        private static readonly IDictionary maxSecurityStrengths = Platform.CreateHashtable();

        static DrbgUtilities()
        {
            maxSecurityStrengths.Add( "SHA-1", 128 );
            maxSecurityStrengths.Add( "SHA-224", 192 );
            maxSecurityStrengths.Add( "SHA-256", 256 );
            maxSecurityStrengths.Add( "SHA-384", 256 );
            maxSecurityStrengths.Add( "SHA-512", 256 );
            maxSecurityStrengths.Add( "SHA-512/224", 192 );
            maxSecurityStrengths.Add( "SHA-512/256", 256 );
        }

        internal static int GetMaxSecurityStrength( IDigest d ) => (int)maxSecurityStrengths[d.AlgorithmName];

        internal static int GetMaxSecurityStrength( IMac m )
        {
            string algorithmName = m.AlgorithmName;
            return (int)maxSecurityStrengths[algorithmName.Substring( 0, algorithmName.IndexOf( "/" ) )];
        }

        internal static byte[] HashDF( IDigest digest, byte[] seedMaterial, int seedLength )
        {
            byte[] destinationArray = new byte[(seedLength + 7) / 8];
            int num1 = destinationArray.Length / digest.GetDigestSize();
            int input = 1;
            byte[] numArray = new byte[digest.GetDigestSize()];
            for (int index = 0; index <= num1; ++index)
            {
                digest.Update( (byte)input );
                digest.Update( (byte)(seedLength >> 24) );
                digest.Update( (byte)(seedLength >> 16) );
                digest.Update( (byte)(seedLength >> 8) );
                digest.Update( (byte)seedLength );
                digest.BlockUpdate( seedMaterial, 0, seedMaterial.Length );
                digest.DoFinal( numArray, 0 );
                int length = destinationArray.Length - (index * numArray.Length) > numArray.Length ? numArray.Length : destinationArray.Length - (index * numArray.Length);
                Array.Copy( numArray, 0, destinationArray, index * numArray.Length, length );
                ++input;
            }
            if (seedLength % 8 != 0)
            {
                int num2 = 8 - (seedLength % 8);
                uint num3 = 0;
                for (int index = 0; index != destinationArray.Length; ++index)
                {
                    uint num4 = destinationArray[index];
                    destinationArray[index] = (byte)((num4 >> num2) | (num3 << (8 - num2)));
                    num3 = num4;
                }
            }
            return destinationArray;
        }

        internal static bool IsTooLarge( byte[] bytes, int maxBytes ) => bytes != null && bytes.Length > maxBytes;
    }
}
