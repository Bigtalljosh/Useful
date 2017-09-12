using System.Runtime.CompilerServices;

/// <summary>
/// Gets a string representation of the calling method name
/// </summary>
/// <param name="caller"></param>
/// <returns></returns>
public static string GetCaller([CallerMemberName] string caller = null) => caller;