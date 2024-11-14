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
    /// The patientnotesService responsible for managing patientnotes related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting patientnotes information.
    /// </remarks>
    public interface IPatientNotesService
    {
        /// <summary>Retrieves a specific patientnotes by its primary key</summary>
        /// <param name="id">The primary key of the patientnotes</param>
        /// <returns>The patientnotes data</returns>
        PatientNotes GetById(Guid id);

        /// <summary>Retrieves a list of patientnotess based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of patientnotess</returns>
        List<PatientNotes> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new patientnotes</summary>
        /// <param name="model">The patientnotes data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(PatientNotes model);

        /// <summary>Updates a specific patientnotes by its primary key</summary>
        /// <param name="id">The primary key of the patientnotes</param>
        /// <param name="updatedEntity">The patientnotes data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, PatientNotes updatedEntity);

        /// <summary>Updates a specific patientnotes by its primary key</summary>
        /// <param name="id">The primary key of the patientnotes</param>
        /// <param name="updatedEntity">The patientnotes data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<PatientNotes> updatedEntity);

        /// <summary>Deletes a specific patientnotes by its primary key</summary>
        /// <param name="id">The primary key of the patientnotes</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The patientnotesService responsible for managing patientnotes related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting patientnotes information.
    /// </remarks>
    public class PatientNotesService : IPatientNotesService
    {
        private Nov14Context _dbContext;

        /// <summary>
        /// Initializes a new instance of the PatientNotes class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public PatientNotesService(Nov14Context dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific patientnotes by its primary key</summary>
        /// <param name="id">The primary key of the patientnotes</param>
        /// <returns>The patientnotes data</returns>
        public PatientNotes GetById(Guid id)
        {
            var entityData = _dbContext.PatientNotes.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of patientnotess based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of patientnotess</returns>/// <exception cref="Exception"></exception>
        public List<PatientNotes> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetPatientNotes(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new patientnotes</summary>
        /// <param name="model">The patientnotes data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(PatientNotes model)
        {
            model.Id = CreatePatientNotes(model);
            return model.Id;
        }

        /// <summary>Updates a specific patientnotes by its primary key</summary>
        /// <param name="id">The primary key of the patientnotes</param>
        /// <param name="updatedEntity">The patientnotes data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, PatientNotes updatedEntity)
        {
            UpdatePatientNotes(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific patientnotes by its primary key</summary>
        /// <param name="id">The primary key of the patientnotes</param>
        /// <param name="updatedEntity">The patientnotes data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<PatientNotes> updatedEntity)
        {
            PatchPatientNotes(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific patientnotes by its primary key</summary>
        /// <param name="id">The primary key of the patientnotes</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeletePatientNotes(id);
            return true;
        }
        #region
        private List<PatientNotes> GetPatientNotes(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.PatientNotes.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<PatientNotes>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(PatientNotes), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<PatientNotes, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreatePatientNotes(PatientNotes model)
        {
            _dbContext.PatientNotes.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdatePatientNotes(Guid id, PatientNotes updatedEntity)
        {
            _dbContext.PatientNotes.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeletePatientNotes(Guid id)
        {
            var entityData = _dbContext.PatientNotes.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.PatientNotes.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchPatientNotes(Guid id, JsonPatchDocument<PatientNotes> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.PatientNotes.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.PatientNotes.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}