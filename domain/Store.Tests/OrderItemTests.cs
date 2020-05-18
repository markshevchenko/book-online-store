﻿using System;
using Xunit;

namespace Store.Tests
{
    public class OrderItemTests
    {
        [Fact]
        public void OrderItem_WithNegativeCount_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new OrderItem(1, 0m, -1));
        }

        [Fact]
        public void OrderItem_WithZeroCount_ThrowsArgumentException()
        {
            Assert.Throws<ArgumentException>(() => new OrderItem(1, 0m, 0));
        }

        [Fact]
        public void OrderItem_WithPositiveCount_SetsCount()
        {
            var orderItem = new OrderItem(1, 0m, 3);

            Assert.Equal(3, orderItem.Count);
        }
    }
}