﻿using AttendanceManager.Domain.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AttendanceManager.Domain.Entities
{
    /// <summary>
    /// No need to inherit EntityBase 
    /// Collection of attendances
    /// </summary>
    public class AttendanceCollection
    {
        [Key]
        public int AttendanceCollectionID { get; set; }
        [ForeignKey("Document")]
        public required int DocumentID { get; set; }
        public required DateTime HeldOn { get; set; }
        public required CourseType CourseType { get; set; }
        public virtual Document? Document { get; set; }
        public ICollection<Attendance>? Attendances { get; set; }
    }
}
