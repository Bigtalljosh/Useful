/// <summary>
/// Used to join a URL with a query string
/// </summary>
/// <param name="url"></param>
/// <param name="parameters"></param>
/// <returns></returns>
public static string JoinQueryToUrlString(string url, object parameters)
{

    // Get our parameters as a list of strings
    var pairs = parameters.GetType().GetProperties().Select(m => HttpUtility.UrlEncode(m.Name) + "=" + HttpUtility.UrlEncode(m.GetValue(parameters, null).ToString()));

    // Return our link
    return url + '?' + string.Join("&", pairs);
}