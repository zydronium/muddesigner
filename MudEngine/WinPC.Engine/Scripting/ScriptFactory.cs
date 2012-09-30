﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using MudDesigner.Engine.Core;
using MudDesigner.Engine.Objects;

namespace MudDesigner.Engine.Scripting
{
    public static class ScriptFactory
    {
        private static Assembly assembly;

        //The assembly loaded that will be used.
        private static List<Assembly> assemblyCollection;

        /// <summary>
        /// Adds another assembly to the factories assembly collection.
        /// </summary>
        /// <param name="assembly">provides the name of the assembly, or file name that needs to be loaded.</param>
        public static void AddAssembly(String assembly)
        {
            Assembly a;

            //See if a file exists first with this assembly name.
            a = File.Exists(assembly) ? Assembly.Load(new AssemblyName(assembly)) : Assembly.Load(assembly);

            if (a == null)
                return;

            //Add the assembly to our assembly collection.
            assemblyCollection.Add(a);
        }

        /// <summary>
        /// Adds another assembly to the factories assembly collection.
        /// </summary>
        /// <param name="assembly">Provides a reference to the assembly that will be added to the collection.</param>
        public static void AddAssembly(Assembly assembly)
        {
            //Add the supplied assembly to our AssemblyCollection
            if (assembly != null)
                assemblyCollection.Add(assembly);
        }


        public static object GetScript(string className, params object[] args)
        {
            Type type = null;

            foreach (Assembly assembly in assemblyCollection)
            {
                type = assembly.GetType(className);
                if (type != null)
                    break;
            }

            if (type == null)
                return null;

            object script;
            if (args.Length == 0)
                script = Activator.CreateInstance(type);
            else
                script = Activator.CreateInstance(type, args);
            return script;
        }

        public static Object FindInheritedScripted(String baseScript, params Object[] arguments)
        {
            Type script = typeof(BaseGameObject);
            Boolean foundScript = false;

            if (assemblyCollection.Count == 0)
                return null;

            try
            {
                foreach (var a in assemblyCollection.Where(a => a != null))
                {
                    foreach (var t in a.GetTypes().Where(t => t.BaseType.Name == baseScript))
                    {
                        script = t;
                        foundScript = true;
                        break;
                    }

                    if (foundScript)
                        break;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            try
            {
                Object obj = Activator.CreateInstance(script, arguments);
                return obj;
            }
            catch
            {
                return null;
            }


        }
    }
}