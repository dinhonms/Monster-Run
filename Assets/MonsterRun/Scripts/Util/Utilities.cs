using UnityEngine;

namespace Util
{
    public static class Utilities
    {
        private static int minCharac = 4;
        private static int maxCharac = 10;

        public static Color GenerateRandomColor()
        {
            var color = new Color(
                Random.Range(0f, 1f),
                Random.Range(0f, 1f),
                Random.Range(0f, 1f)
            );

            return color;
        }

        public static string GenerateRandomName()
        {
            return GenerateRandomName(Random.Range(minCharac, maxCharac));
        }

        private static string GenerateRandomName(int length)
        {
            const string allowedCharacters = "abcdefghijklmnopqrstuvwxyz";
            System.Text.StringBuilder nameBuilder = new System.Text.StringBuilder();

            for (int i = 0; i < length; i++)
            {
                char randomChar = allowedCharacters[Random.Range(0, allowedCharacters.Length)];
                nameBuilder.Append(randomChar);
            }

            // Uppercase the first letter
            nameBuilder[0] = char.ToUpper(nameBuilder[0]);

            return nameBuilder.ToString();
        }
    }

}
