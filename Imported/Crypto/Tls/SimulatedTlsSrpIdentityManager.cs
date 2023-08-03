// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.SimulatedTlsSrpIdentityManager
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Agreement.Srp;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Utilities;

namespace Org.BouncyCastle.Crypto.Tls
{
    public class SimulatedTlsSrpIdentityManager : TlsSrpIdentityManager
    {
        private static readonly byte[] PREFIX_PASSWORD = Strings.ToByteArray( "password" );
        private static readonly byte[] PREFIX_SALT = Strings.ToByteArray( "salt" );
        protected readonly Srp6GroupParameters mGroup;
        protected readonly Srp6VerifierGenerator mVerifierGenerator;
        protected readonly IMac mMac;

        public static SimulatedTlsSrpIdentityManager GetRfc5054Default(
          Srp6GroupParameters group,
          byte[] seedKey )
        {
            Srp6VerifierGenerator verifierGenerator = new Srp6VerifierGenerator();
            verifierGenerator.Init( group, TlsUtilities.CreateHash( 2 ) );
            HMac hmac = new HMac( TlsUtilities.CreateHash( 2 ) );
            hmac.Init( new KeyParameter( seedKey ) );
            return new SimulatedTlsSrpIdentityManager( group, verifierGenerator, hmac );
        }

        public SimulatedTlsSrpIdentityManager(
          Srp6GroupParameters group,
          Srp6VerifierGenerator verifierGenerator,
          IMac mac )
        {
            this.mGroup = group;
            this.mVerifierGenerator = verifierGenerator;
            this.mMac = mac;
        }

        public virtual TlsSrpLoginParameters GetLoginParameters( byte[] identity )
        {
            this.mMac.BlockUpdate( PREFIX_SALT, 0, PREFIX_SALT.Length );
            this.mMac.BlockUpdate( identity, 0, identity.Length );
            byte[] numArray1 = new byte[this.mMac.GetMacSize()];
            this.mMac.DoFinal( numArray1, 0 );
            this.mMac.BlockUpdate( PREFIX_PASSWORD, 0, PREFIX_PASSWORD.Length );
            this.mMac.BlockUpdate( identity, 0, identity.Length );
            byte[] numArray2 = new byte[this.mMac.GetMacSize()];
            this.mMac.DoFinal( numArray2, 0 );
            return new TlsSrpLoginParameters( this.mGroup, this.mVerifierGenerator.GenerateVerifier( numArray1, identity, numArray2 ), numArray1 );
        }
    }
}
