using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Rocket.RocketLoader
{
    public class PatchHelper
    {
        public TypeDefinition Type;

        public PatchHelper(string unturnedTypeName)
        {
            Type = RocketLoader.UnturnedAssembly.MainModule.GetType(unturnedTypeName);
        }
		private static void unlock(FieldDefinition f)
		{
			unlock (f, null);
		}
        private static void unlock(FieldDefinition f, string name )
        {
            f.IsPublic = true;
            f.IsPrivate = false;
            f.IsInitOnly = false;
            if (!String.IsNullOrEmpty(name))
            {
                f.Name = name;
            }
        }

        public MethodDefinition GetMethod(string name)
        {
            return Type.Methods.Where(p => p.Name.ToLower() == name.ToLower()).FirstOrDefault();
        }

        public bool RemoveMethod(string name)
        {
            return Type.Methods.Remove(Type.Methods.Where(p => p.Name.ToLower() == name.ToLower()).FirstOrDefault());
        }

        public FieldDefinition GetField(string name)
        {
            return Type.Fields.Where(p => p.Name.ToLower() == name.ToLower()).FirstOrDefault();
        }

        public FieldDefinition[] GetFieldsByType(Type typeToUnlock)
        {
            return Type.Fields.Where(p => p.FieldType.FullName.Replace('/', '+') == typeToUnlock.FullName).ToArray();
        }

        public FieldDefinition[] GetFieldsByType(string typeToUnlock)
        {
            List<FieldDefinition> fields = Type.Fields.ToList();
            List<FieldDefinition> outFields = new List<FieldDefinition>();

            foreach (FieldDefinition field in fields)
            {
                string fieldname = field.FieldType.FullName;
                fieldname = fieldname.Replace("Steam/", "").Replace("System.Collections.Generic.List", "List").Replace("UnityEngine.", "").Replace("List`1", "List").Replace("SDG.", "");
                fieldname = fieldname.Split('.')[fieldname.Split('.').Count() - 1];
                if (fieldname.ToLower() == typeToUnlock.ToLower())
                {
                    outFields.Add(field);
                }
            }

            return outFields.ToArray();
        }
		private static void unlock(MethodDefinition m)
		{
			unlock( m,null);
		}
        private static void unlock(MethodDefinition m, string name)
        {
            m.IsPublic = true;
            m.IsPrivate = false;
            if (!String.IsNullOrEmpty(name))
            {
                m.Name = name;
            }
        }
		
		public void UnlockFieldByType(Type typeToUnlock)
		{
			UnlockFieldByType (typeToUnlock, null, 0);
		}

		public void UnlockFieldByType(Type typeToUnlock, string name )
		{
			UnlockFieldByType (typeToUnlock, name, 0);
		}
        /// <summary>
        /// Unlocks field with a type that matches a specifiy typen
        /// </summary>
        /// <param name="t"></param>
        /// <param name="typeToUnlock"></param>
        /// <param name="name"></param>
        public void UnlockFieldByType(Type typeToUnlock, string name , int index)
        {
            FieldDefinition[] fields = GetFieldsByType(typeToUnlock);

            if (fields.Count() >= index + 1 && fields[index] != null)
            {
                unlock(fields[index], name);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Warning: could not find " + name);
#if DEBUG
                Debugger.Break();
#endif
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

		public void UnlockFieldByType(string typeToUnlock)
		{
			UnlockFieldByType (typeToUnlock,null,0);
		}

		public void UnlockFieldByType(string typeToUnlock, string name)
		{
			UnlockFieldByType (typeToUnlock,name,0);
		}
        /// <summary>
        /// Unlocks field with a type that matches a specifiy typename
        /// </summary>
        /// <param name="t"></param>
        /// <param name="typeToUnlock"></param>
        /// <param name="name"></param>
        public void UnlockFieldByType(string typeToUnlock, string name , int index)
        {
            FieldDefinition[] fields = GetFieldsByType(typeToUnlock);

            if (fields.Count() >= index + 1 && fields[index] != null)
            {
                unlock(fields[index], name);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Warning: could not find " + name);
#if DEBUG
                Debugger.Break();
#endif
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
		public void UnlockFieldByName(string nameToUnlock)
		{
			UnlockFieldByName (nameToUnlock);
		}
        /// <summary>
        /// Unlocks field that matches a specifiy name
        /// </summary>
        /// <param name="t"></param>
        /// <param name="typeToUnlock"></param>
        /// <param name="name"></param>
        public void UnlockFieldByName(string nameToUnlock, string name)
        {
            FieldDefinition field = GetField(nameToUnlock);

            if (field != null)
            {
                unlock(field, name);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Warning: could not find " + name);
#if DEBUG
                Debugger.Break();
#endif
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
		public void UnlockMethodByName(string nameToUnlock)
		{
			UnlockMethodByName (nameToUnlock);
		}
        /// <summary>
        /// Unlocks method that matches a specifiy name
        /// </summary>
        /// <param name="t"></param>
        /// <param name="typeToUnlock"></param>
        /// <param name="name"></param>
        public void UnlockMethodByName(string nameToUnlock, string name)
        {
            MethodDefinition method = GetMethod(nameToUnlock);

            if (method != null)
            {
                unlock(method, name);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Warning: could not find " + name);
#if DEBUG
                Debugger.Break();
#endif
                Console.ForegroundColor = ConsoleColor.White;
            }
        }
    }
}