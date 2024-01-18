using UnityEngine;

public class ProceduralNameGenerator : MonoBehaviour
{
    [SerializeField] int _minCharac = 4;
    [SerializeField] int _maxCharac = 10;

    public string GenerateRandomName()
    {
        return GenerateRandomName(Random.Range(_minCharac, _maxCharac));
    }

    private string GenerateRandomName(int length)
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
