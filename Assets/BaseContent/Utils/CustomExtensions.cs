using UnityEngine;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System;

public static class CustomExtensions {

	public static T GetRandom<T>(this List<T> objects) {
    if (objects == null || objects.Count == 0) {
      return default(T);
    }

    int indexToReturn = UnityEngine.Random.Range(0, objects.Count);
    return objects[indexToReturn];

  }

  public static T GetRandom<T>(this T[] objects) {
    if (objects == null || objects.Length == 0) {
      return default(T);
    }
    int indexToReturn = UnityEngine.Random.Range(0, objects.Length);
    return objects[indexToReturn];
  }

  public static T TryGetValue<T>(this Dictionary<string,T> dict, string key) {
    if (dict == null || dict.Count == 0 || !dict.ContainsKey(key)) {
      return default(T);
    }
    return dict[key];
  }

  public static T TryGetValue<U,T>(this Dictionary<U, T> dict, U key) {
    if (dict == null || dict.Count == 0 || !dict.ContainsKey(key)) {
      return default(T);
    }

    return dict[key];
  }

	/// <summary>
	/// <para>Looks up for component in all related components</para>
	/// <para>First, looks for current object, then in all its children</para>
	/// <para>Finally, in all its parents</para>
	/// </summary>
	/// <returns>First occurrence for given object Type</returns>
	/// <param name="obj">Object reference</param>
	/// <typeparam name="T">The 1st type parameter.</typeparam>
	public static T LookUpForComponent<T>(this GameObject obj) where T: Component {
		T toReturn = obj.GetComponent(typeof(T)) as T; // first, look up in current hierarchy
		if (toReturn != null) { return toReturn; }
		toReturn = obj.GetComponentsInChildren(typeof(T)) as T;
		if (toReturn != null) { return toReturn; }
		toReturn = obj.GetComponentInParent(typeof(T)) as T;
		return toReturn;
	}

  public static void DestroyChildren(this UnityEngine.Transform parent)
  {
    while (parent.transform.childCount > 0)
    {
      Transform t = parent.GetChild(0);
      t.parent = null;	// reparent, which will change child count immediately.
			if (Application.isPlaying) { UnityEngine.Object.Destroy(t.gameObject); }
			else { UnityEngine.Object.DestroyImmediate(t.gameObject); }
    }
  }

  #region Enum extensions

  //checks if the value contains the provided type
  public static bool Has<T>(this Enum type, T value)
  {
    try
    {
      return (((int)(object)type & (int)(object)value) == (int)(object)value);
    }
    catch
    {
      return false;
    }
  }

  //checks if the value is only the provided type
  public static bool Is<T>(this Enum type, T value)
  {
    try
    {
      return (int)(object)type == (int)(object)value;
    }
    catch
    {
      return false;
    }
  }

  //appends a value
  public static T Add<T>(this Enum type, T value)
  {
    try
    {
      return (T)(object)(((int)(object)type | (int)(object)value));
    }
    catch (Exception ex)
    {
      throw new ArgumentException(
          string.Format(
              "Could not append value from enumerated type '{0}'.",
              typeof(T).Name
              ), ex);
    }
  }

  //completely removes the value
  public static T Remove<T>(this Enum type, T value)
  {
    try
    {
      return (T)(object)(((int)(object)type & ~(int)(object)value));
    }
    catch (Exception ex)
    {
      throw new ArgumentException(
          string.Format(
              "Could not remove value from enumerated type '{0}'.",
              typeof(T).Name
              ), ex);
    }
  }

  /// <summary>
  /// Sets flag state on enum.
  /// Please note that enums are value types so you need to handle the RETURNED value from this method.
  /// Example: myEnumVariable = myEnumVariable.SetFlag(CustomEnumType.Value1, true);
  /// </summary>
  public static T SetFlag<T>(this Enum type, T enumFlag, bool value)
  {
    return value ? type.Add(enumFlag) : type.Remove(enumFlag);
  }

  /// <summary>
  /// parses a string value into a enum value
  /// </summary>
  /// <typeparam name="T"></typeparam>
  /// <param name="val"></param>
  /// <returns></returns>
  public static T ToEnum<T>(this string val)
  {
    return (T)System.Enum.Parse(typeof(T), val);
  }

  /// <summary>
  /// Convert provided enum type to list of values.
  /// This is convenient when you need to iterate enum values.
  /// </summary>
  public static List<T> ToList<T>()
  {
    if (!typeof(T).IsEnum)
      throw new ArgumentException();
    var values = Enum.GetNames(typeof(T));
    return values.Select(value => value.ToEnum<T>()).ToList();
  }

  /// <summary>
  /// Present the enum values as a comma separated string.
  /// </summary>
  public static string GetValues<T>()
  {
    if (!typeof(T).IsEnum)
      throw new ArgumentException();
    var values = Enum.GetNames(typeof(T));
    return string.Join(", ", values);
  }

  #endregion end enum extensions

}
