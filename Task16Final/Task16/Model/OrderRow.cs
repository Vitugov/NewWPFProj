﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using WPFUsefullThings;

namespace Task16.Model
{
    [DisplayNames("Строка заказа", "Номенклатура")]
    public class OrderRow : ProjectModel
    {
        [DisplayName("Наименование товара")]
        public Commodity Commodity { get; set; }
        
        [DisplayName("Количество")]
        public int Quantity { get; set; }

        [DisplayName("Цена")]
        public decimal Price { get; set; }
        
        [DisplayName("Сумма")]
        public decimal Sum { get; set; }
        
        public OrderRow(Commodity commodity, int quantity, decimal price)
        {
            Commodity = commodity;
            Quantity = quantity;
            Price = price;
            Sum = quantity * price;
        }

        public OrderRow() {}

        protected override void UpdateDisplayName()
        {
            DisplayName = $"{Commodity} {Quantity} x {Price} = {Sum}";
        }
    }
}
