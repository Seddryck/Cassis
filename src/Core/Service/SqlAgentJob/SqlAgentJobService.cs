#if ! SqlServer2008R2
using Cassis.Core;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace Cassis.Core.Service.SqlAgentJob
{
    class SqlAgentService : AbstractPackageService
    {
        public new ISqlAgentJobPackage PackageInfo
        {
            get { return base.PackageInfo as ISqlAgentJobPackage; }
        }

        public SqlAgentService(ISqlAgentJobPackage packageInfo)
            : base(packageInfo)
        { }

        public override PackageResponse Run()
        {
            return Run(PackageInfo);
        }

        protected PackageResponse Run(ISqlAgentJobPackage etl)
        {
            var connection = new SqlConnection(string.Format(@"Data Source={0};Initial Catalog=msdb;Integrated Security=SSPI;", etl.Server));
            var command = new SqlCommand("sp_start_job", connection);
            command.CommandType = CommandType.StoredProcedure;

            var returnValue = new SqlParameter("@RETURN_VALUE", SqlDbType.Int);
            returnValue.Direction = ParameterDirection.ReturnValue;
            command.Parameters.Add(returnValue);

            var parameter = new SqlParameter("@job_name", SqlDbType.VarChar);
            parameter.Direction = ParameterDirection.Input;
            command.Parameters.Add(parameter);
            parameter.Value = etl.JobName;

            try
            {
                connection.Open();
                command.ExecuteNonQuery();
                var result = (Int32)command.Parameters["@RETURN_VALUE"].Value;
                return new PackageResponse(result == 0);
            }
            catch (Exception ex)
            {
                var packageEvent = new PackageEvents();
                packageEvent.Errors.Add(ex.Message);
                if (ex.InnerException!=null)
                    packageEvent.Errors.Add(ex.InnerException.Message);
                return new PackageResponse(false, packageEvent);
            }
            finally
            {
                connection.Close();
            }   
        }
    }
}
#endif