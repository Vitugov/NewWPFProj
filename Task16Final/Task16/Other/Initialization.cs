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

        private static readonly List<Commodity> InitCommodities = new List<Commodity>
        {
            new("Модулятор Нейро-Интерфейса"),
            new("Протез Кибернетической Руки"),
            new("Плащ Оптической Камуфляжа"),
            new("Квантовый ЦПУ"),
            new("Инжектор Нанитов Здоровья"),
            new("Молекулярное Лезвие"),
            new("Антигравитационные Ботинки"),
            new("Плазменная Винтовка"),
            new("Ручка Голографической Маскировки"),
            new("ЭМП Граната"),
            new("Шифратор Цифрового Сознания"),
            new("Очки Дополненной Реальности"),
            new("Синтетическое Мышечное Волокно"),
            new("Гель Дермальной Брони"),
            new("Кольцо Электрошоковой Защиты"),
            new("Стелс-Дрон"),
            new("Портативное Устройство Взлома"),
            new("Беспроводной Пауэрбанк"),
            new("Отмычка Биометрических Замков"),
            new("Генератор Личного Силового Поля")
        };
        private static readonly List<OrderRow> InitOrderRows = new List<OrderRow>
        {
            new(InitCommodities[0], 14, 2893.82M),
            new(InitCommodities[1], 2, 857.73M),
            new(InitCommodities[2], 19, 217.88M),
            new(InitCommodities[3], 11, 2624.39M),
            new(InitCommodities[4], 8, 1845.21M),
            new(InitCommodities[5], 5, 1990.98M),
            new(InitCommodities[6], 16, 1682.75M),
            new(InitCommodities[7], 4, 1597.36M),
            new(InitCommodities[8], 20, 483.05M),
            new(InitCommodities[9], 7, 2821.03M),
            new(InitCommodities[10], 3, 2228.19M),
            new(InitCommodities[11], 18, 2735.92M),
            new(InitCommodities[12], 9, 2396.16M),
            new(InitCommodities[13], 17, 1386.47M),
            new(InitCommodities[14], 6, 679.34M),
            new(InitCommodities[15], 1, 1483.27M),
            new(InitCommodities[16], 15, 2742.68M),
            new(InitCommodities[17], 13, 1774.50M),
            new(InitCommodities[18], 10, 998.84M),
            new(InitCommodities[19], 12, 2150.19M),
        };


        private static readonly List<Order> InitOrders = new List<Order>
        {
            new Order(InitClients[2], new DateTime(2024, 1, 3, 14, 22, 15), [InitOrderRows[0]]),
            new Order(InitClients[1], new DateTime(2024, 2, 5, 9, 48, 30), [InitOrderRows[1]]),
            new Order(InitClients[4], new DateTime(2024, 2, 15, 11, 6, 45), [InitOrderRows[2]]),
            new Order(InitClients[0], new DateTime(2024, 3, 10, 13, 31, 5), [InitOrderRows[3]]),
            new Order(InitClients[3], new DateTime(2024, 1, 21, 16, 12, 20), [InitOrderRows[4]]),
            new Order(InitClients[5], new DateTime(2024, 4, 2, 20, 18, 33), [InitOrderRows[5]]),
            new Order(InitClients[7], new DateTime(2024, 3, 27, 15, 54, 29), [InitOrderRows[6]]),
            new Order(InitClients[6], new DateTime(2024, 4, 12, 17, 46, 18), [InitOrderRows[7]]),
            new Order(InitClients[8], new DateTime(2024, 2, 22, 18, 30, 48), [InitOrderRows[8]]),
            new Order(InitClients[9], new DateTime(2024, 1, 12, 10, 59, 56), [InitOrderRows[9]]),
            new Order(InitClients[1], new DateTime(2024, 3, 3, 12, 2, 17), [InitOrderRows[10]]),
            new Order(InitClients[0], new DateTime(2024, 2, 18, 22, 14, 11), [InitOrderRows[11]]),
            new Order(InitClients[2], new DateTime(2024, 4, 25, 19, 27, 34), [InitOrderRows[12]]),
            new Order(InitClients[4], new DateTime(2024, 1, 27, 7, 39, 22), [InitOrderRows[13]]),
            new Order(InitClients[3], new DateTime(2024, 4, 16, 14, 53, 8), [InitOrderRows[14]]),
            new Order(InitClients[5], new DateTime(2024, 2, 11, 23, 12, 56), [InitOrderRows[15]]),
            new Order(InitClients[7], new DateTime(2024, 1, 17, 6, 25, 41), [InitOrderRows[16]]),
            new Order(InitClients[6], new DateTime(2024, 3, 22, 12, 40, 33), [InitOrderRows[17]]),
            new Order(InitClients[8], new DateTime(2024, 1, 30, 21, 18, 22), [InitOrderRows[18]]),
            new Order(InitClients[9], new DateTime(2024, 4, 10, 15, 55, 29), [InitOrderRows[19]])

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
                db.Commodities.AddRange(InitCommodities);
                db.OrderRows.AddRange(InitOrderRows);
                db.Orders.AddRange(InitOrders);
                db.SaveChanges();
            }
        }
    }
}