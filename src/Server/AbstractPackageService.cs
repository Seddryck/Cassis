using Cassis.Contract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cassis.Service
{
    abstract class AbstractPackageService : IPackageService
    {
        private readonly IPackageInfo packageInfo;

        protected IPackageInfo PackageInfo
        {
            get { return packageInfo; }
        }

        public IEnumerable<PackageParameter> Parameters { get; set; }

        public AbstractPackageService(IPackageInfo packageInfo)
        {
            this.packageInfo = packageInfo;
        }

        public abstract PackageResponse Run();

        protected virtual object DefineValue(string value, TypeCode typeCode)
        {
            switch (typeCode)
            {
                case TypeCode.Boolean:
                    return Boolean.Parse(value);
                case TypeCode.Byte:
                    return Byte.Parse(value);
                case TypeCode.Char:
                    return Char.Parse(value);
                case TypeCode.DBNull:
                    return null;
                case TypeCode.DateTime:
                    return DateTime.Parse(value);
                case TypeCode.Decimal:
                    return Decimal.Parse(value);
                case TypeCode.Double:
                    return Double.Parse(value);
                case TypeCode.Empty:
                    return string.Empty;
                case TypeCode.Int16:
                    return Int16.Parse(value);
                case TypeCode.Int32:
                    return Int32.Parse(value);
                case TypeCode.Int64:
                    return Int64.Parse(value);
                case TypeCode.Object:
                    return value;
                case TypeCode.SByte:
                    return SByte.Parse(value);
                case TypeCode.Single:
                    return Single.Parse(value);
                case TypeCode.String:
                    return value;
                case TypeCode.UInt16:
                    return UInt16.Parse(value);
                case TypeCode.UInt32:
                    return UInt32.Parse(value);
                case TypeCode.UInt64:
                    return UInt64.Parse(value);
            }
            return value;
        }
    }
}
