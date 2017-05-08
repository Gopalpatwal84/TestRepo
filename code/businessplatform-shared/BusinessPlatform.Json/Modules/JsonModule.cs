namespace BusinessPlatform.Json.Modules
{
    using System;
    using System.Linq;
    using Acom.IO.FileReaders;
    using Acom.IO.Json;
    using Autofac;

    public class JsonModule : Module
    {
        public static Type[] GetJsonEntityTypes()
        {
            var loaderTypes = typeof(CulturesJsonLoader).Assembly
                .GetExportedTypes()
                .Where(x => x.BaseType != null && x.BaseType.IsGenericType && x.BaseType.GetGenericTypeDefinition() == typeof(JsonLoader<>))
                .Select(x => x.BaseType.GetGenericArguments().First())
                .ToArray();

            return loaderTypes;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterAssemblyTypes(ThisAssembly)
                .AsClosedTypesOf(typeof(JsonLoader<>))
                .SingleInstance();

            builder.Register(x => new EmbeddedResourceFileReader(typeof(CulturesJsonLoader).Namespace + ".Data", ThisAssembly))
                .AsImplementedInterfaces()
                .SingleInstance();

            var repositoryTypes = GetJsonEntityTypes()
                .Select(x => typeof(JsonRepository<>).MakeGenericType(x))
                .ToArray();

            builder.RegisterTypes(repositoryTypes).AsImplementedInterfaces().SingleInstance();
        }
    }
}
