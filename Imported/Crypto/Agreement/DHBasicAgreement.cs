// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Agreement.DHBasicAgreement
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using System;

namespace Org.BouncyCastle.Crypto.Agreement
{
    public class DHBasicAgreement : IBasicAgreement
    {
        private DHPrivateKeyParameters key;
        private DHParameters dhParams;

        public virtual void Init( ICipherParameters parameters )
        {
            if (parameters is ParametersWithRandom)
                parameters = ((ParametersWithRandom)parameters).Parameters;
            this.key = parameters is DHPrivateKeyParameters ? (DHPrivateKeyParameters)parameters : throw new ArgumentException( "DHEngine expects DHPrivateKeyParameters" );
            this.dhParams = this.key.Parameters;
        }

        public virtual int GetFieldSize() => (this.key.Parameters.P.BitLength + 7) / 8;

        public virtual BigInteger CalculateAgreement( ICipherParameters pubKey )
        {
            if (this.key == null)
                throw new InvalidOperationException( "Agreement algorithm not initialised" );
            DHPublicKeyParameters publicKeyParameters = (DHPublicKeyParameters)pubKey;
            if (!publicKeyParameters.Parameters.Equals( dhParams ))
                throw new ArgumentException( "Diffie-Hellman public key has wrong parameters." );
            return publicKeyParameters.Y.ModPow( this.key.X, this.dhParams.P );
        }
    }
}
