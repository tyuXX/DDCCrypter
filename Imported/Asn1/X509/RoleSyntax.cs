﻿// Decompiled with JetBrains decompiler
// Type: Org.BouncyCastle.Asn1.X509.RoleSyntax
// Assembly: BouncyCastle.Crypto, Version=1.8.1.0, Culture=neutral, PublicKeyToken=0e99375e54769942
// MVID: 2C1E8153-B25B-4CDE-9676-EEDAF8A00392
// Assembly location: C:\Users\MÜRVET YÜZDEN ŞEN\Downloads\BouncyCastle.Crypto.dll

using System;
using System.Text;

namespace Org.BouncyCastle.Asn1.X509
{
    public class RoleSyntax : Asn1Encodable
    {
        private readonly GeneralNames roleAuthority;
        private readonly GeneralName roleName;

        public static RoleSyntax GetInstance( object obj )
        {
            if (obj is RoleSyntax)
                return (RoleSyntax)obj;
            return obj != null ? new RoleSyntax( Asn1Sequence.GetInstance( obj ) ) : null;
        }

        public RoleSyntax( GeneralNames roleAuthority, GeneralName roleName )
        {
            if (roleName == null || roleName.TagNo != 6 || ((IAsn1String)roleName.Name).GetString().Equals( "" ))
                throw new ArgumentException( "the role name MUST be non empty and MUST use the URI option of GeneralName" );
            this.roleAuthority = roleAuthority;
            this.roleName = roleName;
        }

        public RoleSyntax( GeneralName roleName )
          : this( null, roleName )
        {
        }

        public RoleSyntax( string roleName )
          : this( new GeneralName( 6, roleName == null ? "" : roleName ) )
        {
        }

        private RoleSyntax( Asn1Sequence seq )
        {
            if (seq.Count < 1 || seq.Count > 2)
                throw new ArgumentException( "Bad sequence size: " + seq.Count );
            for (int index = 0; index != seq.Count; ++index)
            {
                Asn1TaggedObject instance = Asn1TaggedObject.GetInstance( seq[index] );
                switch (instance.TagNo)
                {
                    case 0:
                        this.roleAuthority = GeneralNames.GetInstance( instance, false );
                        break;
                    case 1:
                        this.roleName = GeneralName.GetInstance( instance, true );
                        break;
                    default:
                        throw new ArgumentException( "Unknown tag in RoleSyntax" );
                }
            }
        }

        public GeneralNames RoleAuthority => this.roleAuthority;

        public GeneralName RoleName => this.roleName;

        public string GetRoleNameAsString() => ((IAsn1String)this.roleName.Name).GetString();

        public string[] GetRoleAuthorityAsString()
        {
            if (this.roleAuthority == null)
                return new string[0];
            GeneralName[] names = this.roleAuthority.GetNames();
            string[] authorityAsString = new string[names.Length];
            for (int index = 0; index < names.Length; ++index)
            {
                Asn1Encodable name = names[index].Name;
                authorityAsString[index] = !(name is IAsn1String) ? name.ToString() : ((IAsn1String)name).GetString();
            }
            return authorityAsString;
        }

        public override Asn1Object ToAsn1Object()
        {
            Asn1EncodableVector v = new Asn1EncodableVector( new Asn1Encodable[0] );
            if (this.roleAuthority != null)
                v.Add( new DerTaggedObject( false, 0, roleAuthority ) );
            v.Add( new DerTaggedObject( true, 1, roleName ) );
            return new DerSequence( v );
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder( "Name: " + this.GetRoleNameAsString() + " - Auth: " );
            if (this.roleAuthority == null || this.roleAuthority.GetNames().Length == 0)
            {
                stringBuilder.Append( "N/A" );
            }
            else
            {
                string[] authorityAsString = this.GetRoleAuthorityAsString();
                stringBuilder.Append( '[' ).Append( authorityAsString[0] );
                for (int index = 1; index < authorityAsString.Length; ++index)
                    stringBuilder.Append( ", " ).Append( authorityAsString[index] );
                stringBuilder.Append( ']' );
            }
            return stringBuilder.ToString();
        }
    }
}
