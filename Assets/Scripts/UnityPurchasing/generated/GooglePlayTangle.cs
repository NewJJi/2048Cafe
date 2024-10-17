// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("R5K9sETHIZGW+TTdQMl9bbWsMv31yaL89cievDpxsLCjbDhfgSN6d32Gkz5M5M9D8E906z4xGAyrRC6/dRl5E4M0+tmUuxNIO9fT4xADhHFCeDqSc41Eyuy/5ImmLN4+PCFUeGk34FIZvZXqIiZeV3yfupK+EL+RJLjzd53MlYP7DGps9RSyakpJVardGB5he5v9AKqeltbklAHlEUlmXlXHiaMv+EVhA1GTajo/9NuS/vZyXYorpnBL9mvrK7YiD/vlU+XmnCOhIiwjE6EiKSGhIiIjr+bmYNyMJLtBvKIk6n8qDqd3Rp19BsXB0BXJE6EiARMuJSoJpWul1C4iIiImIyDSKvaSHq3mA2cAwQJdRZ6vDcsYIWMLPYJctU4lWCEgIiMi");
        private static int[] order = new int[] { 4,2,5,13,8,12,11,13,13,9,12,11,13,13,14 };
        private static int key = 35;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
