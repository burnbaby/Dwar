﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DwarAuctionAnalyzer
{
    class JavaScripts
    {
        public static string itemsForDb = @"//Функция возвращает массив тегов <tr> с классом brd2 - все ячейки, внутри которых инфа о предмете
            function getItemsRows()
            {
	            var items = $('tr.brd2');
	            return items;
            }

            //Функция для подсчета цены ставки в меди, в качестве параметра - ячейка, содержащая цену
            function getMoneyValue(moneyContainingCell)
            {
	            //Строку под результат 
	            var moneyStr = '';
	            //В переменной хранится строка, отображающее количество золотых монет - первый элемент ячейки с классом 'mgold'
	            var goldStr = moneyContainingCell.getElementsByClassName('mgold')[0].innerText;
	            //Если монеты считали, переменная не пустая - просто добавляем ее содержимое в переменную результата, обрезая первый символ(пробел)
	            if(goldStr != undefined)
		            moneyStr += goldStr.substr(1);
	            //В переменной хранится строка, отображающее количество серебряных монет - первый элемент ячейки с классом 'msilver'
	            var silverStr = moneyContainingCell.getElementsByClassName('msilver')[0].innerText;
	            //Если монеты считали, переменная пустая - проходим дальше
	            if(silverStr != undefined)
		            //Если числовой эквивалент строки равен 0, добавляем в строку результата '00' и идем дальше
		            if(+silverStr == 0)
			            moneyStr += '00';
		            //Иначе режем первый символ строки(пробел), а все остальное добавляем в строку результата
		            else
			            moneyStr += silverStr.substr(1);
	            //В переменной хранится строка, отображающее количество бронзовых монет - первый элемент ячейки с классом 'mbronze'
	            var bronzeStr = moneyContainingCell.getElementsByClassName('mbronze')[0].innerText;
	            //Если числовой эквивалент строки равен 0, добавляем в строку результата '00' и идем дальше
	            if(+bronzeStr == 0)
		            moneyStr += '00';
	            else
	            //Иначе режем первый символ строки(пробел), а все остальное добавляем в строку результата
		            moneyStr += bronzeStr.substr(1);
	            //Возвращаем результат
	            return moneyStr;
            }

            //В этой функции собираем всю информацию об одном предмете, в качестве параметра - html код ячейки предмета
            function getItemInfo(itemRow)
            {
	            //Создаем массив под результат - всю необходимую информацию о предмете будем складывать туда
	            var itemInfo = [];
	            //Первым в массив результата отправляется - id лота. Атрибут aid, второго тега input ячейки предмета 
	            itemInfo.push($('input',itemRow)[1].getAttribute('aid'));
	            //Добавляем название предмета - текст внутри первого тега 'a' ячейки, хранящей инфу о предмете
	            itemInfo.push($('a',itemRow)[0].innerText);
	            //Добавляем категорию предмета - находится в первом по счету теге <span> тега <td> ячейки предмета
	            itemInfo.push($('span',$('td', itemRow)[2])[0].innerText);
	            //Добавляем прочность предмета - находится во втором по счету теге <span> тега <td> ячейки предмета
	            itemInfo.push($('span',$('td', itemRow)[2])[1].innerText);
	            //Добавляем время оставшееся лоту на аукционе - содержимое 4-го <td> ячейки предмета
	            itemInfo.push($('td',itemRow)[3].innerText);
	            //Добавляем количество предметов в лоте - содержимое 6-го <td> ячейки предмета
	            itemInfo.push($('td',itemRow)[5].innerText);
	            //Добавляем цену ставки в меди - вызываем ф-ю подсчета цены в меди, передавая в нее содержимое 8-го <td> ячейки предмета
	            itemInfo.push(getMoneyValue($('td', itemRow)[7]));
	            //Если содержимое 9-го <td> ячейки предмета отсутствует - т.е. выкупа нет - возвращаем пустую строку
	            if($('td', itemRow)[8].innerText == '')
		            itemInfo.push('');
	            //Иначе вызываем ф-ю подсчета цены в меди, передавая в нее содержимое 9-го <td> ячейки предмета
	            else
		            itemInfo.push(getMoneyValue($('td', itemRow)[8]));
	            return itemInfo;
            }

            //Возвращает необходимую информацию обо всех предметах на странице. Порядок такой: id лота - primaryKey для таблицы, название предмета, категория, прочность,
            //время оставшееся до конца лота, количество предметов в лоте, ставка в меди, выкуп в меди(бывает что выкупа нет, тогда возвращается путсая строка)
            function getItemsForDb()
            {
	            //Кладем в массив все ячейки(теги tr), которые содержат информацию о предмете
	            var items = getItemsRows();
	            //Если элементы есть
	            if(items != undefined)
	            {						
		            //Создаем переменную под результат
		            var results = [];
		            //Для всех предметов получаем нужную информацию и добавляем ее в массив результата(results)
		            for(var i = 0; i < items.length; i++)
			            results.push(getItemInfo(items[i]));
		            return results;
	            }	
	            //а если элементов нет, возвращаем угадай-что?
	            else
		            return undefined;
            }

            getItemsForDb();";

        public static string pagesNumber = @"function getPagesNumber()
           {
            var element = document.getElementsByClassName('pg-inact');
            if(element[element.length - 1] == undefined)
             return 0;
            else
             return +(element[element.length - 1].innerText);
           }
           getPagesNumber();";
    }
}
