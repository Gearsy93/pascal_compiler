# Компилятор для языка Pascal
Задание по курсу Формальные грамматики и методы трансляции - перевод текста с языка программирования Pascal на машинный язык. Программа написана на языке C#.

Структура компилятора:
Модуль ввода-вывода -> Лексический анализатор -> Синтаксический анализатор -> Семантический анализатор -> Генератор

## Реализовано

* Класс ввода-вывода для считывания текста и передачи символов
* Лексический анализатор для поиска лексем и синтаксических ошибок

## TODO
* Синтаксический анализатор для проверки программы на соответствие формальным правилам и поиска синтаксических ошибок
* Семантический анализатор для проверки программы на смысловую правильность и поиск сементических ошибок
* Генератор для перевода проверенной программы в машинный язык

## Лексический анализатор
* Добавить форматированный список ошибок на уровне программы. Если найдена ошибка, после завершения работы конкретного анализатора необходимо вывести на экран список всех ошибок.
* Обновить удаление однострчных и многострочных комментариев
* При выводе ошибок добавить саму строчку с ошибкой и галочку, направленную на лексему
* Добавить ошибку длинная строка
* Добавить таблицу идентификаторов с хешированием
* Добавить поле - значение переменной
*
* Главная задача - убрать сырые токены и засунуть в классы


## Синтаксический анализатор
* Ключевые и идентификаторы слова могут содержаться в строке, нужно это учесть
* Добавить все виды выражение
* Добавить функцию - дерево разбора выражений
