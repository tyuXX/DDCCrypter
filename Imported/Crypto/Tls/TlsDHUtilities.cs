﻿// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.TlsDHUtilities
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Agreement;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities;
using Org.BouncyCastle.Utilities.Encoders;
using System;
using System.Collections;
using System.IO;

namespace Org.BouncyCastle.Crypto.Tls
{
    public abstract class TlsDHUtilities
    {
        internal static readonly BigInteger Two = BigInteger.Two;
        private static readonly string draft_ffdhe2432_p = "FFFFFFFFFFFFFFFFADF85458A2BB4A9AAFDC5620273D3CF1D8B9C583CE2D3695A9E13641146433FBCC939DCE249B3EF97D2FE363630C75D8F681B202AEC4617AD3DF1ED5D5FD65612433F51F5F066ED0856365553DED1AF3B557135E7F57C935984F0C70E0E68B77E2A689DAF3EFE8721DF158A136ADE73530ACCA4F483A797ABC0AB182B324FB61D108A94BB2C8E3FBB96ADAB760D7F4681D4F42A3DE394DF4AE56EDE76372BB190B07A7C8EE0A6D709E02FCE1CDF7E2ECC03404CD28342F619172FE9CE98583FF8E4F1232EEF28183C3FE3B1B4C6FAD733BB5FCBC2EC22005C58EF1837D1683B2C6F34A26C1B2EFFA886B4238611FCFDCDE355B3B6519035BBC34F4DEF99C023861B46FC9D6E6C9077AD91D2691F7F7EE598CB0FAC186D91CAEFE13098533C8B3FFFFFFFFFFFFFFFF";
        internal static readonly DHParameters draft_ffdhe2432 = FromSafeP( draft_ffdhe2432_p );
        private static readonly string draft_ffdhe3072_p = "FFFFFFFFFFFFFFFFADF85458A2BB4A9AAFDC5620273D3CF1D8B9C583CE2D3695A9E13641146433FBCC939DCE249B3EF97D2FE363630C75D8F681B202AEC4617AD3DF1ED5D5FD65612433F51F5F066ED0856365553DED1AF3B557135E7F57C935984F0C70E0E68B77E2A689DAF3EFE8721DF158A136ADE73530ACCA4F483A797ABC0AB182B324FB61D108A94BB2C8E3FBB96ADAB760D7F4681D4F42A3DE394DF4AE56EDE76372BB190B07A7C8EE0A6D709E02FCE1CDF7E2ECC03404CD28342F619172FE9CE98583FF8E4F1232EEF28183C3FE3B1B4C6FAD733BB5FCBC2EC22005C58EF1837D1683B2C6F34A26C1B2EFFA886B4238611FCFDCDE355B3B6519035BBC34F4DEF99C023861B46FC9D6E6C9077AD91D2691F7F7EE598CB0FAC186D91CAEFE130985139270B4130C93BC437944F4FD4452E2D74DD364F2E21E71F54BFF5CAE82AB9C9DF69EE86D2BC522363A0DABC521979B0DEADA1DBF9A42D5C4484E0ABCD06BFA53DDEF3C1B20EE3FD59D7C25E41D2B66C62E37FFFFFFFFFFFFFFFF";
        internal static readonly DHParameters draft_ffdhe3072 = FromSafeP( draft_ffdhe3072_p );
        private static readonly string draft_ffdhe4096_p = "FFFFFFFFFFFFFFFFADF85458A2BB4A9AAFDC5620273D3CF1D8B9C583CE2D3695A9E13641146433FBCC939DCE249B3EF97D2FE363630C75D8F681B202AEC4617AD3DF1ED5D5FD65612433F51F5F066ED0856365553DED1AF3B557135E7F57C935984F0C70E0E68B77E2A689DAF3EFE8721DF158A136ADE73530ACCA4F483A797ABC0AB182B324FB61D108A94BB2C8E3FBB96ADAB760D7F4681D4F42A3DE394DF4AE56EDE76372BB190B07A7C8EE0A6D709E02FCE1CDF7E2ECC03404CD28342F619172FE9CE98583FF8E4F1232EEF28183C3FE3B1B4C6FAD733BB5FCBC2EC22005C58EF1837D1683B2C6F34A26C1B2EFFA886B4238611FCFDCDE355B3B6519035BBC34F4DEF99C023861B46FC9D6E6C9077AD91D2691F7F7EE598CB0FAC186D91CAEFE130985139270B4130C93BC437944F4FD4452E2D74DD364F2E21E71F54BFF5CAE82AB9C9DF69EE86D2BC522363A0DABC521979B0DEADA1DBF9A42D5C4484E0ABCD06BFA53DDEF3C1B20EE3FD59D7C25E41D2B669E1EF16E6F52C3164DF4FB7930E9E4E58857B6AC7D5F42D69F6D187763CF1D5503400487F55BA57E31CC7A7135C886EFB4318AED6A1E012D9E6832A907600A918130C46DC778F971AD0038092999A333CB8B7A1A1DB93D7140003C2A4ECEA9F98D0ACC0A8291CDCEC97DCF8EC9B55A7F88A46B4DB5A851F44182E1C68A007E5E655F6AFFFFFFFFFFFFFFFF";
        internal static readonly DHParameters draft_ffdhe4096 = FromSafeP( draft_ffdhe4096_p );
        private static readonly string draft_ffdhe6144_p = "FFFFFFFFFFFFFFFFADF85458A2BB4A9AAFDC5620273D3CF1D8B9C583CE2D3695A9E13641146433FBCC939DCE249B3EF97D2FE363630C75D8F681B202AEC4617AD3DF1ED5D5FD65612433F51F5F066ED0856365553DED1AF3B557135E7F57C935984F0C70E0E68B77E2A689DAF3EFE8721DF158A136ADE73530ACCA4F483A797ABC0AB182B324FB61D108A94BB2C8E3FBB96ADAB760D7F4681D4F42A3DE394DF4AE56EDE76372BB190B07A7C8EE0A6D709E02FCE1CDF7E2ECC03404CD28342F619172FE9CE98583FF8E4F1232EEF28183C3FE3B1B4C6FAD733BB5FCBC2EC22005C58EF1837D1683B2C6F34A26C1B2EFFA886B4238611FCFDCDE355B3B6519035BBC34F4DEF99C023861B46FC9D6E6C9077AD91D2691F7F7EE598CB0FAC186D91CAEFE130985139270B4130C93BC437944F4FD4452E2D74DD364F2E21E71F54BFF5CAE82AB9C9DF69EE86D2BC522363A0DABC521979B0DEADA1DBF9A42D5C4484E0ABCD06BFA53DDEF3C1B20EE3FD59D7C25E41D2B669E1EF16E6F52C3164DF4FB7930E9E4E58857B6AC7D5F42D69F6D187763CF1D5503400487F55BA57E31CC7A7135C886EFB4318AED6A1E012D9E6832A907600A918130C46DC778F971AD0038092999A333CB8B7A1A1DB93D7140003C2A4ECEA9F98D0ACC0A8291CDCEC97DCF8EC9B55A7F88A46B4DB5A851F44182E1C68A007E5E0DD9020BFD64B645036C7A4E677D2C38532A3A23BA4442CAF53EA63BB454329B7624C8917BDD64B1C0FD4CB38E8C334C701C3ACDAD0657FCCFEC719B1F5C3E4E46041F388147FB4CFDB477A52471F7A9A96910B855322EDB6340D8A00EF092350511E30ABEC1FFF9E3A26E7FB29F8C183023C3587E38DA0077D9B4763E4E4B94B2BBC194C6651E77CAF992EEAAC0232A281BF6B3A739C1226116820AE8DB5847A67CBEF9C9091B462D538CD72B03746AE77F5E62292C311562A846505DC82DB854338AE49F5235C95B91178CCF2DD5CACEF403EC9D1810C6272B045B3B71F9DC6B80D63FDD4A8E9ADB1E6962A69526D43161C1A41D570D7938DAD4A40E329CD0E40E65FFFFFFFFFFFFFFFF";
        internal static readonly DHParameters draft_ffdhe6144 = FromSafeP( draft_ffdhe6144_p );
        private static readonly string draft_ffdhe8192_p = "FFFFFFFFFFFFFFFFADF85458A2BB4A9AAFDC5620273D3CF1D8B9C583CE2D3695A9E13641146433FBCC939DCE249B3EF97D2FE363630C75D8F681B202AEC4617AD3DF1ED5D5FD65612433F51F5F066ED0856365553DED1AF3B557135E7F57C935984F0C70E0E68B77E2A689DAF3EFE8721DF158A136ADE73530ACCA4F483A797ABC0AB182B324FB61D108A94BB2C8E3FBB96ADAB760D7F4681D4F42A3DE394DF4AE56EDE76372BB190B07A7C8EE0A6D709E02FCE1CDF7E2ECC03404CD28342F619172FE9CE98583FF8E4F1232EEF28183C3FE3B1B4C6FAD733BB5FCBC2EC22005C58EF1837D1683B2C6F34A26C1B2EFFA886B4238611FCFDCDE355B3B6519035BBC34F4DEF99C023861B46FC9D6E6C9077AD91D2691F7F7EE598CB0FAC186D91CAEFE130985139270B4130C93BC437944F4FD4452E2D74DD364F2E21E71F54BFF5CAE82AB9C9DF69EE86D2BC522363A0DABC521979B0DEADA1DBF9A42D5C4484E0ABCD06BFA53DDEF3C1B20EE3FD59D7C25E41D2B669E1EF16E6F52C3164DF4FB7930E9E4E58857B6AC7D5F42D69F6D187763CF1D5503400487F55BA57E31CC7A7135C886EFB4318AED6A1E012D9E6832A907600A918130C46DC778F971AD0038092999A333CB8B7A1A1DB93D7140003C2A4ECEA9F98D0ACC0A8291CDCEC97DCF8EC9B55A7F88A46B4DB5A851F44182E1C68A007E5E0DD9020BFD64B645036C7A4E677D2C38532A3A23BA4442CAF53EA63BB454329B7624C8917BDD64B1C0FD4CB38E8C334C701C3ACDAD0657FCCFEC719B1F5C3E4E46041F388147FB4CFDB477A52471F7A9A96910B855322EDB6340D8A00EF092350511E30ABEC1FFF9E3A26E7FB29F8C183023C3587E38DA0077D9B4763E4E4B94B2BBC194C6651E77CAF992EEAAC0232A281BF6B3A739C1226116820AE8DB5847A67CBEF9C9091B462D538CD72B03746AE77F5E62292C311562A846505DC82DB854338AE49F5235C95B91178CCF2DD5CACEF403EC9D1810C6272B045B3B71F9DC6B80D63FDD4A8E9ADB1E6962A69526D43161C1A41D570D7938DAD4A40E329CCFF46AAA36AD004CF600C8381E425A31D951AE64FDB23FCEC9509D43687FEB69EDD1CC5E0B8CC3BDF64B10EF86B63142A3AB8829555B2F747C932665CB2C0F1CC01BD70229388839D2AF05E454504AC78B7582822846C0BA35C35F5C59160CC046FD8251541FC68C9C86B022BB7099876A460E7451A8A93109703FEE1C217E6C3826E52C51AA691E0E423CFC99E9E31650C1217B624816CDAD9A95F9D5B8019488D9C0A0A1FE3075A577E23183F81D4A3F2FA4571EFC8CE0BA8A4FE8B6855DFE72B0A66EDED2FBABFBE58A30FAFABE1C5D71A87E2F741EF8C1FE86FEA6BBFDE530677F0D97D11D49F7A8443D0822E506A9F4614E011E2A94838FF88CD68C8BB7C5C6424CFFFFFFFFFFFFFFFF";
        internal static readonly DHParameters draft_ffdhe8192 = FromSafeP( draft_ffdhe8192_p );

        private static BigInteger FromHex( string hex ) => new BigInteger( 1, Hex.Decode( hex ) );

        private static DHParameters FromSafeP( string hexP )
        {
            BigInteger p = FromHex( hexP );
            BigInteger q = p.ShiftRight( 1 );
            return new DHParameters( p, Two, q );
        }

        public static void AddNegotiatedDheGroupsClientExtension(
          IDictionary extensions,
          byte[] dheGroups )
        {
            extensions[ExtensionType.negotiated_ff_dhe_groups] = CreateNegotiatedDheGroupsClientExtension( dheGroups );
        }

        public static void AddNegotiatedDheGroupsServerExtension( IDictionary extensions, byte dheGroup ) => extensions[ExtensionType.negotiated_ff_dhe_groups] = CreateNegotiatedDheGroupsServerExtension( dheGroup );

        public static byte[] GetNegotiatedDheGroupsClientExtension( IDictionary extensions )
        {
            byte[] extensionData = TlsUtilities.GetExtensionData( extensions, ExtensionType.negotiated_ff_dhe_groups );
            return extensionData != null ? ReadNegotiatedDheGroupsClientExtension( extensionData ) : null;
        }

        public static short GetNegotiatedDheGroupsServerExtension( IDictionary extensions )
        {
            byte[] extensionData = TlsUtilities.GetExtensionData( extensions, ExtensionType.negotiated_ff_dhe_groups );
            return extensionData != null ? ReadNegotiatedDheGroupsServerExtension( extensionData ) : (short)-1;
        }

        public static byte[] CreateNegotiatedDheGroupsClientExtension( byte[] dheGroups ) => dheGroups != null && dheGroups.Length >= 1 && dheGroups.Length <= byte.MaxValue ? TlsUtilities.EncodeUint8ArrayWithUint8Length( dheGroups ) : throw new TlsFatalAlert( 80 );

        public static byte[] CreateNegotiatedDheGroupsServerExtension( byte dheGroup ) => new byte[1]
        {
      dheGroup
        };

        public static byte[] ReadNegotiatedDheGroupsClientExtension( byte[] extensionData )
        {
            MemoryStream memoryStream = extensionData != null ? new MemoryStream( extensionData, false ) : throw new ArgumentNullException( nameof( extensionData ) );
            byte count = TlsUtilities.ReadUint8( memoryStream );
            byte[] numArray = count >= 1 ? TlsUtilities.ReadUint8Array( count, memoryStream ) : throw new TlsFatalAlert( 50 );
            TlsProtocol.AssertEmpty( memoryStream );
            return numArray;
        }

        public static byte ReadNegotiatedDheGroupsServerExtension( byte[] extensionData )
        {
            if (extensionData == null)
                throw new ArgumentNullException( nameof( extensionData ) );
            return extensionData.Length == 1 ? extensionData[0] : throw new TlsFatalAlert( 50 );
        }

        public static DHParameters GetParametersForDHEGroup( short dheGroup )
        {
            switch (dheGroup)
            {
                case 0:
                    return draft_ffdhe2432;
                case 1:
                    return draft_ffdhe3072;
                case 2:
                    return draft_ffdhe4096;
                case 3:
                    return draft_ffdhe6144;
                case 4:
                    return draft_ffdhe8192;
                default:
                    return null;
            }
        }

        public static bool ContainsDheCipherSuites( int[] cipherSuites )
        {
            for (int index = 0; index < cipherSuites.Length; ++index)
            {
                if (IsDheCipherSuite( cipherSuites[index] ))
                    return true;
            }
            return false;
        }

        public static bool IsDheCipherSuite( int cipherSuite )
        {
            switch (cipherSuite)
            {
                case 17:
                case 18:
                case 19:
                case 20:
                case 21:
                case 22:
                case 45:
                case 50:
                case 51:
                case 56:
                case 57:
                case 64:
                case 68:
                case 69:
                case 103:
                case 106:
                case 107:
                case 135:
                case 136:
                case 142:
                case 143:
                case 144:
                case 145:
                case 153:
                case 154:
                case 158:
                case 159:
                case 162:
                case 163:
                case 170:
                case 171:
                case 178:
                case 179:
                case 180:
                case 181:
                case 189:
                case 190:
                case 195:
                case 196:
                case 49276:
                case 49277:
                case 49280:
                case 49281:
                case 49296:
                case 49297:
                case 49302:
                case 49303:
                case 49310:
                case 49311:
                case 49314:
                case 49315:
                case 49318:
                case 49319:
                case 49322:
                case 49323:
                case 52245:
                case 58396:
                case 58397:
                case 58398:
                case 58399:
                    return true;
                default:
                    return false;
            }
        }

        public static bool AreCompatibleParameters( DHParameters a, DHParameters b ) => a.P.Equals( b.P ) && a.G.Equals( b.G );

        public static byte[] CalculateDHBasicAgreement(
          DHPublicKeyParameters publicKey,
          DHPrivateKeyParameters privateKey )
        {
            DHBasicAgreement dhBasicAgreement = new DHBasicAgreement();
            dhBasicAgreement.Init( privateKey );
            return BigIntegers.AsUnsignedByteArray( dhBasicAgreement.CalculateAgreement( publicKey ) );
        }

        public static AsymmetricCipherKeyPair GenerateDHKeyPair(
          SecureRandom random,
          DHParameters dhParams )
        {
            DHBasicKeyPairGenerator keyPairGenerator = new DHBasicKeyPairGenerator();
            keyPairGenerator.Init( new DHKeyGenerationParameters( random, dhParams ) );
            return keyPairGenerator.GenerateKeyPair();
        }

        public static DHPrivateKeyParameters GenerateEphemeralClientKeyExchange(
          SecureRandom random,
          DHParameters dhParams,
          Stream output )
        {
            AsymmetricCipherKeyPair dhKeyPair = GenerateDHKeyPair( random, dhParams );
            WriteDHParameter( ((DHPublicKeyParameters)dhKeyPair.Public).Y, output );
            return (DHPrivateKeyParameters)dhKeyPair.Private;
        }

        public static DHPrivateKeyParameters GenerateEphemeralServerKeyExchange(
          SecureRandom random,
          DHParameters dhParams,
          Stream output )
        {
            AsymmetricCipherKeyPair dhKeyPair = GenerateDHKeyPair( random, dhParams );
            new ServerDHParams( (DHPublicKeyParameters)dhKeyPair.Public ).Encode( output );
            return (DHPrivateKeyParameters)dhKeyPair.Private;
        }

        public static DHParameters ValidateDHParameters( DHParameters parameters )
        {
            BigInteger p = parameters.P;
            BigInteger g = parameters.G;
            if (!p.IsProbablePrime( 2 ))
                throw new TlsFatalAlert( 47 );
            if (g.CompareTo( Two ) < 0 || g.CompareTo( p.Subtract( Two ) ) > 0)
                throw new TlsFatalAlert( 47 );
            return parameters;
        }

        public static DHPublicKeyParameters ValidateDHPublicKey( DHPublicKeyParameters key )
        {
            DHParameters dhParameters = ValidateDHParameters( key.Parameters );
            BigInteger y = key.Y;
            if (y.CompareTo( Two ) < 0 || y.CompareTo( dhParameters.P.Subtract( Two ) ) > 0)
                throw new TlsFatalAlert( 47 );
            return key;
        }

        public static BigInteger ReadDHParameter( Stream input ) => new BigInteger( 1, TlsUtilities.ReadOpaque16( input ) );

        public static void WriteDHParameter( BigInteger x, Stream output ) => TlsUtilities.WriteOpaque16( BigIntegers.AsUnsignedByteArray( x ), output );
    }
}
