﻿// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Pkcs.EncryptedPrivateKeyInfoFactory
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Security;
using System;

namespace Org.BouncyCastle.Pkcs
{
    public sealed class EncryptedPrivateKeyInfoFactory
    {
        private EncryptedPrivateKeyInfoFactory()
        {
        }

        public static EncryptedPrivateKeyInfo CreateEncryptedPrivateKeyInfo(
          DerObjectIdentifier algorithm,
          char[] passPhrase,
          byte[] salt,
          int iterationCount,
          AsymmetricKeyParameter key )
        {
            return CreateEncryptedPrivateKeyInfo( algorithm.Id, passPhrase, salt, iterationCount, PrivateKeyInfoFactory.CreatePrivateKeyInfo( key ) );
        }

        public static EncryptedPrivateKeyInfo CreateEncryptedPrivateKeyInfo(
          string algorithm,
          char[] passPhrase,
          byte[] salt,
          int iterationCount,
          AsymmetricKeyParameter key )
        {
            return CreateEncryptedPrivateKeyInfo( algorithm, passPhrase, salt, iterationCount, PrivateKeyInfoFactory.CreatePrivateKeyInfo( key ) );
        }

        public static EncryptedPrivateKeyInfo CreateEncryptedPrivateKeyInfo(
          string algorithm,
          char[] passPhrase,
          byte[] salt,
          int iterationCount,
          PrivateKeyInfo keyInfo )
        {
            if (!(PbeUtilities.CreateEngine( algorithm ) is IBufferedCipher engine))
                throw new Exception( "Unknown encryption algorithm: " + algorithm );
            Asn1Encodable algorithmParameters = PbeUtilities.GenerateAlgorithmParameters( algorithm, salt, iterationCount );
            ICipherParameters cipherParameters = PbeUtilities.GenerateCipherParameters( algorithm, passPhrase, algorithmParameters );
            engine.Init( true, cipherParameters );
            byte[] encoding = engine.DoFinal( keyInfo.GetEncoded() );
            return new EncryptedPrivateKeyInfo( new AlgorithmIdentifier( PbeUtilities.GetObjectIdentifier( algorithm ), algorithmParameters ), encoding );
        }
    }
}