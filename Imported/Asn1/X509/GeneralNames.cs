// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.GeneralNames
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Utilities;
using System;
using System.Text;

namespace Org.BouncyCastle.Asn1.X509
{
    public class GeneralNames : Asn1Encodable
    {
        private readonly GeneralName[] names;

        public static GeneralNames GetInstance( object obj )
        {
            switch (obj)
            {
                case null:
                case GeneralNames _:
                    return (GeneralNames)obj;
                case Asn1Sequence _:
                    return new GeneralNames( (Asn1Sequence)obj );
                default:
                    throw new ArgumentException( "unknown object in factory: " + Platform.GetTypeName( obj ), nameof( obj ) );
            }
        }

        public static GeneralNames GetInstance( Asn1TaggedObject obj, bool explicitly ) => GetInstance( Asn1Sequence.GetInstance( obj, explicitly ) );

        public GeneralNames( GeneralName name ) => this.names = new GeneralName[1]
        {
      name
        };

        public GeneralNames( GeneralName[] names ) => this.names = (GeneralName[])names.Clone();

        private GeneralNames( Asn1Sequence seq )
        {
            this.names = new GeneralName[seq.Count];
            for (int index = 0; index != seq.Count; ++index)
                this.names[index] = GeneralName.GetInstance( seq[index] );
        }

        public GeneralName[] GetNames() => (GeneralName[])this.names.Clone();

        public override Asn1Object ToAsn1Object() => new DerSequence( names );

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            string newLine = Platform.NewLine;
            stringBuilder.Append( "GeneralNames:" );
            stringBuilder.Append( newLine );
            foreach (GeneralName name in this.names)
            {
                stringBuilder.Append( "    " );
                stringBuilder.Append( name );
                stringBuilder.Append( newLine );
            }
            return stringBuilder.ToString();
        }
    }
}
