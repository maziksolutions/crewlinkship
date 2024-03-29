﻿using System;
using System.Collections.Generic;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblImportCrewDetail
    {
        public int? CrewId { get; set; }
        public int? CountryId { get; set; }
        public int? RankId { get; set; }
        public int? PoolId { get; set; }
        public int? ZonalId { get; set; }
        public int? MtunionId { get; set; }
        public int? NtbrReasonId { get; set; }
        public int? InActiveReasonId { get; set; }
        public string EmpNumber { get; set; }
        public string Status { get; set; }
        public string PreviousStatus { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public DateTime? Dob { get; set; }
        public string PlaceOfBirth { get; set; }
        public string CivilStatus { get; set; }
        public DateTime? Doa { get; set; }
        public string Gender { get; set; }
        public string EnglishFluency { get; set; }
        public string UserImage { get; set; }
        public string ShipCategory { get; set; }
        public DateTime? AppliedOn { get; set; }
        public DateTime? FirstJoinDate { get; set; }
        public string OtherTravelDocNo { get; set; }
        public string ManningOffice { get; set; }
        public string MembershipNumber { get; set; }
        public DateTime? DateOfJoining { get; set; }
        public string Attachment { get; set; }
        public string Benefits { get; set; }
        public string Height { get; set; }
        public string Weight { get; set; }
        public string ShoesSize { get; set; }
        public string BoilerSuitSize { get; set; }
        public string ShirtSize { get; set; }
        public string TrouserSize { get; set; }
        public string HairColor { get; set; }
        public string EyeColor { get; set; }
        public string DistinguishMark { get; set; }
        public string Resume { get; set; }
        public string Remark { get; set; }
        public string ApplicantStatus { get; set; }
        public int? LastVessel { get; set; }
        public int? VesselId { get; set; }
        public DateTime? ReliefDate { get; set; }
        public bool? IsNtbr { get; set; }
        public DateTime? Ntbron { get; set; }
        public string Ntbrby { get; set; }
        public bool? InActive { get; set; }
        public DateTime? InActiveOn { get; set; }
        public string InActiveBy { get; set; }
        public bool? IsDeleted { get; set; }
        public DateTime? RecDate { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? CreatedBy { get; set; }
        public string Signature { get; set; }
        public int? PlanRankId { get; set; }
        public string PlanStatus { get; set; }
        public int? PlanVesselId { get; set; }
        public string ImpRemark { get; set; }
        public int? ApprovedBy { get; set; }
        public string MaskAttachment { get; set; }
        public string MaskRemarks { get; set; }
        public string MaskedBy { get; set; }
    }
}
