using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace crewlinkship.Models
{
    public partial class TblVessel
    {
        public TblVessel()
        {
            TblCrewDetailPlanVessels = new HashSet<TblCrewDetail>();
            TblCrewDetailVessels = new HashSet<TblCrewDetail>();
            TblCrewLists = new HashSet<TblCrewList>();
            TblVesselCbas = new HashSet<TblVesselCba>();
        }

        public int VesselId { get; set; }
        public string VesselName { get; set; }
        public string PreviousName { get; set; }
        public int? ShipId { get; set; }
        public string CallSign { get; set; }
        public string Mmsi { get; set; }
        public string Imo { get; set; }
        public string Official { get; set; }
        public int? ClassificationId { get; set; }
        public string ClassNotation { get; set; }
        public string VesselCode { get; set; }
        public string IceClass { get; set; }
        public int? FlagId { get; set; }
        public int? PortOfRegistry { get; set; }
        public int? OwnerId { get; set; }
        public int? DisponentOwnerId { get; set; }
        public int? PrincipalId { get; set; }
        public string HullNo { get; set; }
        public string KeelLaid { get; set; }
        public string Launched { get; set; }
        public string Delivery { get; set; }
        public DateTime? TakeoverDate { get; set; }
        public string Gthour { get; set; }
        [ForeignKey("PortOfTakeovers")]
        public int? PortOfTakeover { get; set; }
        public int? PortOfHandover { get; set; }
        public DateTime? HandoverDate { get; set; }
        public int? BuilderId { get; set; }
        public string OperatingArea { get; set; }
        public string ClassNo { get; set; }
        public int? CommercialManager1 { get; set; }
        public int? CommercialManager2 { get; set; }
        [ForeignKey("Crewmanager")]
        public int? Crewmanager1 { get; set; }
        public int? Crewmanager2 { get; set; }
        [ForeignKey("Manager")]
        public int? TechManager1 { get; set; }
        public int? TechManager2 { get; set; }
        public int? Bunkermanager1 { get; set; }
        public int? Bunkermanager2 { get; set; }
        public string Net { get; set; }
        public string Gross { get; set; }
        public string Suez { get; set; }
        public string Panama { get; set; }
        [ForeignKey("VendorRegisterPi")]
        public int? Pi { get; set; }
        [ForeignKey("VendorRegisterHm")]
        public int? Hm { get; set; }
        public string Deductible1 { get; set; }
        public string Deductible2 { get; set; }
        public string Loa { get; set; }
        public string Lbp { get; set; }
        public string Breadth { get; set; }
        public string Depth { get; set; }
        public string Height { get; set; }
        public string ServiceSpeed { get; set; }
        public bool? IsHighVoltageEquipment { get; set; }
        public int? EngineTypeId { get; set; }
        public int? EngineModelId { get; set; }
        public string MainEngine { get; set; }
        public string Mcr { get; set; }
        public string Kw { get; set; }
        public string AuxEngine1 { get; set; }
        public string AuxModel1 { get; set; }
        public string AuxMake1 { get; set; }
        public string AuxKw1 { get; set; }
        public string AuxEngine2 { get; set; }
        public string AuxModel2 { get; set; }
        public string AuxMake2 { get; set; }
        public string AuxKw2 { get; set; }
        public string AuxEngine3 { get; set; }
        public string AuxModel3 { get; set; }
        public string AuxMake3 { get; set; }
        public string AuxKw3 { get; set; }
        public string AuxBoiler { get; set; }
        public string AuxModel4 { get; set; }
        public string AuxMake4 { get; set; }
        public string Kghr { get; set; }
        public string EquipmentName { get; set; }
        public string EqupModel5 { get; set; }
        public string EqupMake5 { get; set; }
        public string Propulsion { get; set; }
        public string LifeBoat { get; set; }
        public string Capacity { get; set; }
        public string SeqCapacity { get; set; }
        public string Remarks { get; set; }
        public string LifeRaft1 { get; set; }
        public string LifeRaft2 { get; set; }
        public string LifeRaft3 { get; set; }
        public string CargoEquipment { get; set; }
        public string CommunicationEquipment { get; set; }
        public string VesselParticulars { get; set; }
        public DateTime? AnniversaryDate { get; set; }
        public string Ecdis1 { get; set; }
        public string Ecdis2 { get; set; }
        public string Ecdis2model { get; set; }
        public string Ecdis2mode2 { get; set; }
        public string Ecdis2maker1 { get; set; }
        public string Ecdis2maker2 { get; set; }
        public string Summer { get; set; }
        public string SummerFreeboard { get; set; }
        public string SummerDraft { get; set; }
        public string SummerDeadWeight { get; set; }
        public string SummerDisplacement { get; set; }
        public string Winter { get; set; }
        public string WinterFreeboard { get; set; }
        public string WinterDraft { get; set; }
        public string WinterDeadWeight { get; set; }
        public string WinterDisplacement { get; set; }
        public string Tropical { get; set; }
        public string TropicalFreeboard { get; set; }
        public string TropicalDraft { get; set; }
        public string TropicalDeadWeight { get; set; }
        public string TropicalDisplacement { get; set; }
        public string Lightship { get; set; }
        public string LightshipFreeboard { get; set; }
        public string LightshipDraft { get; set; }
        public string LightshipDeadWeight { get; set; }
        public string LightshipDisplacement { get; set; }
        public string Ballast { get; set; }
        public string BallastFreeboard { get; set; }
        public string BallastDraft { get; set; }
        public string BallastDeadweight { get; set; }
        public string BallastDisplacement { get; set; }
        public string FreshWater { get; set; }
        public string FreshWaterFreeboard { get; set; }
        public string FreshWaterDraft { get; set; }
        public string FreshWaterDeadweight { get; set; }
        public string FreshWaterDisplacement { get; set; }
        public string TropicalFreshWater { get; set; }
        public string TropicalFreshWaterFreeboard { get; set; }
        public string TropicalFreshWaterDraft { get; set; }
        public string TropicalFreshWaterDeadWeight { get; set; }
        public string TropicalFreshWaterDisplacement { get; set; }
        public string WinterAtlantic { get; set; }
        public string WinterAtlanticFreeboard { get; set; }
        public string WinterAtlanticDraft { get; set; }
        public string WinterAtlanticDeadWeight { get; set; }
        public string WinterAtlanticDisplacement { get; set; }
        public int? AccommodationBerth { get; set; }
        public int? HospitalBerth { get; set; }
        public int? TreatmentDays { get; set; }
        public string TreatmentRemark { get; set; }
        public DateTime? RecDate { get; set; }
        public bool? IsDeleted { get; set; }
        public bool? IsActive { get; set; }
        public string AccountInchargeEmail { get; set; }
        public string Email { get; set; }
        public string Fbbfax { get; set; }
        public string Fbbphone { get; set; }
        public string Fleet77Fax { get; set; }
        public string Fleet77Phone { get; set; }
        public string Isp { get; set; }
        public string MobileNo { get; set; }
        public string MobileNo1 { get; set; }
        public string MobileNo2 { get; set; }
        public string NotificationEmail { get; set; }
        public string Satctelex { get; set; }
        public string SatBfax { get; set; }
        public string SatBphone { get; set; }
        public string Vsatfax { get; set; }
        public string Vsatphone { get; set; }
        public string VesselImage { get; set; }
        public int? PoolId { get; set; }
        public int? CreatedBy { get; set; }
        public string ModifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public bool? TransitWageApplicable { get; set; }
        public int? SuezCabinBerth { get; set; }
        public string Bow { get; set; }
        public string BallastEductor { get; set; }
        public int? BallastPump { get; set; }
        public string BallastPumpType { get; set; }
        public string Cpp { get; set; }
        public string CargoCoating { get; set; }
        public string CargoCranesMaker { get; set; }
        public int? CargoPump { get; set; }
        public string CargoPumpType { get; set; }
        public int? CargoTanks { get; set; }
        public string CranesCapacity { get; set; }
        public string CranesModel { get; set; }
        public string CranesNumber { get; set; }
        public int? Ecdisid1 { get; set; }
        public int? Ecdisid2 { get; set; }
        public int? Ecdisid3 { get; set; }
        public string Ecdistype1 { get; set; }
        public string Ecdistype2 { get; set; }
        public string Ecdistype3 { get; set; }
        public string GrabCapacity { get; set; }
        public string GrabMaker { get; set; }
        public int? GrabsNumber { get; set; }
        public string PumpCapacity { get; set; }
        public string Stern { get; set; }
        public int? CargoHolds { get; set; }
        public string QualifiedIndividual { get; set; }
        public int? MainEngineCount { get; set; }
        public bool? IsOperational { get; set; }

        public TblManager Manager { get; set; }
        public TblManager Crewmanager { get; set; }
        public virtual TblVendorRegister VendorRegisterHm { get; set; }
        public virtual TblSeaport PortOfTakeovers { get; set; }
        public virtual TblBuilder Builder { get; set; }
        public virtual TblVendorRegister VendorRegisterPi { get; set; }
        public virtual TblClassification Classification { get; set; }
        public virtual TblDisponentOwner DisponentOwner { get; set; }
        public virtual TblEcdi Ecdisid1Navigation { get; set; }
        public virtual TblEcdi Ecdisid2Navigation { get; set; }
        public virtual TblEcdi Ecdisid3Navigation { get; set; }
        public virtual TblEngineModel EngineModel { get; set; }
        public virtual TblEnginetype EngineType { get; set; }
        public virtual TblCountry Flag { get; set; }
        public virtual TblOwner Owner { get; set; }
        public virtual TblPool Pool { get; set; }
        public virtual TblSeaport PortOfRegistryNavigation { get; set; }
        public virtual TblPrincipal Principal { get; set; }
        public virtual TblShipType Ship { get; set; }
        public virtual ICollection<TblCrewDetail> TblCrewDetailPlanVessels { get; set; }
        public virtual ICollection<TblCrewDetail> TblCrewDetailVessels { get; set; }
        public virtual ICollection<TblCrewList> TblCrewLists { get; set; }
        public virtual ICollection<TblVesselCba> TblVesselCbas { get; set; }
    }
}
