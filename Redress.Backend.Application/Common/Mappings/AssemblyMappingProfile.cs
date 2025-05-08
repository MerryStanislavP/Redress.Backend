using System;
using System.Linq;
using System.Reflection;
using AutoMapper;

namespace Redress.Backend.Application.Common.Mappings
{
    public class AssemblyMappingProfile : Profile
    {
        public AssemblyMappingProfile()
        {
            ApplyMappingsFromAssembly(Assembly.GetExecutingAssembly());
        }

        private void ApplyMappingsFromAssembly(Assembly assembly)
        {
            var types = assembly.GetExportedTypes()
                .Where(type => type.IsClass && !type.IsAbstract && typeof(Profile).IsAssignableFrom(type))
                .ToList();

            foreach (var type in types)
            {
                var instance = Activator.CreateInstance(type);
                if (instance != null)
                {
                    ApplyMappingsFromInstance(instance);
                }
            }
        }

        private void ApplyMappingsFromInstance(object instance)
        {
            var methodInfo = instance.GetType().GetMethod("Configure");
            methodInfo?.Invoke(instance, new object[] { this });
        }
    }
}
