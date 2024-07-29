using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;
using OutOfOffice.Components.Backend;

namespace OutOfOffice.Components.Common.Validators
{
    public class DateGreaterThanAttribute : ValidationAttribute
    {
        public string otherPropertyName;
        public DateGreaterThanAttribute() { }
        public DateGreaterThanAttribute(string otherPropertyName, string errorMessage)
            : base(errorMessage)
        {
            this.otherPropertyName = otherPropertyName;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ValidationResult validationResult = ValidationResult.Success;
            try
            {
                var containerType = validationContext.ObjectInstance.GetType();
                var field = containerType.GetProperty(otherPropertyName);
                var extensionValue = field.GetValue(validationContext.ObjectInstance, null);
                if (extensionValue == null)
                {
                    return validationResult;
                }
                var datatype = extensionValue.GetType();

                if (field == null)
                    return new ValidationResult(string.Format("Unknown property: {0}.", otherPropertyName));

                if (field.PropertyType == typeof(DateTime) || field.PropertyType.IsGenericType && field.PropertyType == typeof(DateTime?))
                {
                    DateTime toValidate = (DateTime)value;
                    DateTime referenceProperty = (DateTime)field.GetValue(validationContext.ObjectInstance, null);

                    if (toValidate.CompareTo(referenceProperty) < 1)
                    {
                        validationResult = new ValidationResult(ErrorMessageString);
                    }
                }
                else
                {
                    validationResult = new ValidationResult("An error occurred while validating the property. OtherProperty is not of type DateTime");
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return validationResult;
        }
    }
}
