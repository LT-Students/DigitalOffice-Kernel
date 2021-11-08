using LT.DigitalOffice.Kernel.Attributes.ParseEntity;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

namespace LT.DigitalOffice.Kernel.Middlewares.ParseEntities.Models.Responses
{
    public interface IFindParseEntitiesResponse
    {
        Dictionary<string, List<string>> Entities { get; }

        static object CreateObj()
        {
            Dictionary<string, List<string>> entitiesProperties = new();

            string asmPath = Path.GetDirectoryName(Assembly.GetCallingAssembly().Location);
            string[] files = Directory.GetFiles(asmPath, "*DigitalOffice*.dll");

            List<Assembly> assemblies = new();

            foreach (string fileName in files)
            {
                assemblies.Add(Assembly.LoadFrom(fileName));
            }

            List<Type> parsedEntities = new();

            foreach (Assembly assembly in assemblies)
            {
                parsedEntities.AddRange(assembly.ExportedTypes
                    .Where(
                        t =>
                            t.IsClass
                            && t.IsPublic
                            && t.GetCustomAttribute(typeof(ParseEntityAttribute)) != null)
                    .ToList());
            }

            foreach (Type entity in parsedEntities)
            {
                var attr = entity.GetCustomAttribute<ParseEntityAttribute>();

                List<string> parsedProperties = entity
                    .GetProperties()
                    .Where(p => p.GetCustomAttribute(typeof(IgnoreParseAttribute)) == null)
                    .Select(p => p.Name)
                    .ToList();

                if (parsedEntities != null && parsedProperties.Any())
                {
                    entitiesProperties.Add(entity.Name, parsedProperties);
                }
            }

            return new
            {
                Entities = entitiesProperties
            };
        }
    }
}
