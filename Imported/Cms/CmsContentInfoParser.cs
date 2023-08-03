// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Cms.CmsContentInfoParser
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Utilities;
using System;
using System.IO;

namespace Org.BouncyCastle.Cms
{
    public class CmsContentInfoParser
    {
        protected ContentInfoParser contentInfo;
        protected Stream data;

        protected CmsContentInfoParser( Stream data )
        {
            this.data = data != null ? data : throw new ArgumentNullException( nameof( data ) );
            try
            {
                this.contentInfo = new ContentInfoParser( (Asn1SequenceParser)new Asn1StreamParser( data ).ReadObject() );
            }
            catch (IOException ex)
            {
                throw new CmsException( "IOException reading content.", ex );
            }
            catch (InvalidCastException ex)
            {
                throw new CmsException( "Unexpected object reading content.", ex );
            }
        }

        public void Close() => Platform.Dispose( this.data );
    }
}
