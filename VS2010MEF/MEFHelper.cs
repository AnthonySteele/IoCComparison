using System;
using System.ComponentModel.Composition.Primitives;
using System.Collections.Generic;
using System.ComponentModel.Composition.ReflectionModel;
using System.ComponentModel.Composition;
using System.Reflection;
using System.Linq;
using System.ComponentModel.Composition.Hosting;
using System.Collections;

namespace IoCComparison
{
    public class ConventionalCatalog : ComposablePartCatalog
    {
        private readonly List<ComposablePartDefinition> parts = new List<ComposablePartDefinition>();

        public void RegisterType<TImplementation>()
        {
            RegisterType<TImplementation, TImplementation>();
        }

        public void RegisterType<TImplementation, TContract>()
        {
            var part = ReflectionModelServices.CreatePartDefinition(
                new Lazy<Type>(() => typeof(TImplementation)),
                false,
                new Lazy<IEnumerable<ImportDefinition>>(() => GetImportDefinitions(typeof(TImplementation))),
                new Lazy<IEnumerable<ExportDefinition>>(() => GetExportDefinitions(typeof(TImplementation), typeof(TContract))),
                new Lazy<IDictionary<string, object>>(() => new Dictionary<string, object>()),
                null);

            this.parts.Add(part);
        }

        private static IEnumerable<ImportDefinition> GetImportDefinitions(Type implementationType)
        {
            var constructors = implementationType.GetConstructors()[0];
            var imports = new List<ImportDefinition>();

            foreach (var param in constructors.GetParameters())
            {
                var cardinality = GetCardinality(param);
                var importType = cardinality == ImportCardinality.ZeroOrMore ? GetCollectionContractType(param.ParameterType) : param.ParameterType;
                var currentParam = param;


                imports.Add(
                    ReflectionModelServices.CreateImportDefinition(
                        new Lazy<ParameterInfo>(() => currentParam),
                        AttributedModelServices.GetContractName(importType),
                        AttributedModelServices.GetTypeIdentity(importType),
                        Enumerable.Empty<KeyValuePair<string, Type>>(),
                        cardinality,
                        CreationPolicy.Any,
                        null));

            }
            return imports.ToArray();
        }

        private static ImportCardinality GetCardinality(ParameterInfo param)
        {
            if (typeof(IEnumerable).IsAssignableFrom(param.ParameterType))
            {
                return ImportCardinality.ZeroOrMore;
            }
            
            return ImportCardinality.ExactlyOne;
        }

        //This is hacky! Needs to be cleaned up as it makes many assumptions.
        private static Type GetCollectionContractType(Type collectionType)
        {
            var itemType = collectionType.GetGenericArguments().First();
            var contractType = itemType.GetGenericArguments().First();
            return contractType;
        }

        private static IEnumerable<ExportDefinition> GetExportDefinitions(Type implementationType, Type contractType)
        {
            var lazyMember = new LazyMemberInfo(implementationType);
            var contracName = AttributedModelServices.GetContractName(contractType);
            var metadata = new Lazy<IDictionary<string, object>>(() =>
            {
                var md = new Dictionary<string, object>();
                md.Add(CompositionConstants.ExportTypeIdentityMetadataName, AttributedModelServices.GetTypeIdentity(contractType));
                return md;
            });

            return new[] { ReflectionModelServices.CreateExportDefinition(lazyMember, contracName, metadata, null) };
        }

        public override IQueryable<ComposablePartDefinition> Parts
        {
            get
            {
                return this.parts.AsQueryable();
            }
        }

        public override IEnumerable<Tuple<ComposablePartDefinition, ExportDefinition>> GetExports(ImportDefinition definition)
        {
            return base.GetExports(definition);
        }
    }

}
