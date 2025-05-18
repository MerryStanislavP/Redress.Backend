using System;
using System.Linq;
using System.Reflection;
using AutoMapper;

namespace Redress.Backend.Application.Common.Mappings
{
    public class AssemblyMappingProfileLoader
    {
        public static void ApplyMappingsFromAssembly(IMapperConfigurationExpression config, Assembly assembly)
        {
            var types = assembly.GetExportedTypes()
                .Where(type =>
                    type.IsClass &&
                    !type.IsAbstract &&
                    typeof(Profile).IsAssignableFrom(type) &&
                    type != typeof(AssemblyMappingProfileLoader)) // ⛔ исключаем самого себя
                .ToList();

            foreach (var type in types)
            {
                if (Activator.CreateInstance(type) is Profile profile)
                {
                    config.AddProfile(profile); // 💡 просто добавляем профиль
                }

                // 🔍 если есть метод Configure — вызываем его, если нужно
                var configureMethod = type.GetMethod("Configure");
                if (configureMethod != null)
                {
                    var instance = Activator.CreateInstance(type);
                    configureMethod.Invoke(instance, new object[] { config });
                }
            }
        }
    }
}
