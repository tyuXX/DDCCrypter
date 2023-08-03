// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Crypto.Tls.DefaultTlsSrpGroupVerifier
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Crypto.Agreement.Srp;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Utilities;
using System.Collections;

namespace Org.BouncyCastle.Crypto.Tls
{
    public class DefaultTlsSrpGroupVerifier : TlsSrpGroupVerifier
    {
        protected static readonly IList DefaultGroups = Platform.CreateArrayList();
        protected readonly IList mGroups;

        static DefaultTlsSrpGroupVerifier()
        {
            DefaultGroups.Add( Srp6StandardGroups.rfc5054_1024 );
            DefaultGroups.Add( Srp6StandardGroups.rfc5054_1536 );
            DefaultGroups.Add( Srp6StandardGroups.rfc5054_2048 );
            DefaultGroups.Add( Srp6StandardGroups.rfc5054_3072 );
            DefaultGroups.Add( Srp6StandardGroups.rfc5054_4096 );
            DefaultGroups.Add( Srp6StandardGroups.rfc5054_6144 );
            DefaultGroups.Add( Srp6StandardGroups.rfc5054_8192 );
        }

        public DefaultTlsSrpGroupVerifier()
          : this( DefaultGroups )
        {
        }

        public DefaultTlsSrpGroupVerifier( IList groups ) => this.mGroups = groups;

        public virtual bool Accept( Srp6GroupParameters group )
        {
            foreach (Srp6GroupParameters mGroup in (IEnumerable)this.mGroups)
            {
                if (this.AreGroupsEqual( group, mGroup ))
                    return true;
            }
            return false;
        }

        protected virtual bool AreGroupsEqual( Srp6GroupParameters a, Srp6GroupParameters b )
        {
            if (a == b)
                return true;
            return this.AreParametersEqual( a.N, b.N ) && this.AreParametersEqual( a.G, b.G );
        }

        protected virtual bool AreParametersEqual( BigInteger a, BigInteger b ) => a == b || a.Equals( b );
    }
}
