using Microsoft.EntityFrameworkCore.Metadata;
using PetCoffee.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace PetCoffee.Infrastructure.Common.Extensions
{
    public static class SoftDeleteQueryExtension
    {
        public static void AddSoftDeleteQueryFilter(this IMutableEntityType entityData)
        {
            var methodToCall = typeof(SoftDeleteQueryExtension)
                .GetMethod(nameof(GetSoftDeleteFilter), BindingFlags.NonPublic | BindingFlags.Static)
                ?.MakeGenericMethod(entityData.ClrType);

            if (methodToCall == null)
            {
                return;
            }
            var filter = methodToCall.Invoke(null, new object[] { });

            if (filter == null)
            {
                return;
            }

            entityData.SetQueryFilter((LambdaExpression)filter);

            var property = entityData.FindProperty(nameof(BaseAuditableEntity.DeletedAt));

            if (property == null)
            {
                return;
            }

            entityData.AddIndex(property);
        }

        private static LambdaExpression GetSoftDeleteFilter<TEntity>() where TEntity : BaseAuditableEntity
        {
            Expression<Func<TEntity, bool>> filter = x => x.DeletedAt == null;
            return filter;
        }
    }
}
