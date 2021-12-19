# Компилятор для языка Pascal
Задание по курсу Формальные грамматики и методы трансляции - перевод текста с языка программирования Pascal на машинный язык. Программа написана на языке C#.

Структура компилятора:
Модуль ввода-вывода -> Лексический анализатор -> Синтаксический анализатор -> Семантический анализатор -> Генератор

## Реализовано

* Класс ввода-вывода для считывания текста и передачи символов
* Лексический анализатор для поиска лексем и синтаксических ошибок
* Синтаксический анализатор для проверки БНФ
* Семантический анализатор

Данная реализация не включает нейтрализацию ошибок и генератор
