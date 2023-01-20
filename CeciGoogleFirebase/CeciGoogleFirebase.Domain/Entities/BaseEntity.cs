using System;
using TimeZoneConverter;

namespace CeciGoogleFirebase.Domain.Entities
{
    public abstract class BaseEntity
    {
        public BaseEntity()
        {
            Active = true;
            RegistrationDate = TimeZoneInfo.ConvertTime(DateTime.Now, TZConvert.GetTimeZoneInfo("E. South America Standard Time"));
        }

        public virtual int Id { get; set; }

        public virtual bool Active { get; set; }

        public virtual DateTime RegistrationDate { get; set; }
    }
}
