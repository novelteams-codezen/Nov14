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
    /// The patientmedicalhistorynoteService responsible for managing patientmedicalhistorynote related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting patientmedicalhistorynote information.
    /// </remarks>
    public interface IPatientMedicalHistoryNoteService
    {
        /// <summary>Retrieves a specific patientmedicalhistorynote by its primary key</summary>
        /// <param name="id">The primary key of the patientmedicalhistorynote</param>
        /// <returns>The patientmedicalhistorynote data</returns>
        PatientMedicalHistoryNote GetById(Guid id);

        /// <summary>Retrieves a list of patientmedicalhistorynotes based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of patientmedicalhistorynotes</returns>
        List<PatientMedicalHistoryNote> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc");

        /// <summary>Adds a new patientmedicalhistorynote</summary>
        /// <param name="model">The patientmedicalhistorynote data to be added</param>
        /// <returns>The result of the operation</returns>
        Guid Create(PatientMedicalHistoryNote model);

        /// <summary>Updates a specific patientmedicalhistorynote by its primary key</summary>
        /// <param name="id">The primary key of the patientmedicalhistorynote</param>
        /// <param name="updatedEntity">The patientmedicalhistorynote data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Update(Guid id, PatientMedicalHistoryNote updatedEntity);

        /// <summary>Updates a specific patientmedicalhistorynote by its primary key</summary>
        /// <param name="id">The primary key of the patientmedicalhistorynote</param>
        /// <param name="updatedEntity">The patientmedicalhistorynote data to be updated</param>
        /// <returns>The result of the operation</returns>
        bool Patch(Guid id, JsonPatchDocument<PatientMedicalHistoryNote> updatedEntity);

        /// <summary>Deletes a specific patientmedicalhistorynote by its primary key</summary>
        /// <param name="id">The primary key of the patientmedicalhistorynote</param>
        /// <returns>The result of the operation</returns>
        bool Delete(Guid id);
    }

    /// <summary>
    /// The patientmedicalhistorynoteService responsible for managing patientmedicalhistorynote related operations.
    /// </summary>
    /// <remarks>
    /// This service for adding, retrieving, updating, and deleting patientmedicalhistorynote information.
    /// </remarks>
    public class PatientMedicalHistoryNoteService : IPatientMedicalHistoryNoteService
    {
        private Nov14Context _dbContext;

        /// <summary>
        /// Initializes a new instance of the PatientMedicalHistoryNote class.
        /// </summary>
        /// <param name="dbContext">dbContext value to set.</param>
        public PatientMedicalHistoryNoteService(Nov14Context dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>Retrieves a specific patientmedicalhistorynote by its primary key</summary>
        /// <param name="id">The primary key of the patientmedicalhistorynote</param>
        /// <returns>The patientmedicalhistorynote data</returns>
        public PatientMedicalHistoryNote GetById(Guid id)
        {
            var entityData = _dbContext.PatientMedicalHistoryNote.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            return entityData;
        }

        /// <summary>Retrieves a list of patientmedicalhistorynotes based on specified filters</summary>
        /// <param name="filters">The filter criteria in JSON format. Use the following format: [{"PropertyName": "PropertyName", "Operator": "Equal", "Value": "FilterValue"}] </param>
        /// <param name="searchTerm">To searching data.</param>
        /// <param name="pageNumber">The page number.</param>
        /// <param name="pageSize">The page size.</param>
        /// <param name="sortField">The entity's field name to sort.</param>
        /// <param name="sortOrder">The sort order asc or desc.</param>
        /// <returns>The filtered list of patientmedicalhistorynotes</returns>/// <exception cref="Exception"></exception>
        public List<PatientMedicalHistoryNote> Get(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            var result = GetPatientMedicalHistoryNote(filters, searchTerm, pageNumber, pageSize, sortField, sortOrder);
            return result;
        }

        /// <summary>Adds a new patientmedicalhistorynote</summary>
        /// <param name="model">The patientmedicalhistorynote data to be added</param>
        /// <returns>The result of the operation</returns>
        public Guid Create(PatientMedicalHistoryNote model)
        {
            model.Id = CreatePatientMedicalHistoryNote(model);
            return model.Id;
        }

        /// <summary>Updates a specific patientmedicalhistorynote by its primary key</summary>
        /// <param name="id">The primary key of the patientmedicalhistorynote</param>
        /// <param name="updatedEntity">The patientmedicalhistorynote data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Update(Guid id, PatientMedicalHistoryNote updatedEntity)
        {
            UpdatePatientMedicalHistoryNote(id, updatedEntity);
            return true;
        }

        /// <summary>Updates a specific patientmedicalhistorynote by its primary key</summary>
        /// <param name="id">The primary key of the patientmedicalhistorynote</param>
        /// <param name="updatedEntity">The patientmedicalhistorynote data to be updated</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Patch(Guid id, JsonPatchDocument<PatientMedicalHistoryNote> updatedEntity)
        {
            PatchPatientMedicalHistoryNote(id, updatedEntity);
            return true;
        }

        /// <summary>Deletes a specific patientmedicalhistorynote by its primary key</summary>
        /// <param name="id">The primary key of the patientmedicalhistorynote</param>
        /// <returns>The result of the operation</returns>
        /// <exception cref="Exception"></exception>
        public bool Delete(Guid id)
        {
            DeletePatientMedicalHistoryNote(id);
            return true;
        }
        #region
        private List<PatientMedicalHistoryNote> GetPatientMedicalHistoryNote(List<FilterCriteria> filters = null, string searchTerm = "", int pageNumber = 1, int pageSize = 1, string sortField = null, string sortOrder = "asc")
        {
            if (pageSize < 1)
            {
                throw new ApplicationException("Page size invalid!");
            }

            if (pageNumber < 1)
            {
                throw new ApplicationException("Page mumber invalid!");
            }

            var query = _dbContext.PatientMedicalHistoryNote.IncludeRelated().AsQueryable();
            int skip = (pageNumber - 1) * pageSize;
            var result = FilterService<PatientMedicalHistoryNote>.ApplyFilter(query, filters, searchTerm);
            if (!string.IsNullOrEmpty(sortField))
            {
                var parameter = Expression.Parameter(typeof(PatientMedicalHistoryNote), "b");
                var property = Expression.Property(parameter, sortField);
                var lambda = Expression.Lambda<Func<PatientMedicalHistoryNote, object>>(Expression.Convert(property, typeof(object)), parameter);
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

        private Guid CreatePatientMedicalHistoryNote(PatientMedicalHistoryNote model)
        {
            _dbContext.PatientMedicalHistoryNote.Add(model);
            _dbContext.SaveChanges();
            return model.Id;
        }

        private void UpdatePatientMedicalHistoryNote(Guid id, PatientMedicalHistoryNote updatedEntity)
        {
            _dbContext.PatientMedicalHistoryNote.Update(updatedEntity);
            _dbContext.SaveChanges();
        }

        private bool DeletePatientMedicalHistoryNote(Guid id)
        {
            var entityData = _dbContext.PatientMedicalHistoryNote.IncludeRelated().FirstOrDefault(entity => entity.Id == id);
            if (entityData == null)
            {
                throw new ApplicationException("No data found!");
            }

            _dbContext.PatientMedicalHistoryNote.Remove(entityData);
            _dbContext.SaveChanges();
            return true;
        }

        private void PatchPatientMedicalHistoryNote(Guid id, JsonPatchDocument<PatientMedicalHistoryNote> updatedEntity)
        {
            if (updatedEntity == null)
            {
                throw new ApplicationException("Patch document is missing!");
            }

            var existingEntity = _dbContext.PatientMedicalHistoryNote.FirstOrDefault(t => t.Id == id);
            if (existingEntity == null)
            {
                throw new ApplicationException("No data found!");
            }

            updatedEntity.ApplyTo(existingEntity);
            _dbContext.PatientMedicalHistoryNote.Update(existingEntity);
            _dbContext.SaveChanges();
        }
        #endregion
    }
}