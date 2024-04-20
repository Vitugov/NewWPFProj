using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task16.Model;

namespace Task16.Other
{
    public class Initialization
    {
        public static Initialization Instance { get; set; }

        private static readonly List<Client> InitClients = new List<Client>
        {
            new Client("Новак", "Ян", "Кшиштоф", "+48123456789", "jan.nowak@gmail.com"),
            new Client("Ковальски", "Петр", "Анджей", "+48111222333", "piotr.kowalski@yahoo.com"),
            new Client("Вишневски", "Томаш", "Юзеф", "+48123456780", "tomasz.wisniewski@outlook.com"),
            new Client("Домбровски", "Михал", "Казимеж", "+48123456781", "michal.dabrowski@mail.ru"),
            new Client("Левандовски", "Кшиштоф", "Богдан", "+48123456782", "krzysztof.lewandowski@yandex.ru"),
            new Client("Вуйцик", "Марек", "Станислав", "+48123456783", "marek.wojcik@protonmail.com"),
            new Client("Каминьски", "Яцек", "Тадеуш", "+48123456784", "jacek.kaminski@icloud.com"),
            new Client("Зеленски", "Рафал", "Роман", "+48123456785", "rafal.zielinski@aol.com"),
            new Client("Шиманьски", "Кароль", "Йежи", "+48123456786", "karol.szymanski@inbox.ru"),
            new Client("Вожняк", "Гжегож", "Генрик", "+48123456787", "grzegorz.wozniak@live.com")
        };

        private static readonly List<Order> InitOrders = new List<Order>
        {
            new Order(InitClients[0], 101, "Модулятор Нейро-Интерфейса"),
            new Order(InitClients[1], 102, "Протез Кибернетической Руки"),
            new Order(InitClients[2], 103, "Плащ Оптической Камуфляжа"),
            new Order(InitClients[3], 104, "Квантовый ЦПУ"),
            new Order(InitClients[4], 105, "Инжектор Нанитов Здоровья"),
            new Order(InitClients[5], 106, "Молекулярное Лезвие"),
            new Order(InitClients[6], 107, "Антигравитационные Ботинки"),
            new Order(InitClients[7], 108, "Плазменная Винтовка"),
            new Order(InitClients[8], 109, "Ручка Голографической Маскировки"),
            new Order(InitClients[9], 110, "ЭМП Граната"),
            new Order(InitClients[0], 111, "Шифратор Цифрового Сознания"),
            new Order(InitClients[1], 112, "Очки Дополненной Реальности"),
            new Order(InitClients[2], 113, "Синтетическое Мышечное Волокно"),
            new Order(InitClients[3], 114, "Гель Дермальной Брони"),
            new Order(InitClients[4], 115, "Кольцо Электрошоковой Защиты"),
            new Order(InitClients[5], 116, "Стелс-Дрон"),
            new Order(InitClients[6], 117, "Портативное Устройство Взлома"),
            new Order(InitClients[7], 118, "Беспроводной Пауэрбанк"),
            new Order(InitClients[8], 119, "Отмычка Биометрических Замков"),
            new Order(InitClients[9], 120, "Генератор Личного Силового Поля")
        };

        static Initialization()
        {
            Instance = new Initialization();
        }
        public void Init()
        {
            using (var db = new SqliteContext())
            {
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                db.Clients.AddRange(InitClients);
                db.Orders.AddRange(InitOrders);
                db.SaveChanges();
            }
        }
    } 
}