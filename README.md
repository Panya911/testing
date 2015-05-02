Флягин Павел
Махотин Иван
# Блок "Тестирование"

Сделайте fork этого репозитория.
Делайте commit и push после окончания выполнения каждого задания.

## Задача 1. Основы тестирования

В проекте Kontur.Courses.Testing есть одна корректная реализация интерфейса IWordsStatistics и много некорректных.

Вам нужно дополнить набор тестов в классе WordsStatistics_Tests.cs так, чтобы их проходила корректная реализация, 
а все остальные реализации падали хотя бы на одном тесте.

Проект устроен так, что при его запуске, он проверяет требуемые условия.
А при запуске тестов — тестирует какую-то одну реализацию (какая указана в инициализаторе поля createStat).


## Задача 2. Тесты как исполняемая спецификация

В файле Kontur.Courses.Testing.Patterns\Specifications\MarkdownProcessor.cs 
разработайте алгоритм рендеринга подмножества разметки MarkDown и создайте для вашей реализации
систему исполняемой спецификации в виде модульных тестов.

Описание языка Markdown ищите тут: Kontur.Courses.Testing.Patterns\Specifications\Markdown.txt

* Применяйте паттерн AAA
* Избегайте анти-паттернов
* Корректно выбирайте имена тестов — в окошке Resharper-UnitTestSessions имена тестов должны читаться как спецификация.

Для реализации метода Render достаточно трех замен регулярными выражениями.
Справку по регулярным выражениям можно найти тут: http://regexlib.com/CheatSheet.aspx

## Задача 3. TDD

Выполните TDD Kata "Bowling Game"

The game consists of 10 frames as shown above.  In each frame the player has
two opportunities to knock down 10 pins.  The score for the frame is the total
number of pins knocked down, plus bonuses for strikes and spares.

A spare is when the player knocks down all 10 pins in two tries.  The bonus for
that frame is the number of pins knocked down by the next roll.  So in frame 3
above, the score is 10 (the total number knocked down) plus a bonus of 5 (the
number of pins knocked down on the next roll.)

A strike is when the player knocks down all 10 pins on his first try.  The bonus
for that frame is the value of the next two balls rolled.

In the tenth frame a player who rolls a spare or strike is allowed to roll the extra
balls to complete the frame.  However no more than three balls can be rolled in
tenth frame.

Отталкивайтесь от такого первоначального дизайна:

	class Game 
	{
		void Roll(int pins) { ... }
		int GetScore() { ... }
	}
