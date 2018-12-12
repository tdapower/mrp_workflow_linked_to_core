//******************************************
// Authur            :Shanika Amarasinghe
// Date              :25/05/2012
// Reviewed By       :
// Description       : Store All the Stored Procedures
//******************************************
using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Web;

/// <summary>
/// Summary description for GenericStoredProcedure
/// </summary>
public class GenericStoredProcedure
{
    //-----Testing
    public static string SelectCustomer = "SP_SELECT_CUSTOMER";
    public static string InsertCustomer = "SP_INSERT_CUSTOMER";
    public static string UpdateCustomer = "SP_UPDATE_CUSTOMER";
    public static string DeleteCustomer = "SP_DELETE_CUSTOMER";
    //------

    //----Servicing----------------------------------------------------------------
    //------After Policy Service-Load Data PHS
    public static string SelectPolicy = "WF_PHS_SELECTPOLICY";
    public static string SelectPolicyByPolicyNumber = "WF_PHS_SELECT_BYPOLICY_NO";

    //------After Policy Service-Load Data MRP
    public static string SelectPolicyFromMRP = "WF_MRP_SELECTPOLICY";
    public static string SelectPolicyByPolicyNumberMRP = "WF_MRP_SELECT_BYPOLICY_NO";

    //------Referance data
    public static string SelectJobType = "WF_PHS_SELECTJOBTYPE";
    public static string SelectBranch = "WF_PHS_SELECTBRANCH";
    public static string SelectStatus = "WF_PHS_SELECTSTATUS";
    public static string SelectIFPassed = "WF_PHS_SELECT_IFPASSED";
    public static string SelectPendingReason = "WF_PHS_SELECT_PENDING_REASON";
    public static string SelectUsers = "WF_PHS_SELECTUSERS";
    public static string SelectPendingCategory = "WF_PHS_SELECT_PENDINGCAT";
    public static string SelectUserLog = "WF_PHS_GET_USERLOG";
    public static string SelectUsersLevels = "WF_PHS_SELECTUSERS_LEVELS";

    //-------Insert Register
    public static string RegisterInsert = "WF_PHS_INSERT_REGISTER";
    public static string GetJobNoAtInsert = "WF_PHS_GETJOBNO";

    //-------Get Job After Insert
    public static string SelectJobAfterInsert = "WF_PHS_SELECTJOB_AFTERINSERT";
    public static string SelectJobByJobNo = "WF_PHS_SELECT_JOBBYJOBNO";

    //-------Update Register
    public static string UpdateRegister = "WF_PHS_UPDATE_REGISTER";

    //-------Update Register-Allocated User Email
    public static string UpdateRegisterAssignUserMail = "WF_PHS_UPDATE_ASIGN_USERMAIL";

    //-------View Attatched Documents
    public static string GetDocuments = "WF_PHS_GET_DOCUMENTS";

    //-------Insert into Pending Reason Table
    public static string InsertPendings = "WF_PHS_INSERT_PENDINGREASON";
    //-------Select Pending Reason Table
    public static string SelectPendings = "WF_PHS_SELECT_PENDINGS_JOBNO";
    public static string SelectOtherPendings = "WF_PHS_SELECT_OTHERPEND_JOBNO";

    //-------Populate data in from referance page
    public static string PopulateJobTypeRef = "WF_PHS_POPULATE_JOBTYPE_REF";
    public static string PopulateIFPassedRef = "WF_PHS_POPULATE_IFPASSED_REF";
    public static string PopulatePendingReasonRef = "WF_PHS_POPULATE_PENDING_REF";
    public static string PopulateUsers = "WF_PHS_POPULATE__USERS";

    //------Job Allocation
    public static string GetJobsForAllocation = "WF_PHS_SELECT_JOBFORALLOCATION";
    public static string UpdateRegisterUserAllocation = "WF_PHS_UPDATE_REGISTER_USER";

    //------Reminder
    public static string SelectJobForReminder = "WF_PHS_SELECTJOB_REMINDER";
    public static string SelectPendingTableForJob = "WF_PHS_SELECTPENDING_REMINDER";
    public static string UpdatePendingReasonTable = "WF_PHS_UPDATE_PENDINGRREASON";
    public static string UpdateClearPendings = "WF_PHS_UPDATE_CLEARPENDINGS";

    //-------Dash board
    public static string ViewRegister_DashBoard = "WF_PHS_SELECT_REGISTER_VIEW";
    public static string ViewCutOffExceeded = "WF_PHS_SELECT_CUTOFF_EXCEEDED";
    public static string ViewFirstReminder = "WF_PHS_SELECT_1STREMINDER";
    public static string ViewSecReminder = "WF_PHS_SELECT_2NDREMINDER";

    //-------Reports
    public static string ViewReports = "WF_PHS_SELECT_REPORTS";
    public static string ViewReportStatusUnderwritingLevel = "WF_PHS_RPT_STATUS_UL";
    public static string ViewReportStatusSupervisorLevel = "WF_PHS_RPT_CURRENTSTATUS_SL";
    public static string DeleteReportRecords = "WF_PHS_DELETE_RECORDS_RPT";
    public static string ViewReportAgeAnalysis_NotAttendedjobs = "WF_PHS_RPT_AGEANALYSIS_JOBS";
    public static string ViewReportPendingJobsBranchWise = "WF_PHS_RPT_PENDINGJOBS";
    public static string ViewReportTotalJobs = "WF_PHS_RPT_TOTAL_JOBS";
    public static string ViewReportAgeAnalysis_PendingJobs = "WF_PHS_RPT_AGEANALY_PENDINGS";
    public static string ViewReportJobWiseAnalysis = "WF_PHS_RPT_JOBWISEANALYSIS";
    public static string ViewReportBenchmarkAnalysis = "WF_PHS_RPT_BANCHMARK_ANALYSIS";
    public static string ViewReportCurrentStatusSupervisoryLevelJobWise = "WF_PHS_RPT_PASSEDJOBWISE";
    public static string DeleteReportRecordsPendingJobs = "WF_PHS_DELETE_PENDINGSRPT";
    public static string ViewFullRegister = "WF_PHS_REGISTERALL_RECORDS";
    public static string ViewFullPendings = "WF_PHS_REGISTERALL_PENDINGS";

    //------Get Mail Address
    public static string GetMailAddress = "WF_PHS_SELECT_EMAILADDRESS";
    public static string GetMailAddressUser = "WF_PHS_SELECT_EMAIL_USER";

    //------User Log
    public static string UserLog = "WF_PHS_POPULATE_USERLOGINSERT";

    //------Covering Letter
    public static string CoveringLetter = "WF_PHS_SELECT_COVERINGLETTER";
    public static string DeleterCoveringLetterRecords = "WF_PHS_DELETE_LETTER_RPT";

    //------Inquiry Details
    public static string GetInqiuryDetailsEvents = "WF_PHS_INQUIRY_GETEVENTS";
    public static string GetInqiuryPendings = "WF_PHS_INQUIRY_PENDINGS";
    public static string GetPassedDetails = "WF_PHS_INQUIRY_PASSEDEVENTS";


    //------Select Pending Documents Job Wise
    public static string SelectPendingDocumentsJobWise = "WF_PHS_SELECT_JOBTYPEDOCS";


    //------Insert Other Documents at job creation
    public static string InsertOtherDocuments = "WF_PHS_INSERT_OTHERDOCUMENTS";

    //------SMS- Job Complete Date
    public static string JobCompleteDateForSMS = "WF_PHS_SELECT_SMSCOMPLETE";

    //------SMS-EMAIL Set Log in Events Table
    public static string SMS_EMAIL_LOG = "WF_PHS_EVENTS_SMS_EMAIL";

    //----Servicing END----------------------------------------------------------------




    //----Benchmark Monitoring System----------------------------------------------------------------
    public static string SelectAssighnedUsers = "WF_SELECT_ASSIGNEEDUSERS";
    public static string SelectApproveUsers = "WF_SELECT_APPROVEUSERS";
    public static string SelectBranchesForBenchMarkMonitor = "WF_SELECT_BRANCHES";
    public static string PaymnetSheetLabour = "WF_MTR_PS_LABOUR_DET";
    public static string SearchSpareParts = "WF_SEARCH_SPARE_PARTS";
    public static string PaymnetSheetReplacement = "WF_MTR_PS_REPLACEMENT_DET";
    public static string PaymnetSheetDeduction = "WF_MTR_PS_DEDUCTION_DET";
    public static string DeliveryItem = "WF_MTR_DO_ITEM_LIST";
    public static string SelectReport1 = "WF_BENCHMARK_ANALYSIS_RPT1TEST";

    public static string SelectReport2 = "WF_BENCHMARK_ANALYSIS_RPT_2";
    public static string SelectReport2FastTrack = "WF_BENCHMARK_ANALYSIS_RPT_2_FT";

    public static string SelectReport3 = "WF_BENCHMARK_ANALYSIS_RPT_3";

    public static string SelectReport4 = "WF_BENCHMARK_ANALYSIS_RPT_4";
    public static string SelectReport4FastTrack = "WF_BM_ANALYSIS_RPT_4_FT";
    
    public static string SelectReport5 = "WF_BENCHMARK_ANAL_RPT_4_ZONAL";
    public static string SelectReport5FastTrack = "WF_BM_ANAL_RPT_4_ZONAL_FT";
    
    public static string SelectReport6 = "WF_BENCHMARK_ANALY_RPT_2_ZONAL";
    public static string SelectReport6FastTrack = "WF_BM_ANALY_RPT_2_ZONAL_FT";
    
    public static string SelectReport7 = "WF_BENCHMARK_ANALY_RPT1_ZONAL";

    public static string SelectNewBusinessDocument = "WF_LIFE_NB_DOCUMENT_DATA";

    public static string SelectReport10 = "WF_BENCHMARK_ZONAL";
    public static string SelectReport11 = "WF_BENCHMARK_POLICY_ACKNOW";

    public static string SelectReportBenchMarkAgeAnalysis = "WF_BENCHMARK_AGE_ANALYSIS";

    public static string SelectReport12 = "WF_BENCHMARK_POL_ACKNOW_ZONAL";
    public static string SelectReport15 = "WF_BENCHMARK_LIFE_PENDING_WF";
    public static string DeleteReports = "WF_DELETE_RECORDS_RPT";
    public static string PendingsRecievedReport = "WF_RPT_PENDING_RECEIVED";
    public static string PendingsProposalReport = "WF_RPT_PENDING_PROPOSAL";
    public static string RefundProposalReport = "WF_RPT_REFUND_PROPOSAL";
    public static string DeleteReportPendingsRecieved = "WF_DELETE_PENDINGRECORDS_RPT";

    //Proposal Details Report
    public static string ProposalDetailsReport = "WF_PROPOSALDETAILS_RPT";
    public static string DeleteReportProposalDetails = "WF_DELETE_PROPOSALDETAILS_RPT";
    public static string SelectProposalStatus = "WF_SELECT_PROPOSAL_STATUS";

    //----Benchmark Monitoring System----------------------------------------------------------------



    //----MRP Workflow Benchmark Monitoring----------------------------------------------------------------
    public static string SelectMRPWFBenchmarkReport = "MRP_WF_BENCHMARK_RPT";
    public static string SelectMRPWFStatusWiseProposalsReport = "MRP_WF_STATUS_PROPOSALS";
    public static string SelectMRPWFBenchmarkSummaryReport = "MRP_WF_BENCHMARK_SUM_RPT";
    public static string SelectMRPWFBenchmarkSummaryReport4 = "MRP_WF_BENCHMARK_SUM_RPT_4";
    public static string SelectMRPWFBenchmarkSummaryReport2 = "MRP_WF_BENCHMARK_SUM_RPT_2";
    public static string SelectMRPWFBenchmarkSummaryReport3 = "MRP_WF_BENCHMARK_SUM_RPT_3";

    public static string SelectMRPSQuotationSummaryReport = "MRPS_QUOTATION_REPORT";
    public static string SelectMRPWFBenchmarkSummaryReportPendingLetters = "MRP_WF_PENDING_BENCHMARK_RPT";
    public static string SelectMRPWFBenchmarkSummaryReportPendingLetters2 = "MRP_WF_PENDING_BENCHMARK_RPT2";
    public static string SelectMRPWFBenchmarkSummaryReportCoverNote = "MRP_WF_CVR_NOTE_BENCHMARK_RPT";
    public static string SelectMRPWFBenchmarkSummaryReportCoverNote2 = "MRP_WF_CVR_NOTE_BENCHMARK_RPT2";
    public static string SelectMRPWFBenchmarkSummaryReportPolicyIssue = "MRP_WF_POL_ISSUE_BENCHMARK_RPT";
    public static string SelectMRPWFBenchmarkSummaryReportPolicyIssue2 = "MRP_WF_POL_ISSUE_BENCHMARK_RP2";

    public static string SelectMRPWFFSTBenchmarkSummaryReport = "MRP_WF_FST_BENCHMARK_SMRY_RPT";

    public static string SelectMRPWFFSTBenchmarkDailyReport = "MRP_WF_FST_BENCHMARK_DTL_RPT";


    public static string SelectMRPWFBenchmarkSPendingComplete = "MRP_WF_PEND_CLEAR_BENCHMARK";
    public static string SelectMRPWFBenchmarkJobAlloSummary = "MRP_WF_JOB_ALLO_SUMMARY";


    // public static string SelectMRPWFBenchmarkReport = "MRP_WF_BENCHMARK_RPT_TEST";
    //---------------------------------------------------------------


    //----Bulk Receipt Printing----------------------------------------------------------------
    public static string SelectGeneralReceiptsReport = "SP_BULK_RECEIPT_GENERAL";
    public static string SelectLifeReceiptsReport = "SP_BULK_RECEIPT_LIFE_TCS";
    public static string SelectLifeNonTCSReceiptsReport = "SP_BULK_RECEIPT_LIFE_NON_TCS";
    public static string SelectRanmagaReceiptsReport = "SP_BULK_RECEIPT_RANMAGA";

    //---------------------------------------------------------------


    //------------edit by suranga-------------------------------
    public static string Select_PettyCash_Accounts = "Select_PettyCash_Accounts";
    public static string insert_PettyCash_Accounts_list = "Insert_PettyCash_Accounts_list";
    public static string Select_PettyCash_Accounts_List = "Select_PettyCash_Accounts_List";
    public static string Select_Petty_Payment_Voucher = "Select_Petty_Payment_Voucher";

    public static string Select_petty_pending_Adv_chit = "Select_petty_pending_Adv_chit";
    public static string Select_Petty_Pending_Add_chits = "Select_Petty_Pending_Add_chits";
    public static string Insert_PettyCash_Advance_Chit = "Insert_PettyCash_Advance_Chit";
    public static string Select_petty_Advance_chit = "Select_petty_Advance_chit";
    public static string Select_Employee = "Select_Employee";
    public static string Select_emp_petty_pending_Adv = "Select_emp_petty_pending_Adv";
    public static string Select_Petty_Acc_available = "Select_Petty_Acc_available";
    public static string Select_Branches = "Petty_Select_Branches";
    public static string Insert_PettyCashBook = "Insert_PettyCashBook";
    public static string Select_petty_cashbook = "Select_petty_cashbook";
    public static string Select_petty_cashbook_by_id = "Select_petty_cashbook_by_id";
    public static string Select_petty_Reimbursement = "Select_petty_Reimbursement";
    public static string Select_petty_Reimbursement1 = "Select_petty_Reimbursement1";
    public static string Select_petty_reimbursement_by_id = "Select_petty_re_by_id";
    public static string Select_Petty_Float_Balance = "Select_Petty_Float_Balance";
    public static string Insert_PettyCashAccounts = "Insert_PettyCashAccounts";
    public static string Select_PettyCash_Acc_by_id = "Select_PettyCash_Acc_by_id";
    public static string Select_Petty_Pending_PV = "Select_Petty_Pending_PV";
    public static string Select_Petty_Pending_PV_by_id = "Select_Petty_Pending_PV_by_id";
    public static string Select_Reimbursement_List = "Select_Reimbursement_List";
    public static string Select_Petty_Reimbursement = "Select_Petty_Reimbursement";
    public static string Select_reimbursement_Pending = "Select_reimbursement_Pending";
    public static string Select_Petty_Pending_Reim = "Select_Petty_Pending_Reim";
    public static string Select_Petty_Approved_PV = "Select_Petty_Approved_PV";
    public static string Select_Petty_Approved_PV_by_id = "Select_Petty_Approved_PV_by_id";
    public static string Select_petty_Adv_chit_by_id = "Select_petty_Adv_chit_by_id";
    public static string Select_petty_approved_Adv_chit = "Select_petty_approved_Adv_chit";
    public static string Select_petty_app_privilages = "Select_petty_app_privilages";
    public static string Select_reimbursement_Approved = "Select_reimbursement_Approved";
    public static string Select_Petty_Pen_Add_chits_Amt = "Select_Petty_Pen_Add_chits_Amt";
    public static string Select_Petty_Pen_PV_Amt = "Select_Petty_Pen_PV_Amt";
    public static string Insert_Denomination = "Insert_Denomination";
    public static string Select_Petty_Denomination = "Select_Petty_Denomination";
    public static string Select_Petty_Unpaid_V_Amt = "Select_Petty_Unpaid_V_Amt";
    public static string Select_Petty_Unpaid_Voucher = "Select_Petty_Unpaid_Voucher";
    public static string Select_Petty_Accounts_by_ID = "Select_Petty_Accounts_by_ID";
    public static string Select_petty_Accounts_by_id = "Select_petty_Accounts_by_id";
    public static string Select_Reimbursement_Status = "Select_Reimbursement_Status1";
    public static string Select_petty_authentication = "Select_petty_authentications";
    public static string Select_petty_mail_auth = "Select_petty_mail_auth";
    public static string Select_Petty_Auth_Details = "Select_Petty_Auth_Details";
    public static string Select_PettyCash_Letter = "Select_PettyCash_Letter";
    public static string Select_PettyCash_Used_Acnt = "Select_PettyCash_Used_Acnt";
    public static string Update_Petty_Account_Change = "Update_Petty_Account_Change";
    public static string Select_Petty_Reim_Voucher = "Select_Petty_Reim_Voucher";
    public static string Remove_Petty_Account_Change = "Remove_Petty_Account_Change";
    public static string Select_Petty_Branch_IDs = "Select_Petty_Branch_IDs";
    public static string select_Petty_Account_Amount = "select_Petty_Account_Amount";
    public static string petty_add_new_reim_account = "petty_add_new_reim_account";
    public static string Select_petty_branch_mgr = "Select_petty_branch_mgr";
    public static string Select_emp_payment_voucher = "Select_emp_payment_voucher";
    public static string Select_petty_account_temp = "Select_petty_account_temp";
    public static string Select_Increasing_Float = "Select_Increasing_Float";
    public static string Insert_Increasing_Float = "Insert_Increasing_Float";
    public static string Select_Petty_HDO_Branches = "Select_Petty_HDO_Branches";
    public static string Select_petty_Mgr_Auth = "Select_petty_Mgr_Auth";
    public static string Select_Petty_Last_Voucher = "Select_Petty_Last_Voucher";
    public static string Select_PettyCashp_Department = "Select_PettyCashp_Department";
    public static string Select_Petty_Gen_Accounts_List = "Select_Petty_Gen_Accounts_List";
    public static string Select_Petty_Gen_PV = "Select_Petty_Gen_PV";
    public static string Select_petty_gen_account_temp = "Select_petty_gen_account_temp";
    public static string Select_Gen_Petty_Pending_Reim = "Select_Gen_Petty_Pending_Reim";
    public static string Select_gen_Petty_Reimbursement = "Select_gen_Petty_Reimbursement";
    public static string Select_Petty_User_Types = "Select_Petty_User_Types";
    public static string petty_insert_user = "petty_insert_user";
    public static string Select_Petty_Gen_Approved_PV = "Select_Petty_Gen_Approved_PV";
    public static string Select_Petty_Gen_App_PV_by_id = "Select_Petty_Gen_App_PV_by_id";
    public static string Select_Petty_Users = "Select_Petty_Users";
    public static string Select_departments = "Select_departments";
    public static string petty_Adv_chit_cancel = "petty_Adv_chit_cancel";
    public static string petty_payment_voucher_cancel = "petty_payment_voucher_cancel";
    public static string Select_Employee_Life = "Select_Employee_Life1";
    public static string Select_Employee_Gen = "Select_Employee_Gen";
    public static string petty_Select_Reimbursements = "petty_Select_Reimbursements";
    public static string petty_Select_Reimbursements1 = "petty_Select_Reimbursements1";
    public static string Select_Petty_Zon_Accounts_List = "Select_Petty_Zon_Accounts_List";
    public static string Select_petty_zon_account_temp = "Select_petty_zon_account_temp";
    public static string Select_Petty_Zon_PV = "Select_Petty_Zon_PV";
    public static string Select_Zon_Petty_Pending_Reim = "Select_Zon_Petty_Pending_Reim";
    public static string Select_Petty_Zon_App_PV_by_id = "Select_Petty_Zon_App_PV_by_id";
    public static string Select_zon_Petty_Reimbursement = "Select_zon_Petty_Reimbursement";
    public static string Select_Zonal_Petty_Accounts = "Select_Zonal_Petty_Accounts";
    public static string Select_Zonal_Petty_Branches = "Select_Zonal_Petty_Branches";
    public static string Select_PettyCash_Zon_Letter = "Select_PettyCash_Zon_Letter";
    public static string Select_petty_final_mail = "Select_petty_final_mail";
    public static string Select_Petty_Manager = "Select_Petty_Manager";

    //--------------------petty cash-Return update-------------------------------------
    public static string Select_Return_Reimbursement = "Select_Return_Reimbursement";
    public static string Select_zon_Petty_Reimburs1 = "Select_zon_Petty_Reimburs1";
    public static string Select_Petty_Rtn_Reimbursement = "Select_Petty_Rtn_Reimbursement";
    public static string Select_Zon_Petty_Pending_Reim1 = "Select_Zon_Petty_Pending_Reim1";
    public static string Select_Petty_Pending_Reim1 = "Select_Petty_Pending_Reim1";
    public static string Select_PettyCash_Letter1 = "Select_PettyCash_Letter1";
    public static string Select_Petty_Return_Reim = "Select_Petty_Return_Reim";
    public static string Select_PettyCash_Accounts1 = "Select_PettyCash_Accounts1";
    public static string Select_PettyCash_Accounts_Lst1 = "Select_PettyCash_Accounts_Lst1";
    public static string Select_PettyCash_Acc_by_id1 = "Select_PettyCash_Acc_by_id1";
    public static string Select_Petty_Cancel_Reim = "Select_Petty_Cancel_Reim";
    public static string Select_Gen_Petty_Cancel_Reim = "Select_Gen_Petty_Cancel_Reim";
    public static string Select_PettyCash_Used_Acnt1 = "Select_PettyCash_Used_Acnt1";
    public static string Select_Petty_Accounts_by_ID1 = "Select_Petty_Accounts_by_ID1";
    public static string Select_Petty_Acc_Category = "Select_Petty_Acc_Category";
    public static string Select_PettyCash_Acc_Cat = "Select_PettyCash_Acc_Cat";
    public static string Select_petty_Accounts_by_id1 = "Select_petty_Accounts_by_id1";
    public static string Select_Zonal_Petty_Branches1 = "Select_Zonal_Petty_Branches1";
    public static string Select_Petty_Zon_Accounts_Lst1 = "Select_Petty_Zon_Accounts_Lst1";
    public static string Select_Zonal_Petty_Accounts1 = "Select_Zonal_Petty_Accounts1";
    public static string petty_Select_Reconciliation = "petty_Select_Reconciliation";
    public static string Select_petty_branch_mgr_l = "Select_petty_branch_mgr_l";
    public static string Select_PettyCash_Zon_Letter1 = "Select_PettyCash_Zon_Letter1";
    public static string Select_Petty_Zon_App_PV_by_id1 = "Select_Petty_Zon_App_PV_by_id1";
    public static string Select_Petty_Approved_PV_by_id1 = "Select_Petty_Approve_PV_by_id1";
    public static string Select_Petty_Returned_PV_by_id = "Select_Petty_Returned_PV_by_id";
    public static string Select_Petty_Gen_Rtn_PV_by_id = "Select_Petty_Gen_Rtn_PV_by_id";
    public static string Select_petty_branch_cashier = "Select_petty_branch_cashier";

    public static string Select_Petty_Apr_PV_Amt1 = "Select_Petty_Apr_PV_Amt1";
    public static string Select_Petty_Pen_PV_Amt1 = "Select_Petty_Pen_PV_Amt1";
    public static string Insert_Denomination1 = "Insert_Denomination1";
    public static string Select_Petty_HDO_Branches1 = "Select_Petty_HDO_Branches1";
    public static string Select_zon_Petty_Reimburse1 = "Select_zon_Petty_Reimburse1";
    public static string Select_Petty_Reimbursement1 = "Select_Petty_Reimbursement1";
    public static string Select_PettyCash_Zon_Letter2 = "Select_PettyCash_Zon_Letter2";
    public static string Select_PettyCash_Letter2 = "Select_PettyCash_Letter2";
    public static string Select_petty_Final_App_Auth = "Select_petty_Final_App_Auth";


    //------------------credit note system---------------------------------------------
    public static string ms_select_cn_resons = "ms_select_cn_resons";
    public static string ms_select_policy_details = "ms_select_policy_details";
    public static string ms_select_relevent_document = "ms_select_relevent_document";
    public static string ms_select_utl_policy_details = "ms_select_utl_policy_details";
    public static string ms_select_utl_debit_details = "ms_select_utl_debit_details";
    public static string ms_select_receipt_amount = "ms_select_receipt_amount";
    public static string MS_Insert_Policy_Utl_Dtl = "MS_Insert_Policy_Utl_Dtl";
    public static string ms_select_pending_policy = "ms_select_pending_policy";
    public static string MS_Update_Utl_Policy_Status = "MS_Update_Utl_Policy_Status";
    public static string ms_select_policy_to_rec = "ms_select_policy_to_rec";
    public static string ms_select_debit_to_rec = "ms_select_debit_to_rec";
    public static string ms_select_doc_to_rec = "ms_select_doc_to_rec";
    public static string ms_select_uderwriters = "ms_select_uderwriters";
    public static string ms_select_approved_policy = "ms_select_approved_policy";

    public static string MS_Insert_Ass_Underwriter = "MS_Insert_Ass_Underwriter";
    public static string ms_select_utl_credit_details = "ms_select_utl_credit_details";
    public static string ms_select_assigned_policy = "ms_select_assigned_policy";
    public static string ms_select_cn_details = "ms_select_cn_details";
    public static string ms_select_user_auth = "ms_select_user_auth";
    public static string ms_select_pending_doc = "ms_select_pending_doc";
    public static string MS_Insert_Credit_Note_Values = "MS_Insert_Credit_Note_Values";
    public static string MS_Updated_Policy_Utl_Dtl = "MS_Updated_Policy_Utl_Dtl";
    public static string ms_select_policy_det_report = "ms_select_policy_det_report";
    public static string ms_complete_credit_note = "ms_complete_credit_note";
    public static string MS_select_Credit_Note_Values = "MS_select_Credit_Note_Values";
    public static string MS_Updated_Pol_Utl_start = "MS_Updated_Pol_Utl_start";
    public static string MS_Update_Pol_Utl_Status = "MS_Update_Pol_Utl_Status";
    public static string ms_select_utl_job_by_id = "ms_select_utl_job_by_id";
    public static string ms_return_policy_dtl = "ms_return_policy_dtl";
    public static string ms_select_short_term_ratio = "ms_select_short_term_ratio";
    public static string ms_select_auth_level = "ms_select_auth_level";
    public static string ms_select_zone_codes = "ms_select_zone_codes";
    public static string ms_select_pending_docs = "ms_select_pending_docs";
    public static string ms_select_pending_doc_pol = "ms_select_pending_doc_pol";
    public static string ms_select_pending_policies = "ms_select_pending_policies";
    public static string ms_select_bench_mark = "ms_select_bench_mark";
    public static string ms_select_usr_bench_mark = "ms_select_usr_bench_mark";
    public static string ms_select_authentications = "ms_select_authentications";
    public static string ms_user_authentications = "ms_user_authentications";
    public static string ms_select_assign_UW = "ms_select_assign_UW";
    public static string MS_Update_Policy_Dbt_Nos = "MS_Update_Policy_Dbt_Nos";
    //--------------------------------------------------------------------------------

    //------------------------------MotorNew Business--------------------------------

    public static string MNB_Insert_Motor_Cover_Details = "Insert_Motor_Coover_Details";
    public static string Select_Revision_ID = "Select_Revision_ID";
    public static string MNB_Insert_Motor_Quot_Details = "MNB_Insert_Motor_Quot_Details";
    public static string Select_quotation_docs = "Select_quotation_docs";
    public static string Select_policy_cover_details = "Select_policy_cover_details";
    public static string Select_quotation_details = "Select_quotation_details";
    public static string Select_quotation_search = "Select_quotation_search";
    public static string MNB_select_quoatation_details = "MNB_select_quoatation_details";
    public static string MNB_select_job_details = "MNB_select_job_details";
    public static string MNB_select_job_cover_details = "MNB_select_job_cover_details";
    public static string MNB_select_quoat_req_details = "MNB_select_quoat_req_details";
    public static string MNB_Insert_Motor_CoverNote_Det = "MNB_Insert_Motor_CoverNote_Det";
    public static string MNB_select_cn_details = "MNB_select_cn_details";
    public static string MNB_Update_Motor_CoverNote_Det = "MNB_Update_Motor_CoverNote_Det";
    public static string mnb_Select_quotation_docs = "mnb_Select_quotation_docs";
    public static string MNB_select_cover_note_details = "MNB_select_cover_note_details";
    public static string MNB_select_policy_details = "MNB_select_policy_details";
    public static string mnb_select_risk_types = "mnb_select_risk_types";
    public static string mnb_select_vehicle_types = "mnb_select_vehicle_types";
    public static string mnb_select_ncb_rates = "mnb_select_ncb_rates";


    //------------------------------End MotorNew Business--------------------------------

}
