namespace API.DataAccess.DTOs.Attribute
{
    using API.DataAccess.DTOs.Authentication;
    using System;
    using System.ComponentModel.DataAnnotations;

    public class ValidPharmacyTimesAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var pharmacy = (RegisterPharmacyDTO)validationContext.ObjectInstance;

            if (pharmacy.IsOpenAllTime)
            {
                if (pharmacy.OpenTime.HasValue || pharmacy.CloseTime.HasValue)
                {
                    return new ValidationResult("OpenTime and CloseTime must be null when the pharmacy is open all the time.");
                }
            }
            else
            {
                if (!pharmacy.OpenTime.HasValue || !pharmacy.CloseTime.HasValue)
                {
                    return new ValidationResult("OpenTime and CloseTime are required when the pharmacy is not open all the time.");
                }
                if (pharmacy.CloseTime <= pharmacy.OpenTime)
                {
                    return new ValidationResult("CloseTime must be greater than OpenTime.");
                }
            }

            return ValidationResult.Success;
        }
    }

}
