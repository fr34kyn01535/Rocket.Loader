using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Rocket.RocketLoader
{
    public class PatchHelper
    {
        private TypeDefinition type;
        public PatchHelper(string unturnedTypeName) {
            type = RocketLoader.UnturnedAssembly.MainModule.GetType(unturnedTypeName);
        }

        private static void unlock(FieldDefinition f, string name = null)
        {
            f.IsPublic = true;
            f.IsPrivate = false;
            if (!String.IsNullOrEmpty(name))
            {
                f.Name = name;
            }
        }


        public MethodDefinition GetMethod(string name)
        {
            return type.Methods.Where(p => p.Name.ToLower() == name.ToLower()).FirstOrDefault();
        }

        public FieldDefinition GetField(string name)
        {
            return type.Fields.Where(p => p.Name.ToLower() == name.ToLower()).FirstOrDefault();
        }
        public FieldDefinition[] GetFieldsByType(Type typeToUnlock)
        {
            return type.Fields.Where(p => p.FieldType.FullName.Replace('/', '+') == typeToUnlock.FullName).ToArray();
        }

        public FieldDefinition[] GetFieldsByType(string typeToUnlock)
        {
            List<FieldDefinition> fields = type.Fields.ToList();
            List<FieldDefinition> outFields = new List<FieldDefinition>();

            foreach (FieldDefinition field in fields) {
                string fieldname = field.FieldType.Name;

                if (field.FieldType.FullName.Contains("System.Collections.Generic.List")) {
                    fieldname = field.FieldType.FullName.Replace("System.Collections.Generic.List`1", "List");
                }
                
                if (fieldname.ToLower() == typeToUnlock.ToLower())
                {
                    outFields.Add(field);
                }
            }

            return outFields.ToArray();
        }

        private static void unlock(MethodDefinition m, string name = null)
        {
            m.IsPublic = true;
            m.IsPrivate = false;
            if (!String.IsNullOrEmpty(name))
            {
                m.Name = name;
            }
        }

        /// <summary>
        /// Unlocks field with a type that matches a specifiy typen
        /// </summary>
        /// <param name="t"></param>
        /// <param name="typeToUnlock"></param>
        /// <param name="name"></param>
        public void UnlockFieldByType(Type typeToUnlock, string name = null, int index = 0)
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

        /// <summary>
        /// Unlocks field with a type that matches a specifiy typename 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="typeToUnlock"></param>
        /// <param name="name"></param>
        public void UnlockFieldByType(string typeToUnlock, string name = null, int index = 0)
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

        /// <summary>
        /// Unlocks field that matches a specifiy name 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="typeToUnlock"></param>
        /// <param name="name"></param>
        public void UnlockFieldByName(string nameToUnlock, string name = null)
        {
            FieldDefinition field = GetField(nameToUnlock);

            if (field!= null)
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

        /// <summary>
        /// Unlocks method that matches a specifiy name 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="typeToUnlock"></param>
        /// <param name="name"></param>
        public void UnlockMethodByName(string nameToUnlock, string name = null)
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
