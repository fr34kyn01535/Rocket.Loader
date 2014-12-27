using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rocket.Patches
{
    public static class PatchHelper
    {
        public static void Unlock(FieldDefinition f, string name = null)
        {
            f.IsPublic = true;
            f.IsPrivate = false;
            if (!String.IsNullOrEmpty(name))
            {
                f.Name = name;
            }
        }

        internal static void Unlock(MethodDefinition m, string name = null)
        {
            m.IsPublic = true;
            m.IsPrivate = false;
            if (!String.IsNullOrEmpty(name))
            {
                m.Name = name;
            }
        }



        /// <summary>
        /// Unlocks first occurence of type that matches a specifiy typename 
        /// </summary>
        /// <param name="t"></param>
        /// <param name="typeToUnlock"></param>
        /// <param name="name"></param>
        public static void UnlockByType(TypeDefinition t, string typeToUnlock, string name = null, int index = 0)
        {
            FieldDefinition[] fields = t.Fields.Where(f => f.ToString().ToLower().Contains(typeToUnlock.ToLower())).ToArray();

            if (fields.Count() >= index + 1 && fields[index] != null)
            {
                Unlock(fields[index], name);
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine("Warning: could not find " + name);
                Console.ForegroundColor = ConsoleColor.White;
            }
        }

        /// <summary>
        /// Unlocks all occurence specified of type that matches a specifiy typename and sets the name in chronological order
        /// </summary>
        /// <param name="t"></param>
        /// <param name="typeToUnlock"></param>
        /// <param name="name"></param>
        public static void UnlockByType(TypeDefinition t, string typeToUnlock, string[] names)
        {
            FieldDefinition[] fields = t.Fields.Where(f => f.ToString().ToLower().Contains(typeToUnlock.ToLower())).ToArray();
            if (fields != null)
            {
                for (int i = 0; i < names.Length; i++)
                {
                    Unlock(fields[i], names[i]);
                }
            }
        }

    }
}
