﻿// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Operators.VerifierResult
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;

namespace Org.BouncyCastle.Crypto.Operators
{
    internal class VerifierResult : IVerifier
    {
        private readonly ISigner sig;

        internal VerifierResult( ISigner sig ) => this.sig = sig;

        public bool IsVerified( byte[] signature ) => this.sig.VerifySignature( signature );

        public bool IsVerified( byte[] signature, int off, int length )
        {
            byte[] destinationArray = new byte[length];
            Array.Copy( signature, 0, destinationArray, off, destinationArray.Length );
            return this.sig.VerifySignature( signature );
        }
    }
}
