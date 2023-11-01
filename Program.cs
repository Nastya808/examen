using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Threading.Tasks;

namespace examen
{
    class Dictionary
    {
        public string Name { get; set; }
        public Dictionary<string, List<string>> Words { get; set; }
    }

    class Program
    {
        static List<Dictionary> dictionaries = new List<Dictionary>();

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            LoadDictionaries(); 


            Console.WriteLine("Ласкаво просимо до програми 'Словники'!");
            bool exit = false;

            while (!exit)
            {
                Console.WriteLine("\nГоловне меню:");
                Console.WriteLine("1. Створити словник");
                Console.WriteLine("2. Додати слово та переклад");
                Console.WriteLine("3. Редагувати слово або переклад");
                Console.WriteLine("4. Видалити слово (якщо більше одного перекладу)");
                Console.WriteLine("5. Пошук перекладу");
                Console.WriteLine("6. Вивести всі словники та переклади");
                Console.WriteLine("7. Вийти");

                int choice = int.Parse(Console.ReadLine());

                switch (choice)
                {
                    case 1:
                        CreateDictionary();
                        break;
                    case 2:
                        AddWordAndTranslation();
                        break;
                    case 3:
                        EditWordOrTranslation();
                        break;
                    case 4:
                        DeleteWord();
                        break;
                    case 5:
                        SearchForTranslation();
                        break;
                    case 6:
                        ViewAllDictionaries();
                        break;
                    case 7:
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Некоректний вибір. Будь ласка, спробуйте ще раз.");
                        break;
                }
            }

            SaveDictionaries();
        }

        static void CreateDictionary()
        {
            Console.Write("Введіть назву словника: ");
            string name = Console.ReadLine();

            if (dictionaries.Exists(d => d.Name == name))
            {
                Console.WriteLine("Словник з такою назвою вже існує.");
            }
            else
            {
                Dictionary newDictionary = new Dictionary
                {
                    Name = name,
                    Words = new Dictionary<string, List<string>>()
                };

                dictionaries.Add(newDictionary);
                Console.WriteLine("Словник створено успішно.");
            }
        }

        static void AddWordAndTranslation()
        {
            Console.Write("Введіть назву словника: ");
            string dictionaryName = Console.ReadLine();

            var dictionary = dictionaries.Find(d => d.Name == dictionaryName);
            if (dictionary == null)
            {
                Console.WriteLine("Словник не знайдено.");
                return;
            }

            Console.Write("Введіть слово: ");
            string word = Console.ReadLine();
            Console.Write("Введіть переклад: ");
            string translation = Console.ReadLine();

            if (!dictionary.Words.ContainsKey(word))
            {
                dictionary.Words[word] = new List<string>();
            }

            dictionary.Words[word].Add(translation);
            Console.WriteLine("Слово та переклад додано успішно.");
        }

        static void EditWordOrTranslation()
        {
            Console.Write("Введіть назву словника: ");
            string dictionaryName = Console.ReadLine();

            var dictionary = dictionaries.Find(d => d.Name == dictionaryName);
            if (dictionary == null)
            {
                Console.WriteLine("Словник не знайдено.");
                return;
            }

            Console.Write("Введіть слово для редагування: ");
            string word = Console.ReadLine();
            if (!dictionary.Words.ContainsKey(word))
            {
                Console.WriteLine("Слово не знайдено в словнику.");
                return;
            }

            Console.WriteLine("Поточні переклади:");
            foreach (var translation in dictionary.Words[word])
            {
                Console.WriteLine(translation);
            }

            Console.Write("Введіть нове слово (залиште порожнім для збереження поточного): ");
            string newWord = Console.ReadLine();
            if (!string.IsNullOrEmpty(newWord))
            {
                dictionary.Words.Add(newWord, dictionary.Words[word]);
                dictionary.Words.Remove(word);
            }

            Console.Write("Введіть новий переклад (залиште порожнім для збереження поточного): ");
            string newTranslation = Console.ReadLine();

            if (!string.IsNullOrEmpty(newTranslation))
            {
                dictionary.Words[newWord].Add(newTranslation);
                dictionary.Words[word].Remove(newTranslation);
            }

            Console.WriteLine("Слово або переклад відредаговано успішно.");
        }

        static void DeleteWord()
        {
            Console.Write("Введіть назву словника: ");
            string dictionaryName = Console.ReadLine();

            var dictionary = dictionaries.Find(d => d.Name == dictionaryName);
            if (dictionary == null)
            {
                Console.WriteLine("Словник не знайдено.");
                return;
            }

            Console.Write("Введіть слово для видалення: ");
            string word = Console.ReadLine();

            if (dictionary.Words.ContainsKey(word))
            {
                if (dictionary.Words[word].Count > 1)
                {
                    dictionary.Words.Remove(word);
                    Console.WriteLine("Слово видалено успішно.");
                }
                else
                {
                    Console.WriteLine("Слово не може бути видалено, оскільки у нього є лише один переклад.");
                }
            }
            else
            {
                Console.WriteLine("Слово не знайдено в словнику.");
            }
        }

        static void SearchForTranslation()
        {
            Console.Write("Введіть назву словника: ");
            string dictionaryName = Console.ReadLine();

            var dictionary = dictionaries.Find(d => d.Name == dictionaryName);
            if (dictionary == null)
            {
                Console.WriteLine("Словник не знайдено.");
                return;
            }

            Console.Write("Введіть слово для пошуку: ");
            string word = Console.ReadLine();

            if (dictionary.Words.ContainsKey(word))
            {
                Console.WriteLine($"Переклади для слова '{word}':");
                foreach (var translation in dictionary.Words[word])
                {
                    Console.WriteLine(translation);
                }
            }
            else
            {
                Console.WriteLine($"Слово '{word}' не знайдено в словнику.");
            }
        }

        static void ViewAllDictionaries()
        {
            if (dictionaries.Count == 0)
            {
                Console.WriteLine("Словники не знайдені.");
            }
            else
            {
                Console.WriteLine("Усі словники та їх переклади:");
                foreach (var dictionary in dictionaries)
                {
                    Console.WriteLine($"Словник: {dictionary.Name}");
                    foreach (var word in dictionary.Words)
                    {
                        Console.WriteLine($"  Слово: {word.Key}");
                        foreach (var translation in word.Value)
                        {
                            Console.WriteLine($"    Переклад: {translation}");
                        }
                    }
                }
            }
        }

        static void LoadDictionaries()
        {
            if (File.Exists("dictionaries.txt"))
            {
                string[] lines = File.ReadAllLines("dictionaries.txt");

                foreach (string line in lines)
                {
                    string[] parts = line.Split('|');
                    if (parts.Length == 2)
                    {
                        string dictionaryName = parts[0];
                        string dictionaryData = parts[1];
                        var dictionary = new Dictionary { Name = dictionaryName, Words = new Dictionary<string, List<string>>() };
                        string[] wordTranslations = dictionaryData.Split(';');
                        foreach (var wordTranslation in wordTranslations)
                        {
                            string[] wordTranslationParts = wordTranslation.Split(':');
                            if (wordTranslationParts.Length == 2)
                            {
                                string word = wordTranslationParts[0];
                                string[] translations = wordTranslationParts[1].Split(',');
                                dictionary.Words[word] = new List<string>(translations);
                            }
                        }
                        dictionaries.Add(dictionary);
                    }
                }
            }
        }

        static void SaveDictionaries()
        {
            using (StreamWriter writer = new StreamWriter("dictionaries.txt"))
            {
                foreach (var dictionary in dictionaries)
                {
                    writer.Write(dictionary.Name + "|");
                    List<string> wordTranslations = new List<string>();
                    foreach (var word in dictionary.Words)
                    {
                        string translations = string.Join(",", word.Value);
                        wordTranslations.Add($"{word.Key}:{translations}");
                    }
                    string dictionaryData = string.Join(";", wordTranslations);
                    writer.WriteLine(dictionaryData);
                }
            }
        }
    }
}