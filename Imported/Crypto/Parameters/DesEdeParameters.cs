// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Parameters.DesEdeParameters
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

namespace Org.BouncyCastle.Crypto.Parameters
{
    public class DesEdeParameters : DesParameters
    {
        public const int DesEdeKeyLength = 24;

        private static byte[] FixKey( byte[] key, int keyOff, int keyLen )
        {
            byte[] numArray = new byte[24];
            switch (keyLen)
            {
                case 16:
                    Array.Copy( key, keyOff, numArray, 0, 16 );
                    Array.Copy( key, keyOff, numArray, 16, 8 );
                    break;
                case 24:
                    Array.Copy( key, keyOff, numArray, 0, 24 );
                    break;
                default:
                    throw new ArgumentException( "Bad length for DESede key: " + keyLen, nameof( keyLen ) );
            }
            return !IsWeakKey( numArray ) ? numArray : throw new ArgumentException( "attempt to create weak DESede key" );
        }

        public DesEdeParameters( byte[] key )
          : base( FixKey( key, 0, key.Length ) )
        {
        }

        public DesEdeParameters( byte[] key, int keyOff, int keyLen )
          : base( FixKey( key, keyOff, keyLen ) )
        {
        }

        public static bool IsWeakKey( byte[] key, int offset, int length )
        {
            for (int offset1 = offset; offset1 < length; offset1 += 8)
            {
                if (DesParameters.IsWeakKey( key, offset1 ))
                    return true;
            }
            return false;
        }

        public new static bool IsWeakKey( byte[] key, int offset ) => IsWeakKey( key, offset, key.Length - offset );

        public new static bool IsWeakKey( byte[] key ) => IsWeakKey( key, 0, key.Length );

        public static bool IsRealEdeKey( byte[] key, int offset ) => key.Length != 16 ? IsReal3Key( key, offset ) : IsReal2Key( key, offset );

        public static bool IsReal2Key( byte[] key, int offset )
        {
            bool flag = false;
            for (int index = offset; index != offset + 8; ++index)
                flag |= key[index] != key[index + 8];
            return flag;
        }

        public static bool IsReal3Key( byte[] key, int offset )
        {
            bool flag1 = false;
            bool flag2 = false;
            bool flag3 = false;
            for (int index = offset; index != offset + 8; ++index)
            {
                flag1 |= key[index] != key[index + 8];
                flag2 |= key[index] != key[index + 16];
                flag3 |= key[index + 8] != key[index + 16];
            }
            return flag1 && flag2 && flag3;
        }
    }
}
