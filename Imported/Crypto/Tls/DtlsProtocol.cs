// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.DtlsProtocol
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using System;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
    public abstract class DtlsProtocol
    {
        protected readonly SecureRandom mSecureRandom;

        protected DtlsProtocol( SecureRandom secureRandom ) => this.mSecureRandom = secureRandom != null ? secureRandom : throw new ArgumentNullException( nameof( secureRandom ) );

        protected virtual void ProcessFinished( byte[] body, byte[] expected_verify_data )
        {
            MemoryStream memoryStream = new MemoryStream( body, false );
            byte[] b = TlsUtilities.ReadFully( expected_verify_data.Length, memoryStream );
            TlsProtocol.AssertEmpty( memoryStream );
            if (!Arrays.ConstantTimeAreEqual( expected_verify_data, b ))
                throw new TlsFatalAlert( 40 );
        }

        internal static void ApplyMaxFragmentLengthExtension(
          DtlsRecordLayer recordLayer,
          short maxFragmentLength )
        {
            if (maxFragmentLength < 0)
                return;
            if (!MaxFragmentLength.IsValid( (byte)maxFragmentLength ))
                throw new TlsFatalAlert( 80 );
            int plaintextLimit = 1 << (8 + maxFragmentLength);
            recordLayer.SetPlaintextLimit( plaintextLimit );
        }

        protected static short EvaluateMaxFragmentLengthExtension(
          bool resumedSession,
          IDictionary clientExtensions,
          IDictionary serverExtensions,
          byte alertDescription )
        {
            short fragmentLengthExtension = TlsExtensionsUtilities.GetMaxFragmentLengthExtension( serverExtensions );
            if (fragmentLengthExtension >= 0 && (!MaxFragmentLength.IsValid( (byte)fragmentLengthExtension ) || (!resumedSession && fragmentLengthExtension != TlsExtensionsUtilities.GetMaxFragmentLengthExtension( clientExtensions ))))
                throw new TlsFatalAlert( alertDescription );
            return fragmentLengthExtension;
        }

        protected static byte[] GenerateCertificate( Certificate certificate )
        {
            MemoryStream output = new MemoryStream();
            certificate.Encode( output );
            return output.ToArray();
        }

        protected static byte[] GenerateSupplementalData( IList supplementalData )
        {
            MemoryStream output = new MemoryStream();
            TlsProtocol.WriteSupplementalData( output, supplementalData );
            return output.ToArray();
        }

        protected static void ValidateSelectedCipherSuite(
          int selectedCipherSuite,
          byte alertDescription )
        {
            switch (TlsUtilities.GetEncryptionAlgorithm( selectedCipherSuite ))
            {
                case 1:
                case 2:
                    throw new TlsFatalAlert( alertDescription );
            }
        }
    }
}
