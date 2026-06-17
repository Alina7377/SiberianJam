using UnityEngine;
using SaveSystemData;
using System.Security.Cryptography;
using System.Text;

public abstract class SavedObject: MonoBehaviour
{
    [SerializeField] protected string _guid;

    protected string CreatHashID(string name, Vector3 position)
    {
        // 1. Объединяем все входные данные в одну строку
        string combinedData = $"{name}{position.x}{position.y}{position.z}";

        // Создаем объект алгоритма SHA256
        using (SHA256 sha256 = SHA256.Create())
        {
            // 2. Преобразуем строку в массив байтов
            byte[] data = Encoding.UTF8.GetBytes(combinedData);

            // 3. Вычисляем хеш
            byte[] hashBytes = sha256.ComputeHash(data);

            // 4. Конвертируем байты хеша в читаемую строку
            StringBuilder sb = new StringBuilder();
            foreach (byte b in hashBytes)
                sb.Append(b.ToString("X2")); // X2 - форматирует байт как двузначное шестнадцатеричное число

            return sb.ToString();
        }
    }

    public abstract SObjectData SaveData();

    public abstract void LoadData(SObjectData loadData);
}
