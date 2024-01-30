using System.Security.Cryptography;

namespace KeyGen
{
    internal class Program
    {
        public static void GenerateKeysAndSave(string publicKeyPath, string privateKeyPath)
        {
            using (var rsa = new RSACryptoServiceProvider(2048))
            {
                // Получаем открытый ключ в формате PEM
                var publicKey = rsa.ToXmlString(false);

                // Получаем закрытый ключ в формате PEM
                var privateKey = rsa.ToXmlString(true);

                // Сохраняем ключи в файлы
                File.WriteAllText(publicKeyPath, publicKey);
                File.WriteAllText(privateKeyPath, privateKey);
            }
        }

        public static void Main()
        {
            // Указываем пути для сохранения ключей
            string publicKeyPath = "C:\\Users\\carna\\Source\\Repos\\IiyaMezer\\GB_SHARP_FINAL\\Messenger\\WebApiLib\\Rsa\\public_key.pem";
            string privateKeyPath = "C:\\Users\\carna\\Source\\Repos\\IiyaMezer\\GB_SHARP_FINAL\\Messenger\\WebApiLib\\Rsa\\private_key.pem";

            // Генерируем ключи и сохраняем их в файлы
            GenerateKeysAndSave(publicKeyPath, privateKeyPath);

            Console.WriteLine("RSA keys generated and saved successfully.");
        }
    }
}
