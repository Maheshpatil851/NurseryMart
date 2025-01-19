using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NurseryMart.Entities
{
    public class Trail
    {
        [Key] // Primary Key
        public int TrailId { get; set; }  // Unique identifier for the Trail
        public DateTimeOffset? CreatedOn { get; set; }  // Created date
        public DateTimeOffset? ModifiedOn { get; set; }  // Modified date
        public string? CreatedBy { get; set; }  // Created by user
        public string? ModifiedBy { get; set; }  // Modified by user
        public bool IsActive { get; set; }  // Active status of the record
        public bool IsMarkedAsDelete { get; set; }  // Flag indicating if the record is marked for deletion
        public DateTimeOffset? LastActiveOn { get; set; }  // Last active timestamp

        // Navigation property for Authorize (One-to-many)
        [JsonIgnore]
        public ICollection<Authorize> Authorize { get; set; }
    }
}