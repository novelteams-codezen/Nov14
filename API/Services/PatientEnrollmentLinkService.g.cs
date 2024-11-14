using Nov14.Models;
using Nov14.Data;
using Nov14.Filter;
using Nov14.Entities;
using Nov14.Logger;
using Microsoft.AspNetCore.JsonPatch;
using System.Linq.Expressions;

namespace Nov14.Services
{
    /// <summary>
    /// The patientenrollmentlinkService responsible for managing patientenrollmentlink related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting patientenrollmentlink information.
    /// </remarks>
    public interface IPatientEnrollmentLinkService
    {
        /// <summary>Retrieves a specific patientenrollmentlink by its primary key</summary>
        /// <param name="id">The primary key of the patientenrollmentlink</param>
        /// <returns>The patientenrollmentlink data</returns>
        PatientEnrollmentLink GetById(Guid id);

        /// <summary>Retrieves a list of patientenrollmentlinks based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of patientenrollmentlinks</returns>
        List<PatientEnrollmentLink> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new patientenrollmentlink</summary>
        /// <param name="model">The patientenrollmentlink data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(PatientEnrollmentLink model);

        /// <summary>Updates a specific patientenrollmentlink by its primary key</summary>
        /// <param name="id">The primary key of the patientenrollmentlink</param>
        /// <param name="updatedEntity">The patientenrollmentlink data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, PatientEnrollmentLink updatedEntity);

        /// <summary>Updates a specific patientenrollmentlink by its primary key</summary>
        /// <param name="id">The primary key of the patientenrollmentlink</param>
        /// <param name="updatedEntity">The patientenrollmentlink data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<PatientEnrollmentLink> updatedEntity);

        /// <summary>Deletes a specific patientenrollmentlink by its primary key</summary>
        /// <param name="id">The primary key of the patientenrollmentlink</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The patientenrollmentlinkService responsible for managing patientenrollmentlink related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting patientenrollmentlink information.
    /// </remarks>
    public class PatientEnrollmentLinkService : IPatientEnrollmentLinkService
    {
        private Nov14Context _dbContext;

        /// <summary>
        /// Initializes a new instance of the PatientEnrollmentLink class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public PatientEnrollmentLinkService(Nov14Context dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific patientenrollmentlink by its primary key</summary>
        /// <param name="id">The primary key of the patientenrollmentlink</param>
        /// <returns>The patientenrollmentlink data</returns>
        public PatientEnrollmentLink GetById(Guid id)
        {
            var entityData = _dbContext.PatientEnrollmentLink.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of patientenrollmentlinks based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of patientenrollmentlinks</returns>/// <exception cref="Exception"></exception>
        public List<PatientEnrollmentLink> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetPatientEnrollmentLink(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new patientenrollmentlink</summary>
        /// <param name="model">The patientenrollmentlink data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(PatientEnrollmentLink model)
        {
            model.Id = CreatePatientEnrollmentLink(model);
            return model.Id;
        }

        /// <summary>Updates a specific patientenrollmentlink by its primary key</summary>
        /// <param name="id">The primary key of the patientenrollmentlink</param>
        /// <param name="updatedEntity">The patientenrollmentlink data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, PatientEnrollmentLink updatedEntity)
        {
            UpdatePatientEnrollmentLink(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific patientenrollmentlink by its primary key</summary>
        /// <param name="id">The primary key of the patientenrollmentlink</param>
        /// <param name="updatedEntity">The patientenrollmentlink data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<PatientEnrollmentLink> updatedEntity)
        {
            PatchPatientEnrollmentLink(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific patientenrollmentlink by its primary key</summary>
        /// <param name="id">The primary key of the patientenrollmentlink</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeletePatientEnrollmentLink(id);
            return true;
        }
        #region
        private List<PatientEnrollmentLink> GetPatientEnrollmentLink(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.PatientEnrollmentLink.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<PatientEnrollmentLink>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(PatientEnrollmentLink), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<PatientEnrollmentLink, object>>(Expression.Convert(property, typeof(object)), parameter);
                if (sortOrder.Equals("asc", StringComparison.OrdinalIgnoreCase))
                {
                    result = result.OrderBy(lambda);
                }
                else if (sortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase))
                {
                    result = result.OrderByDescending(lambda);
                }
                else
                {
                    throw new ApplicationException("Invalid sort order. Use 'asc' or 'desc'");
                }
            }

            var paginatedResult = result.Skip(skip).Take(pageSize).ToList();
            return paginatedResult;
        }

        private Guid CreatePatientEnrollmentLink(PatientEnrollmentLink model)
        {
            _dbContext.PatientEnrollmentLink.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdatePatientEnrollmentLink(Guid id, PatientEnrollmentLink updatedEntity)
        {
            _dbContext.PatientEnrollmentLink.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeletePatientEnrollmentLink(Guid id)
        {
            var entityData = _dbContext.PatientEnrollmentLink.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.PatientEnrollmentLink.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchPatientEnrollmentLink(Guid id, JsonPatchDocument<PatientEnrollmentLink> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.PatientEnrollmentLink.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.PatientEnrollmentLink.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}