﻿namespace DGBot
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using DSharpPlus.CommandsNext;
    using DSharpPlus.CommandsNext.Attributes;

    public static class PlusExtensions
    {
        public static IEnumerable<Type> GetAllCommandModules()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic)
                .SelectMany(a => a.GetTypes())
                .Where(t => t.BaseType == typeof(BaseCommandModule) && !t.IsAbstract);
        }

        public static IEnumerable<MethodInfo> GetAllCommands()
        {
            return AppDomain.CurrentDomain.GetAssemblies()
                .Where(a => !a.IsDynamic)
                .SelectMany(a => a.GetTypes())
                .Where(t => t.BaseType == typeof(BaseCommandModule))
                .SelectMany(t => t.GetMethods())
                .Where(m => m.GetCustomAttribute<CommandAttribute>() != null);
        }

        public static CommandsNextExtension RegisterAllCommandModules(this CommandsNextExtension cmdExt)
        {
            var cmdModules = GetAllCommandModules();

            foreach (var cmdModule in cmdModules)
            {
                try
                {
                    cmdExt.RegisterCommands(cmdModule);
                }
                catch (Exception e)
                {
                    //Ignored
                }
            }

            return cmdExt;
        }

    }
}