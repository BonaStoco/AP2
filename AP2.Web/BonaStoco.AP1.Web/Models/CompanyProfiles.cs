using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace BonaStoco.AP1.Web.Models
{
    public class CompanyProfiles
    {
        const string COMPANY_PROFILE_FIELD = "companyprofile";
        HttpSessionStateBase context;
        public CompanyProfiles(HttpContextBase context)
        {
            this.context = context.Session;
        }
        public CompanyProfiles(HttpSessionStateBase context)
        {
            this.context = context;
        }
        public int CompanyId { 
            get { 
                return GetCompanyProfile().CompanyId; 
            }
            set
            {
                GetCompanyProfile().CompanyId = value;
            }
        }
        public string CompanyName { 
            get { 
                return GetCompanyProfile().CompanyName; 
            }
            set
            {
                GetCompanyProfile().CompanyName = value;
            }
        }
        public RoleId Role{
            get { return GetCompanyProfile().Role; }
            set { GetCompanyProfile().Role = value; }
        }
        public string RoleName
        {
            get { return GetCompanyProfile().RoleName; }
            set { GetCompanyProfile().RoleName = value; }
        }
        public string HomePage
        {
            get { return GetCompanyProfile().HomePage; }
            set { GetCompanyProfile().HomePage = value; }
        }
        public string CompanyReserved
        {
            get { return GetCompanyProfile().CompanyReserved; }
            set { GetCompanyProfile().CompanyReserved = value; }
        }

        //public int Alamat { get; set; }
        //public string Npwp { get; set; }
        //public string Nppkp { get; set; }
        //public int LocationId { get; set; }
        //public int PortId { get; set; }
        //public DateTime TanggalBergabung { get; set; }
        //public string Tarif { get; set; }

        private CompanyProfileModel GetCompanyProfile()
        {
            if (context[COMPANY_PROFILE_FIELD] == null)
                context[COMPANY_PROFILE_FIELD] = new CompanyProfileModel();
            return (CompanyProfileModel)context[COMPANY_PROFILE_FIELD];
        }
    }

    public class CompanyProfileModel
    {
        public int CompanyId { get; set; }
        public string CompanyName { get; set; }
        public int CategoryId{get;set;}
        public RoleId Role{get;set;}
        public string RoleName { get; set; }
        public string HomePage { get; set; }
        public string CompanyReserved { get; set; }
        
        //public string Alamat { get; set; }
        //public string Npwp { get; set; }
        //public string Nppkp { get; set; }
        //public int LocationId { get; set; }
        //public int PortId { get; set; }
        //public DateTime TanggalBergabung { get; set; }
        //public string Tarif { get; set; }
    }
}