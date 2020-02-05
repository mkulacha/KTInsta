using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using KTInsta.Entities;
using Microsoft.Extensions.Logging;

namespace KTInsta.Repositories
{
    public class KTInstaCardsRepositoriesFactory
    {
        public enum GetActionTypes
        {
            ThrowsUnauthorizedException,
            ReturnsNull
        }

        private class Filter
        {
            public object FilterExpression { get; set; }

            public GetActionTypes Action { get; set; }
        }


        private static volatile Dictionary<Type, List<Filter>> filters;

        private static object syncRoot = new Object();

        static KTInstaCardsRepositoriesFactory()
        {
            lock (syncRoot)
            {
                Reset();
            }
        }


        public static TRepository Get<TRepository, TEntity>(bool autoDetectChangesEnabled = true)
            where TRepository : KTInstaCardsRepositories<TEntity>
            where TEntity : BaseModel
        {
            Expression<Func<TEntity, bool>> filter;
            String username;
            bool throwsUnauthorized;


            filter = null;
            username = null;
            throwsUnauthorized = true;


            Object repo;

            if (typeof(TRepository) == typeof(KTInstaCardsRepositories<TEntity>))
            {
                repo = new KTInstaCardsRepositories<TEntity>(username, filter, throwsUnauthorized, autoDetectChangesEnabled);
            }
            else
            {
                throw new NotImplementedException();
            }
            return repo as TRepository;
        }

        /// <summary>
        /// This will return a repository with the correct filters applied.
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <returns></returns>
        public static KTInstaCardsRepositories<TEntity> Get<TEntity>(bool autoDetectChangesEnabled = true) where TEntity : BaseModel
        {
           
            var entityRepository = Get<KTInstaCardsRepositories<TEntity>, TEntity>(autoDetectChangesEnabled);

            return entityRepository;
        }

        /// <summary>
        /// DO NOT USE. Required for initial login only.
        /// </summary>
        public static KTInstaCardsRepositories<TEntity> GetUnsecured<TEntity>() where TEntity : BaseModel
        {
            return new KTInstaCardsRepositories<TEntity>(null, null);
        }

        /// <summary>
        /// This clears all filters
        /// </summary>
        public static void Reset()
        {
         
            lock (syncRoot)
            {
                filters = new Dictionary<Type, List<Filter>>();
            }
        }
    }
}
