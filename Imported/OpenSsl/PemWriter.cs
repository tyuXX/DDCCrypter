// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.OpenSsl.PemWriter
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using Org.BouncyCastle.Security;
using Org.BouncyCastle.Utilities.IO.Pem;
using System.IO;

namespace Org.BouncyCastle.OpenSsl
{
    public class PemWriter : Org.BouncyCastle.Utilities.IO.Pem.PemWriter
    {
        public PemWriter( TextWriter writer )
          : base( writer )
        {
        }

        public void WriteObject( object obj )
        {
            try
            {
                this.WriteObject( new MiscPemGenerator( obj ) );
            }
            catch (PemGenerationException ex)
            {
                if (ex.InnerException is IOException)
                    throw (IOException)ex.InnerException;
                throw ex;
            }
        }

        public void WriteObject( object obj, string algorithm, char[] password, SecureRandom random ) => this.WriteObject( new MiscPemGenerator( obj, algorithm, password, random ) );
    }
}
