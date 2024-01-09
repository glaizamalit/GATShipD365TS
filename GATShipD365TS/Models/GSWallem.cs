namespace GATShipD365TS.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class GSWallem : DbContext
    {
        public GSWallem()
            : base("name=GSWallem")
        {
        }

        public virtual DbSet<debitcreditnote> debitcreditnotes { get; set; }
        public virtual DbSet<dyna_fields> dyna_fields { get; set; }
        public virtual DbSet<dyna_values> dyna_values { get; set; }
        public virtual DbSet<Expense> Expenses { get; set; }
        public virtual DbSet<incoming_invoice> incoming_invoice { get; set; }
        public virtual DbSet<PortCall> PortCalls { get; set; }
        public virtual DbSet<Vessel> Vessels { get; set; }
        public virtual DbSet<ExpenseView> ExpenseViews { get; set; }

        public virtual DbSet<Hotlist> Hotlists { get; set; }
        public virtual DbSet<Client> Clients { get; set; }
        public virtual DbSet<Client_Type> Client_Types { get; set; }

        public virtual DbSet<expense_template_dyna> expense_template_dyna { get; set; }
        public virtual DbSet<Harbour> Harbours { get; set; }
        public virtual DbSet<Dock> Docks { get; set; }
        public virtual DbSet<Quay> Quays { get; set; }
        public virtual DbSet<vatcode> Vatcodes { get; set; }
        public virtual DbSet<Setup> Setups { get; set; }


        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<debitcreditnote>()
                .Property(e => e.DCN_NUMBER)
                .IsUnicode(false);

            modelBuilder.Entity<debitcreditnote>()
                .Property(e => e.DCN_HISTORY)
                .IsUnicode(false);

            modelBuilder.Entity<debitcreditnote>()
                .Property(e => e.DCN_DEBIT_REF)
                .IsUnicode(false);

            modelBuilder.Entity<debitcreditnote>()
                .Property(e => e.DCN_FILE_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<debitcreditnote>()
                .Property(e => e.DCN_TOTAL_AMOUNT)
                .HasPrecision(15, 5);

            modelBuilder.Entity<debitcreditnote>()
                .Property(e => e.DCN_KID)
                .IsUnicode(false);

            modelBuilder.Entity<debitcreditnote>()
                .Property(e => e.DCN_FILE_NAME2)
                .IsUnicode(false);

            modelBuilder.Entity<debitcreditnote>()
                .Property(e => e.DCN_PRE_ADDRESS)
                .IsUnicode(false);

            modelBuilder.Entity<debitcreditnote>()
                .Property(e => e.DCN_ADDRESS)
                .IsUnicode(false);

            modelBuilder.Entity<debitcreditnote>()
                .Property(e => e.DCN_DEBTOR_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<debitcreditnote>()
                .Property(e => e.DCN_CURRENCY_CODE)
                .IsUnicode(false);

            modelBuilder.Entity<debitcreditnote>()
                .Property(e => e.DCN_EXCHANGE_RATE)
                .HasPrecision(15, 5);

            modelBuilder.Entity<debitcreditnote>()
                .Property(e => e.DCN_SETTLEMENT_AMOUNT_PAID)
                .HasPrecision(15, 5);

            modelBuilder.Entity<debitcreditnote>()
                .Property(e => e.DCN_PORTCALL_ROE)
                .HasPrecision(20, 10);

            modelBuilder.Entity<debitcreditnote>()
                .Property(e => e.DCN_VAT_AMOUNT)
                .HasPrecision(20, 10);

            modelBuilder.Entity<dyna_fields>()
                .Property(e => e.CONTAINER)
                .IsUnicode(false);

            modelBuilder.Entity<dyna_fields>()
                .Property(e => e.CAPTION)
                .IsUnicode(false);

            modelBuilder.Entity<dyna_fields>()
                .Property(e => e.DOC_CODE)
                .IsUnicode(false);

            modelBuilder.Entity<dyna_fields>()
                .Property(e => e.FIELD_LIMIT_FROM)
                .HasPrecision(15, 7);

            modelBuilder.Entity<dyna_fields>()
                .Property(e => e.FIELD_LIMIT_TO)
                .HasPrecision(15, 7);

            modelBuilder.Entity<dyna_fields>()
                .Property(e => e.HISTORY)
                .IsUnicode(false);

            modelBuilder.Entity<dyna_fields>()
                .Property(e => e.COMBO_ITEMS)
                .IsUnicode(false);

            modelBuilder.Entity<dyna_fields>()
                .Property(e => e.COMPONENT_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<dyna_fields>()
                .Property(e => e.CAPTION_HINT)
                .IsUnicode(false);

            modelBuilder.Entity<dyna_fields>()
                .Property(e => e.INTEGRATION_REFERENCE)
                .IsUnicode(false);

            modelBuilder.Entity<dyna_fields>()
                .HasMany(e => e.dyna_values)
                .WithRequired(e => e.dyna_fields)
                .HasForeignKey(e => e.FIELD_ID)
                .WillCascadeOnDelete(false);

            modelBuilder.Entity<dyna_values>()
                .Property(e => e.FOREIGN_KEY)
                .IsUnicode(false);

            modelBuilder.Entity<dyna_values>()
                .Property(e => e.VALUE_TEXT)
                .IsUnicode(false);

            modelBuilder.Entity<dyna_values>()
                .Property(e => e.VALUE_FLOAT)
                .HasPrecision(15, 7);

            modelBuilder.Entity<Expense>()
                .Property(e => e.INITIALS)
                .IsUnicode(false);

            modelBuilder.Entity<Expense>()
                .Property(e => e.EXPENSE_TEXT)
                .IsUnicode(false);

            modelBuilder.Entity<Expense>()
                .Property(e => e.QUANTITY)
                .HasPrecision(20, 10);

            modelBuilder.Entity<Expense>()
                .Property(e => e.PRICE)
                .HasPrecision(20, 10);

            modelBuilder.Entity<Expense>()
                .Property(e => e.AMOUNT)
                .HasPrecision(20, 10);

            modelBuilder.Entity<Expense>()
                .Property(e => e.HANDLING)
                .HasPrecision(20, 10);

            modelBuilder.Entity<Expense>()
                .Property(e => e.VAT)
                .HasPrecision(20, 10);

            modelBuilder.Entity<Expense>()
                .Property(e => e.TOTAL_SUM)
                .HasPrecision(20, 10);

            modelBuilder.Entity<Expense>()
                .Property(e => e.VOUCHER_REF)
                .IsUnicode(false);

            modelBuilder.Entity<Expense>()
                .Property(e => e.ACCOUNT_CODE)
                .IsUnicode(false);

            modelBuilder.Entity<Expense>()
                .Property(e => e.MISC)
                .IsUnicode(false);

            modelBuilder.Entity<Expense>()
                .Property(e => e.AGENCY_FEE)
                .HasPrecision(20, 10);

            modelBuilder.Entity<Expense>()
                .Property(e => e.REBATE_PERCENT)
                .HasPrecision(20, 10);

            modelBuilder.Entity<Expense>()
                .Property(e => e.REBATE_TOTAL)
                .HasPrecision(20, 10);

            modelBuilder.Entity<Expense>()
                .Property(e => e.POSTAGE_PETTIES)
                .HasPrecision(20, 10);

            modelBuilder.Entity<Expense>()
                .Property(e => e.TELEX_FAX)
                .HasPrecision(20, 10);

            modelBuilder.Entity<Expense>()
                .Property(e => e.POSTAGE_FOR_VESSEL)
                .HasPrecision(20, 10);

            modelBuilder.Entity<Expense>()
                .Property(e => e.HAULING_EXPENSES_KM)
                .HasPrecision(20, 10);

            modelBuilder.Entity<Expense>()
                .Property(e => e.HAULING_EXPENSES_TOTAL)
                .HasPrecision(20, 10);

            modelBuilder.Entity<Expense>()
                .Property(e => e.BANK_CHARGES)
                .HasPrecision(20, 10);

            modelBuilder.Entity<Expense>()
                .Property(e => e.WATER_CLERK_OVERTIME)
                .HasPrecision(20, 10);

            modelBuilder.Entity<Expense>()
                .Property(e => e.USER_RULE_MULTIPLY_1)
                .HasPrecision(20, 10);

            modelBuilder.Entity<Expense>()
                .Property(e => e.USER_RULE_VALUE_1)
                .HasPrecision(20, 10);

            modelBuilder.Entity<Expense>()
                .Property(e => e.USER_RULE_MULTIPLY_2)
                .HasPrecision(20, 10);

            modelBuilder.Entity<Expense>()
                .Property(e => e.USER_RULE_VALUE_2)
                .HasPrecision(20, 10);

            modelBuilder.Entity<Expense>()
                .Property(e => e.USER_DEFINED_CAPTION_1)
                .IsUnicode(false);

            modelBuilder.Entity<Expense>()
                .Property(e => e.USER_DEFINED_VALUE_1)
                .HasPrecision(20, 10);

            modelBuilder.Entity<Expense>()
                .Property(e => e.USER_DEFINED_CAPTION_2)
                .IsUnicode(false);

            modelBuilder.Entity<Expense>()
                .Property(e => e.USER_DEFINED_VALUE_2)
                .HasPrecision(20, 10);

            modelBuilder.Entity<Expense>()
                .Property(e => e.TOTAL_FEES)
                .HasPrecision(20, 10);

            modelBuilder.Entity<Expense>()
                .Property(e => e.AMOUNT_ADVANCE)
                .HasPrecision(20, 10);

            modelBuilder.Entity<Expense>()
                .Property(e => e.AMOUNT_RECEIVED)
                .HasPrecision(20, 10);

            modelBuilder.Entity<Expense>()
                .Property(e => e.PAID_HISTORY)
                .IsUnicode(false);

            modelBuilder.Entity<Expense>()
                .Property(e => e.AMOUNT_FOREIGN_CURRENCY)
                .HasPrecision(20, 10);

            modelBuilder.Entity<Expense>()
                .Property(e => e.AMOUNT_LOCAL_CURRENCY)
                .HasPrecision(20, 10);

            modelBuilder.Entity<Expense>()
                .Property(e => e.AMOUNT_AS_TEXT)
                .IsUnicode(false);

            modelBuilder.Entity<Expense>()
                .Property(e => e.HISTORY)
                .IsUnicode(false);

            modelBuilder.Entity<Expense>()
                .Property(e => e.BATCH_NO)
                .IsUnicode(false);

            modelBuilder.Entity<Expense>()
                .Property(e => e.CODE1)
                .IsUnicode(false);

            modelBuilder.Entity<Expense>()
                .Property(e => e.CODE2)
                .IsUnicode(false);

            modelBuilder.Entity<Expense>()
                .Property(e => e.PRINTED_USER)
                .IsUnicode(false);

            modelBuilder.Entity<Expense>()
                .Property(e => e.ORIGINAL_BUDGET)
                .HasPrecision(15, 5);

            modelBuilder.Entity<Expense>()
                .Property(e => e.LINK_DOCUMENT)
                .IsUnicode(false);

            modelBuilder.Entity<Expense>()
                .Property(e => e.COUNTER_ACCOUNT)
                .IsUnicode(false);

            modelBuilder.Entity<Expense>()
                .Property(e => e.ORIGINAL_ACTUAL_VALUE)
                .HasPrecision(15, 5);

            modelBuilder.Entity<Expense>()
                .Property(e => e.WORK_HOURS)
                .HasPrecision(15, 5);

            modelBuilder.Entity<Expense>()
                .Property(e => e.WORK_PRICE)
                .HasPrecision(15, 5);

            modelBuilder.Entity<Expense>()
                .Property(e => e.WORK_AMOUNT)
                .HasPrecision(15, 5);

            modelBuilder.Entity<Expense>()
                .Property(e => e.EXCHANGE_RATE)
                .HasPrecision(15, 5);

            modelBuilder.Entity<Expense>()
                .Property(e => e.PROFIT_PCT)
                .HasPrecision(15, 5);

            modelBuilder.Entity<Expense>()
                .Property(e => e.PROFIT)
                .HasPrecision(15, 5);

            modelBuilder.Entity<Expense>()
                .Property(e => e.APPROVAL_COMMENT)
                .IsUnicode(false);

            modelBuilder.Entity<Expense>()
                .Property(e => e.INVOICE_CUR)
                .IsUnicode(false);

            modelBuilder.Entity<Expense>()
                .Property(e => e.ROE_INV_CUR)
                .HasPrecision(20, 10);

            modelBuilder.Entity<Expense>()
                .Property(e => e.VAT_DESCRIPTION)
                .IsUnicode(false);

            modelBuilder.Entity<Expense>()
                .Property(e => e.INVOICE_AMOUNT)
                .HasPrecision(20, 10);

            modelBuilder.Entity<Expense>()
                .Property(e => e.PO_NO)
                .IsUnicode(false);

            modelBuilder.Entity<Expense>()
                .Property(e => e.ROE_CTM)
                .HasPrecision(20, 10);

            modelBuilder.Entity<incoming_invoice>()
                .Property(e => e.IIN_NUMBER)
                .IsUnicode(false);

            modelBuilder.Entity<incoming_invoice>()
                .Property(e => e.IIN_AMOUNT)
                .HasPrecision(20, 5);

            modelBuilder.Entity<incoming_invoice>()
                .Property(e => e.IIN_VAT_AMOUNT)
                .HasPrecision(20, 5);

            modelBuilder.Entity<incoming_invoice>()
                .Property(e => e.IIN_CURRENCY_CODE)
                .IsUnicode(false);

            modelBuilder.Entity<incoming_invoice>()
                .Property(e => e.IIN_ROE)
                .HasPrecision(20, 8);

            modelBuilder.Entity<incoming_invoice>()
                .Property(e => e.IIN_COMMENT)
                .IsUnicode(false);

            modelBuilder.Entity<incoming_invoice>()
                .Property(e => e.IIN_HISTORY)
                .IsUnicode(false);

            modelBuilder.Entity<incoming_invoice>()
                .Property(e => e.IIN_REFERENCE_NO)
                .IsUnicode(false);

            modelBuilder.Entity<incoming_invoice>()
                .Property(e => e.IIN_INTERNAL_NUMBER)
                .IsUnicode(false);

            modelBuilder.Entity<incoming_invoice>()
                .Property(e => e.IIN_PO_NO)
                .IsUnicode(false);

            modelBuilder.Entity<incoming_invoice>()
                .HasMany(e => e.Expenses)
                .WithOptional(e => e.incoming_invoice)
                .HasForeignKey(e => e.INCOMING_INVOICE_ID);

            modelBuilder.Entity<PortCall>()
                .Property(e => e.PORTCALL_NUMBER)
                .IsUnicode(false);

            modelBuilder.Entity<PortCall>()
                .Property(e => e.CANCEL_BY)
                .IsUnicode(false);

            modelBuilder.Entity<PortCall>()
                .Property(e => e.ANC_PLACE)
                .IsUnicode(false);

            modelBuilder.Entity<PortCall>()
                .Property(e => e.MASTER_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<PortCall>()
                .Property(e => e.VOY)
                .IsUnicode(false);

            modelBuilder.Entity<PortCall>()
                .Property(e => e.CHARTERER)
                .IsUnicode(false);

            modelBuilder.Entity<PortCall>()
                .Property(e => e.FIXED_BY)
                .IsUnicode(false);

            modelBuilder.Entity<PortCall>()
                .Property(e => e.ARRIVAL_DRAFT)
                .HasPrecision(20, 10);

            modelBuilder.Entity<PortCall>()
                .Property(e => e.DEPARTURE_DRAFT)
                .HasPrecision(20, 10);

            modelBuilder.Entity<PortCall>()
                .Property(e => e.DROP_ANCHOR_PLACE)
                .IsUnicode(false);

            modelBuilder.Entity<PortCall>()
                .Property(e => e.BUNKERS_ON_ARRIVAL_MDO)
                .HasPrecision(20, 10);

            modelBuilder.Entity<PortCall>()
                .Property(e => e.BUNKERS_ON_ARRIVAL_GO)
                .HasPrecision(20, 10);

            modelBuilder.Entity<PortCall>()
                .Property(e => e.BUNKERS_ON_ARRIVAL_USER_TEXT)
                .IsUnicode(false);

            modelBuilder.Entity<PortCall>()
                .Property(e => e.BUNKERS_ON_ARRIVAL_USER_VALUE)
                .HasPrecision(20, 10);

            modelBuilder.Entity<PortCall>()
                .Property(e => e.DRAFT_ON_ARRIVAL_FWO)
                .HasPrecision(20, 10);

            modelBuilder.Entity<PortCall>()
                .Property(e => e.DRAFT_ON_DEPARTURE_FWO)
                .HasPrecision(20, 10);

            modelBuilder.Entity<PortCall>()
                .Property(e => e.DRAFT_ON_ARRIVAL_AFT)
                .HasPrecision(20, 10);

            modelBuilder.Entity<PortCall>()
                .Property(e => e.DRAFT_ON_DEPARTURE_AFT)
                .HasPrecision(20, 10);

            modelBuilder.Entity<PortCall>()
                .Property(e => e.BUNKERS_TAKEN_MDO)
                .HasPrecision(20, 10);

            modelBuilder.Entity<PortCall>()
                .Property(e => e.BUNKERS_TAKEN_GO)
                .HasPrecision(20, 10);

            modelBuilder.Entity<PortCall>()
                .Property(e => e.BUNKERS_TAKEN_USER_TEXT)
                .IsUnicode(false);

            modelBuilder.Entity<PortCall>()
                .Property(e => e.BUNKERS_TAKEN_USER_VALUE)
                .HasPrecision(20, 10);

            modelBuilder.Entity<PortCall>()
                .Property(e => e.BUNKERS_ON_DEP_MDO)
                .HasPrecision(20, 10);

            modelBuilder.Entity<PortCall>()
                .Property(e => e.BUNKERS_ON_DEP_GO)
                .HasPrecision(20, 10);

            modelBuilder.Entity<PortCall>()
                .Property(e => e.BUNKERS_ON_DEP_USER_TEXT)
                .IsUnicode(false);

            modelBuilder.Entity<PortCall>()
                .Property(e => e.BUNKERS_ON_DEP_USER_VALUE)
                .IsUnicode(false);

            modelBuilder.Entity<PortCall>()
                .Property(e => e.AGENT_NEXT_PORT)
                .IsUnicode(false);

            modelBuilder.Entity<PortCall>()
                .Property(e => e.PROSPECT_MISC)
                .IsUnicode(false);

            modelBuilder.Entity<PortCall>()
                .Property(e => e.NOTES)
                .IsUnicode(false);

            modelBuilder.Entity<PortCall>()
                .Property(e => e.REMARKS)
                .IsUnicode(false);

            modelBuilder.Entity<PortCall>()
                .Property(e => e.HISTORY)
                .IsUnicode(false);

            modelBuilder.Entity<PortCall>()
                .Property(e => e.CARGO_NO)
                .IsUnicode(false);

            modelBuilder.Entity<PortCall>()
                .Property(e => e.INBOUND_PILOT)
                .IsUnicode(false);

            modelBuilder.Entity<PortCall>()
                .Property(e => e.OUTBOUND_PILOT)
                .IsUnicode(false);

            modelBuilder.Entity<PortCall>()
                .Property(e => e.MASTER_CERTIFICATE)
                .IsUnicode(false);

            modelBuilder.Entity<PortCall>()
                .Property(e => e.SLOP)
                .HasPrecision(20, 10);

            modelBuilder.Entity<PortCall>()
                .Property(e => e.SLUDGE)
                .HasPrecision(20, 10);

            modelBuilder.Entity<PortCall>()
                .Property(e => e.JNO)
                .IsUnicode(false);

            modelBuilder.Entity<PortCall>()
                .Property(e => e.CARGONUMBER)
                .IsUnicode(false);

            modelBuilder.Entity<PortCall>()
                .Property(e => e.CARGO_DESC)
                .IsUnicode(false);

            modelBuilder.Entity<PortCall>()
                .Property(e => e.USERDEF_1)
                .IsUnicode(false);

            modelBuilder.Entity<PortCall>()
                .Property(e => e.USERDEF_2)
                .IsUnicode(false);

            modelBuilder.Entity<PortCall>()
                .Property(e => e.PERSON_IN_CHARGE)
                .IsUnicode(false);

            modelBuilder.Entity<PortCall>()
                .Property(e => e.LOCKED_BY)
                .IsUnicode(false);

            modelBuilder.Entity<PortCall>()
                .Property(e => e.GSCID)
                .IsUnicode(false);

            modelBuilder.Entity<PortCall>()
                .Property(e => e.CHANGE_STAMP)
                .IsFixedLength();

            modelBuilder.Entity<PortCall>()
                .Property(e => e.PROSPECTS_HINT)
                .IsUnicode(false);

            modelBuilder.Entity<PortCall>()
                .Property(e => e.INV_CURRENCY)
                .IsUnicode(false);

            modelBuilder.Entity<PortCall>()
                .Property(e => e.DOC_FOLDER)
                .IsUnicode(false);

            modelBuilder.Entity<PortCall>()
                .Property(e => e.EXP_INSTRUCTIONS)
                .IsUnicode(false);         

            modelBuilder.Entity<Vessel>()
                .Property(e => e.VESSEL_TYPE_CODE)
                .IsUnicode(false);

            modelBuilder.Entity<Vessel>()
                .Property(e => e.NATIONAL_CODES_ID)
                .IsUnicode(false);

            modelBuilder.Entity<Vessel>()
                .Property(e => e.VESSEL_PREFIX)
                .IsUnicode(false);

            modelBuilder.Entity<Vessel>()
                .Property(e => e.NAME)
                .IsUnicode(false);

            modelBuilder.Entity<Vessel>()
                .Property(e => e.LLOYDSNR)
                .IsUnicode(false);

            modelBuilder.Entity<Vessel>()
                .Property(e => e.DWT)
                .HasPrecision(20, 10);

            modelBuilder.Entity<Vessel>()
                .Property(e => e.GT)
                .HasPrecision(20, 10);

            modelBuilder.Entity<Vessel>()
                .Property(e => e.NT)
                .HasPrecision(20, 10);

            modelBuilder.Entity<Vessel>()
                .Property(e => e.LOA)
                .HasPrecision(20, 10);

            modelBuilder.Entity<Vessel>()
                .Property(e => e.BEAM)
                .HasPrecision(20, 10);

            modelBuilder.Entity<Vessel>()
                .Property(e => e.SUMMERDRAFT)
                .HasPrecision(20, 10);

            modelBuilder.Entity<Vessel>()
                .Property(e => e.CBMTANK)
                .HasPrecision(20, 10);

            modelBuilder.Entity<Vessel>()
                .Property(e => e.AIR_DRAFT)
                .HasPrecision(20, 10);

            modelBuilder.Entity<Vessel>()
                .Property(e => e.SBT)
                .HasPrecision(20, 10);

            modelBuilder.Entity<Vessel>()
                .Property(e => e.CALLSIGN)
                .IsUnicode(false);

            modelBuilder.Entity<Vessel>()
                .Property(e => e.MOBILE1)
                .IsUnicode(false);

            modelBuilder.Entity<Vessel>()
                .Property(e => e.MOBILE2)
                .IsUnicode(false);

            modelBuilder.Entity<Vessel>()
                .Property(e => e.TELEFAX)
                .IsUnicode(false);

            modelBuilder.Entity<Vessel>()
                .Property(e => e.TELEX)
                .IsUnicode(false);

            modelBuilder.Entity<Vessel>()
                .Property(e => e.FLAG)
                .IsUnicode(false);

            modelBuilder.Entity<Vessel>()
                .Property(e => e.HOMEPORT)
                .IsUnicode(false);

            modelBuilder.Entity<Vessel>()
                .Property(e => e.REG_PLACE)
                .IsUnicode(false);

            modelBuilder.Entity<Vessel>()
                .Property(e => e.KNOTS)
                .HasPrecision(20, 10);

            modelBuilder.Entity<Vessel>()
                .Property(e => e.HOLD_HATCH_DESC)
                .IsUnicode(false);

            modelBuilder.Entity<Vessel>()
                .Property(e => e.CLASS)
                .IsUnicode(false);

            modelBuilder.Entity<Vessel>()
                .Property(e => e.CRANES)
                .IsUnicode(false);

            modelBuilder.Entity<Vessel>()
                .Property(e => e.COMMENTS)
                .IsUnicode(false);

            modelBuilder.Entity<Vessel>()
                .Property(e => e.HISTORY)
                .IsUnicode(false);

            modelBuilder.Entity<Vessel>()
                .Property(e => e.LL)
                .HasPrecision(20, 10);

            modelBuilder.Entity<Vessel>()
                .Property(e => e.USER_VALUE)
                .HasPrecision(20, 10);

            modelBuilder.Entity<Vessel>()
                .Property(e => e.CGT)
                .HasPrecision(20, 10);

            modelBuilder.Entity<Vessel>()
                .Property(e => e.TONNAGE_CERTIFICATE)
                .IsUnicode(false);

            modelBuilder.Entity<Vessel>()
                .Property(e => e.USER_TEXT_VALUE)
                .IsUnicode(false);

            modelBuilder.Entity<Vessel>()
                .Property(e => e.ISPS_SERTIFICATE)
                .IsUnicode(false);

            modelBuilder.Entity<Vessel>()
                .Property(e => e.ICE_CLASS)
                .IsUnicode(false);

            modelBuilder.Entity<Vessel>()
                .Property(e => e.GSCID)
                .IsUnicode(false);

            modelBuilder.Entity<Vessel>()
                .Property(e => e.WARNING)
                .IsUnicode(false);

            modelBuilder.Entity<Vessel>()
                .Property(e => e.PUBLIC_ADV)
                .IsUnicode(false);

            modelBuilder.Entity<Vessel>()
                .Property(e => e.DOC_FOLDER)
                .IsUnicode(false);

            modelBuilder.Entity<Vessel>()
                .Property(e => e.CSI)
                .HasPrecision(15, 5);

            modelBuilder.Entity<Vessel>()
                .Property(e => e.ESI)
                .HasPrecision(15, 5);           

            modelBuilder.Entity<ExpenseView>()
                .Property(e => e.EXP_GRP)
                .IsUnicode(false);

            modelBuilder.Entity<ExpenseView>()
                .Property(e => e.EXP_TEXT)
                .IsUnicode(false);

            modelBuilder.Entity<ExpenseView>()
                .Property(e => e.EXP_QUANTITY)
                .HasPrecision(38, 16);

            modelBuilder.Entity<ExpenseView>()
                .Property(e => e.EXP_PRICE)
                .HasPrecision(38, 18);

            modelBuilder.Entity<ExpenseView>()
                .Property(e => e.EXP_AMOUNT)
                .HasPrecision(38, 17);

            modelBuilder.Entity<ExpenseView>()
                .Property(e => e.EXP_HANDLING)
                .HasPrecision(38, 6);

            modelBuilder.Entity<ExpenseView>()
                .Property(e => e.EXP_VAT)
                .HasPrecision(38, 6);

            modelBuilder.Entity<ExpenseView>()
                .Property(e => e.EXP_LOCAL_VAT)
                .HasPrecision(38, 6);

            modelBuilder.Entity<ExpenseView>()
                .Property(e => e.VAT_PERCENTAGE)
                .HasPrecision(20, 10);

            modelBuilder.Entity<ExpenseView>()
                .Property(e => e.EXP_TOTAL_SUM)
                .HasPrecision(22, 10);

            modelBuilder.Entity<ExpenseView>()
                .Property(e => e.EXP_LOCAL_SUM)
                .HasPrecision(38, 15);

            modelBuilder.Entity<ExpenseView>()
                .Property(e => e.EXP_TOTAL_PROFIT)
                .HasPrecision(38, 6);

            modelBuilder.Entity<ExpenseView>()
                .Property(e => e.CLIENT_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<ExpenseView>()
                .Property(e => e.CLIENT_ACCOUNT)
                .IsUnicode(false);

            modelBuilder.Entity<ExpenseView>()
                .Property(e => e.EXP_ORIG_BUDGET)
                .HasPrecision(15, 5);

            modelBuilder.Entity<ExpenseView>()
                .Property(e => e.EXP_LINK_DOCUMENT)
                .IsUnicode(false);

            modelBuilder.Entity<ExpenseView>()
                .Property(e => e.EXP_ACCOUNT_CODE)
                .IsUnicode(false);

            modelBuilder.Entity<ExpenseView>()
                .Property(e => e.EXPENSE_STATUS_TEXT)
                .IsUnicode(false);

            modelBuilder.Entity<ExpenseView>()
                .Property(e => e.SUPPNAME)
                .IsUnicode(false);

            modelBuilder.Entity<ExpenseView>()
                .Property(e => e.SUPP_ACCOUNT)
                .IsUnicode(false);

            modelBuilder.Entity<ExpenseView>()
                .Property(e => e.InvoiceNo)
                .IsUnicode(false);

            modelBuilder.Entity<ExpenseView>()
                .Property(e => e.VOUCHER_REF)
                .IsUnicode(false);

            modelBuilder.Entity<ExpenseView>()
                .Property(e => e.INITIALS)
                .IsUnicode(false);

            modelBuilder.Entity<ExpenseView>()
                .Property(e => e.BATCH_NO)
                .IsUnicode(false);

            modelBuilder.Entity<ExpenseView>()
                .Property(e => e.EXP_WORK_HOURS)
                .HasPrecision(15, 5);

            modelBuilder.Entity<ExpenseView>()
                .Property(e => e.EXP_WORK_PRICE)
                .HasPrecision(15, 5);

            modelBuilder.Entity<ExpenseView>()
                .Property(e => e.EXP_WORK_AMOUNT)
                .HasPrecision(15, 5);

            modelBuilder.Entity<ExpenseView>()
                .Property(e => e.EXP_HANDLING_PERCENT)
                .HasPrecision(20, 10);

            modelBuilder.Entity<ExpenseView>()
                .Property(e => e.LOCAL_CURRENCY_CODE)
                .IsUnicode(false);

            modelBuilder.Entity<ExpenseView>()
                .Property(e => e.INVOICE_CURRENCY)
                .IsUnicode(false);

            modelBuilder.Entity<ExpenseView>()
                .Property(e => e.EXP_CURRENCY_NAME)
                .IsUnicode(false);

            modelBuilder.Entity<ExpenseView>()
                .Property(e => e.EXP_EXCHANGE_RATE)
                .HasPrecision(15, 5);

            modelBuilder.Entity<ExpenseView>()
                .Property(e => e.INV_EXCHANGE_RATE)
                .HasPrecision(20, 10);

            modelBuilder.Entity<ExpenseView>()
                .Property(e => e.EXP_ORIGINAL_ACTUAL_VALUE)
                .HasPrecision(15, 5);

            modelBuilder.Entity<ExpenseView>()
                .Property(e => e.EXP_MISC)
                .IsUnicode(false);
        }
    }
}
