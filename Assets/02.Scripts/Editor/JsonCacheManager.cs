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
        // 캐시에 존재하면 바로 반환
        if (cache.ContainsKey(filename))
        {
            return (T)cache[filename];
        }

        // 캐시에 없으면 파일을 찾음
        var cachedData = cacheList.Find(c => c.fileName == filename);
        if (cachedData == null || cachedData.jsonFile == null)
        {
            Debug.LogError($"File {filename} not found in cache manager.");
            return default;
        }

        // 파일을 캐싱하고 반환
        T data = JsonUtility.FromJson<T>(cachedData.jsonFile.text);
        cache[filename] = data;
        cachedData.isCached = true;  // Inspector에서 확인 가능하도록 설정
        return data;
    }

    public void ClearCache(string filename)
    {
        if (cache.ContainsKey(filename))
        {
            cache.Remove(filename);
            var cachedData = cacheList.Find(c => c.fileName == filename);
            if (cachedData != null)
                cachedData.isCached = false;  // 캐시 상태를 갱신
        }
    }

    public void ClearAllCache()
    {
        cache.Clear();
        foreach (var cachedData in cacheList)
        {
            cachedData.isCached = false;  // Inspector에서 상태 갱신
        }
    }
}