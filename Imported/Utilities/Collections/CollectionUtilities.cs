// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Utilities.Collections.CollectionUtilities
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;
using System.Collections;
using System.Text;

namespace Org.BouncyCastle.Utilities.Collections
{
    public abstract class CollectionUtilities
    {
        public static void AddRange( IList to, IEnumerable range )
        {
            foreach (object obj in range)
                to.Add( obj );
        }

        public static bool CheckElementsAreOfType( IEnumerable e, Type t )
        {
            foreach (object o in e)
            {
                if (!t.IsInstanceOfType( o ))
                    return false;
            }
            return true;
        }

        public static IDictionary ReadOnly( IDictionary d ) => new UnmodifiableDictionaryProxy( d );

        public static IList ReadOnly( IList l ) => new UnmodifiableListProxy( l );

        public static ISet ReadOnly( ISet s ) => new UnmodifiableSetProxy( s );

        public static string ToString( IEnumerable c )
        {
            StringBuilder stringBuilder = new StringBuilder( "[" );
            IEnumerator enumerator = c.GetEnumerator();
            if (enumerator.MoveNext())
            {
                stringBuilder.Append( enumerator.Current.ToString() );
                while (enumerator.MoveNext())
                {
                    stringBuilder.Append( ", " );
                    stringBuilder.Append( enumerator.Current.ToString() );
                }
            }
            stringBuilder.Append( ']' );
            return stringBuilder.ToString();
        }
    }
}
