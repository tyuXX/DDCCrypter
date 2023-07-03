using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace DDCCrypter
{
    public static class Engine
    {
        public static string ArgProcess( List<string> sargs)
        {
            ArgStore args = new ArgStore(true);
            foreach (string str in sargs)
            {
                string[] strs = str.Split( new char[] { '=' } ,2);
                if(strs.Length == 2)
                {
                    args.Add( new Arg( strs[0], strs[1] ) );
                }
            }
            if (sargs.Contains( "trim" ))
            {
                args.SetArgValue( "estring" ,args.GetArgValue( "estring" ).Replace(" ",string.Empty).Replace("\n",string.Empty) );
            }
            return Selector(args);
        }
        private static string Selector( ArgStore args )
        {
            switch('E' + args.GetArgValue("type")){
                case nameof(EMD5):
                    {
                        return EMD5(args);
                    }
                case nameof(ESHA256):
                    {
                        return ESHA256(args);
                    }
                case nameof(ESHA1):
                    {
                        return ESHA1(args);
                    }
                case nameof(ESHA384):
                    {
                        return ESHA384(args);
                    }
                case nameof(ESHA512):
                    {
                        return ESHA512(args);
                    }
                case nameof(EBASE64):
                    {
                        return EBASE64(args);
                    }
                case nameof(EBİNARY):
                    {
                        return EBİNARY(args);
                    }
                case nameof(EASCIITOUTF8):
                    {
                        return EASCIITOUTF8(args);
                    }
                case nameof(EUTF8TOASCII):
                    {
                        return EUTF8TOASCII(args);
                    }
                default:
                    {
                        return "";
                    }
            }
        }
        private static string EBASE64(ArgStore args)
        {
            if (bool.Parse( args.GetArgValue( "do" ) )) 
            {
                return Convert.ToBase64String(Encoding.UTF8.GetBytes(args.GetArgValue( "estring" )));
            }
            return Encoding.UTF8.GetString( Convert.FromBase64String( args.GetArgValue( "estring" ) ) );
        }
        private static string EBİNARY(ArgStore args)
        {
            if (bool.Parse( args.GetArgValue( "do" ) )) 
            {

            }
            return "";
        }
        private static string EASCIITOUTF8(ArgStore args)
        {
            return Encoding.UTF8.GetString( Encoding.ASCII.GetBytes( args.GetArgValue( "estring" ) ) );
        }
        private static string EUTF8TOASCII(ArgStore args)
        {
            return Encoding.ASCII.GetString( Encoding.UTF8.GetBytes( args.GetArgValue( "estring" ) ) );
        }
        private static string EMD5(ArgStore args)
        {
            if (bool.Parse( args.GetArgValue( "do" ) ))
            {
                byte[] data = Encoding.UTF8.GetBytes( args.GetArgValue( "estring" ) );
                using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
                {
                    byte[] keys = md5.ComputeHash( Encoding.UTF8.GetBytes( args.GetArgValue( "hash" ) ) );
                    using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                    {
                        ICryptoTransform transform = tripDes.CreateEncryptor();
                        byte[] results = transform.TransformFinalBlock( data, 0, data.Length );
                        return Convert.ToBase64String( results, 0, results.Length );
                    }
                }
            }
            else
            {
                byte[] data = Convert.FromBase64String( args.GetArgValue( "estring" ) );
                using (MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider())
                {
                    byte[] keys = md5.ComputeHash( Encoding.UTF8.GetBytes( args.GetArgValue( "hash" ) ) );
                    using (TripleDESCryptoServiceProvider tripDes = new TripleDESCryptoServiceProvider() { Key = keys, Mode = CipherMode.ECB, Padding = PaddingMode.PKCS7 })
                    {
                        ICryptoTransform transform = tripDes.CreateDecryptor();
                        byte[] results = transform.TransformFinalBlock( data, 0, data.Length );
                        return Encoding.UTF8.GetString( results );
                    }
                }
            }
        }
        private static string ESHA256(ArgStore args)
        {
            if (!bool.Parse( args.GetArgValue("do"))){ return ""; }
            return BitConverter.ToString( new SHA256Managed().ComputeHash( Encoding.UTF8.GetBytes( args.GetArgValue( "estring" ) ) ) ).Replace( "-", string.Empty );
        }
        private static string ESHA1(ArgStore args)
        {
            if (!bool.Parse( args.GetArgValue("do"))){ return ""; }
            return BitConverter.ToString( new SHA1Managed().ComputeHash( Encoding.UTF8.GetBytes( args.GetArgValue( "estring" ) ) ) ).Replace( "-", string.Empty );
        }
        private static string ESHA384(ArgStore args)
        {
            if (!bool.Parse( args.GetArgValue("do"))){ return ""; }
            return BitConverter.ToString( new SHA384Managed().ComputeHash( Encoding.UTF8.GetBytes( args.GetArgValue( "estring" ) ) ) ).Replace( "-", string.Empty );
        }
        private static string ESHA512(ArgStore args)
        {
            if (!bool.Parse( args.GetArgValue("do"))){ return ""; }
            return BitConverter.ToString( new SHA512Managed().ComputeHash( Encoding.UTF8.GetBytes( args.GetArgValue( "estring" ) ) ) ).Replace( "-", string.Empty );
        }
    }
    public struct Arg
    {
        public Arg(string arg,string value)
        {
            this.arg = arg;
            this.value = value;
        }
        public string arg;
        public string value;
    }
    public struct ArgStore
    {
        public string GetArgValue(string Name)
        {
            foreach (Arg arg in args)
            {
                if(arg.arg == Name)
                {
                    return arg.value;
                }
            }
            return "CFA";
        }
        public Arg GetArg(string Name)
        {
            foreach (Arg arg in args)
            {
                if(arg.arg == Name)
                {
                    return arg;
                }
            }
            return new Arg();
        }
        public void SetArgValue(string Name,string value)
        {
            args[args.IndexOf( GetArg( Name ) )] = new Arg( GetArg( Name ).arg ,value);
        }
        public void Add(Arg arg)
        {
            args.Add(arg);
        }
        public ArgStore(List<Arg> args)
        {
            this.args = args;
        }
        public ArgStore(bool gen = true)
        {
            args = new List<Arg> { };
        }
        public Array GetArray { get { return args.ToArray(); } }
        private List<Arg> args;
    }
}
