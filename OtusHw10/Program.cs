using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        // Создаем директории
        DirectoryInfo dir1 = Directory.CreateDirectory(@"c:\Otus\TestDir1");
        DirectoryInfo dir2 = Directory.CreateDirectory(@"c:\Otus\TestDir2");

        // Создаем и записываем файлы
        await CreateAndWriteFiles(dir1);
        await CreateAndWriteFiles(dir2);

        // Читаем и выводим содержимое файлов
        await ReadFiles(dir1);
        await ReadFiles(dir2);
    }

    static async Task CreateAndWriteFiles(DirectoryInfo directory)
    {
        for (int i = 1; i <= 10; i++)
        {
            string fileName = $"File{i}.txt";
            string filePath = Path.Combine(directory.FullName, fileName);

            // Создаем файл и записываем его имя в кодировке UTF8
            FileStream fileStream = File.Create(filePath);

            using (StreamWriter writer = new StreamWriter(fileStream, Encoding.UTF8))
            {
                try
                {
                    await writer.WriteAsync(fileName);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Ошибка при записи в файл {filePath}: {ex.Message}");
                }
            }

            // Добавляем текущую дату в файл
            await AddDateToFile(filePath);
        }
    }

    static async Task AddDateToFile(string filePath)
    {
        string date = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

        // Синхронно добавляем дату в файл
        try
        {
            string content = await File.ReadAllTextAsync(filePath);
            await File.WriteAllTextAsync(filePath, $"{content} {date}");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при записи в файл {filePath}: {ex.Message}");
        }

        // Асинхронно добавляем дату в файл
        try
        {
            using (StreamWriter writer = File.AppendText(filePath))
            {
                await writer.WriteAsync($" {date}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Ошибка при записи в файл {filePath}: {ex.Message}");
        }
    }

    static async Task ReadFiles(DirectoryInfo directory)
    {
        foreach (FileInfo file in directory.GetFiles())
        {
            try
            {
                string content = await File.ReadAllTextAsync(file.FullName);
                Console.WriteLine($"{file.Name}: {content}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при чтении файла {file.FullName}: {ex.Message}");
            }
        }
    }
}
