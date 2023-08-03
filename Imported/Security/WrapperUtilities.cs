// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Security.WrapperUtilities
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Kisa;
using Org.BouncyCastle.Asn1.Nist;
using Org.BouncyCastle.Asn1.Ntt;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Utilities;
using System.Collections;

namespace Org.BouncyCastle.Security
{
    public sealed class WrapperUtilities
    {
        private static readonly IDictionary algorithms = Platform.CreateHashtable();

        private WrapperUtilities()
        {
        }

        static WrapperUtilities()
        {
            ((WrapperUtilities.WrapAlgorithm)Enums.GetArbitraryValue( typeof( WrapperUtilities.WrapAlgorithm ) )).ToString();
            algorithms[NistObjectIdentifiers.IdAes128Wrap.Id] = "AESWRAP";
            algorithms[NistObjectIdentifiers.IdAes192Wrap.Id] = "AESWRAP";
            algorithms[NistObjectIdentifiers.IdAes256Wrap.Id] = "AESWRAP";
            algorithms[NttObjectIdentifiers.IdCamellia128Wrap.Id] = "CAMELLIAWRAP";
            algorithms[NttObjectIdentifiers.IdCamellia192Wrap.Id] = "CAMELLIAWRAP";
            algorithms[NttObjectIdentifiers.IdCamellia256Wrap.Id] = "CAMELLIAWRAP";
            algorithms[PkcsObjectIdentifiers.IdAlgCms3DesWrap.Id] = "DESEDEWRAP";
            algorithms["TDEAWRAP"] = "DESEDEWRAP";
            algorithms[PkcsObjectIdentifiers.IdAlgCmsRC2Wrap.Id] = "RC2WRAP";
            algorithms[KisaObjectIdentifiers.IdNpkiAppCmsSeedWrap.Id] = "SEEDWRAP";
        }

        public static IWrapper GetWrapper( DerObjectIdentifier oid ) => GetWrapper( oid.Id );

        public static IWrapper GetWrapper( string algorithm )
        {
            string upperInvariant = Platform.ToUpperInvariant( algorithm );
            string s = (string)algorithms[upperInvariant] ?? upperInvariant;
            try
            {
                switch ((WrapperUtilities.WrapAlgorithm)Enums.GetEnumValue( typeof( WrapperUtilities.WrapAlgorithm ), s ))
                {
                    case WrapAlgorithm.AESWRAP:
                        return new AesWrapEngine();
                    case WrapAlgorithm.CAMELLIAWRAP:
                        return new CamelliaWrapEngine();
                    case WrapAlgorithm.DESEDEWRAP:
                        return new DesEdeWrapEngine();
                    case WrapAlgorithm.RC2WRAP:
                        return new RC2WrapEngine();
                    case WrapAlgorithm.SEEDWRAP:
                        return new SeedWrapEngine();
                    case WrapAlgorithm.DESEDERFC3211WRAP:
                        return new Rfc3211WrapEngine( new DesEdeEngine() );
                    case WrapAlgorithm.AESRFC3211WRAP:
                        return new Rfc3211WrapEngine( new AesFastEngine() );
                    case WrapAlgorithm.CAMELLIARFC3211WRAP:
                        return new Rfc3211WrapEngine( new CamelliaEngine() );
                }
            }
            catch (ArgumentException ex)
            {
            }
            return new WrapperUtilities.BufferedCipherWrapper( CipherUtilities.GetCipher( algorithm ) ?? throw new SecurityUtilityException( "Wrapper " + algorithm + " not recognised." ) );
        }

        public static string GetAlgorithmName( DerObjectIdentifier oid ) => (string)algorithms[oid.Id];

        private enum WrapAlgorithm
        {
            AESWRAP,
            CAMELLIAWRAP,
            DESEDEWRAP,
            RC2WRAP,
            SEEDWRAP,
            DESEDERFC3211WRAP,
            AESRFC3211WRAP,
            CAMELLIARFC3211WRAP,
        }

        private class BufferedCipherWrapper : IWrapper
        {
            private readonly IBufferedCipher cipher;
            private bool forWrapping;

            public BufferedCipherWrapper( IBufferedCipher cipher ) => this.cipher = cipher;

            public string AlgorithmName => this.cipher.AlgorithmName;

            public void Init( bool forWrapping, ICipherParameters parameters )
            {
                this.forWrapping = forWrapping;
                this.cipher.Init( forWrapping, parameters );
            }

            public byte[] Wrap( byte[] input, int inOff, int length )
            {
                if (!this.forWrapping)
                    throw new InvalidOperationException( "Not initialised for wrapping" );
                return this.cipher.DoFinal( input, inOff, length );
            }

            public byte[] Unwrap( byte[] input, int inOff, int length )
            {
                if (this.forWrapping)
                    throw new InvalidOperationException( "Not initialised for unwrapping" );
                return this.cipher.DoFinal( input, inOff, length );
            }
        }
    }
}
