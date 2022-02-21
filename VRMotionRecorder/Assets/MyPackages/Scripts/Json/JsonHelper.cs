using FileIOUtility;
using System;
using System.Collections;
using System.Threading.Tasks;
using UnityEngine;
using StrOpe = StringOperationUtil.OptimizedFastStringOperation;

public static class JsonHelper<T>
{
	public static T Read( string file_name )
	{
		string text = FileIOHelper.ReadFile(StrOpe.i + Application.streamingAssetsPath + "/" + file_name );
		return JsonUtility.FromJson<T>( text );
	}

	public static async Task<T> ReadAsync( string file_name )
	{
		string text = await FileIOHelper.ReadFileAsync(StrOpe.i + Application.streamingAssetsPath + "/" + file_name );
		return JsonUtility.FromJson<T>( text );
	}

	public static T ReadWithDecryption( string file_name, string password )
	{
		string text = FileIOHelper.ReadFile(StrOpe.i + Application.streamingAssetsPath + "/" + file_name );
		string decryption_text = "{}";
		try
		{
			decryption_text = EncryptionUtil.DecryptString( text, password );
		}
		catch( Exception e )
		{
			Debug.LogError( e );
			return JsonUtility.FromJson<T>( decryption_text );
		}

		return JsonUtility.FromJson<T>( decryption_text );
	}

	public static async Task<T> ReadWithDecryptionAsync( string file_name, string password )
	{
		string text = await FileIOHelper.ReadFileAsync(StrOpe.i + Application.streamingAssetsPath + "/" + file_name );
		string decryption_text = "{}";
		try
		{
			decryption_text = EncryptionUtil.DecryptString( text, password );
		}
		catch( Exception e )
		{
			Debug.LogError( e );
			return JsonUtility.FromJson<T>( decryption_text );
		}

		return JsonUtility.FromJson<T>( decryption_text );
	}

	public static void Write( string file_name, T data )
	{
		string file_path = StrOpe.i + Application.streamingAssetsPath + "/" + file_name;
		string json = JsonUtility.ToJson( data, true );
		FileIOHelper.WriteFile( file_path, json );
	}

	public static void WriteWithEncryption( string file_name, T data, string password )
	{
		string file_path = StrOpe.i + Application.streamingAssetsPath + "/" + file_name;
		string clear_text		= JsonUtility.ToJson( data, true );
		string encryption_text  = "";
		try
		{
			encryption_text = EncryptionUtil.EncryptString( clear_text, password );
		}
		catch( Exception e )
		{
			Debug.LogError( e );
			return;
		}

		FileIOHelper.WriteFile( file_path, encryption_text );
	}

	public static T[] FromJson(string json)
    {
        Wrapper wrapper = UnityEngine.JsonUtility.FromJson<Wrapper>(json);
        return wrapper.forecasts;
    }

    [Serializable]
    private class Wrapper
    {
        public T[] forecasts;
    }
}
