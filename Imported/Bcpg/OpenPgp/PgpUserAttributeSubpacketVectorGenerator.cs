// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Bcpg.OpenPgp.PgpUserAttributeSubpacketVectorGenerator
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Bcpg.Attr;
using Org.BouncyCastle.Utilities;
using System.Collections;

namespace Org.BouncyCastle.Bcpg.OpenPgp
{
    public class PgpUserAttributeSubpacketVectorGenerator
    {
        private IList list = Platform.CreateArrayList();

        public virtual void SetImageAttribute( ImageAttrib.Format imageType, byte[] imageData )
        {
            if (imageData == null)
                throw new ArgumentException( "attempt to set null image", nameof( imageData ) );
            this.list.Add( new ImageAttrib( imageType, imageData ) );
        }

        public virtual PgpUserAttributeSubpacketVector Generate()
        {
            UserAttributeSubpacket[] packets = new UserAttributeSubpacket[this.list.Count];
            for (int index = 0; index < this.list.Count; ++index)
                packets[index] = (UserAttributeSubpacket)this.list[index];
            return new PgpUserAttributeSubpacketVector( packets );
        }
    }
}
