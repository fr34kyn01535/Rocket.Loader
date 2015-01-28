//
// TypeSystem.cs
//
// Author:
//   Jb Evain (jbevain@gmail.com)
//
// Copyright (c) 2008 - 2011 Jb Evain
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
//
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//

using Mono.Cecil.Metadata;
using System;

namespace Mono.Cecil
{
    public abstract class TypeSystem
    {
        private sealed class CoreTypeSystem : TypeSystem
        {
            public CoreTypeSystem(ModuleDefinition module)
                : base(module)
            {
            }

            internal override TypeReference LookupType(string @namespace, string name)
            {
                var type = LookupTypeDefinition(@namespace, name) ?? LookupTypeForwarded(@namespace, name);
                if (type != null)
                    return type;

                throw new NotSupportedException();
            }

            private TypeReference LookupTypeDefinition(string @namespace, string name)
            {
                var metadata = module.MetadataSystem;
                if (metadata.Types == null)
                    Initialize(module.Types);

                return module.Read(new Row<string, string>(@namespace, name), (row, reader) =>
                {
                    var types = reader.metadata.Types;

                    for (int i = 0; i < types.Length; i++)
                    {
                        if (types[i] == null)
                            types[i] = reader.GetTypeDefinition((uint)i + 1);

                        var type = types[i];

                        if (type.Name == row.Col2 && type.Namespace == row.Col1)
                            return type;
                    }

                    return null;
                });
            }

            private TypeReference LookupTypeForwarded(string @namespace, string name)
            {
                if (!module.HasExportedTypes)
                    return null;

                var exported_types = module.ExportedTypes;
                for (int i = 0; i < exported_types.Count; i++)
                {
                    var exported_type = exported_types[i];

                    if (exported_type.Name == name && exported_type.Namespace == @namespace)
                        return exported_type.CreateReference();
                }

                return null;
            }

            private static void Initialize(object obj)
            {
            }
        }

        private sealed class CommonTypeSystem : TypeSystem
        {
            private AssemblyNameReference corlib;

            public CommonTypeSystem(ModuleDefinition module)
                : base(module)
            {
            }

            internal override TypeReference LookupType(string @namespace, string name)
            {
                return CreateTypeReference(@namespace, name);
            }

            public AssemblyNameReference GetCorlibReference()
            {
                if (corlib != null)
                    return corlib;

                const string mscorlib = "mscorlib";

                var references = module.AssemblyReferences;

                for (int i = 0; i < references.Count; i++)
                {
                    var reference = references[i];
                    if (reference.Name == mscorlib)
                        return corlib = reference;
                }

                corlib = new AssemblyNameReference
                {
                    Name = mscorlib,
                    Version = GetCorlibVersion(),
                    PublicKeyToken = new byte[] { 0xb7, 0x7a, 0x5c, 0x56, 0x19, 0x34, 0xe0, 0x89 },
                };

                references.Add(corlib);

                return corlib;
            }

            private Version GetCorlibVersion()
            {
                switch (module.Runtime)
                {
                    case TargetRuntime.Net_1_0:
                    case TargetRuntime.Net_1_1:
                        return new Version(1, 0, 0, 0);

                    case TargetRuntime.Net_2_0:
                        return new Version(2, 0, 0, 0);

                    case TargetRuntime.Net_4_0:
                        return new Version(4, 0, 0, 0);

                    default:
                        throw new NotSupportedException();
                }
            }

            private TypeReference CreateTypeReference(string @namespace, string name)
            {
                return new TypeReference(@namespace, name, module, GetCorlibReference());
            }
        }

        private readonly ModuleDefinition module;

        private TypeReference type_object;
        private TypeReference type_void;
        private TypeReference type_bool;
        private TypeReference type_char;
        private TypeReference type_sbyte;
        private TypeReference type_byte;
        private TypeReference type_int16;
        private TypeReference type_uint16;
        private TypeReference type_int32;
        private TypeReference type_uint32;
        private TypeReference type_int64;
        private TypeReference type_uint64;
        private TypeReference type_single;
        private TypeReference type_double;
        private TypeReference type_intptr;
        private TypeReference type_uintptr;
        private TypeReference type_string;
        private TypeReference type_typedref;

        private TypeSystem(ModuleDefinition module)
        {
            this.module = module;
        }

        internal static TypeSystem CreateTypeSystem(ModuleDefinition module)
        {
            if (module.IsCorlib())
                return new CoreTypeSystem(module);

            return new CommonTypeSystem(module);
        }

        internal abstract TypeReference LookupType(string @namespace, string name);

        private TypeReference LookupSystemType(string name, ElementType element_type)
        {
            var type = LookupType("System", name);
            type.etype = element_type;
            return type;
        }

        private TypeReference LookupSystemValueType(string name, ElementType element_type)
        {
            var type = LookupSystemType(name, element_type);
            type.IsValueType = true;
            return type;
        }

        public IMetadataScope Corlib
        {
            get
            {
                var common = this as CommonTypeSystem;
                if (common == null)
                    return module;

                return common.GetCorlibReference();
            }
        }

        public TypeReference Object
        {
            get { return type_object ?? (type_object = LookupSystemType("Object", ElementType.Object)); }
        }

        public TypeReference Void
        {
            get { return type_void ?? (type_void = LookupSystemType("Void", ElementType.Void)); }
        }

        public TypeReference Boolean
        {
            get { return type_bool ?? (type_bool = LookupSystemValueType("Boolean", ElementType.Boolean)); }
        }

        public TypeReference Char
        {
            get { return type_char ?? (type_char = LookupSystemValueType("Char", ElementType.Char)); }
        }

        public TypeReference SByte
        {
            get { return type_sbyte ?? (type_sbyte = LookupSystemValueType("SByte", ElementType.I1)); }
        }

        public TypeReference Byte
        {
            get { return type_byte ?? (type_byte = LookupSystemValueType("Byte", ElementType.U1)); }
        }

        public TypeReference Int16
        {
            get { return type_int16 ?? (type_int16 = LookupSystemValueType("Int16", ElementType.I2)); }
        }

        public TypeReference UInt16
        {
            get { return type_uint16 ?? (type_uint16 = LookupSystemValueType("UInt16", ElementType.U2)); }
        }

        public TypeReference Int32
        {
            get { return type_int32 ?? (type_int32 = LookupSystemValueType("Int32", ElementType.I4)); }
        }

        public TypeReference UInt32
        {
            get { return type_uint32 ?? (type_uint32 = LookupSystemValueType("UInt32", ElementType.U4)); }
        }

        public TypeReference Int64
        {
            get { return type_int64 ?? (type_int64 = LookupSystemValueType("Int64", ElementType.I8)); }
        }

        public TypeReference UInt64
        {
            get { return type_uint64 ?? (type_uint64 = LookupSystemValueType("UInt64", ElementType.U8)); }
        }

        public TypeReference Single
        {
            get { return type_single ?? (type_single = LookupSystemValueType("Single", ElementType.R4)); }
        }

        public TypeReference Double
        {
            get { return type_double ?? (type_double = LookupSystemValueType("Double", ElementType.R8)); }
        }

        public TypeReference IntPtr
        {
            get { return type_intptr ?? (type_intptr = LookupSystemValueType("IntPtr", ElementType.I)); }
        }

        public TypeReference UIntPtr
        {
            get { return type_uintptr ?? (type_uintptr = LookupSystemValueType("UIntPtr", ElementType.U)); }
        }

        public TypeReference String
        {
            get { return type_string ?? (type_string = LookupSystemType("String", ElementType.String)); }
        }

        public TypeReference TypedReference
        {
            get { return type_typedref ?? (type_typedref = LookupSystemValueType("TypedReference", ElementType.TypedByRef)); }
        }
    }
}