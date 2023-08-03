// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.X509ExtensionsGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System.Collections;

namespace Org.BouncyCastle.Asn1.X509
{
    public class X509ExtensionsGenerator
    {
        private IDictionary extensions = Platform.CreateHashtable();
        private IList extOrdering = Platform.CreateArrayList();

        public void Reset()
        {
            this.extensions = Platform.CreateHashtable();
            this.extOrdering = Platform.CreateArrayList();
        }

        public void AddExtension( DerObjectIdentifier oid, bool critical, Asn1Encodable extValue )
        {
            byte[] derEncoded;
            try
            {
                derEncoded = extValue.GetDerEncoded();
            }
            catch (Exception ex)
            {
                throw new ArgumentException( "error encoding value: " + ex );
            }
            this.AddExtension( oid, critical, derEncoded );
        }

        public void AddExtension( DerObjectIdentifier oid, bool critical, byte[] extValue )
        {
            if (this.extensions.Contains( oid ))
                throw new ArgumentException( "extension " + oid + " already added" );
            this.extOrdering.Add( oid );
            this.extensions.Add( oid, new X509Extension( critical, new DerOctetString( extValue ) ) );
        }

        public bool IsEmpty => this.extOrdering.Count < 1;

        public X509Extensions Generate() => new( this.extOrdering, this.extensions );
    }
}
