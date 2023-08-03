// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.TlsUtilities
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Nist;
using Org.BouncyCastle.Asn1.Pkcs;
using Org.BouncyCastle.Asn1.X509;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Date;
using Org.BouncyCastle.Utilities.IO;
using System;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
    public abstract class TlsUtilities
    {
        public static readonly byte[] EmptyBytes = new byte[0];
        public static readonly short[] EmptyShorts = new short[0];
        public static readonly int[] EmptyInts = new int[0];
        public static readonly long[] EmptyLongs = new long[0];
        internal static readonly byte[] SSL_CLIENT = new byte[4]
        {
       67,
       76,
       78,
       84
        };
        internal static readonly byte[] SSL_SERVER = new byte[4]
        {
       83,
       82,
       86,
       82
        };
        internal static readonly byte[][] SSL3_CONST = GenSsl3Const();

        public static void CheckUint8( int i )
        {
            if (!IsValidUint8( i ))
                throw new TlsFatalAlert( 80 );
        }

        public static void CheckUint8( long i )
        {
            if (!IsValidUint8( i ))
                throw new TlsFatalAlert( 80 );
        }

        public static void CheckUint16( int i )
        {
            if (!IsValidUint16( i ))
                throw new TlsFatalAlert( 80 );
        }

        public static void CheckUint16( long i )
        {
            if (!IsValidUint16( i ))
                throw new TlsFatalAlert( 80 );
        }

        public static void CheckUint24( int i )
        {
            if (!IsValidUint24( i ))
                throw new TlsFatalAlert( 80 );
        }

        public static void CheckUint24( long i )
        {
            if (!IsValidUint24( i ))
                throw new TlsFatalAlert( 80 );
        }

        public static void CheckUint32( long i )
        {
            if (!IsValidUint32( i ))
                throw new TlsFatalAlert( 80 );
        }

        public static void CheckUint48( long i )
        {
            if (!IsValidUint48( i ))
                throw new TlsFatalAlert( 80 );
        }

        public static void CheckUint64( long i )
        {
            if (!IsValidUint64( i ))
                throw new TlsFatalAlert( 80 );
        }

        public static bool IsValidUint8( int i ) => (i & byte.MaxValue) == i;

        public static bool IsValidUint8( long i ) => (i & byte.MaxValue) == i;

        public static bool IsValidUint16( int i ) => (i & ushort.MaxValue) == i;

        public static bool IsValidUint16( long i ) => (i & ushort.MaxValue) == i;

        public static bool IsValidUint24( int i ) => (i & 16777215) == i;

        public static bool IsValidUint24( long i ) => (i & 16777215L) == i;

        public static bool IsValidUint32( long i ) => (i & uint.MaxValue) == i;

        public static bool IsValidUint48( long i ) => (i & 281474976710655L) == i;

        public static bool IsValidUint64( long i ) => true;

        public static bool IsSsl( TlsContext context ) => context.ServerVersion.IsSsl;

        public static bool IsTlsV11( ProtocolVersion version ) => ProtocolVersion.TLSv11.IsEqualOrEarlierVersionOf( version.GetEquivalentTLSVersion() );

        public static bool IsTlsV11( TlsContext context ) => IsTlsV11( context.ServerVersion );

        public static bool IsTlsV12( ProtocolVersion version ) => ProtocolVersion.TLSv12.IsEqualOrEarlierVersionOf( version.GetEquivalentTLSVersion() );

        public static bool IsTlsV12( TlsContext context ) => IsTlsV12( context.ServerVersion );

        public static void WriteUint8( byte i, Stream output ) => output.WriteByte( i );

        public static void WriteUint8( byte i, byte[] buf, int offset ) => buf[offset] = i;

        public static void WriteUint16( int i, Stream output )
        {
            output.WriteByte( (byte)(i >> 8) );
            output.WriteByte( (byte)i );
        }

        public static void WriteUint16( int i, byte[] buf, int offset )
        {
            buf[offset] = (byte)(i >> 8);
            buf[offset + 1] = (byte)i;
        }

        public static void WriteUint24( int i, Stream output )
        {
            output.WriteByte( (byte)(i >> 16) );
            output.WriteByte( (byte)(i >> 8) );
            output.WriteByte( (byte)i );
        }

        public static void WriteUint24( int i, byte[] buf, int offset )
        {
            buf[offset] = (byte)(i >> 16);
            buf[offset + 1] = (byte)(i >> 8);
            buf[offset + 2] = (byte)i;
        }

        public static void WriteUint32( long i, Stream output )
        {
            output.WriteByte( (byte)(i >> 24) );
            output.WriteByte( (byte)(i >> 16) );
            output.WriteByte( (byte)(i >> 8) );
            output.WriteByte( (byte)i );
        }

        public static void WriteUint32( long i, byte[] buf, int offset )
        {
            buf[offset] = (byte)(i >> 24);
            buf[offset + 1] = (byte)(i >> 16);
            buf[offset + 2] = (byte)(i >> 8);
            buf[offset + 3] = (byte)i;
        }

        public static void WriteUint48( long i, Stream output )
        {
            output.WriteByte( (byte)(i >> 40) );
            output.WriteByte( (byte)(i >> 32) );
            output.WriteByte( (byte)(i >> 24) );
            output.WriteByte( (byte)(i >> 16) );
            output.WriteByte( (byte)(i >> 8) );
            output.WriteByte( (byte)i );
        }

        public static void WriteUint48( long i, byte[] buf, int offset )
        {
            buf[offset] = (byte)(i >> 40);
            buf[offset + 1] = (byte)(i >> 32);
            buf[offset + 2] = (byte)(i >> 24);
            buf[offset + 3] = (byte)(i >> 16);
            buf[offset + 4] = (byte)(i >> 8);
            buf[offset + 5] = (byte)i;
        }

        public static void WriteUint64( long i, Stream output )
        {
            output.WriteByte( (byte)(i >> 56) );
            output.WriteByte( (byte)(i >> 48) );
            output.WriteByte( (byte)(i >> 40) );
            output.WriteByte( (byte)(i >> 32) );
            output.WriteByte( (byte)(i >> 24) );
            output.WriteByte( (byte)(i >> 16) );
            output.WriteByte( (byte)(i >> 8) );
            output.WriteByte( (byte)i );
        }

        public static void WriteUint64( long i, byte[] buf, int offset )
        {
            buf[offset] = (byte)(i >> 56);
            buf[offset + 1] = (byte)(i >> 48);
            buf[offset + 2] = (byte)(i >> 40);
            buf[offset + 3] = (byte)(i >> 32);
            buf[offset + 4] = (byte)(i >> 24);
            buf[offset + 5] = (byte)(i >> 16);
            buf[offset + 6] = (byte)(i >> 8);
            buf[offset + 7] = (byte)i;
        }

        public static void WriteOpaque8( byte[] buf, Stream output )
        {
            WriteUint8( (byte)buf.Length, output );
            output.Write( buf, 0, buf.Length );
        }

        public static void WriteOpaque16( byte[] buf, Stream output )
        {
            WriteUint16( buf.Length, output );
            output.Write( buf, 0, buf.Length );
        }

        public static void WriteOpaque24( byte[] buf, Stream output )
        {
            WriteUint24( buf.Length, output );
            output.Write( buf, 0, buf.Length );
        }

        public static void WriteUint8Array( byte[] uints, Stream output ) => output.Write( uints, 0, uints.Length );

        public static void WriteUint8Array( byte[] uints, byte[] buf, int offset )
        {
            for (int index = 0; index < uints.Length; ++index)
            {
                WriteUint8( uints[index], buf, offset );
                ++offset;
            }
        }

        public static void WriteUint8ArrayWithUint8Length( byte[] uints, Stream output )
        {
            CheckUint8( uints.Length );
            WriteUint8( (byte)uints.Length, output );
            WriteUint8Array( uints, output );
        }

        public static void WriteUint8ArrayWithUint8Length( byte[] uints, byte[] buf, int offset )
        {
            CheckUint8( uints.Length );
            WriteUint8( (byte)uints.Length, buf, offset );
            WriteUint8Array( uints, buf, offset + 1 );
        }

        public static void WriteUint16Array( int[] uints, Stream output )
        {
            for (int index = 0; index < uints.Length; ++index)
                WriteUint16( uints[index], output );
        }

        public static void WriteUint16Array( int[] uints, byte[] buf, int offset )
        {
            for (int index = 0; index < uints.Length; ++index)
            {
                WriteUint16( uints[index], buf, offset );
                offset += 2;
            }
        }

        public static void WriteUint16ArrayWithUint16Length( int[] uints, Stream output )
        {
            int i = 2 * uints.Length;
            CheckUint16( i );
            WriteUint16( i, output );
            WriteUint16Array( uints, output );
        }

        public static void WriteUint16ArrayWithUint16Length( int[] uints, byte[] buf, int offset )
        {
            int i = 2 * uints.Length;
            CheckUint16( i );
            WriteUint16( i, buf, offset );
            WriteUint16Array( uints, buf, offset + 2 );
        }

        public static byte[] EncodeOpaque8( byte[] buf )
        {
            CheckUint8( buf.Length );
            return Arrays.Prepend( buf, (byte)buf.Length );
        }

        public static byte[] EncodeUint8ArrayWithUint8Length( byte[] uints )
        {
            byte[] buf = new byte[1 + uints.Length];
            WriteUint8ArrayWithUint8Length( uints, buf, 0 );
            return buf;
        }

        public static byte[] EncodeUint16ArrayWithUint16Length( int[] uints )
        {
            byte[] buf = new byte[2 + (2 * uints.Length)];
            WriteUint16ArrayWithUint16Length( uints, buf, 0 );
            return buf;
        }

        public static byte ReadUint8( Stream input )
        {
            int num = input.ReadByte();
            return num >= 0 ? (byte)num : throw new EndOfStreamException();
        }

        public static byte ReadUint8( byte[] buf, int offset ) => buf[offset];

        public static int ReadUint16( Stream input )
        {
            int num1 = input.ReadByte();
            int num2 = input.ReadByte();
            if ((num1 | num2) < 0)
                throw new EndOfStreamException();
            return (num1 << 8) | num2;
        }

        public static int ReadUint16( byte[] buf, int offset ) => (int)(((uint)buf[offset] << 8) | buf[++offset]);

        public static int ReadUint24( Stream input )
        {
            int num1 = input.ReadByte();
            int num2 = input.ReadByte();
            int num3 = input.ReadByte();
            if ((num1 | num2 | num3) < 0)
                throw new EndOfStreamException();
            return (num1 << 16) | (num2 << 8) | num3;
        }

        public static int ReadUint24( byte[] buf, int offset ) => (int)(((uint)buf[offset] << 16) | ((uint)buf[++offset] << 8) | buf[++offset]);

        public static long ReadUint32( Stream input )
        {
            int num1 = input.ReadByte();
            int num2 = input.ReadByte();
            int num3 = input.ReadByte();
            int num4 = input.ReadByte();
            if (num4 < 0)
                throw new EndOfStreamException();
            return (uint)((num1 << 24) | (num2 << 16) | (num3 << 8) | num4);
        }

        public static long ReadUint32( byte[] buf, int offset ) => ((uint)buf[offset] << 24) | ((uint)buf[++offset] << 16) | ((uint)buf[++offset] << 8) | buf[++offset];

        public static long ReadUint48( Stream input ) => ((ReadUint24( input ) & uint.MaxValue) << 24) | (ReadUint24( input ) & uint.MaxValue);

        public static long ReadUint48( byte[] buf, int offset ) => ((ReadUint24( buf, offset ) & uint.MaxValue) << 24) | (ReadUint24( buf, offset + 3 ) & uint.MaxValue);

        public static byte[] ReadAllOrNothing( int length, Stream input )
        {
            if (length < 1)
                return EmptyBytes;
            byte[] buf = new byte[length];
            int num = Streams.ReadFully( input, buf );
            if (num == 0)
                return null;
            if (num != length)
                throw new EndOfStreamException();
            return buf;
        }

        public static byte[] ReadFully( int length, Stream input )
        {
            if (length < 1)
                return EmptyBytes;
            byte[] buf = new byte[length];
            if (length != Streams.ReadFully( input, buf ))
                throw new EndOfStreamException();
            return buf;
        }

        public static void ReadFully( byte[] buf, Stream input )
        {
            if (Streams.ReadFully( input, buf, 0, buf.Length ) < buf.Length)
                throw new EndOfStreamException();
        }

        public static byte[] ReadOpaque8( Stream input )
        {
            byte[] buf = new byte[(ReadUint8( input ))];
            ReadFully( buf, input );
            return buf;
        }

        public static byte[] ReadOpaque16( Stream input )
        {
            byte[] buf = new byte[ReadUint16( input )];
            ReadFully( buf, input );
            return buf;
        }

        public static byte[] ReadOpaque24( Stream input ) => ReadFully( ReadUint24( input ), input );

        public static byte[] ReadUint8Array( int count, Stream input )
        {
            byte[] numArray = new byte[count];
            for (int index = 0; index < count; ++index)
                numArray[index] = ReadUint8( input );
            return numArray;
        }

        public static int[] ReadUint16Array( int count, Stream input )
        {
            int[] numArray = new int[count];
            for (int index = 0; index < count; ++index)
                numArray[index] = ReadUint16( input );
            return numArray;
        }

        public static ProtocolVersion ReadVersion( byte[] buf, int offset ) => ProtocolVersion.Get( buf[offset], buf[offset + 1] );

        public static ProtocolVersion ReadVersion( Stream input )
        {
            int major = input.ReadByte();
            int minor = input.ReadByte();
            return minor >= 0 ? ProtocolVersion.Get( major, minor ) : throw new EndOfStreamException();
        }

        public static int ReadVersionRaw( byte[] buf, int offset ) => (buf[offset] << 8) | buf[offset + 1];

        public static int ReadVersionRaw( Stream input )
        {
            int num1 = input.ReadByte();
            int num2 = input.ReadByte();
            if (num2 < 0)
                throw new EndOfStreamException();
            return (num1 << 8) | num2;
        }

        public static Asn1Object ReadAsn1Object( byte[] encoding )
        {
            MemoryStream inputStream = new MemoryStream( encoding, false );
            Asn1Object asn1Object = new Asn1InputStream( inputStream, encoding.Length ).ReadObject();
            if (asn1Object == null)
                throw new TlsFatalAlert( 50 );
            if (inputStream.Position != inputStream.Length)
                throw new TlsFatalAlert( 50 );
            return asn1Object;
        }

        public static Asn1Object ReadDerObject( byte[] encoding )
        {
            Asn1Object asn1Object = ReadAsn1Object( encoding );
            return Arrays.AreEqual( asn1Object.GetEncoded( "DER" ), encoding ) ? asn1Object : throw new TlsFatalAlert( 50 );
        }

        public static void WriteGmtUnixTime( byte[] buf, int offset )
        {
            int num = (int)(DateTimeUtilities.CurrentUnixMs() / 1000L);
            buf[offset] = (byte)(num >> 24);
            buf[offset + 1] = (byte)(num >> 16);
            buf[offset + 2] = (byte)(num >> 8);
            buf[offset + 3] = (byte)num;
        }

        public static void WriteVersion( ProtocolVersion version, Stream output )
        {
            output.WriteByte( (byte)version.MajorVersion );
            output.WriteByte( (byte)version.MinorVersion );
        }

        public static void WriteVersion( ProtocolVersion version, byte[] buf, int offset )
        {
            buf[offset] = (byte)version.MajorVersion;
            buf[offset + 1] = (byte)version.MinorVersion;
        }

        public static IList GetDefaultDssSignatureAlgorithms() => VectorOfOne( new SignatureAndHashAlgorithm( 2, 2 ) );

        public static IList GetDefaultECDsaSignatureAlgorithms() => VectorOfOne( new SignatureAndHashAlgorithm( 2, 3 ) );

        public static IList GetDefaultRsaSignatureAlgorithms() => VectorOfOne( new SignatureAndHashAlgorithm( 2, 1 ) );

        public static byte[] GetExtensionData( IDictionary extensions, int extensionType ) => extensions != null ? (byte[])extensions[extensionType] : null;

        public static IList GetDefaultSupportedSignatureAlgorithms()
        {
            byte[] numArray1 = new byte[5]
            {
         2,
         3,
         4,
         5,
         6
            };
            byte[] numArray2 = new byte[3]
            {
         1,
         2,
         3
            };
            IList arrayList = Platform.CreateArrayList();
            for (int index1 = 0; index1 < numArray2.Length; ++index1)
            {
                for (int index2 = 0; index2 < numArray1.Length; ++index2)
                    arrayList.Add( new SignatureAndHashAlgorithm( numArray1[index2], numArray2[index1] ) );
            }
            return arrayList;
        }

        public static SignatureAndHashAlgorithm GetSignatureAndHashAlgorithm(
          TlsContext context,
          TlsSignerCredentials signerCredentials )
        {
            SignatureAndHashAlgorithm andHashAlgorithm = null;
            if (IsTlsV12( context ))
            {
                andHashAlgorithm = signerCredentials.SignatureAndHashAlgorithm;
                if (andHashAlgorithm == null)
                    throw new TlsFatalAlert( 80 );
            }
            return andHashAlgorithm;
        }

        public static bool HasExpectedEmptyExtensionData(
          IDictionary extensions,
          int extensionType,
          byte alertDescription )
        {
            byte[] extensionData = GetExtensionData( extensions, extensionType );
            if (extensionData == null)
                return false;
            if (extensionData.Length != 0)
                throw new TlsFatalAlert( alertDescription );
            return true;
        }

        public static TlsSession ImportSession( byte[] sessionID, SessionParameters sessionParameters ) => new TlsSessionImpl( sessionID, sessionParameters );

        public static bool IsSignatureAlgorithmsExtensionAllowed( ProtocolVersion clientVersion ) => ProtocolVersion.TLSv12.IsEqualOrEarlierVersionOf( clientVersion.GetEquivalentTLSVersion() );

        public static void AddSignatureAlgorithmsExtension(
          IDictionary extensions,
          IList supportedSignatureAlgorithms )
        {
            extensions[13] = CreateSignatureAlgorithmsExtension( supportedSignatureAlgorithms );
        }

        public static IList GetSignatureAlgorithmsExtension( IDictionary extensions )
        {
            byte[] extensionData = GetExtensionData( extensions, 13 );
            return extensionData != null ? ReadSignatureAlgorithmsExtension( extensionData ) : null;
        }

        public static byte[] CreateSignatureAlgorithmsExtension( IList supportedSignatureAlgorithms )
        {
            MemoryStream output = new MemoryStream();
            EncodeSupportedSignatureAlgorithms( supportedSignatureAlgorithms, false, output );
            return output.ToArray();
        }

        public static IList ReadSignatureAlgorithmsExtension( byte[] extensionData )
        {
            MemoryStream memoryStream = extensionData != null ? new MemoryStream( extensionData, false ) : throw new ArgumentNullException( nameof( extensionData ) );
            IList signatureAlgorithms = ParseSupportedSignatureAlgorithms( false, memoryStream );
            TlsProtocol.AssertEmpty( memoryStream );
            return signatureAlgorithms;
        }

        public static void EncodeSupportedSignatureAlgorithms(
          IList supportedSignatureAlgorithms,
          bool allowAnonymous,
          Stream output )
        {
            if (supportedSignatureAlgorithms == null)
                throw new ArgumentNullException( nameof( supportedSignatureAlgorithms ) );
            if (supportedSignatureAlgorithms.Count < 1 || supportedSignatureAlgorithms.Count >= 32768)
                throw new ArgumentException( "must have length from 1 to (2^15 - 1)", nameof( supportedSignatureAlgorithms ) );
            int i = 2 * supportedSignatureAlgorithms.Count;
            CheckUint16( i );
            WriteUint16( i, output );
            foreach (SignatureAndHashAlgorithm signatureAlgorithm in (IEnumerable)supportedSignatureAlgorithms)
            {
                if (!allowAnonymous && signatureAlgorithm.Signature == 0)
                    throw new ArgumentException( "SignatureAlgorithm.anonymous MUST NOT appear in the signature_algorithms extension" );
                signatureAlgorithm.Encode( output );
            }
        }

        public static IList ParseSupportedSignatureAlgorithms( bool allowAnonymous, Stream input )
        {
            int num = ReadUint16( input );
            if (num < 2 || (num & 1) != 0)
                throw new TlsFatalAlert( 50 );
            int capacity = num / 2;
            IList arrayList = Platform.CreateArrayList( capacity );
            for (int index = 0; index < capacity; ++index)
            {
                SignatureAndHashAlgorithm andHashAlgorithm = SignatureAndHashAlgorithm.Parse( input );
                if (!allowAnonymous && andHashAlgorithm.Signature == 0)
                    throw new TlsFatalAlert( 47 );
                arrayList.Add( andHashAlgorithm );
            }
            return arrayList;
        }

        public static void VerifySupportedSignatureAlgorithm(
          IList supportedSignatureAlgorithms,
          SignatureAndHashAlgorithm signatureAlgorithm )
        {
            if (supportedSignatureAlgorithms == null)
                throw new ArgumentNullException( nameof( supportedSignatureAlgorithms ) );
            if (supportedSignatureAlgorithms.Count < 1 || supportedSignatureAlgorithms.Count >= 32768)
                throw new ArgumentException( "must have length from 1 to (2^15 - 1)", nameof( supportedSignatureAlgorithms ) );
            if (signatureAlgorithm == null)
                throw new ArgumentNullException( nameof( signatureAlgorithm ) );
            if (signatureAlgorithm.Signature != 0)
            {
                foreach (SignatureAndHashAlgorithm signatureAlgorithm1 in (IEnumerable)supportedSignatureAlgorithms)
                {
                    if (signatureAlgorithm1.Hash == signatureAlgorithm.Hash && signatureAlgorithm1.Signature == signatureAlgorithm.Signature)
                        return;
                }
            }
            throw new TlsFatalAlert( 47 );
        }

        public static byte[] PRF(
          TlsContext context,
          byte[] secret,
          string asciiLabel,
          byte[] seed,
          int size )
        {
            if (context.ServerVersion.IsSsl)
                throw new InvalidOperationException( "No PRF available for SSLv3 session" );
            byte[] byteArray = Strings.ToByteArray( asciiLabel );
            byte[] numArray = Concat( byteArray, seed );
            int prfAlgorithm = context.SecurityParameters.PrfAlgorithm;
            if (prfAlgorithm == 0)
                return PRF_legacy( secret, byteArray, numArray, size );
            IDigest prfHash = CreatePrfHash( prfAlgorithm );
            byte[] output = new byte[size];
            HMacHash( prfHash, secret, numArray, output );
            return output;
        }

        public static byte[] PRF_legacy( byte[] secret, string asciiLabel, byte[] seed, int size )
        {
            byte[] byteArray = Strings.ToByteArray( asciiLabel );
            byte[] labelSeed = Concat( byteArray, seed );
            return PRF_legacy( secret, byteArray, labelSeed, size );
        }

        internal static byte[] PRF_legacy( byte[] secret, byte[] label, byte[] labelSeed, int size )
        {
            int length = (secret.Length + 1) / 2;
            byte[] numArray1 = new byte[length];
            byte[] numArray2 = new byte[length];
            Array.Copy( secret, 0, numArray1, 0, length );
            Array.Copy( secret, secret.Length - length, numArray2, 0, length );
            byte[] output1 = new byte[size];
            byte[] output2 = new byte[size];
            HMacHash( CreateHash( 1 ), numArray1, labelSeed, output1 );
            HMacHash( CreateHash( 2 ), numArray2, labelSeed, output2 );
            for (int index1 = 0; index1 < size; ++index1)
            {
                byte[] numArray3;
                IntPtr index2;
                (numArray3 = output1)[(int)(index2 = (IntPtr)index1)] = (byte)(numArray3[(int)index2] ^ (uint)output2[index1]);
            }
            return output1;
        }

        internal static byte[] Concat( byte[] a, byte[] b )
        {
            byte[] destinationArray = new byte[a.Length + b.Length];
            Array.Copy( a, 0, destinationArray, 0, a.Length );
            Array.Copy( b, 0, destinationArray, a.Length, b.Length );
            return destinationArray;
        }

        internal static void HMacHash( IDigest digest, byte[] secret, byte[] seed, byte[] output )
        {
            HMac hmac = new HMac( digest );
            hmac.Init( new KeyParameter( secret ) );
            byte[] input = seed;
            int digestSize = digest.GetDigestSize();
            int num = (output.Length + digestSize - 1) / digestSize;
            byte[] output1 = new byte[hmac.GetMacSize()];
            byte[] numArray = new byte[hmac.GetMacSize()];
            for (int index = 0; index < num; ++index)
            {
                hmac.BlockUpdate( input, 0, input.Length );
                hmac.DoFinal( output1, 0 );
                input = output1;
                hmac.BlockUpdate( input, 0, input.Length );
                hmac.BlockUpdate( seed, 0, seed.Length );
                hmac.DoFinal( numArray, 0 );
                Array.Copy( (Array)numArray, 0, (Array)output, digestSize * index, System.Math.Min( digestSize, output.Length - (digestSize * index) ) );
            }
        }

        internal static void ValidateKeyUsage( X509CertificateStructure c, int keyUsageBits )
        {
            X509Extensions extensions = c.TbsCertificate.Extensions;
            if (extensions == null)
                return;
            X509Extension extension = extensions.GetExtension( X509Extensions.KeyUsage );
            if (extension != null && (KeyUsage.GetInstance( extension ).GetBytes()[0] & keyUsageBits) != keyUsageBits)
                throw new TlsFatalAlert( 46 );
        }

        internal static byte[] CalculateKeyBlock( TlsContext context, int size )
        {
            SecurityParameters securityParameters = context.SecurityParameters;
            byte[] masterSecret = securityParameters.MasterSecret;
            byte[] numArray = Concat( securityParameters.ServerRandom, securityParameters.ClientRandom );
            return IsSsl( context ) ? CalculateKeyBlock_Ssl( masterSecret, numArray, size ) : PRF( context, masterSecret, "key expansion", numArray, size );
        }

        internal static byte[] CalculateKeyBlock_Ssl( byte[] master_secret, byte[] random, int size )
        {
            IDigest hash1 = CreateHash( 1 );
            IDigest hash2 = CreateHash( 2 );
            int digestSize = hash1.GetDigestSize();
            byte[] numArray1 = new byte[hash2.GetDigestSize()];
            byte[] numArray2 = new byte[size + digestSize];
            int index = 0;
            int outOff = 0;
            while (outOff < size)
            {
                byte[] input = SSL3_CONST[index];
                hash2.BlockUpdate( input, 0, input.Length );
                hash2.BlockUpdate( master_secret, 0, master_secret.Length );
                hash2.BlockUpdate( random, 0, random.Length );
                hash2.DoFinal( numArray1, 0 );
                hash1.BlockUpdate( master_secret, 0, master_secret.Length );
                hash1.BlockUpdate( numArray1, 0, numArray1.Length );
                hash1.DoFinal( numArray2, outOff );
                outOff += digestSize;
                ++index;
            }
            return Arrays.CopyOfRange( numArray2, 0, size );
        }

        internal static byte[] CalculateMasterSecret( TlsContext context, byte[] pre_master_secret )
        {
            SecurityParameters securityParameters = context.SecurityParameters;
            byte[] numArray = securityParameters.extendedMasterSecret ? securityParameters.SessionHash : Concat( securityParameters.ClientRandom, securityParameters.ServerRandom );
            if (IsSsl( context ))
                return CalculateMasterSecret_Ssl( pre_master_secret, numArray );
            string asciiLabel = securityParameters.extendedMasterSecret ? ExporterLabel.extended_master_secret : "master secret";
            return PRF( context, pre_master_secret, asciiLabel, numArray, 48 );
        }

        internal static byte[] CalculateMasterSecret_Ssl( byte[] pre_master_secret, byte[] random )
        {
            IDigest hash1 = CreateHash( 1 );
            IDigest hash2 = CreateHash( 2 );
            int digestSize = hash1.GetDigestSize();
            byte[] numArray = new byte[hash2.GetDigestSize()];
            byte[] output = new byte[digestSize * 3];
            int outOff = 0;
            for (int index = 0; index < 3; ++index)
            {
                byte[] input = SSL3_CONST[index];
                hash2.BlockUpdate( input, 0, input.Length );
                hash2.BlockUpdate( pre_master_secret, 0, pre_master_secret.Length );
                hash2.BlockUpdate( random, 0, random.Length );
                hash2.DoFinal( numArray, 0 );
                hash1.BlockUpdate( pre_master_secret, 0, pre_master_secret.Length );
                hash1.BlockUpdate( numArray, 0, numArray.Length );
                hash1.DoFinal( output, outOff );
                outOff += digestSize;
            }
            return output;
        }

        internal static byte[] CalculateVerifyData(
          TlsContext context,
          string asciiLabel,
          byte[] handshakeHash )
        {
            if (IsSsl( context ))
                return handshakeHash;
            SecurityParameters securityParameters = context.SecurityParameters;
            byte[] masterSecret = securityParameters.MasterSecret;
            int verifyDataLength = securityParameters.VerifyDataLength;
            return PRF( context, masterSecret, asciiLabel, handshakeHash, verifyDataLength );
        }

        public static IDigest CreateHash( byte hashAlgorithm )
        {
            switch (hashAlgorithm)
            {
                case 1:
                    return new MD5Digest();
                case 2:
                    return new Sha1Digest();
                case 3:
                    return new Sha224Digest();
                case 4:
                    return new Sha256Digest();
                case 5:
                    return new Sha384Digest();
                case 6:
                    return new Sha512Digest();
                default:
                    throw new ArgumentException( "unknown HashAlgorithm", nameof( hashAlgorithm ) );
            }
        }

        public static IDigest CreateHash(
          SignatureAndHashAlgorithm signatureAndHashAlgorithm )
        {
            return signatureAndHashAlgorithm != null ? CreateHash( signatureAndHashAlgorithm.Hash ) : new CombinedHash();
        }

        public static IDigest CloneHash( byte hashAlgorithm, IDigest hash )
        {
            switch (hashAlgorithm)
            {
                case 1:
                    return new MD5Digest( (MD5Digest)hash );
                case 2:
                    return new Sha1Digest( (Sha1Digest)hash );
                case 3:
                    return new Sha224Digest( (Sha224Digest)hash );
                case 4:
                    return new Sha256Digest( (Sha256Digest)hash );
                case 5:
                    return new Sha384Digest( (Sha384Digest)hash );
                case 6:
                    return new Sha512Digest( (Sha512Digest)hash );
                default:
                    throw new ArgumentException( "unknown HashAlgorithm", nameof( hashAlgorithm ) );
            }
        }

        public static IDigest CreatePrfHash( int prfAlgorithm ) => prfAlgorithm == 0 ? new CombinedHash() : CreateHash( GetHashAlgorithmForPrfAlgorithm( prfAlgorithm ) );

        public static IDigest ClonePrfHash( int prfAlgorithm, IDigest hash ) => prfAlgorithm == 0 ? new CombinedHash( (CombinedHash)hash ) : CloneHash( GetHashAlgorithmForPrfAlgorithm( prfAlgorithm ), hash );

        public static byte GetHashAlgorithmForPrfAlgorithm( int prfAlgorithm )
        {
            switch (prfAlgorithm)
            {
                case 0:
                    throw new ArgumentException( "legacy PRF not a valid algorithm", nameof( prfAlgorithm ) );
                case 1:
                    return 4;
                case 2:
                    return 5;
                default:
                    throw new ArgumentException( "unknown PrfAlgorithm", nameof( prfAlgorithm ) );
            }
        }

        public static DerObjectIdentifier GetOidForHashAlgorithm( byte hashAlgorithm )
        {
            switch (hashAlgorithm)
            {
                case 1:
                    return PkcsObjectIdentifiers.MD5;
                case 2:
                    return X509ObjectIdentifiers.IdSha1;
                case 3:
                    return NistObjectIdentifiers.IdSha224;
                case 4:
                    return NistObjectIdentifiers.IdSha256;
                case 5:
                    return NistObjectIdentifiers.IdSha384;
                case 6:
                    return NistObjectIdentifiers.IdSha512;
                default:
                    throw new ArgumentException( "unknown HashAlgorithm", nameof( hashAlgorithm ) );
            }
        }

        internal static short GetClientCertificateType(
          Certificate clientCertificate,
          Certificate serverCertificate )
        {
            if (clientCertificate.IsEmpty)
                return -1;
            X509CertificateStructure certificateAt = clientCertificate.GetCertificateAt( 0 );
            SubjectPublicKeyInfo subjectPublicKeyInfo = certificateAt.SubjectPublicKeyInfo;
            try
            {
                AsymmetricKeyParameter key = PublicKeyFactory.CreateKey( subjectPublicKeyInfo );
                if (key.IsPrivate)
                    throw new TlsFatalAlert( 80 );
                switch (key)
                {
                    case RsaKeyParameters _:
                        ValidateKeyUsage( certificateAt, 128 );
                        return 1;
                    case DsaPublicKeyParameters _:
                        ValidateKeyUsage( certificateAt, 128 );
                        return 2;
                    case ECPublicKeyParameters _:
                        ValidateKeyUsage( certificateAt, 128 );
                        return 64;
                    default:
                        throw new TlsFatalAlert( 43 );
                }
            }
            catch (Exception ex)
            {
                throw new TlsFatalAlert( 43, ex );
            }
        }

        internal static void TrackHashAlgorithms(
          TlsHandshakeHash handshakeHash,
          IList supportedSignatureAlgorithms )
        {
            if (supportedSignatureAlgorithms == null)
                return;
            foreach (SignatureAndHashAlgorithm signatureAlgorithm in (IEnumerable)supportedSignatureAlgorithms)
            {
                byte hash = signatureAlgorithm.Hash;
                handshakeHash.TrackHashAlgorithm( hash );
            }
        }

        public static bool HasSigningCapability( byte clientCertificateType )
        {
            switch (clientCertificateType)
            {
                case 1:
                case 2:
                case 64:
                    return true;
                default:
                    return false;
            }
        }

        public static TlsSigner CreateTlsSigner( byte clientCertificateType )
        {
            switch (clientCertificateType)
            {
                case 1:
                    return new TlsRsaSigner();
                case 2:
                    return new TlsDssSigner();
                case 64:
                    return new TlsECDsaSigner();
                default:
                    throw new ArgumentException( "not a type with signing capability", nameof( clientCertificateType ) );
            }
        }

        private static byte[][] GenSsl3Const()
        {
            int length = 10;
            byte[][] numArray = new byte[length][];
            for (int index = 0; index < length; ++index)
            {
                byte[] buf = new byte[index + 1];
                Arrays.Fill( buf, (byte)(65 + index) );
                numArray[index] = buf;
            }
            return numArray;
        }

        private static IList VectorOfOne( object obj )
        {
            IList arrayList = Platform.CreateArrayList( 1 );
            arrayList.Add( obj );
            return arrayList;
        }

        public static int GetCipherType( int ciphersuite )
        {
            switch (GetEncryptionAlgorithm( ciphersuite ))
            {
                case 1:
                case 2:
                case 100:
                case 101:
                    return 0;
                case 3:
                case 4:
                case 5:
                case 6:
                case 7:
                case 8:
                case 9:
                case 12:
                case 13:
                case 14:
                    return 1;
                case 10:
                case 11:
                case 15:
                case 16:
                case 17:
                case 18:
                case 19:
                case 20:
                case 102:
                    return 2;
                default:
                    throw new TlsFatalAlert( 80 );
            }
        }

        public static int GetEncryptionAlgorithm( int ciphersuite )
        {
            switch (ciphersuite)
            {
                case 1:
                    return 0;
                case 2:
                case 44:
                case 45:
                case 46:
                case 49153:
                case 49158:
                case 49163:
                case 49168:
                case 49209:
                    return 0;
                case 4:
                case 24:
                    return 2;
                case 5:
                case 138:
                case 142:
                case 146:
                case 49154:
                case 49159:
                case 49164:
                case 49169:
                case 49174:
                case 49203:
                    return 2;
                case 10:
                case 13:
                case 16:
                case 19:
                case 22:
                case 139:
                case 143:
                case 147:
                case 49155:
                case 49160:
                case 49165:
                case 49170:
                case 49178:
                case 49179:
                case 49180:
                case 49204:
                    return 7;
                case 47:
                case 48:
                case 49:
                case 50:
                case 51:
                case 140:
                case 144:
                case 148:
                case 49156:
                case 49161:
                case 49166:
                case 49171:
                case 49181:
                case 49182:
                case 49183:
                case 49205:
                    return 8;
                case 53:
                case 54:
                case 55:
                case 56:
                case 57:
                case 141:
                case 145:
                case 149:
                case 49157:
                case 49162:
                case 49167:
                case 49172:
                case 49184:
                case 49185:
                case 49186:
                case 49206:
                    return 9;
                case 59:
                case 176:
                case 180:
                case 184:
                case 49210:
                    return 0;
                case 60:
                case 62:
                case 63:
                case 64:
                case 103:
                case 174:
                case 178:
                case 182:
                case 49187:
                case 49189:
                case 49191:
                case 49193:
                case 49207:
                    return 8;
                case 61:
                case 104:
                case 105:
                case 106:
                case 107:
                    return 9;
                case 65:
                case 66:
                case 67:
                case 68:
                case 69:
                    return 12;
                case 132:
                case 133:
                case 134:
                case 135:
                case 136:
                    return 13;
                case 150:
                case 151:
                case 152:
                case 153:
                case 154:
                    return 14;
                case 156:
                case 158:
                case 160:
                case 162:
                case 164:
                case 168:
                case 170:
                case 172:
                case 49195:
                case 49197:
                case 49199:
                case 49201:
                    return 10;
                case 157:
                case 159:
                case 161:
                case 163:
                case 165:
                case 169:
                case 171:
                case 173:
                case 49196:
                case 49198:
                case 49200:
                case 49202:
                    return 11;
                case 175:
                case 179:
                case 183:
                case 49188:
                case 49190:
                case 49192:
                case 49194:
                case 49208:
                    return 9;
                case 177:
                case 181:
                case 185:
                case 49211:
                    return 0;
                case 186:
                case 187:
                case 188:
                case 189:
                case 190:
                case 49266:
                case 49268:
                case 49270:
                case 49272:
                case 49300:
                case 49302:
                case 49304:
                case 49306:
                    return 12;
                case 192:
                case 193:
                case 194:
                case 195:
                case 196:
                    return 13;
                case 49267:
                case 49269:
                case 49271:
                case 49273:
                case 49301:
                case 49303:
                case 49305:
                case 49307:
                    return 13;
                case 49274:
                case 49276:
                case 49278:
                case 49280:
                case 49282:
                case 49286:
                case 49288:
                case 49290:
                case 49292:
                case 49294:
                case 49296:
                case 49298:
                    return 19;
                case 49275:
                case 49277:
                case 49279:
                case 49281:
                case 49283:
                case 49287:
                case 49289:
                case 49291:
                case 49293:
                case 49295:
                case 49297:
                case 49299:
                    return 20;
                case 49308:
                case 49310:
                case 49316:
                case 49318:
                case 49324:
                    return 15;
                case 49309:
                case 49311:
                case 49317:
                case 49319:
                case 49325:
                    return 17;
                case 49312:
                case 49314:
                case 49320:
                case 49322:
                case 49326:
                    return 16;
                case 49313:
                case 49315:
                case 49321:
                case 49323:
                case 49327:
                    return 18;
                case 52243:
                case 52244:
                case 52245:
                    return 102;
                case 58384:
                case 58386:
                case 58388:
                case 58390:
                case 58392:
                case 58394:
                case 58396:
                case 58398:
                    return 100;
                case 58385:
                case 58387:
                case 58389:
                case 58391:
                case 58393:
                case 58395:
                case 58397:
                case 58399:
                    return 101;
                default:
                    throw new TlsFatalAlert( 80 );
            }
        }

        public static int GetKeyExchangeAlgorithm( int ciphersuite )
        {
            switch (ciphersuite)
            {
                case 1:
                case 2:
                case 4:
                case 5:
                case 10:
                case 47:
                case 53:
                case 59:
                case 60:
                case 61:
                case 65:
                case 132:
                case 150:
                case 156:
                case 157:
                case 186:
                case 192:
                case 49274:
                case 49275:
                case 49308:
                case 49309:
                case 49312:
                case 49313:
                case 58384:
                case 58385:
                    return 1;
                case 13:
                case 48:
                case 54:
                case 62:
                case 66:
                case 104:
                case 133:
                case 151:
                case 164:
                case 165:
                case 187:
                case 193:
                case 49282:
                case 49283:
                    return 7;
                case 16:
                case 49:
                case 55:
                case 63:
                case 67:
                case 105:
                case 134:
                case 152:
                case 160:
                case 161:
                case 188:
                case 194:
                case 49278:
                case 49279:
                    return 9;
                case 19:
                case 50:
                case 56:
                case 64:
                case 68:
                case 106:
                case 135:
                case 153:
                case 162:
                case 163:
                case 189:
                case 195:
                case 49280:
                case 49281:
                    return 3;
                case 22:
                case 51:
                case 57:
                case 69:
                case 103:
                case 107:
                case 136:
                case 154:
                case 158:
                case 159:
                case 190:
                case 196:
                case 49276:
                case 49277:
                case 49310:
                case 49311:
                case 49314:
                case 49315:
                case 52245:
                case 58398:
                case 58399:
                    return 5;
                case 44:
                case 138:
                case 139:
                case 140:
                case 141:
                case 168:
                case 169:
                case 174:
                case 175:
                case 176:
                case 177:
                case 49294:
                case 49295:
                case 49300:
                case 49301:
                case 49316:
                case 49317:
                case 49320:
                case 49321:
                case 58390:
                case 58391:
                    return 13;
                case 45:
                case 142:
                case 143:
                case 144:
                case 145:
                case 170:
                case 171:
                case 178:
                case 179:
                case 180:
                case 181:
                case 49296:
                case 49297:
                case 49302:
                case 49303:
                case 49318:
                case 49319:
                case 49322:
                case 49323:
                case 58396:
                case 58397:
                    return 14;
                case 46:
                case 146:
                case 147:
                case 148:
                case 149:
                case 172:
                case 173:
                case 182:
                case 183:
                case 184:
                case 185:
                case 49298:
                case 49299:
                case 49304:
                case 49305:
                case 58394:
                case 58395:
                    return 15;
                case 49153:
                case 49154:
                case 49155:
                case 49156:
                case 49157:
                case 49189:
                case 49190:
                case 49197:
                case 49198:
                case 49268:
                case 49269:
                case 49288:
                case 49289:
                    return 16;
                case 49158:
                case 49159:
                case 49160:
                case 49161:
                case 49162:
                case 49187:
                case 49188:
                case 49195:
                case 49196:
                case 49266:
                case 49267:
                case 49286:
                case 49287:
                case 49324:
                case 49325:
                case 49326:
                case 49327:
                case 52244:
                case 58388:
                case 58389:
                    return 17;
                case 49163:
                case 49164:
                case 49165:
                case 49166:
                case 49167:
                case 49193:
                case 49194:
                case 49201:
                case 49202:
                case 49272:
                case 49273:
                case 49292:
                case 49293:
                    return 18;
                case 49168:
                case 49169:
                case 49170:
                case 49171:
                case 49172:
                case 49191:
                case 49192:
                case 49199:
                case 49200:
                case 49270:
                case 49271:
                case 49290:
                case 49291:
                case 52243:
                case 58386:
                case 58387:
                    return 19;
                case 49178:
                case 49181:
                case 49184:
                    return 21;
                case 49179:
                case 49182:
                case 49185:
                    return 23;
                case 49180:
                case 49183:
                case 49186:
                    return 22;
                case 49203:
                case 49204:
                case 49205:
                case 49206:
                case 49207:
                case 49208:
                case 49209:
                case 49210:
                case 49211:
                case 49306:
                case 49307:
                case 58392:
                case 58393:
                    return 24;
                default:
                    throw new TlsFatalAlert( 80 );
            }
        }

        public static int GetMacAlgorithm( int ciphersuite )
        {
            switch (ciphersuite)
            {
                case 1:
                case 4:
                    return 1;
                case 2:
                case 5:
                case 10:
                case 13:
                case 16:
                case 19:
                case 22:
                case 44:
                case 45:
                case 46:
                case 47:
                case 48:
                case 49:
                case 50:
                case 51:
                case 53:
                case 54:
                case 55:
                case 56:
                case 57:
                case 65:
                case 66:
                case 67:
                case 68:
                case 69:
                case 132:
                case 133:
                case 134:
                case 135:
                case 136:
                case 138:
                case 139:
                case 140:
                case 141:
                case 142:
                case 143:
                case 144:
                case 145:
                case 146:
                case 147:
                case 148:
                case 149:
                case 150:
                case 151:
                case 152:
                case 153:
                case 154:
                case 49153:
                case 49154:
                case 49155:
                case 49156:
                case 49157:
                case 49158:
                case 49159:
                case 49160:
                case 49161:
                case 49162:
                case 49163:
                case 49164:
                case 49165:
                case 49166:
                case 49167:
                case 49168:
                case 49169:
                case 49170:
                case 49171:
                case 49172:
                case 49178:
                case 49179:
                case 49180:
                case 49181:
                case 49182:
                case 49183:
                case 49184:
                case 49185:
                case 49186:
                case 49203:
                case 49204:
                case 49205:
                case 49206:
                case 49209:
                case 58384:
                case 58385:
                case 58386:
                case 58387:
                case 58388:
                case 58389:
                case 58390:
                case 58391:
                case 58392:
                case 58393:
                case 58394:
                case 58395:
                case 58396:
                case 58397:
                case 58398:
                case 58399:
                    return 2;
                case 59:
                case 60:
                case 61:
                case 62:
                case 63:
                case 64:
                case 103:
                case 104:
                case 105:
                case 106:
                case 107:
                case 174:
                case 176:
                case 178:
                case 180:
                case 182:
                case 184:
                case 186:
                case 187:
                case 188:
                case 189:
                case 190:
                case 192:
                case 193:
                case 194:
                case 195:
                case 196:
                case 49187:
                case 49189:
                case 49191:
                case 49193:
                case 49207:
                case 49210:
                case 49266:
                case 49268:
                case 49270:
                case 49272:
                case 49300:
                case 49302:
                case 49304:
                case 49306:
                    return 3;
                case 156:
                case 157:
                case 158:
                case 159:
                case 160:
                case 161:
                case 162:
                case 163:
                case 164:
                case 165:
                case 168:
                case 169:
                case 170:
                case 171:
                case 172:
                case 173:
                case 49195:
                case 49196:
                case 49197:
                case 49198:
                case 49199:
                case 49200:
                case 49201:
                case 49202:
                case 49274:
                case 49275:
                case 49276:
                case 49277:
                case 49278:
                case 49279:
                case 49280:
                case 49281:
                case 49282:
                case 49283:
                case 49286:
                case 49287:
                case 49288:
                case 49289:
                case 49290:
                case 49291:
                case 49292:
                case 49293:
                case 49294:
                case 49295:
                case 49296:
                case 49297:
                case 49298:
                case 49299:
                case 49308:
                case 49309:
                case 49310:
                case 49311:
                case 49312:
                case 49313:
                case 49314:
                case 49315:
                case 49316:
                case 49317:
                case 49318:
                case 49319:
                case 49320:
                case 49321:
                case 49322:
                case 49323:
                case 49324:
                case 49325:
                case 49326:
                case 49327:
                case 52243:
                case 52244:
                case 52245:
                    return 0;
                case 175:
                case 177:
                case 179:
                case 181:
                case 183:
                case 185:
                case 49188:
                case 49190:
                case 49192:
                case 49194:
                case 49208:
                case 49211:
                case 49267:
                case 49269:
                case 49271:
                case 49273:
                case 49301:
                case 49303:
                case 49305:
                case 49307:
                    return 4;
                default:
                    throw new TlsFatalAlert( 80 );
            }
        }

        public static ProtocolVersion GetMinimumVersion( int ciphersuite )
        {
            switch (ciphersuite)
            {
                case 59:
                case 60:
                case 61:
                case 62:
                case 63:
                case 64:
                case 103:
                case 104:
                case 105:
                case 106:
                case 107:
                case 156:
                case 157:
                case 158:
                case 159:
                case 160:
                case 161:
                case 162:
                case 163:
                case 164:
                case 165:
                case 168:
                case 169:
                case 170:
                case 171:
                case 172:
                case 173:
                case 186:
                case 187:
                case 188:
                case 189:
                case 190:
                case 191:
                case 192:
                case 193:
                case 194:
                case 195:
                case 196:
                case 197:
                case 49187:
                case 49188:
                case 49189:
                case 49190:
                case 49191:
                case 49192:
                case 49193:
                case 49194:
                case 49195:
                case 49196:
                case 49197:
                case 49198:
                case 49199:
                case 49200:
                case 49201:
                case 49202:
                case 49266:
                case 49267:
                case 49268:
                case 49269:
                case 49270:
                case 49271:
                case 49272:
                case 49273:
                case 49274:
                case 49275:
                case 49276:
                case 49277:
                case 49278:
                case 49279:
                case 49280:
                case 49281:
                case 49282:
                case 49283:
                case 49284:
                case 49285:
                case 49286:
                case 49287:
                case 49288:
                case 49289:
                case 49290:
                case 49291:
                case 49292:
                case 49293:
                case 49294:
                case 49295:
                case 49296:
                case 49297:
                case 49298:
                case 49299:
                case 49308:
                case 49309:
                case 49310:
                case 49311:
                case 49312:
                case 49313:
                case 49314:
                case 49315:
                case 49316:
                case 49317:
                case 49318:
                case 49319:
                case 49320:
                case 49321:
                case 49322:
                case 49323:
                case 49324:
                case 49325:
                case 49326:
                case 49327:
                case 52243:
                case 52244:
                case 52245:
                    return ProtocolVersion.TLSv12;
                default:
                    return ProtocolVersion.SSLv3;
            }
        }

        public static bool IsAeadCipherSuite( int ciphersuite ) => 2 == GetCipherType( ciphersuite );

        public static bool IsBlockCipherSuite( int ciphersuite ) => 1 == GetCipherType( ciphersuite );

        public static bool IsStreamCipherSuite( int ciphersuite ) => 0 == GetCipherType( ciphersuite );

        public static bool IsValidCipherSuiteForVersion( int cipherSuite, ProtocolVersion serverVersion ) => GetMinimumVersion( cipherSuite ).IsEqualOrEarlierVersionOf( serverVersion.GetEquivalentTLSVersion() );
    }
}
