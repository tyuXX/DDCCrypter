// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Cms.CmsCompressedDataParser
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Asn1;
using Org.BouncyCastle.Asn1.Cms;
using Org.BouncyCastle.Utilities.Zlib;
using System.IO;

namespace Org.BouncyCastle.Cms
{
    public class CmsCompressedDataParser : CmsContentInfoParser
    {
        public CmsCompressedDataParser( byte[] compressedData )
          : this( new MemoryStream( compressedData, false ) )
        {
        }

        public CmsCompressedDataParser( Stream compressedData )
          : base( compressedData )
        {
        }

        public CmsTypedStream GetContent()
        {
            try
            {
                ContentInfoParser encapContentInfo = new CompressedDataParser( (Asn1SequenceParser)this.contentInfo.GetContent( 16 ) ).GetEncapContentInfo();
                Asn1OctetStringParser content = (Asn1OctetStringParser)encapContentInfo.GetContent( 4 );
                return new CmsTypedStream( encapContentInfo.ContentType.ToString(), new ZInputStream( content.GetOctetStream() ) );
            }
            catch (IOException ex)
            {
                throw new CmsException( "IOException reading compressed content.", ex );
            }
        }
    }
}
