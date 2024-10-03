using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "JsonCacheManager", menuName = "Cache/JsonCacheManager")]
public class JsonCacheManager : ScriptableObject
{
    [System.Serializable]
    public class CachedData
    {
        public string fileName;
        public TextAsset jsonFile;
        public bool isCached;
    }

    public List<CachedData> cacheList = new List<CachedData>();

    private Dictionary<string, object> cache = new Dictionary<string, object>();

    public T GetJsonData<T>(string filename)
    {
        // ĳ�ÿ� �����ϸ� �ٷ� ��ȯ
        if (cache.ContainsKey(filename))
        {
            return (T)cache[filename];
        }

        // ĳ�ÿ� ������ ������ ã��
        var cachedData = cacheList.Find(c => c.fileName == filename);
        if (cachedData == null || cachedData.jsonFile == null)
        {
            Debug.LogError($"File {filename} not found in cache manager.");
            return default;
        }

        // ������ ĳ���ϰ� ��ȯ
        T data = JsonUtility.FromJson<T>(cachedData.jsonFile.text);
        cache[filename] = data;
        cachedData.isCached = true;  // Inspector���� Ȯ�� �����ϵ��� ����
        return data;
    }

    public void ClearCache(string filename)
    {
        if (cache.ContainsKey(filename))
        {
            cache.Remove(filename);
            var cachedData = cacheList.Find(c => c.fileName == filename);
            if (cachedData != null)
                cachedData.isCached = false;  // ĳ�� ���¸� ����
        }
    }

    public void ClearAllCache()
    {
        cache.Clear();
        foreach (var cachedData in cacheList)
        {
            cachedData.isCached = false;  // Inspector���� ���� ����
        }
    }
}