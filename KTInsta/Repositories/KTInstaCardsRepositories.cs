using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading;
using KTInsta.Entities;

namespace KTInsta.Repositories
{
    public class KTInstaCardsRepositories<TEntity> : IDisposable, IKTInstaCradsRepositories<TEntity> where TEntity : BaseModel
    {
        protected DbContext DbContext;

        private DbSet<TEntity> entityDbSet { get; set; }

        private DbSet<TEntity> EntityDbSet
        {
            get
            {
                ValidateCurrentUser();
                return entityDbSet;
            }
        }

        /// <summary>
        ///The DBSet scoped/filtered to what the person can access
        /// </summary>
        private IQueryable<TEntity> queryableEntitySet { get; set; }

        /// <summary>
        /// Provides access to the DBSet to query pre filtered items
        /// </summary>
        protected IQueryable<TEntity> QueryableEntitySet
        {
            get
            {
                ValidateCurrentUser();
                return queryableEntitySet;
            }
        }


        private Func<TEntity, bool> matches { get; set; }

        private Func<TEntity, bool> Matches
        {
            get
            {
                ValidateCurrentUser();
                return matches;
            }
        }

        private void ValidateCurrentUser()
        {
            if (userName != null)
            {
                if (Thread.CurrentPrincipal == null || Thread.CurrentPrincipal.Identity == null)
                    throw new ApplicationException("To access this repository it must be running under the context of an authenticated user");

                if (userName != Thread.CurrentPrincipal.Identity.Name)
                    throw new ApplicationException("This repository has been created for '" + userName + "' but the current user is '" + Thread.CurrentPrincipal.Identity.Name + "'. This is usually due to the creation of a repository or manager before impersonation.");
            }
        }

        private bool ThrowUnauthorizedOnMatchFail { get; set; }

        /// <summary>
        /// This is the username of the user this repository was set up for, if applicable
        /// </summary>
        private string userName { get; set; }

        [Obsolete("Use the RepositoryFactory to create a repository")]
        public KTInstaCardsRepositories()
            : this(null, null, true)
        {

        }

        internal KTInstaCardsRepositories(String userName, Expression<Func<TEntity, bool>> filterExpression, bool throwUnauthorizedOnMatchFail = true, bool autoDetectChangesEnabled = true)
        {
            DbContext = new KTInstaContext();
            //Context.Configuration.AutoDetectChangesEnabled = false; //perf fix

            entityDbSet = DbContext.Set<TEntity>();
            // EntityDbQuery = EntityDbSet.AsNoTracking();

            this.userName = userName;

            // If applicable, apply filters
            if (filterExpression != null)
            {
                queryableEntitySet = EntityDbSet.Where(filterExpression);
                matches = filterExpression.Compile();
                ThrowUnauthorizedOnMatchFail = throwUnauthorizedOnMatchFail;
            }
            else
            {
                queryableEntitySet = EntityDbSet;
                matches = entity => true; // No filtering, always matches
                ThrowUnauthorizedOnMatchFail = false;
            }
        }

        public TEntity Create()
        {
            return EntityDbSet.Create();
        }

        public T Create<T>() where T : BaseModel
        {
            return DbContext.Set<T>().Create();
        }

        /// <summary>
        ///  Searches on all values the user can 'see' according to auth rules
        /// </summary>
        public virtual IEnumerable<TEntity> Get(
            Expression<Func<TEntity, bool>> filter,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {

            IQueryable<TEntity> query = QueryableEntitySet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                query = orderBy(query);
            }

            var executedQuery = query.ToList();

            return executedQuery;
        }

        /// <summary>
        /// Returns the maximum value the current user can 'see' according to auth rules
        /// </summary>
        public virtual TVal Max<TVal>(Expression<Func<TEntity, TVal>> func)
        {
            var result = QueryableEntitySet.Max(func);
            return result;
        }

        [Obsolete]
        public TEntity FirstOrDefaultFunc(Func<TEntity, bool> predicate = null)
        {

            TEntity result;
            if (predicate != null)
                result = QueryableEntitySet.FirstOrDefault(predicate);
            else
                result = QueryableEntitySet.FirstOrDefault();

            return result;
        }

        /// <summary>
        /// Returns the first value that the user can 'see' according to auth rules
        /// </summary>
        public virtual TEntity FirstOrDefault(Expression<Func<TEntity, bool>> predicate = null)
        {

            TEntity result;
            if (predicate != null)
                result = QueryableEntitySet.FirstOrDefault(predicate);
            else
                result = QueryableEntitySet.FirstOrDefault();

            return result;
        }


        public virtual TEntity First(Expression<Func<TEntity, bool>> predicate = null)
        {
            if (predicate != null)
                return QueryableEntitySet.First(predicate);

            return QueryableEntitySet.First();
        }

        public TEntity Single(Expression<Func<TEntity, bool>> predicate)
        {
            var entity = SingleOrDefault(predicate);

            if (entity == null)
                throw new InvalidOperationException();

            return entity;
        }

        /// <summary>
        /// Returns the single value that matches the predicate, null if no match or an exception
        /// if a match is found but the user does not have access to it.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public TEntity SingleOrDefault(Expression<Func<TEntity, bool>> predicate)
        {
            var entity = EntityDbSet.SingleOrDefault(predicate);

            if (entity != null)
            {
                // Check authorization for this id
                if (!Matches(entity))
                    if (ThrowUnauthorizedOnMatchFail)
                        entity = null;
                    else
                        entity = null;
            }

            return entity;
        }

        /// <summary>
        /// Gets all instances of TEntity type.
        /// </summary>
        /// <returns></returns>
        public virtual IEnumerable<TEntity> GetAll()
        {

            var result = QueryableEntitySet.ToList();


            return result;
        }

        public virtual TEntity GetByID(object id)
        {
            var entity = EntityDbSet.Find(id);
            if (entity != null)
            {
                // Check authorization for this id
                if (!Matches(entity))
                    if (ThrowUnauthorizedOnMatchFail)
                        entity = null;
                    else
                        entity = null;
            }

            return entity;
        }

        public virtual void Insert(TEntity entity)
        {
            EntityDbSet.Add(entity);
        }

        public virtual void InsertAndSave(TEntity entity)
        {
            EntityDbSet.Add(entity);
            SaveChanges();
        }

        /// <summary>
        /// With the perf fix, it uses a local dbcontext and releases it after it is done
        /// It also persists the changes after it is done.
        /// </summary>
        /// <param name="entities"></param>
        public virtual void InsertRange(IEnumerable<TEntity> entities)
        {
            EntityDbSet.AddRange(entities);
        }

        public virtual void Delete(object id)
        {
            TEntity entityToDelete = EntityDbSet.Find(id);

            Delete(entityToDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if (DbContext.Entry(entityToDelete).State == EntityState.Detached)
            {
                EntityDbSet.Attach(entityToDelete);
            }
            EntityDbSet.Remove(entityToDelete);
        }

        /// <summary>
        /// Deletes a bucket of entities of any type, in the order they are in the list.
        /// </summary>
        /// <param name="entities"></param>
        public virtual void DeleteEntityList(IEnumerable<BaseModel> entities)
        {
            foreach (BaseModel entity in entities)
            {
                Type entityType = entity.GetType();
                var dbSet = DbContext.Set(entityType);

                EntityState state = DbContext.Entry(entity).State;
                if (state == EntityState.Detached)
                {
                    dbSet.Attach(entity);
                }
                dbSet.Remove(entity);
            }
        }

        public virtual void RemoveRange(IEnumerable<TEntity> entities)
        {
            DbContext.Set<TEntity>().RemoveRange(entities);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            EntityDbSet.Attach(entityToUpdate);
            DbContext.Entry(entityToUpdate).State = EntityState.Modified;
        }

        /// <summary>
        /// Call this if entityToUpdate came from this repo and you are tracking changes. This will only persist to the DB
        /// if there are changes made to this instance of the object
        /// </summary>
        /// <param name="entityToUpdate"></param>
        public virtual void UpdateIfModified(TEntity entityToUpdate)
        {
            EntityDbSet.Attach(entityToUpdate);
        }

        internal class PendingDbChange
        {
            public string State { get; set; }
            public string TableName { get; set; }
            public string RecordID { get; set; }
            public string ColumnName { get; set; }
            public string NewValue { get; set; }
            public string OriginalValue { get; set; }
        }

#if DEBUG
        /// <summary>
        /// This will trace the entites about to be written to the DB.
        /// </summary>
        public string GetEntityChangesToBePersisted()
        {
            List<PendingDbChange> AuditLogs = new List<PendingDbChange>();
            var changeTrack = DbContext.ChangeTracker.Entries().Where(p => p.State == EntityState.Added || p.State == EntityState.Deleted || p.State == EntityState.Modified);
            foreach (var entry in changeTrack)
            {
                if (entry.Entity != null)
                {
                    string entityName = string.Empty;
                    string state = string.Empty;
                    switch (entry.State)
                    {
                        case EntityState.Modified:
                            entityName = ObjectContext.GetObjectType(entry.Entity.GetType()).Name;
                            state = entry.State.ToString();
                            foreach (string prop in entry.OriginalValues.PropertyNames)
                            {
                                object currentValue = entry.CurrentValues[prop];
                                if (currentValue == null)
                                    currentValue = "NULL";
                                object originalValue = entry.OriginalValues[prop];
                                if (originalValue == null)
                                    originalValue = "NULL";

                                if (!currentValue.Equals(originalValue))
                                {
                                    AuditLogs.Add(new PendingDbChange
                                    {
                                        TableName = entityName,
                                        State = state,
                                        ColumnName = prop,
                                        OriginalValue = Convert.ToString(originalValue),
                                        NewValue = Convert.ToString(currentValue),
                                    });
                                }
                            }
                            break;
                        case EntityState.Added:
                            entityName = ObjectContext.GetObjectType(entry.Entity.GetType()).Name;
                            state = entry.State.ToString();
                            foreach (string prop in entry.CurrentValues.PropertyNames)
                            {
                                AuditLogs.Add(new PendingDbChange
                                {
                                    TableName = entityName,
                                    State = state,
                                    ColumnName = prop,
                                    OriginalValue = string.Empty,
                                    NewValue = Convert.ToString(entry.CurrentValues[prop]),
                                });

                            }
                            break;
                        case EntityState.Deleted:
                            entityName = ObjectContext.GetObjectType(entry.Entity.GetType()).Name;
                            state = entry.State.ToString();
                            foreach (string prop in entry.OriginalValues.PropertyNames)
                            {
                                AuditLogs.Add(new PendingDbChange
                                {
                                    TableName = entityName,
                                    State = state,
                                    ColumnName = prop,
                                    OriginalValue = Convert.ToString(entry.OriginalValues[prop]),
                                    NewValue = string.Empty,
                                });

                            }
                            break;
                        default:
                            break;
                    }
                }
            }

            StringWriter sb = new StringWriter();
            sb.WriteLine("Table Name \t\t Column Name \t Action \t Old Value \t New Value");
            sb.WriteLine("--------------------------------------------------------------------------------");
            foreach (PendingDbChange a in AuditLogs)
            {
                if (string.IsNullOrEmpty(a.OriginalValue))
                {
                    a.OriginalValue = "\t\t";
                }
                if (string.IsNullOrEmpty(a.NewValue))
                {
                    a.NewValue = "\t";
                }
                sb.WriteLine(a.TableName + " \t " + a.ColumnName + "\t " + a.State + " \t " + a.OriginalValue + " \t " + a.NewValue);
            }

            return sb.ToString();
        }
#endif

        public int SaveChanges()
        {
            int retVal = 0;
            try
            {
                retVal = DbContext.SaveChanges();
            }
            catch (DbEntityValidationException e)
            {
                StringBuilder sb = new StringBuilder();

                var msgs = e.EntityValidationErrors;
                foreach (var dbEntityValidationResult in msgs)
                {
                    sb.AppendLine("Entity: " + dbEntityValidationResult.Entry.Entity.GetType());

                    foreach (var message in dbEntityValidationResult.ValidationErrors)
                    {
                        string messageString = "Property: " + message.PropertyName + " StatusMessage: " + message.ErrorMessage;
                        sb.AppendLine("  " + messageString);
                    }
                }
            }
            catch (DbUpdateException ex)
            {
                // Get the inner most message
                Exception innerException = ex;

                while (innerException.InnerException != null)
                {
                    innerException = innerException.InnerException;
                }

                throw innerException; // provides more details
            }
            catch (Exception ex)
            {
                throw;
            }

            return retVal;
        }



        /// <summary>
        /// Removes all records from TEntitys table, runs a program generated SQL Query,
        /// this functions is much more performant than DeleteAll() 
        /// </summary>
        public virtual void DeleteAllUsingTruncateQuery()
        {

            ObjectContext objCtx = ((IObjectContextAdapter)DbContext).ObjectContext;
            var tableName = ((IObjectContextAdapter)DbContext).ObjectContext.MetadataWorkspace.GetItems<EntityContainer>(DataSpace.SSpace).First()
                .BaseEntitySets.First(meta => meta.ElementType.Name == typeof(TEntity).Name).Table;

            string command = String.Format("TRUNCATE TABLE [{0}]", tableName);
            objCtx.ExecuteStoreCommand(command);
        }


        /// <summary>
        /// Saves the list of entities specified as an input parameter.
        /// By default, the save operation is made in chunks of 1000 entities for performance reasons.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <returns>true if saving of the list raised an exception.</returns>
        public void BatchInsertList(List<TEntity> entities, int batchSize = 1000)
        {
            bool previousAutoDetectChangesSetting = DbContext.Configuration.AutoDetectChangesEnabled;
            DbContext.Configuration.AutoDetectChangesEnabled = false;

            var start = DateTime.Now;

            while (entities.Count > 0)
            {
                IEnumerable<TEntity> itemsToBeSaved = null;

                if (entities.Count >= batchSize)
                {
                    itemsToBeSaved = entities.Take(batchSize);
                }
                else
                {
                    itemsToBeSaved = entities.Take(entities.Count);
                }

                AddRangeOfEntitiesToContextAndSave(itemsToBeSaved);

                if (entities.Count >= batchSize)
                {
                    entities.RemoveRange(0, batchSize);
                }
                else
                {
                    entities.RemoveRange(0, entities.Count);
                }
            }

            var end = DateTime.Now;
            TimeSpan executionTime = end - start;

            DbContext.Configuration.AutoDetectChangesEnabled = previousAutoDetectChangesSetting;
        }

        /// <summary>
        /// Adds the entities in context as batch and saves them all in the database.
        /// </summary>
        /// <param name="entities">The entities.</param>
        private void AddRangeOfEntitiesToContextAndSave(IEnumerable<TEntity> entities)
        {
            InsertRange(entities);
            int savedItems = this.SaveChanges();
        }

        /// <summary>
        /// Determine if an object exixts in the database
        /// </summary>
        /// <returns>true if the object exists</returns>
        public virtual bool Any(Expression<Func<TEntity, bool>> predicate)
        {

            bool result;
            if (predicate != null)
                result = DbContext.Set<TEntity>().Any(predicate);
            else
                result = DbContext.Set<TEntity>().Any();

            return result;
        }

        public bool AnyFunc(Func<TEntity, bool> predicate)
        {

            bool result;
            if (predicate != null)
                result = DbContext.Set<TEntity>().Any(predicate);
            else
                result = DbContext.Set<TEntity>().Any();

            return result;

        }

        // for disposing stuff correctly
        private volatile bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    DbContext.Dispose();
                }
            }
            this.disposed = true;
            DbContext = null;
        }

        public void Dispose()
        {
            Dispose(true);
            // GC.SuppressFinalize(this); // based on Layton's reco
        }

        // Finalizer for repository class
        ~KTInstaCardsRepositories()
        {
            Dispose(false);
        }

        public IQueryable<TEntity> GetAsQueryable()
        {
            return QueryableEntitySet;
        }

        public DateTime GetDbDateTime()
        {
            var dQuery = DbContext.Database.SqlQuery<DateTime>("select getdate()");
            DateTime dbDate = dQuery.AsEnumerable().First();
            return dbDate;
        }
    }
}

