using FluentNHibernate.Mapping;

namespace AccessControl.Data.Mappings
{
    internal static class MappingExtensions
    {
        public static IdentityPart HiLo<TEntity>(this IdentityGenerationStrategyBuilder<IdentityPart> generatedBy)
            where TEntity : class
        {
            return generatedBy.HiLo("HiLo", "NextHi", "100", $"Entity = '{typeof(TEntity).Name}'");
        }
    }
}