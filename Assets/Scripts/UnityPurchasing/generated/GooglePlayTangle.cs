// WARNING: Do not modify! Generated file.

namespace UnityEngine.Purchasing.Security {
    public class GooglePlayTangle
    {
        private static byte[] data = System.Convert.FromBase64String("wjp7WzOWidSFcPcqIJRcPrv5NwQSohm0nH9LW/fQCy3Ej+uAnB3iKhnEAVfLzb2X8u6j5hWyPua1ACRYH5ySna0fnJefH5ycnQb51yQoeEsE02OYnOEv4BeumMr1Qszj+ER+GK0fnL+tkJuUtxvVG2qQnJycmJ2e/e9Q+BR7akQHiaYXeLx3fjycWeYWMkWequ/JpE+RyuLUkXvj2wlvCFgnROBRsnPI8PJwd6DdTuTGZLKhSJMX7L9GmDseDTJygOrTn0ZVgAZnXswa2ERlQwN7E+kNyKi8DzbxsNNqpx019l3GS4MM1JTyvMLCo1ylDNsTjpJ0ZkygJS8GkfrsxVJsQP7n4BMWkTv5uavmdglCknbhqGYQ4DTt11N3NK1CVJ+enJ2c");
        private static int[] order = new int[] { 5,8,13,8,7,5,8,9,11,12,13,12,12,13,14 };
        private static int key = 157;

        public static readonly bool IsPopulated = true;

        public static byte[] Data() {
        	if (IsPopulated == false)
        		return null;
            return Obfuscator.DeObfuscate(data, order, key);
        }
    }
}
