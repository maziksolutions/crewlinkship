using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace crewlinkship.Models
{
    public partial class shipCrewlinkContext : DbContext
    {
        public shipCrewlinkContext()
        {

        }

        public shipCrewlinkContext(DbContextOptions<shipCrewlinkContext> options)
            : base(options)
        {
        }

        public virtual DbSet<TblActivitySignOff> TblActivitySignOffs { get; set; }
        public virtual DbSet<TblActivitySignOn> TblActivitySignOns { get; set; }
        public virtual DbSet<TblAddZonal> TblAddZonals { get; set; }
        public virtual DbSet<TblAssignmentsWithOther> TblAssignmentsWithOthers { get; set; }
        public virtual DbSet<TblAssignmentsWithOur> TblAssignmentsWithOurs { get; set; }
        public virtual DbSet<TblAuthority> TblAuthorities { get; set; }
        public virtual DbSet<TblBudgetCode> TblBudgetCodes { get; set; }
        public virtual DbSet<TblBudgetSubCode> TblBudgetSubCodes { get; set; }
        public virtual DbSet<TblBuilder> TblBuilders { get; set; }
        public virtual DbSet<TblCba> TblCbas { get; set; }
        public virtual DbSet<TblCbaUnion> TblCbaUnions { get; set; }
        public virtual DbSet<TblCdc> TblCdcs { get; set; }
        public virtual DbSet<TblCity> TblCities { get; set; }
        public virtual DbSet<TblClassification> TblClassifications { get; set; }
        public virtual DbSet<TblCountry> TblCountries { get; set; }
        public virtual DbSet<TblCourseRegister> TblCourseRegisters { get; set; }
        public virtual DbSet<TblCrewAddress> TblCrewAddresses { get; set; }
        public virtual DbSet<TblCrewBankDetail> TblCrewBankDetails { get; set; }
        public virtual DbSet<TblCrewCorrespondence> TblCrewCorrespondences { get; set; }
        public virtual DbSet<TblCrewCorrespondenceAddress> TblCrewCorrespondenceAddresses { get; set; }
        public virtual DbSet<TblCrewCourse> TblCrewCourses { get; set; }
        public virtual DbSet<TblCrewDetail> TblCrewDetails { get; set; }
        public virtual DbSet<TblCrewLicense> TblCrewLicenses { get; set; }
        public virtual DbSet<TblCrewList> TblCrewLists { get; set; }
        public virtual DbSet<TblCrewOtherDocument> TblCrewOtherDocuments { get; set; }
        public virtual DbSet<TblDisponentOwner> TblDisponentOwners { get; set; }
        public virtual DbSet<TblEcdi> TblEcdis { get; set; }
        public virtual DbSet<TblEngineModel> TblEngineModels { get; set; }
        public virtual DbSet<TblEngineSubType> TblEngineSubTypes { get; set; }
        public virtual DbSet<TblEnginetype> TblEnginetypes { get; set; }
        public virtual DbSet<TblInstitute> TblInstitutes { get; set; }
        public virtual DbSet<TblIssuingAuthority> TblIssuingAuthorities { get; set; }
        public virtual DbSet<TblLicenceRegister> TblLicenceRegisters { get; set; }
        public virtual DbSet<TblManager> TblManagers { get; set; }
        public virtual DbSet<TblOtherDocument> TblOtherDocuments { get; set; }
        public virtual DbSet<TblOwner> TblOwners { get; set; }
        public virtual DbSet<TblPassport> TblPassports { get; set; }
        public virtual DbSet<TblPool> TblPools { get; set; }
        public virtual DbSet<TblPrincipal> TblPrincipals { get; set; }
        public virtual DbSet<TblRankRegister> TblRankRegisters { get; set; }
        public virtual DbSet<TblSeaport> TblSeaports { get; set; }
        public virtual DbSet<TblShipType> TblShipTypes { get; set; }
        public virtual DbSet<TblSignOffReason> TblSignOffReasons { get; set; }
        public virtual DbSet<TblSignOnReason> TblSignOnReasons { get; set; }
        public virtual DbSet<TblState> TblStates { get; set; }
        public virtual DbSet<TblVendorRegister> TblVendorRegisters { get; set; }
        public virtual DbSet<TblVessel> TblVessels { get; set; }
        public virtual DbSet<TblVesselCba> TblVesselCbas { get; set; }
        public virtual DbSet<TblVisa> TblVisas { get; set; }
        public virtual DbSet<TblWageComponent> TblWageComponents { get; set; }
        public virtual DbSet<TblWageStructure> TblWageStructures { get; set; }
        public virtual DbSet<TblYellowfever> TblYellowfevers { get; set; }
        public virtual DbSet<Userlogin> Userlogins { get; set; }
        public virtual DbSet<VwActiveCrewList> VwActiveCrewLists { get; set; }
        public virtual DbSet<VwOcimfexp> VwOcimfexps { get; set; }
        public virtual DbSet<VwTankerExp> VwTankerExps { get; set; }

        public virtual DbSet<OCIMFVM> OCIMFVMs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {

                optionsBuilder.UseSqlServer("Server=DESKTOP-HRR2RKV;Database=shipCrewlink;Trusted_Connection=True;");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "Latin1_General_CI_AS");

            modelBuilder.Entity<TblActivitySignOff>(entity =>
            {
                entity.HasKey(e => e.ActivitySignOffId);

                entity.ToTable("tblActivitySignOff");

                entity.Property(e => e.AllowEndTravel).HasDefaultValueSql("((0))");

                entity.Property(e => e.DoagivenDate).HasColumnName("DOAGivenDate");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.RecDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Crew)
                    .WithMany(p => p.TblActivitySignOffs)
                    .HasForeignKey(d => d.CrewId);

                entity.HasOne(d => d.CrewList)
                    .WithMany(p => p.TblActivitySignOffs)
                    .HasForeignKey(d => d.CrewListId);

                entity.HasOne(d => d.Seaport)
                    .WithMany(p => p.TblActivitySignOffs)
                    .HasForeignKey(d => d.SeaportId);

                entity.HasOne(d => d.SignOffReason)
                    .WithMany(p => p.TblActivitySignOffs)
                    .HasForeignKey(d => d.SignOffReasonId);
            });

            modelBuilder.Entity<TblActivitySignOn>(entity =>
            {
                entity.HasKey(e => e.ActivitySignOnId);

                entity.ToTable("tblActivitySignOn");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsSignon).HasDefaultValueSql("((0))");

                entity.Property(e => e.RecDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.TblActivitySignOns)
                    .HasForeignKey(d => d.CountryId);

                entity.HasOne(d => d.Crew)
                    .WithMany(p => p.TblActivitySignOns)
                    .HasForeignKey(d => d.CrewId);

                entity.HasOne(d => d.Rank)
                    .WithMany(p => p.TblActivitySignOns)
                    .HasForeignKey(d => d.RankId);

                entity.HasOne(d => d.Seaport)
                    .WithMany(p => p.TblActivitySignOns)
                    .HasForeignKey(d => d.SeaportId);

                entity.HasOne(d => d.SignOnReason)
                    .WithMany(p => p.TblActivitySignOns)
                    .HasForeignKey(d => d.SignOnReasonId);
            });

            modelBuilder.Entity<TblAddZonal>(entity =>
            {
                entity.HasKey(e => e.ZonalId);

                entity.ToTable("tblAddZonal");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.RecDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.ZonalName).HasMaxLength(450);
            });

            modelBuilder.Entity<TblAssignmentsWithOther>(entity =>
            {
                entity.HasKey(e => e.OtherAssignmentsId);

                entity.ToTable("tblAssignmentsWithOthers");

                entity.Property(e => e.Dwt).HasColumnName("DWT");

                entity.Property(e => e.Grt).HasColumnName("GRT");

                entity.Property(e => e.Imo).HasColumnName("IMO");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.Kw).HasColumnName("KW");

                entity.Property(e => e.RecDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.TblAssignmentsWithOthers)
                    .HasForeignKey(d => d.CountryId);

                entity.HasOne(d => d.Crew)
                    .WithMany(p => p.TblAssignmentsWithOthers)
                    .HasForeignKey(d => d.CrewId);

                entity.HasOne(d => d.EngineModel)
                    .WithMany(p => p.TblAssignmentsWithOthers)
                    .HasForeignKey(d => d.EngineModelId);

                entity.HasOne(d => d.Manager)
                    .WithMany(p => p.TblAssignmentsWithOthers)
                    .HasForeignKey(d => d.ManagerId);

                entity.HasOne(d => d.Rank)
                    .WithMany(p => p.TblAssignmentsWithOthers)
                    .HasForeignKey(d => d.RankId);

                entity.HasOne(d => d.Seaport)
                    .WithMany(p => p.TblAssignmentsWithOthers)
                    .HasForeignKey(d => d.SeaportId);

                entity.HasOne(d => d.Ship)
                    .WithMany(p => p.TblAssignmentsWithOthers)
                    .HasForeignKey(d => d.ShipId);

                entity.HasOne(d => d.SignOffReason)
                    .WithMany(p => p.TblAssignmentsWithOthers)
                    .HasForeignKey(d => d.SignOffReasonId);
            });

            modelBuilder.Entity<TblAssignmentsWithOur>(entity =>
            {
                entity.HasKey(e => e.OurAssignmentsId);

                entity.ToTable("tblAssignmentsWithOur");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.RecDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.CrewList)
                    .WithMany(p => p.TblAssignmentsWithOurs)
                    .HasForeignKey(d => d.CrewListId);

                entity.HasOne(d => d.EngineModel)
                    .WithMany(p => p.TblAssignmentsWithOurs)
                    .HasForeignKey(d => d.EngineModelId);

                entity.HasOne(d => d.Rank)
                    .WithMany(p => p.TblAssignmentsWithOurs)
                    .HasForeignKey(d => d.RankId);

                entity.HasOne(d => d.ShipTypeShip)
                    .WithMany(p => p.TblAssignmentsWithOurs)
                    .HasForeignKey(d => d.ShipTypeShipId);

                entity.HasOne(d => d.SignOffReason)
                    .WithMany(p => p.TblAssignmentsWithOurs)
                    .HasForeignKey(d => d.SignOffReasonId);

                entity.HasOne(d => d.VendorRegister)
                    .WithMany(p => p.TblAssignmentsWithOurs)
                    .HasForeignKey(d => d.VendorRegisterId);

                entity.HasOne(d => d.Vessel)
                    .WithMany(p => p.TblAssignmentsWithOurs)
                    .HasForeignKey(d => d.VesselId);
            });

            modelBuilder.Entity<TblAuthority>(entity =>
            {
                entity.HasKey(e => e.AuthorityId);

                entity.ToTable("tblAuthority");

                entity.Property(e => e.Authorities).HasMaxLength(450);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.RecDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.TblAuthorities)
                    .HasForeignKey(d => d.CityId);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.TblAuthorities)
                    .HasForeignKey(d => d.CountryId);

                entity.HasOne(d => d.State)
                    .WithMany(p => p.TblAuthorities)
                    .HasForeignKey(d => d.StateId);
            });

            modelBuilder.Entity<TblBudgetCode>(entity =>
            {
                entity.HasKey(e => e.BudgetCodeId);

                entity.ToTable("tblBudgetCode");

                entity.Property(e => e.BudgetCodes).HasMaxLength(450);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.RecDate).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<TblBudgetSubCode>(entity =>
            {
                entity.HasKey(e => e.SubCodeId);

                entity.ToTable("tblBudgetSubCode");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.RecDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.BudgetCode)
                    .WithMany(p => p.TblBudgetSubCodes)
                    .HasForeignKey(d => d.BudgetCodeId);
            });

            modelBuilder.Entity<TblBuilder>(entity =>
            {
                entity.HasKey(e => e.BuilderId);

                entity.ToTable("tblBuilders");

                entity.Property(e => e.Builder).HasMaxLength(450);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.RecDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.TblBuilders)
                    .HasForeignKey(d => d.CityId);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.TblBuilders)
                    .HasForeignKey(d => d.CountryId);

                entity.HasOne(d => d.State)
                    .WithMany(p => p.TblBuilders)
                    .HasForeignKey(d => d.StateId);
            });

            modelBuilder.Entity<TblCba>(entity =>
            {
                entity.HasKey(e => e.Cbaid);

                entity.ToTable("tblCBA");

                entity.Property(e => e.Cbaid).HasColumnName("CBAId");

                entity.Property(e => e.Cbadescription).HasColumnName("CBADescription");

                entity.Property(e => e.Cbaname).HasColumnName("CBAName");

                entity.Property(e => e.CbaunionId).HasColumnName("CBAUnionId");

                entity.Property(e => e.IsAvc).HasColumnName("IsAVC");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsLocked)
                    .IsRequired()
                    .HasDefaultValueSql("(CONVERT([bit],(0)))");

                entity.Property(e => e.IsNusi).HasColumnName("IsNUSI");

                entity.Property(e => e.IsPf).HasColumnName("IsPF");

                entity.Property(e => e.RecDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Cbaunion)
                    .WithMany(p => p.TblCbas)
                    .HasForeignKey(d => d.CbaunionId);
            });

            modelBuilder.Entity<TblCbaUnion>(entity =>
            {
                entity.HasKey(e => e.UnionId);

                entity.ToTable("tblCbaUnion");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.RecDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.UnionName).HasMaxLength(450);
            });

            modelBuilder.Entity<TblCdc>(entity =>
            {
                entity.HasKey(e => e.Cdcid);

                entity.ToTable("tblCDC");

                entity.Property(e => e.Cdcid).HasColumnName("CDCId");

                entity.Property(e => e.Cdcnumber)
                    .HasMaxLength(450)
                    .HasColumnName("CDCNumber");

                entity.Property(e => e.Doe).HasColumnName("DOE");

                entity.Property(e => e.Doi).HasColumnName("DOI");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.RecDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.TblCdcs)
                    .HasForeignKey(d => d.CountryId);

                entity.HasOne(d => d.Crew)
                    .WithMany(p => p.TblCdcs)
                    .HasForeignKey(d => d.CrewId);
            });

            modelBuilder.Entity<TblCity>(entity =>
            {
                entity.HasKey(e => e.CityId);

                entity.ToTable("tblCity");

                entity.Property(e => e.CityName).HasMaxLength(450);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.RecDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.TblCities)
                    .HasForeignKey(d => d.CountryId);

                entity.HasOne(d => d.State)
                    .WithMany(p => p.TblCities)
                    .HasForeignKey(d => d.StateId);
            });

            modelBuilder.Entity<TblClassification>(entity =>
            {
                entity.HasKey(e => e.ClassificationId);

                entity.ToTable("tblClassification");

                entity.Property(e => e.Classifications).HasMaxLength(450);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.RecDate).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<TblCountry>(entity =>
            {
                entity.HasKey(e => e.CountryId);

                entity.ToTable("tblCountry");

                entity.Property(e => e.CountryName).HasMaxLength(450);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.RecDate).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<TblCourseRegister>(entity =>
            {
                entity.HasKey(e => e.CourseId);

                entity.ToTable("tblCourseRegister");

                entity.Property(e => e.CourseName).HasMaxLength(450);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.RecDate).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<TblCrewAddress>(entity =>
            {
                entity.HasKey(e => e.CrewAddressId);

                entity.ToTable("tblCrewAddress");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.RecDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Airport)
                    .WithMany(p => p.TblCrewAddressAirports)
                    .HasForeignKey(d => d.AirportId);

                entity.HasOne(d => d.City)
                    .WithMany(p => p.TblCrewAddressCities)
                    .HasForeignKey(d => d.CityId);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.TblCrewAddresses)
                    .HasForeignKey(d => d.CountryId);

                entity.HasOne(d => d.Crew)
                    .WithMany(p => p.TblCrewAddresses)
                    .HasForeignKey(d => d.CrewId);

                entity.HasOne(d => d.State)
                    .WithMany(p => p.TblCrewAddresses)
                    .HasForeignKey(d => d.StateId);
            });

            modelBuilder.Entity<TblCrewBankDetail>(entity =>
            {
                entity.HasKey(e => e.CrewBankDetailsId);

                entity.ToTable("tblCrewBankDetails");

                entity.Property(e => e.Ifsccode).HasColumnName("IFSCCode");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.RecDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.TblCrewBankDetails)
                    .HasForeignKey(d => d.CityId);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.TblCrewBankDetails)
                    .HasForeignKey(d => d.CountryId);

                entity.HasOne(d => d.Crew)
                    .WithMany(p => p.TblCrewBankDetails)
                    .HasForeignKey(d => d.CrewId);

                entity.HasOne(d => d.State)
                    .WithMany(p => p.TblCrewBankDetails)
                    .HasForeignKey(d => d.StateId);
            });

            modelBuilder.Entity<TblCrewCorrespondence>(entity =>
            {
                entity.HasKey(e => e.CrewCorrespondenceId);

                entity.ToTable("tblCrewCorrespondence");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.RecDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Crew)
                    .WithMany(p => p.TblCrewCorrespondences)
                    .HasForeignKey(d => d.CrewId);
            });

            modelBuilder.Entity<TblCrewCorrespondenceAddress>(entity =>
            {
                entity.HasKey(e => e.CrewAddressId);

                entity.ToTable("tblCrewCorrespondenceAddress");

                entity.Property(e => e.Caddress1).HasColumnName("CAddress1");

                entity.Property(e => e.Caddress2).HasColumnName("CAddress2");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.RecDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.SameAddress).HasDefaultValueSql("((0))");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.TblCrewCorrespondenceAddresses)
                    .HasForeignKey(d => d.CityId);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.TblCrewCorrespondenceAddresses)
                    .HasForeignKey(d => d.CountryId);

                entity.HasOne(d => d.Crew)
                    .WithMany(p => p.TblCrewCorrespondenceAddresses)
                    .HasForeignKey(d => d.CrewId);

                entity.HasOne(d => d.State)
                    .WithMany(p => p.TblCrewCorrespondenceAddresses)
                    .HasForeignKey(d => d.StateId);
            });

            modelBuilder.Entity<TblCrewCourse>(entity =>
            {
                entity.HasKey(e => e.CrewCoursesId);

                entity.ToTable("tblCrewCourses");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.RecDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Authority)
                    .WithMany(p => p.TblCrewCourses)
                    .HasForeignKey(d => d.AuthorityId);

                entity.HasOne(d => d.CourseNavigation)
                    .WithMany(p => p.TblCrewCourses)
                    .HasForeignKey(d => d.CourseId);

                entity.HasOne(d => d.Crew)
                    .WithMany(p => p.TblCrewCourses)
                    .HasForeignKey(d => d.CrewId);

                entity.HasOne(d => d.Institute)
                    .WithMany(p => p.TblCrewCourses)
                    .HasForeignKey(d => d.InstituteId);
            });

            modelBuilder.Entity<TblCrewDetail>(entity =>
            {
                entity.HasKey(e => e.CrewId);

                entity.ToTable("tblCrewDetails");

                entity.Property(e => e.Doa).HasColumnName("DOA");

                entity.Property(e => e.Dob).HasColumnName("DOB");

                entity.Property(e => e.InActive).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsNtbr)
                    .HasColumnName("IsNTBR")
                    .HasDefaultValueSql("((0))");

                entity.Property(e => e.MtunionId).HasColumnName("MTUnionId");

                entity.Property(e => e.Ntbrby).HasColumnName("NTBRBy");

                entity.Property(e => e.Ntbron).HasColumnName("NTBROn");

                entity.Property(e => e.RecDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.TblCrewDetails)
                    .HasForeignKey(d => d.CountryId);

                entity.HasOne(d => d.PlanRank)
                    .WithMany(p => p.TblCrewDetailPlanRanks)
                    .HasForeignKey(d => d.PlanRankId);

                entity.HasOne(d => d.PlanVessel)
                    .WithMany(p => p.TblCrewDetailPlanVessels)
                    .HasForeignKey(d => d.PlanVesselId);

                entity.HasOne(d => d.Pool)
                    .WithMany(p => p.TblCrewDetails)
                    .HasForeignKey(d => d.PoolId);

                entity.HasOne(d => d.Rank)
                    .WithMany(p => p.TblCrewDetailRanks)
                    .HasForeignKey(d => d.RankId);

                entity.HasOne(d => d.Vessel)
                    .WithMany(p => p.TblCrewDetailVessels)
                    .HasForeignKey(d => d.VesselId);
            });

            modelBuilder.Entity<TblCrewLicense>(entity =>
            {
                entity.HasKey(e => e.CrewLicenseId);

                entity.ToTable("tblCrewLicense");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.RecDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Authority)
                    .WithMany(p => p.TblCrewLicenses)
                    .HasForeignKey(d => d.AuthorityId);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.TblCrewLicenses)
                    .HasForeignKey(d => d.CountryId);

                entity.HasOne(d => d.Crew)
                    .WithMany(p => p.TblCrewLicenses)
                    .HasForeignKey(d => d.CrewId);

                entity.HasOne(d => d.License)
                    .WithMany(p => p.TblCrewLicenses)
                    .HasForeignKey(d => d.LicenseId);
            });

            modelBuilder.Entity<TblCrewList>(entity =>
            {
                entity.HasKey(e => e.CrewListId);

                entity.ToTable("tblCrewList");

                entity.Property(e => e.Er).HasColumnName("ER");

                entity.Property(e => e.Ermonth).HasColumnName("ERMonth");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsSignOff).HasDefaultValueSql("((0))");

                entity.Property(e => e.RecDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Crew)
                    .WithMany(p => p.TblCrewLists)
                    .HasForeignKey(d => d.CrewId);

                entity.HasOne(d => d.Rank)
                    .WithMany(p => p.TblCrewListRanks)
                    .HasForeignKey(d => d.RankId);

                entity.HasOne(d => d.ReliverRank)
                    .WithMany(p => p.TblCrewListReliverRanks)
                    .HasForeignKey(d => d.ReliverRankId);

                entity.HasOne(d => d.Vessel)
                    .WithMany(p => p.TblCrewLists)
                    .HasForeignKey(d => d.VesselId);
            });

            modelBuilder.Entity<TblCrewOtherDocument>(entity =>
            {
                entity.HasKey(e => e.CrewOtherDocumentsId);

                entity.ToTable("tblCrewOtherDocuments");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.RecDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Authority)
                    .WithMany(p => p.TblCrewOtherDocuments)
                    .HasForeignKey(d => d.AuthorityId);

                entity.HasOne(d => d.Crew)
                    .WithMany(p => p.TblCrewOtherDocuments)
                    .HasForeignKey(d => d.CrewId);

                entity.HasOne(d => d.Document)
                    .WithMany(p => p.TblCrewOtherDocuments)
                    .HasForeignKey(d => d.DocumentId);
            });

            modelBuilder.Entity<TblDisponentOwner>(entity =>
            {
                entity.HasKey(e => e.DisponentOwnerId);

                entity.ToTable("tblDisponentOwner");

                entity.Property(e => e.DisponentOwners).HasMaxLength(450);

                entity.Property(e => e.IsActive).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.RecDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.TblDisponentOwners)
                    .HasForeignKey(d => d.CityId);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.TblDisponentOwners)
                    .HasForeignKey(d => d.CountryId);

                entity.HasOne(d => d.State)
                    .WithMany(p => p.TblDisponentOwners)
                    .HasForeignKey(d => d.StateId);
            });

            modelBuilder.Entity<TblEcdi>(entity =>
            {
                entity.HasKey(e => e.Ecdisid);

                entity.ToTable("tblECDIS");

                entity.Property(e => e.Ecdisid).HasColumnName("ECDISId");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.RecDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.TblEcdis)
                    .HasForeignKey(d => d.CityId);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.TblEcdis)
                    .HasForeignKey(d => d.CountryId);

                entity.HasOne(d => d.State)
                    .WithMany(p => p.TblEcdis)
                    .HasForeignKey(d => d.StateId);
            });

            modelBuilder.Entity<TblEngineModel>(entity =>
            {
                entity.HasKey(e => e.EngineModelId);

                entity.ToTable("tblEngineModel");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.Model).HasMaxLength(450);

                entity.Property(e => e.RecDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.TblEngineModels)
                    .HasForeignKey(d => d.CityId);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.TblEngineModels)
                    .HasForeignKey(d => d.CountryId);

                entity.HasOne(d => d.EngineSubType)
                    .WithMany(p => p.TblEngineModels)
                    .HasForeignKey(d => d.EngineSubTypeId);

                entity.HasOne(d => d.EngineType)
                    .WithMany(p => p.TblEngineModels)
                    .HasForeignKey(d => d.EngineTypeId);

                entity.HasOne(d => d.State)
                    .WithMany(p => p.TblEngineModels)
                    .HasForeignKey(d => d.StateId);
            });

            modelBuilder.Entity<TblEngineSubType>(entity =>
            {
                entity.HasKey(e => e.EngineSubTypeId);

                entity.ToTable("tblEngineSubType");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.RecDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.SubType).HasMaxLength(450);

                entity.HasOne(d => d.EngineType)
                    .WithMany(p => p.TblEngineSubTypes)
                    .HasForeignKey(d => d.EngineTypeId);
            });

            modelBuilder.Entity<TblEnginetype>(entity =>
            {
                entity.HasKey(e => e.EngineTypeId);

                entity.ToTable("tblEnginetype");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.RecDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Type).HasMaxLength(450);
            });

            modelBuilder.Entity<TblInstitute>(entity =>
            {
                entity.HasKey(e => e.InstituteId);

                entity.ToTable("tblInstitutes");

                entity.Property(e => e.InstituteName).HasMaxLength(450);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.RecDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.TblInstitutes)
                    .HasForeignKey(d => d.CityId);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.TblInstitutes)
                    .HasForeignKey(d => d.CountryId);

                entity.HasOne(d => d.State)
                    .WithMany(p => p.TblInstitutes)
                    .HasForeignKey(d => d.StateId);
            });

            modelBuilder.Entity<TblIssuingAuthority>(entity =>
            {
                entity.HasKey(e => e.AuthorityId);

                entity.ToTable("tblIssuingAuthority");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.IssuingAuthorities).HasMaxLength(450);

                entity.Property(e => e.RecDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.TblIssuingAuthorities)
                    .HasForeignKey(d => d.CountryId);
            });

            modelBuilder.Entity<TblLicenceRegister>(entity =>
            {
                entity.HasKey(e => e.LicenceId);

                entity.ToTable("tblLicenceRegister");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.LicenceName).HasMaxLength(450);

                entity.Property(e => e.RecDate).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<TblManager>(entity =>
            {
                entity.HasKey(e => e.ManagerId);

                entity.ToTable("tblManager");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.Managers).HasMaxLength(450);

                entity.Property(e => e.RecDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.TblManagers)
                    .HasForeignKey(d => d.CityId);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.TblManagers)
                    .HasForeignKey(d => d.CountryId);

                entity.HasOne(d => d.State)
                    .WithMany(p => p.TblManagers)
                    .HasForeignKey(d => d.StateId);
            });

            modelBuilder.Entity<TblOtherDocument>(entity =>
            {
                entity.HasKey(e => e.DocumentId);

                entity.ToTable("tblOtherDocuments");

                entity.Property(e => e.DocumentName).HasMaxLength(450);

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.RecDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.TblOtherDocuments)
                    .HasForeignKey(d => d.CountryId);
            });

            modelBuilder.Entity<TblOwner>(entity =>
            {
                entity.HasKey(e => e.OwnerId);

                entity.ToTable("tblOwner");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.OwnerName).HasMaxLength(450);

                entity.Property(e => e.RecDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.TblOwners)
                    .HasForeignKey(d => d.CityId);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.TblOwners)
                    .HasForeignKey(d => d.CountryId);

                entity.HasOne(d => d.State)
                    .WithMany(p => p.TblOwners)
                    .HasForeignKey(d => d.StateId);
            });

            modelBuilder.Entity<TblPassport>(entity =>
            {
                entity.HasKey(e => e.PassportId);

                entity.ToTable("tblPassport");

                entity.Property(e => e.Doe).HasColumnName("DOE");

                entity.Property(e => e.Doi).HasColumnName("DOI");

                entity.Property(e => e.Ecnr).HasColumnName("ECNR");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.PassportNumber).HasMaxLength(450);

                entity.Property(e => e.RecDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.TblPassports)
                    .HasForeignKey(d => d.CountryId);
            });

            modelBuilder.Entity<TblPool>(entity =>
            {
                entity.HasKey(e => e.PoolId);

                entity.ToTable("tblPool");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.PoolName).HasMaxLength(450);

                entity.Property(e => e.RecDate).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<TblPrincipal>(entity =>
            {
                entity.HasKey(e => e.PrincipalId);

                entity.ToTable("tblPrincipal");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.Principal).HasMaxLength(450);

                entity.Property(e => e.RecDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.TblPrincipals)
                    .HasForeignKey(d => d.CityId);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.TblPrincipals)
                    .HasForeignKey(d => d.CountryId);

                entity.HasOne(d => d.State)
                    .WithMany(p => p.TblPrincipals)
                    .HasForeignKey(d => d.StateId);
            });

            modelBuilder.Entity<TblRankRegister>(entity =>
            {
                entity.HasKey(e => e.RankId);

                entity.ToTable("tblRankRegister");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.RankName).HasMaxLength(450);

                entity.Property(e => e.RecDate).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<TblSeaport>(entity =>
            {
                entity.HasKey(e => e.SeaportId);

                entity.ToTable("tblSeaport");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.RecDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.SeaportName).HasMaxLength(450);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.TblSeaports)
                    .HasForeignKey(d => d.CountryId);
            });

            modelBuilder.Entity<TblShipType>(entity =>
            {
                entity.HasKey(e => e.ShipId);

                entity.ToTable("tblShipType");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.RecDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.Type).HasMaxLength(450);
            });

            modelBuilder.Entity<TblSignOffReason>(entity =>
            {
                entity.HasKey(e => e.SignOffReasonId);

                entity.ToTable("tblSignOffReason");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.Reason).HasMaxLength(450);

                entity.Property(e => e.RecDate).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<TblSignOnReason>(entity =>
            {
                entity.HasKey(e => e.SignOnReasonId);

                entity.ToTable("tblSignOnReason");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.Reason).HasMaxLength(450);

                entity.Property(e => e.RecDate).HasDefaultValueSql("(getdate())");
            });

            modelBuilder.Entity<TblState>(entity =>
            {
                entity.HasKey(e => e.StateId);

                entity.ToTable("tblState");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.RecDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.StateName).HasMaxLength(450);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.TblStates)
                    .HasForeignKey(d => d.CountryId);
            });

            modelBuilder.Entity<TblVendorRegister>(entity =>
            {
                entity.HasKey(e => e.VendorRegisterId);

                entity.ToTable("tblVendorRegister");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.Picname).HasColumnName("PICName");

                entity.Property(e => e.RecDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.City)
                    .WithMany(p => p.TblVendorRegisters)
                    .HasForeignKey(d => d.CityId);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.TblVendorRegisters)
                    .HasForeignKey(d => d.CountryId);

                entity.HasOne(d => d.Seaport)
                    .WithMany(p => p.TblVendorRegisters)
                    .HasForeignKey(d => d.SeaportId);

                entity.HasOne(d => d.State)
                    .WithMany(p => p.TblVendorRegisters)
                    .HasForeignKey(d => d.StateId);
            });

            modelBuilder.Entity<TblVessel>(entity =>
            {
                entity.HasKey(e => e.VesselId);

                entity.ToTable("tblVessel");

                entity.Property(e => e.Bow).HasColumnName("BOW");

                entity.Property(e => e.CargoHolds).HasColumnName("cargoHolds");

                entity.Property(e => e.Cpp).HasColumnName("CPP");

                entity.Property(e => e.Ecdis1).HasColumnName("ECDIS1");

                entity.Property(e => e.Ecdis2).HasColumnName("ECDIS2");

                entity.Property(e => e.Ecdis2maker1).HasColumnName("ECDIS2Maker1");

                entity.Property(e => e.Ecdis2maker2).HasColumnName("ECDIS2Maker2");

                entity.Property(e => e.Ecdis2mode2).HasColumnName("ECDIS2Mode2");

                entity.Property(e => e.Ecdis2model).HasColumnName("ECDIS2Model");

                entity.Property(e => e.Ecdisid1).HasColumnName("ECDISId1");

                entity.Property(e => e.Ecdisid2).HasColumnName("ECDISId2");

                entity.Property(e => e.Ecdisid3).HasColumnName("ECDISId3");

                entity.Property(e => e.Ecdistype1).HasColumnName("ECDISType1");

                entity.Property(e => e.Ecdistype2).HasColumnName("ECDISType2");

                entity.Property(e => e.Ecdistype3).HasColumnName("ECDISType3");

                entity.Property(e => e.Fbbfax).HasColumnName("FBBFax");

                entity.Property(e => e.Fbbphone).HasColumnName("FBBPhone");

                entity.Property(e => e.Gthour).HasColumnName("GTHour");

                entity.Property(e => e.Hm).HasColumnName("HM");

                entity.Property(e => e.Imo).HasColumnName("IMO");

                entity.Property(e => e.IsActive).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.IsHighVoltageEquipment).HasDefaultValueSql("((0))");

                entity.Property(e => e.Isp).HasColumnName("ISP");

                entity.Property(e => e.Kghr).HasColumnName("KGHR");

                entity.Property(e => e.Kw).HasColumnName("KW");

                entity.Property(e => e.Lbp).HasColumnName("LBP");

                entity.Property(e => e.Loa).HasColumnName("LOA");

                entity.Property(e => e.Mcr).HasColumnName("MCR");

                entity.Property(e => e.Mmsi).HasColumnName("MMSI");

                entity.Property(e => e.Pi).HasColumnName("PI");

                entity.Property(e => e.RecDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.SatBfax).HasColumnName("SatBFax");

                entity.Property(e => e.SatBphone).HasColumnName("SatBPhone");

                entity.Property(e => e.Satctelex).HasColumnName("SATCTelex");

                entity.Property(e => e.Stern).HasColumnName("STERN");

                entity.Property(e => e.VesselName).HasMaxLength(450);

                entity.Property(e => e.Vsatfax).HasColumnName("VSATFax");

                entity.Property(e => e.Vsatphone).HasColumnName("VSATPhone");

                entity.HasOne(d => d.Builder)
                    .WithMany(p => p.TblVessels)
                    .HasForeignKey(d => d.BuilderId);

                entity.HasOne(d => d.Classification)
                    .WithMany(p => p.TblVessels)
                    .HasForeignKey(d => d.ClassificationId);

                entity.HasOne(d => d.DisponentOwner)
                    .WithMany(p => p.TblVessels)
                    .HasForeignKey(d => d.DisponentOwnerId);

                entity.HasOne(d => d.Ecdisid1Navigation)
                    .WithMany(p => p.TblVesselEcdisid1Navigations)
                    .HasForeignKey(d => d.Ecdisid1);

                entity.HasOne(d => d.Ecdisid2Navigation)
                    .WithMany(p => p.TblVesselEcdisid2Navigations)
                    .HasForeignKey(d => d.Ecdisid2);

                entity.HasOne(d => d.Ecdisid3Navigation)
                    .WithMany(p => p.TblVesselEcdisid3Navigations)
                    .HasForeignKey(d => d.Ecdisid3);

                entity.HasOne(d => d.EngineModel)
                    .WithMany(p => p.TblVessels)
                    .HasForeignKey(d => d.EngineModelId);

                entity.HasOne(d => d.EngineType)
                    .WithMany(p => p.TblVessels)
                    .HasForeignKey(d => d.EngineTypeId);

                entity.HasOne(d => d.Flag)
                    .WithMany(p => p.TblVessels)
                    .HasForeignKey(d => d.FlagId);

                entity.HasOne(d => d.Owner)
                    .WithMany(p => p.TblVessels)
                    .HasForeignKey(d => d.OwnerId);

                entity.HasOne(d => d.Pool)
                    .WithMany(p => p.TblVessels)
                    .HasForeignKey(d => d.PoolId);

                entity.HasOne(d => d.PortOfRegistryNavigation)
                    .WithMany(p => p.TblVessels)
                    .HasForeignKey(d => d.PortOfRegistry);

                entity.HasOne(d => d.Principal)
                    .WithMany(p => p.TblVessels)
                    .HasForeignKey(d => d.PrincipalId);

                entity.HasOne(d => d.Ship)
                    .WithMany(p => p.TblVessels)
                    .HasForeignKey(d => d.ShipId);
            });

            modelBuilder.Entity<TblVesselCba>(entity =>
            {
                entity.HasKey(e => e.VesselCbaid);

                entity.ToTable("tblVesselCBA");

                entity.Property(e => e.VesselCbaid).HasColumnName("VesselCBAId");

                entity.Property(e => e.Cbarating).HasColumnName("CBARating");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.OfficerCba).HasColumnName("OfficerCBA");

                entity.Property(e => e.RecDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.TblVesselCbas)
                    .HasForeignKey(d => d.CountryId);

                entity.HasOne(d => d.Vessel)
                    .WithMany(p => p.TblVesselCbas)
                    .HasForeignKey(d => d.VesselId);
            });

            modelBuilder.Entity<TblVisa>(entity =>
            {
                entity.HasKey(e => e.VisaId);

                entity.ToTable("tblVisa");

                entity.Property(e => e.Doe).HasColumnName("DOE");

                entity.Property(e => e.Doi).HasColumnName("DOI");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.RecDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.VisaNumber).HasMaxLength(450);

                entity.HasOne(d => d.Country)
                    .WithMany(p => p.TblVisas)
                    .HasForeignKey(d => d.CountryId);
            });

            modelBuilder.Entity<TblWageComponent>(entity =>
            {
                entity.HasKey(e => e.WageId);

                entity.ToTable("tblWageComponent");

                entity.Property(e => e.IsCba).HasColumnName("IsCBA");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.RecDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.BudgetCode)
                    .WithMany(p => p.TblWageComponents)
                    .HasForeignKey(d => d.BudgetCodeId);

                entity.HasOne(d => d.SubCode)
                    .WithMany(p => p.TblWageComponents)
                    .HasForeignKey(d => d.SubCodeId);
            });

            modelBuilder.Entity<TblWageStructure>(entity =>
            {
                entity.HasKey(e => e.WageStructureId);

                entity.ToTable("tblWageStructure");

                entity.Property(e => e.Cbaid).HasColumnName("CBAId");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.RecDate).HasDefaultValueSql("(getdate())");

                entity.Property(e => e.SubCode)
                    .HasMaxLength(20)
                    .IsUnicode(false);

                entity.HasOne(d => d.Cba)
                    .WithMany(p => p.TblWageStructures)
                    .HasForeignKey(d => d.Cbaid);

                entity.HasOne(d => d.Rank)
                    .WithMany(p => p.TblWageStructures)
                    .HasForeignKey(d => d.RankId);

                entity.HasOne(d => d.Wage)
                    .WithMany(p => p.TblWageStructures)
                    .HasForeignKey(d => d.WageId);
            });

            modelBuilder.Entity<TblYellowfever>(entity =>
            {
                entity.HasKey(e => e.YellowFeverId);

                entity.ToTable("tblYellowfever");

                entity.Property(e => e.IsDeleted).HasDefaultValueSql("((0))");

                entity.Property(e => e.RecDate).HasDefaultValueSql("(getdate())");

                entity.HasOne(d => d.VendorRegister)
                    .WithMany(p => p.TblYellowfevers)
                    .HasForeignKey(d => d.VendorRegisterId);
            });
            modelBuilder.Entity<Userlogin>(entity =>
            {
                
                entity.HasKey(e => e.UerId);

                entity.ToTable("Userlogin");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(30)
                    .IsUnicode(false)
                    .HasColumnName("password");

                entity.Property(e => e.UerId)
                    .ValueGeneratedOnAdd()
                    .HasColumnName("UerID");

                entity.Property(e => e.UserName)
                    .IsRequired()
                    .HasMaxLength(255)
                    .IsUnicode(false);

                entity.Property(e => e.UserType)
                    .HasMaxLength(255)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VwActiveCrewList>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwActiveCrewList");

                entity.Property(e => e.Er).HasColumnName("ER");

                entity.Property(e => e.Ermonth).HasColumnName("ERMonth");

                entity.Property(e => e.Firstname)
                    .IsUnicode(false)
                    .HasColumnName("firstname");

                entity.Property(e => e.LastName)
                    .IsUnicode(false)
                    .HasColumnName("lastName");

                entity.Property(e => e.MiddleName).IsUnicode(false);

                entity.Property(e => e.VesselName).HasMaxLength(450);
            });

            modelBuilder.Entity<VwOcimfexp>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwOCIMFExp");

                entity.Property(e => e.Crewid).HasColumnName("crewid");

                entity.Property(e => e.RankExperience)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.TotalExperience)
                    .HasMaxLength(36)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<VwTankerExp>(entity =>
            {
                entity.HasNoKey();

                entity.ToView("vwTankerExp");

                entity.Property(e => e.Crewid).HasColumnName("crewid");

                entity.Property(e => e.TankerExperience)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.TotalExperience)
                    .HasMaxLength(36)
                    .IsUnicode(false);

                entity.Property(e => e.VesselTypeid).HasColumnName("vesselTypeid");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
