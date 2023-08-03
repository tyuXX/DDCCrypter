// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.PbeParametersGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;
using System.Text;

namespace Org.BouncyCastle.Crypto
{
    public abstract class PbeParametersGenerator
    {
        protected byte[] mPassword;
        protected byte[] mSalt;
        protected int mIterationCount;

        public virtual void Init( byte[] password, byte[] salt, int iterationCount )
        {
            if (password == null)
                throw new ArgumentNullException( nameof( password ) );
            if (salt == null)
                throw new ArgumentNullException( nameof( salt ) );
            this.mPassword = Arrays.Clone( password );
            this.mSalt = Arrays.Clone( salt );
            this.mIterationCount = iterationCount;
        }

        public virtual byte[] Password => Arrays.Clone( this.mPassword );

        [Obsolete( "Use 'Password' property" )]
        public byte[] GetPassword() => this.Password;

        public virtual byte[] Salt => Arrays.Clone( this.mSalt );

        [Obsolete( "Use 'Salt' property" )]
        public byte[] GetSalt() => this.Salt;

        public virtual int IterationCount => this.mIterationCount;

        [Obsolete( "Use version with 'algorithm' parameter" )]
        public abstract ICipherParameters GenerateDerivedParameters( int keySize );

        public abstract ICipherParameters GenerateDerivedParameters( string algorithm, int keySize );

        [Obsolete( "Use version with 'algorithm' parameter" )]
        public abstract ICipherParameters GenerateDerivedParameters( int keySize, int ivSize );

        public abstract ICipherParameters GenerateDerivedParameters(
          string algorithm,
          int keySize,
          int ivSize );

        public abstract ICipherParameters GenerateDerivedMacParameters( int keySize );

        public static byte[] Pkcs5PasswordToBytes( char[] password ) => password == null ? new byte[0] : Strings.ToByteArray( password );

        [Obsolete( "Use version taking 'char[]' instead" )]
        public static byte[] Pkcs5PasswordToBytes( string password ) => password == null ? new byte[0] : Strings.ToByteArray( password );

        public static byte[] Pkcs5PasswordToUtf8Bytes( char[] password ) => password == null ? new byte[0] : Encoding.UTF8.GetBytes( password );

        [Obsolete( "Use version taking 'char[]' instead" )]
        public static byte[] Pkcs5PasswordToUtf8Bytes( string password ) => password == null ? new byte[0] : Encoding.UTF8.GetBytes( password );

        public static byte[] Pkcs12PasswordToBytes( char[] password ) => Pkcs12PasswordToBytes( password, false );

        public static byte[] Pkcs12PasswordToBytes( char[] password, bool wrongPkcs12Zero )
        {
            if (password == null || password.Length < 1)
                return new byte[wrongPkcs12Zero ? 2 : 0];
            byte[] bytes = new byte[(password.Length + 1) * 2];
            Encoding.BigEndianUnicode.GetBytes( password, 0, password.Length, bytes, 0 );
            return bytes;
        }
    }
}
