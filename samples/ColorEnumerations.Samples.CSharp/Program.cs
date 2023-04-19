using ColorEnumerations;

var alternatingColors = AlternatingColors.Create(21, new RGB(0, 0, 0), new RGB(0, 0, 0));
Console.WriteLine($"{alternatingColors.Count()}");

// Use enumerations with limit
var randomColors1 = RandomColors.Create(10);
foreach (var randomColor in randomColors1)
    Console.WriteLine(randomColor);

// Use enumerations with Skip/Take
var randomColors2 = RandomColors.Create().Skip(100).Take(10);
foreach (var randomColor in randomColors2)
    Console.WriteLine(randomColor);

// But do not use it without limits and Skip/Take and then operate with functions that need full computation!
// Like: RandomColors.Create().Count()
