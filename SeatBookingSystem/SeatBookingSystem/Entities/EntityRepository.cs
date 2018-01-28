using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNet.Identity.EntityFramework;

namespace SeatBookingSystem.Entities
{
    /// <summary>
    /// The repository of entity.
    /// </summary>
    /// <typeparam name="TEntity">The entity</typeparam>
    public class EntityRepository<TEntity> where TEntity : class
    {
        private Lazy<SeatBookingContext> lazyDbContext;

        /// <summary>
        /// The entity repository
        /// </summary>
        /// <param name="context"></param>
        public EntityRepository(SeatBookingContext context)
        {
            lazyDbContext = new Lazy<SeatBookingContext>(() => context);
        }
         
        /// <summary>
        /// Gets all entities.
        /// </summary>
        /// <returns>All the entities</returns>
        public List<TEntity> GetAll()
        {
            return this.lazyDbContext.Value.Set<TEntity>().ToList();
        }

        /// <summary>
        /// Gets the entities.
        /// </summary>
        /// <param name="candidate">The candidate.</param>
        /// <returns>The entities</returns>
        public List<TEntity> GetEntities(Func<TEntity, bool> candidate)
        {
            return this.GetAll().Where(candidate).ToList();
        }

        /// <summary>
        /// Gets the single entity.
        /// </summary>
        /// <param name="candidate">The candidate.</param>
        /// <returns>The single entity</returns>
        public TEntity GetSingleEntity(Func<TEntity, bool> candidate)
        {
            return this.GetAll().Where(candidate).FirstOrDefault();
        }

        /// <summary>
        /// Adds the entity.
        /// </summary>
        /// <param name="entity">The entity.</param>
        public void AddEntity(TEntity entity)
        {
            this.lazyDbContext.Value.Set<TEntity>().Add(entity);
            this.SaveDbChanges();
        }

        /// <summary>
        /// Adds the entities.
        /// </summary>
        /// <param name="entities">The entities.</param>
        public void AddEntites(IEnumerable<TEntity> entities)
        {
            this.lazyDbContext.Value.Set<TEntity>().AddRange(entities);
            this.SaveDbChanges();
        }

        /// <summary>
        /// Removes the entity.
        /// </summary>
        /// <param name="entity">Delete the entity</param>
        public void RemoveEntity(TEntity entity)
        {
            this.lazyDbContext.Value.Set<TEntity>().Remove(entity);
            this.SaveDbChanges();
        }

        /// <summary>
        /// Removes the entities.
        /// </summary>
        /// <param name="entities">The entities</param>
        public void RemoveEntities(IEnumerable<TEntity> entities)
        {
            this.lazyDbContext.Value.Set<TEntity>().RemoveRange(entities);
            this.SaveDbChanges();
        }

        /// <summary>
        /// Creates the entity repository.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>The entity repository</returns>
        public static EntityRepository<TEntity> Create(SeatBookingContext context)
        {
            return new EntityRepository<TEntity>(context);
        }

        /// <summary>
        /// Saves the database changes
        /// </summary>
        private void SaveDbChanges()
        {
            this.lazyDbContext.Value.SaveChanges();
        } 
    }
}