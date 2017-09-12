
/// <summary>
/// Validates a VAT code
/// </summary>
public class ValidVAT : ValidationAttribute
{
protected override ValidationResult IsValid(object value, ValidationContext validationContext)
{
    // Create a list of valid VAT codes
    IEnumerable<string> VATCodes = new[] { "BE", "DE", "DK", "ES", "FI", "FR", "GR", "IE", "IT", "LU", "NL", "PT", "SE", "CY", "CZ", "EE", "HU", "LV", "LT", "MT", "PL", "SK", "SI" };

    // If our code is in the list return true
    var result = VATCodes.Contains(value);

    // Return validation result
    return result ? ValidationResult.Success : new ValidationResult("Customer is a non-UK EC customer and requires a valid VAT registration code", new[] { validationContext.MemberName });
}
}

/// <summary>
/// Ensures the field is uppercase
/// </summary>
public class UpperCase : ValidationAttribute
{
protected override ValidationResult IsValid(object value, ValidationContext validationContext)
{
    if (value != null)
    {
        validationContext.ObjectType.GetProperty(validationContext.DisplayName)
            .SetValue(validationContext.ObjectInstance, value.ToString().ToUpper(), null);
    }
    return null;
}
}

/// <summary>
/// Ensures the field can be parsed as numeric
/// </summary>
public class NumericOnly : ValidationAttribute
{
protected override ValidationResult IsValid(object value, ValidationContext validationContext)
{
    int res;
    var numeric = int.TryParse(value?.ToString() ?? "", out res);

    return numeric ? ValidationResult.Success : new ValidationResult("Property must be numeric", new[] { validationContext.MemberName });
}
}

/// <summary>
/// Validates a phone number
/// </summary>
public class PhoneNumber : ValidationAttribute
{
protected override ValidationResult IsValid(object value, ValidationContext validationContext)
{
    var regex = new Regex("^[0-9-+()x ]{7,20}$");
    var res = regex.Match(value.ToString()).Success;

    return res ? ValidationResult.Success : new ValidationResult("Phone number is not valid", new[] { validationContext.MemberName });
}
}

/// <summary>
/// Validates an email address
/// </summary>
public class EmailAddress : ValidationAttribute
{
protected override ValidationResult IsValid(object value, ValidationContext validationContext)
{
    var regex = new Regex("[A-Za-z0-9!#$%&'*+-/=?^_`{|}~]+@[A-Za-z0-9-]+(.[A-Za-z0-9-]+)*");
    var res = regex.Match(value.ToString()).Success;

    return res ? ValidationResult.Success : new ValidationResult("Email address is not valid", new[] { validationContext.MemberName });
}
}

/// <summary>
/// Used as our replacement for the ModelState class
/// </summary>
public static class DataAnnotationExtentions
{
/// <summary>
/// Used to validate an object
/// </summary>
/// <typeparam name="T">The type of object to validate</typeparam>
/// <param name="model">The object to validate</param>
/// <param name="required"></param>
public static void Validate<T>(this T model, bool required = true) where T : class
{

    // Validate first
    model.ValidatePrimative(required);

    // model is not null and is required, then validate the properties of the object
    if (model != null && required)
    {

        // Get our object type
        var type = model.GetType();

        // If we are a list, validate each item
        if (type.IsGenericType && typeof(IEnumerable).IsAssignableFrom(type))
            foreach (var item in (IEnumerable)model)
                item.Validate();
        else
        {

            // Using reflect (This will not slow anything down too much, it is used in the normal ModelState, so should work fine here!)
            var properties = type.GetProperties();

            // Loop through the properties
            foreach (var property in properties)
            {

                // Get our type
                var propertyType = property.PropertyType;
                var propertyRequired = property.GetCustomAttributes(typeof(RequiredAttribute), false).Any();
                var value = property.GetValue(model, null);

                // Check if we are required or not
                if (!propertyType.IsPrimitive && propertyType != typeof(decimal) && propertyType != typeof(string) && propertyType != typeof(DateTime))
                    value.Validate(propertyRequired);
                else
                    value.ValidatePrimative(propertyRequired);
            }
        }
    }
}

/// <summary>
/// Used to validate a single object
/// </summary>
/// <typeparam name="T">The type of object to validate</typeparam>
/// <param name="model">The object to validate</param>
/// <param name="required"></param>
private static void ValidatePrimative<T>(this T model, bool required) where T : class
{

    // If the model is required and it is null, throw an error
    if (required) ThrowIf.ArgumentIsNull(() => model);

    // If our model is not null
    if (model != null)
    {

        // Create our variables
        var context = new ValidationContext(model);
        var errorResults = new List<ValidationResult>();

        // Try to validate our model
        if (!Validator.TryValidateObject(model, context, errorResults, true))
        {

            // If we have any results, it means we have some errors
            if (errorResults.Count > 0)
            {

                // Create our string builder
                var sb = new StringBuilder();

                // For each error, stash the error message into our string builder
                foreach (var result in errorResults)
                    sb.Append(result.ErrorMessage);

                // Throw a generic exception and join our errors
                throw new Exception(sb.ToString());
            }
        }
    }
}
