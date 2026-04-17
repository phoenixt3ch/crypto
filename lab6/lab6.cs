// Основы крипт 434 

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
					
public class Program
{
	// Словарь для расшифровки (для каждого символа шифротекста - символ расшифрованного текста)
	static Dictionary<Char, Char> dict = new Dictionary<Char, Char>();
	// Частоты символов шифротекста (число повторов каждой буквы)
	static Dictionary<Char, int> char_freqs = new Dictionary<Char, int>();
	// Частоты биграмм шифротекста (пар букв)
	static Dictionary<String, int> bigram_freqs = new Dictionary<String, int>();
	// Частоты триграмм шифротекста (троек букв)
	static Dictionary<String, int> trigram_freqs = new Dictionary<String, int>();
	
	// Шифротекст
	static String c_text = "щ  зсдлъэд фцяб  цоцюъбк сня пбь  емйюсцдъ й  цяъцк  йюцсцъб йюмсэъъбжэ яцжмжэ, м й ясыфцк - яьэъъбж цяъцгюмлъбж еямъэдж зця цяъцк исбудк, \n" + "ъдйжцюсн ъм юц, аюц цъц зсэъмяьдлмьц ядйнюимж щьмядьчшдщ. эе щйдо гюэо еямъэк юцьчиц ящм яцжм пбьэ лэьбжэ: яцж, фяд  фцйюэъэшм \"ицъюэъдъюмьч\", \n" + 
		"ям йюцнвэк сняцж й ъэж юсмиюэс дфцсцщм, еъмждъэюбк йщцэжэ пьэъмжэ.  цйюмьчъцд щйд ьмщиэ, щзьцюч яц ющдсйицк. юсмиюэс  дфцсцщм  ицфямюц  зсэъмяьдлмь щцсцъэъы, \n" + "э ъм щбщдйид  пбьм эецпсмлдъм щцсцъм,  ядслмвмн  щ  иьтщд пьэъ. щйд  ьмщиэ  цоцюъцфц сням  пбьэ жнйъбд, сбпъбд, м зця  ъэжэ едьдъъбд зцящмьб. емяъэд \n" + 
		"ящдсэ ьмщци щбоцяэьэ ъм цфсцжъбк  ящцс-жцъдюъбк,  ими  дфц  ъмебщмьэ  эеясдщьд.  ъм  ъдж  пбьэ юцлд цяъцгюмлъбд  жнйъбд,  лэщцсбпъбд  э нэаъбд  ьмщиэ,  \n" + "м зцйсдяэъд--ящыогюмлъбк \"жцъдюъбк\"  юсмиюэс. щ емяъдк амйюэ ящцсм - сня  ймсмтуди й зцфсдпмжэ э иьмяцщбжэ, иэудщуэжэ зцьаэвмжэ исбй. \n" +
		"цоцюъбк  сня зцьыаэь  йщцд  ъмещмъэд  двд щ  юд  щсдждъм,  ицфям  еядйч смесдудъц пбьц юцсфцщмюч яэачт, зсэъцйэжцк зцяжцйицщъбжэ цоцюъэимжэ. щздсдяэ  ьмщци, \n" + "ъм зьцвмяэ, щяцьч  уэсцицфц юсцюымсм, йюцньэ здсдъцйъбд змьмюиэ   э   юцьзэьэйч   юцсфцщшб  й  ицсеэъмжэ  э   ждуимжэ,  ъмзцьъдъъбжэ \n" + 
		"щйдщцежцлъбжэ  зсцяыиюмжэ.  оцяэьэ цоцюъэиэ,  цпщдумъъбд  ыюимжэ, юдюдсимжэ, емкшмжэ. ы пмп эе ицсеэъ юцсамьэ фцьцщб иыс э шбзьню, щ ждуимо щэелмьэ зцсцйнюм,  \n" + "ицюцсбо зсцямщшб, щбъэжмн эе ждуим,аюцпб  зцимемюч  зциызмюдьт,  ъдзсдждъъц зцяъэжмьэ  ъмя  фцьцщцк,  ядслм ем йщнемъъбд  емяъэд  ъцфэ. \n" + 
		"ъм  жцйюцщцк  здсдя  змьмюимжэ  йъцщмьэ зэсцлъэиэ, пьэъъэиэ,  юцсфцщшб фсдаъдщэимжэ,  лмсдъъбжэ  ъм зцйюъцж  жмйьд. йпэюдъвэиэ смеьэщмьэ, зц ицздкид ем  йюмимъ,  \n" + "фцснаэк йпэюдъч - ьтпэжбк  юцфям ждяцщбк ъмзэюци, йцфсдщмщуэк эещцеаэицщ э йьылмвэо,  емждсемщуэо щ оцьцяъбо  ьмщимо. ьдюцж  йпэюдъвэицщ \n" + 
		"йждъньэ юцсфцщшб  ищмймжэ, э  ймжбк  ьтпэжбк эе ъэо  пбь фсыудщбк, эе  щмсдъбо  фсыу,  ицюцсбд  щ  жцадъцж  щэяд ьдлмьэ  яьн  зсцямлэ зэсмжэямжэ ъм ьцюимо, м ищмй \n" + "адсзмьэ эе щдясм исылимжэ. жнйъбд э сбпъбд ьмщиэ йцйюцньэ эе  ящыо цюядьдъэк. щ  здсщцж ьдлмьц ъм зцьимо жнйц смеъбо йцсюцщ - яэач, иысб, \n" + 
		"фыйэ, эъядкиэ, змьдъбд зцсцйнюм яьн лмсицфц  э щ ьдянъбо  щмъъмо - пдьбд зцсцйнюм  яьн емьэщъцфц.  ъм истачно  зц йюдъмж пбьэ смещдумъб юыуэ пмсмуицщ э  зцдъъбо \n" + "жцьцицж юдьню, м щдйч зцюцьци емъню  цицсцимжэ  щйдщцежцлъбо смеждсцщ э  зсэфцюцщьдъэк--ицзадъбо, щмсдъбо, зсцщдйъбо. щц щюцсцж  цюядьдъээ, юджъцж, \n" + 
		"цйщдвдъъцж юцьчиц ящдсчт  щц ящцс, щэйдьэ ядйнюиэ жнйъбо  юыу. зця щйджэ ьмщимжэ - зцящмьб. цоцюъбк  сня пбщмь цйцпдъъц  цлэщьдъъбж  здсдя  пцьчуэжэ  зсмеяъэимжэ.\n" + "и ьмщимж  зцяхделмьэ ъм юбйнаъбо сбймимо смйрсмъадъъбд иызаэоэ, э ем ъэжэ йьылмвэд щбъцйэьэ эе ьмщци";
	
	// Подсчет частот букв, биграмм и триграмм
	public static void ParseWord(String word)
	{
		// Подсчет букв
		foreach (Char letter in word)
			if (char_freqs.ContainsKey(letter))
				char_freqs[letter]++;
			else
				char_freqs.Add(letter, 1);
		// Подсчет биграмм
		for (int i = 0; i < word.Length - 1; i++)
		{
			String bigram = word.Substring(i, 2);
			if (bigram_freqs.ContainsKey(bigram))
				bigram_freqs[bigram]++;
			else
				bigram_freqs.Add(bigram, 1);
		}
		// Подсчет триграмм
		for (int i = 0; i < word.Length - 2; i++)
		{
			String trigram = word.Substring(i, 3);
			if (trigram_freqs.ContainsKey(trigram))
				trigram_freqs[trigram]++;
			else
				trigram_freqs.Add(trigram, 1);
		}
	}
	
	// Вывод на консоль для сопоставления слов:
	// В первой строке выводит заданный набор слов шифротекста
	// Во второй строке - соответствующие им наборы слов расшифрованного текста (по словарю dict)
	static void printHashByDict(IEnumerable<String> words)
	{
		foreach (String word in words)
			Console.Write("{0} ", word);			
		Console.WriteLine();
		foreach (String word in words) {
			for (int i = 0; i < word.Length; i++)
				Console.Write(dict[word[i]]);
			Console.Write(" ");
		}
		Console.WriteLine();
	}
	
	public static void Main()
	{
		// 1. Разделяем шифротекст на слова
		String[] c_words = c_text.Split(new char[] { ',', '.', ' ', ':', '-', '\"', '\n'}, StringSplitOptions.RemoveEmptyEntries);
		
		// 2. Собираем статистику по всем словам (частоты букв, пар и троек букв)
		foreach (String word in c_words)
			ParseWord(word);
			
		// 2.1. Сортируем буквы шифротекста по частоте и выводим на консоль
		var char_list = char_freqs.ToList();
		char_list.Sort((x, y) => (x.Value < y.Value) ? 1 : ((x.Value == y.Value) ? 0 : -1));
		Console.WriteLine("Число символов в алфавите шифротекста {0}", char_list.Count());
		foreach (var p in char_list)
			Console.WriteLine("{0} {1}", p.Key, p.Value);
			
		// 2.2. Сортируем биграммы шифротекста по частоте и выводим на консоль первые 5 самых частых
		int show_count = 5;
		var bigram_list = bigram_freqs.ToList();
		bigram_list.Sort((x, y) => (x.Value < y.Value) ? 1 : ((x.Value == y.Value) ? 0 : -1));
		Console.WriteLine("\nБиграммы (частые {0})", show_count);
		for (int i = 0; i < show_count; i++)
			Console.WriteLine("{0} {1}", bigram_list[i].Key, bigram_list[i].Value);
		
		// 2.3. Сортируем триграммы шифротекста по частоте и выводим на консоль первые 5 самых частых
		var trigtam_list = trigram_freqs.ToList();
		trigtam_list.Sort((x, y) => (x.Value < y.Value) ? 1 : ((x.Value == y.Value) ? 0 : -1));
		Console.WriteLine("\nТриграммы (частые {0})", show_count);
		for (int i = 0; i < show_count; i++)
			Console.WriteLine("{0} {1}", trigtam_list[i].Key, trigtam_list[i].Value);
		
		// 3. Эталонный список русских символов (по убыванию частоты)
		List<Char> rus_letters = new List<Char>() {'о', 'е', 'а', 'и', 'н', 'т', 'с', 'р', 'в', 'л', 'к', 'м', 'д', 'п', 'у', 'я', 'ы', 'ь', 'г', 'з', 'б', 'ч', 'й', 'х', 'ж', 'ш', 'ю', 'ц', 'щ', 'э', 'ф', 'ъ' };
		// Формируем список букв шифротекста, отсортированный по убыванию частоты
		List<Char> sort_letters = char_list.Select(x => x.Key).ToList();		
		
		// 4. Вручную заполняем словарь для расшифровки (таблица сопоставления символов шифротекста и расшифрованного текста)
		// щ э й ъм юд - это предлоги
		dict.Add('щ', 'в');	// первое слово текста - либо В, либо С
		dict.Add('ц', 'о');	// по частоте
		// цъц - о_о - либо обо, либо оно, но есть удвоение ъъ - это нн (ии, оо, сс), значит ъ - это н
		dict.Add('ъ', 'н');
		// ъэжэ - н___ - возможно, ними, так как есть удвоение ээ - то "э", вероятно, "и"
		dict.Add('э', 'и');	// тогда ъэжэ - ними, значит ж - это м
		dict.Add('ж', 'м');
		// ъдж - это либо ним, либо нем (нём), либо нам
		// если бы "д" было "и" или "а", то в шифротексте был бы такой предлог, но его нет => д - это е
		dict.Add('д', 'е');
		// йщцэжэ (_воими) + щйд (в_е) => "й" - это "с"
		dict.Add('й', 'с');
		// Предлог ъм (н_) - это на, не, ни или но (поскольку буквы о,и,е уже расшифрованы, то м - это а)
		dict.Add('м', 'а');
		// цйцпдъъц (осо_енно) - п - это б
		dict.Add('п', 'б');
		// ъмещмъэд (на_вание) - е - это з
		dict.Add('е', 'з');
		// щсдждъм (в_емена) - с - это р
		dict.Add('с', 'р');
		// И далее в таком духе
		dict.Add('и', 'к');		
		dict.Add('ь', 'л');
		dict.Add('ю', 'т');
		dict.Add('к', 'й');
		dict.Add('з', 'п');
		dict.Add('ы', 'у');		
		dict.Add('л', 'ж');
		dict.Add('я', 'д');
		dict.Add('б', 'ы');
		dict.Add('ф', 'г');
		dict.Add('о', 'х');
		dict.Add('н', 'я');
		dict.Add('ч', 'ь');
		dict.Add('ш', 'ц');
		dict.Add('г', 'э');
		dict.Add('у', 'ш');
		dict.Add('а', 'ч');
		dict.Add('в', 'щ');
		dict.Add('т', 'ю');
		dict.Add('х', 'ъ');
		dict.Add('р', 'ф');
		
		// Для символов шифротекста, для которых не подставили замену, подставляем символ '_'
		for (int i = 0; i < sort_letters.Count; i++)
			if (!dict.ContainsKey(sort_letters[i]))
				//dict.Add(sort_letters[i], rus_letters[i]);
				dict.Add(sort_letters[i], '_');
			
		// 5. Для проверки нашего словаря, выводим удвоения - пары из двух одинаковых букв
		// (в русском это обычно "ии", "нн", "оо", "сс")
		Console.WriteLine("\nУдвоения");		//  стр. 452
		HashSet<String> doubles = bigram_freqs.Keys.Where(s => s[0] == s[1]).ToHashSet();
		printHashByDict(doubles);
		// Выводим предлоги из 1й, 2х, 3х букв
		// Для каждого предлога из шифротекста внизу будет выведена его расшифровка по словарю dict
		Console.WriteLine("\nПредлоги");
		for (int L = 1; L <= 3; L++) {
			HashSet<String> short_words = c_words.Where(s => s.Length == L).ToHashSet();
			printHashByDict(short_words);
		}
		// Выводим словарь
		bool showDict = true;
		if (showDict) {
			Console.WriteLine("\nDictionary:");		
			foreach (var p in dict)
				Console.Write("{0} ", p.Value);
			Console.Write('\n');
			foreach (var p in dict)
				Console.Write("{0} ", p.Key);
		}
		
		// Расшифровываем текст
		StringBuilder text_ = new StringBuilder();
		for (int i = 0; i < c_text.Length; i++)
			if (dict.ContainsKey(c_text[i]))
				text_.Append(dict[c_text[i]]);
		else
			text_.Append(c_text[i]);
		String text = text_.ToString();	// расшифрованный текст
	
		// Выводим строки текста и шифротекста парами - сверху строку шифротекста, под ним - строку расшифрованного текста
		String[] c_lines = c_text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
		String[] lines = text.Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
		Console.WriteLine("\nТекст:");
		for (int i = 0; i < lines.Count(); i++) {
			//Console.WriteLine(c_lines[i]);
			Console.WriteLine("{0}\n{1}\n", c_lines[i], lines[i]);
		}
	}	
}