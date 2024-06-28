## Результаты тестирования с использованием Postman
  
```
→ 01_Зарегистрировать пользователя
  POST http://arch.homework/identity/users/register [200 OK, 225B, 266ms]
  √  [INFO] Request headers: [{"key":"Content-Type","value":"application/json","system":true},{"key":"User-Agent","value":"PostmanRuntime/7.39.0","system":true},{"key":"Accept","value":"*/*","system":true},{"key":"Cache-Control","value":"no-cache","system":true},{"key":"Postman-Token","value":"4d95d8e6-a5d4-4c1b-bc6a-0b125d46f62c","system":true},{"key":"Host","value":"arch.homework","system":true},{"key":"Accept-Encoding","value":"gzip, deflate, br","system":true},{"key":"Connection","value":"keep-alive","system":true},{"key":"Content-Length","value":"109","system":true}]
  √  [INFO] Request body: {
  "login": "Titus_Greenholt",
  "password": "mSv0TyQus2q_aPk",
  "email": "Shana.Kovacek92@gmail.com"
}
  √  [INFO] Response headers: [{"key":"Date","value":"Fri, 28 Jun 2024 13:49:05 GMT"},{"key":"Content-Type","value":"application/json; charset=utf-8"},{"key":"Transfer-Encoding","value":"chunked"},{"key":"Connection","value":"keep-alive"}]
  √  [INFO] Response body: {"id":1,"login":"Titus_Greenholt","email":"Shana.Kovacek92@gmail.com"}
  √  HTTPStatus 200
  √  User is created
```

```
→ 02_Вход пользователя
  POST http://arch.homework/identity/users/login [200 OK, 422B, 21ms]
  √  [INFO] Request headers: [{"key":"Content-Type","value":"application/json","system":true},{"key":"User-Agent","value":"PostmanRuntime/7.39.0","system":true},{"key":"Accept","value":"*/*","system":true},{"key":"Cache-Control","value":"no-cache","system":true},{"key":"Postman-Token","value":"16880ff2-c899-407b-9155-7f0360ed4867","system":true},{"key":"Host","value":"arch.homework","system":true},{"key":"Accept-Encoding","value":"gzip, deflate, br","system":true},{"key":"Connection","value":"keep-alive","system":true},{"key":"Content-Length","value":"68","system":true}]
  √  [INFO] Request body: {
  "login": "Titus_Greenholt",
  "password": "mSv0TyQus2q_aPk"
}
  √  [INFO] Response headers: [{"key":"Date","value":"Fri, 28 Jun 2024 13:49:07 GMT"},{"key":"Content-Length","value":"0"},{"key":"Connection","value":"keep-alive"},{"key":"Cache-Control","value":"no-cache,no-store"},{"key":"Expires","value":"-1"},{"key":"Pragma","value":"no-cache"},{"key":"Set-Cookie","value":"sakurlyk.identity.session=CfDJ8FmpluDZL3ZJsCjAFhdHnf5gBzP2f%2FkExw5WsPqIqZZYrVjCmj195wr4PdcfXXJ0wsgrxgnJW8%2B5wZkqDd%2F6O7t9qGZMjkYGtqxOcy%2FxWwGKC85guqOep4zp%2BJlAkWQpPBstZhMIKvKv3HdXQ2O4QoXfnvor3J7QBOkJBdSG8qXg; path=/; samesite=lax; httponly"}]
  √  [INFO] Response body:
  √  HTTPStatus 200
  √  Session cookies set
```

```
→ 03_Создание нового продукта на складе (5 штук по цене 100)
  POST http://arch.homework/warehouse/products [200 OK, 219B, 130ms]
  √  [INFO] Request headers: [{"key":"Content-Type","value":"application/json","system":true},{"key":"User-Agent","value":"PostmanRuntime/7.39.0","system":true},{"key":"Accept","value":"*/*","system":true},{"key":"Cache-Control","value":"no-cache","system":true},{"key":"Postman-Token","value":"9bd47984-087a-4894-a00f-12ddddb52bbf","system":true},{"key":"Host","value":"arch.homework","system":true},{"key":"Accept-Encoding","value":"gzip, deflate, br","system":true},{"key":"Connection","value":"keep-alive","system":true},{"key":"Content-Length","value":"60","system":true},{"key":"Cookie","value":"sakurlyk.identity.session=CfDJ8FmpluDZL3ZJsCjAFhdHnf5gBzP2f%2FkExw5WsPqIqZZYrVjCmj195wr4PdcfXXJ0wsgrxgnJW8%2B5wZkqDd%2F6O7t9qGZMjkYGtqxOcy%2FxWwGKC85guqOep4zp%2BJlAkWQpPBstZhMIKvKv3HdXQ2O4QoXfnvor3J7QBOkJBdSG8qXg","system":true}]
  √  [INFO] Request body: {
  "name": "Mouse",
  "quantity": 5,
  "price": 100.0
}
  √  [INFO] Response headers: [{"key":"Date","value":"Fri, 28 Jun 2024 13:49:08 GMT"},{"key":"Content-Type","value":"application/json; charset=utf-8"},{"key":"Transfer-Encoding","value":"chunked"},{"key":"Connection","value":"keep-alive"}]
  √  [INFO] Response body: {"productId":1,"productName":"Mouse","quantity":5,"price":100.0}
  √  HTTPStatus 200
  √  Product is create
```

```
→ 04_Получение списка доступных товаров на складе
  GET http://arch.homework/warehouse/products [200 OK, 221B, 21ms]
  √  [INFO] Request headers: [{"key":"User-Agent","value":"PostmanRuntime/7.39.0","system":true},{"key":"Accept","value":"*/*","system":true},{"key":"Cache-Control","value":"no-cache","system":true},{"key":"Postman-Token","value":"dc14de09-1800-4769-8302-94264c291d5a","system":true},{"key":"Host","value":"arch.homework","system":true},{"key":"Accept-Encoding","value":"gzip, deflate, br","system":true},{"key":"Connection","value":"keep-alive","system":true},{"key":"Cookie","value":"sakurlyk.identity.session=CfDJ8FmpluDZL3ZJsCjAFhdHnf5gBzP2f%2FkExw5WsPqIqZZYrVjCmj195wr4PdcfXXJ0wsgrxgnJW8%2B5wZkqDd%2F6O7t9qGZMjkYGtqxOcy%2FxWwGKC85guqOep4zp%2BJlAkWQpPBstZhMIKvKv3HdXQ2O4QoXfnvor3J7QBOkJBdSG8qXg","system":true}]
  √  [INFO] Request body:
  √  [INFO] Response headers: [{"key":"Date","value":"Fri, 28 Jun 2024 13:49:09 GMT"},{"key":"Content-Type","value":"application/json; charset=utf-8"},{"key":"Transfer-Encoding","value":"chunked"},{"key":"Connection","value":"keep-alive"}]
  √  [INFO] Response body: [{"productId":1,"productName":"Mouse","quantity":5,"price":100.0}]
  √  HTTPStatus 200
  √  Session cookies set
  √  Product is exists
```

```
→ 05_Установка количества {+2} для продукта {id} на складе
  POST http://arch.homework/warehouse/products/quantity [200 OK, 99B, 39ms]
  √  [INFO] Request headers: [{"key":"Content-Type","value":"application/json","system":true},{"key":"User-Agent","value":"PostmanRuntime/7.39.0","system":true},{"key":"Accept","value":"*/*","system":true},{"key":"Cache-Control","value":"no-cache","system":true},{"key":"Postman-Token","value":"94e59aa9-41f5-4457-82cf-ce85bef204a3","system":true},{"key":"Host","value":"arch.homework","system":true},{"key":"Accept-Encoding","value":"gzip, deflate, br","system":true},{"key":"Connection","value":"keep-alive","system":true},{"key":"Content-Length","value":"40","system":true},{"key":"Cookie","value":"sakurlyk.identity.session=CfDJ8FmpluDZL3ZJsCjAFhdHnf5gBzP2f%2FkExw5WsPqIqZZYrVjCmj195wr4PdcfXXJ0wsgrxgnJW8%2B5wZkqDd%2F6O7t9qGZMjkYGtqxOcy%2FxWwGKC85guqOep4zp%2BJlAkWQpPBstZhMIKvKv3HdXQ2O4QoXfnvor3J7QBOkJBdSG8qXg","system":true}]
  √  [INFO] Request body: {
  "productId": 1,
  "quantity": 7
}
  √  [INFO] Response headers: [{"key":"Date","value":"Fri, 28 Jun 2024 13:49:10 GMT"},{"key":"Content-Length","value":"0"},{"key":"Connection","value":"keep-alive"}]
  √  [INFO] Response body:
  √  HTTPStatus 200
  √  Session cookies set
```

```
→ 06_Установка цены {+25} для продукта {id} на складе
  POST http://arch.homework/warehouse/products/price [200 OK, 99B, 25ms]
  √  [INFO] Request headers: [{"key":"Content-Type","value":"application/json","system":true},{"key":"User-Agent","value":"PostmanRuntime/7.39.0","system":true},{"key":"Accept","value":"*/*","system":true},{"key":"Cache-Control","value":"no-cache","system":true},{"key":"Postman-Token","value":"44490410-7a15-4fc6-a307-13f151907c44","system":true},{"key":"Host","value":"arch.homework","system":true},{"key":"Accept-Encoding","value":"gzip, deflate, br","system":true},{"key":"Connection","value":"keep-alive","system":true},{"key":"Content-Length","value":"39","system":true},{"key":"Cookie","value":"sakurlyk.identity.session=CfDJ8FmpluDZL3ZJsCjAFhdHnf5gBzP2f%2FkExw5WsPqIqZZYrVjCmj195wr4PdcfXXJ0wsgrxgnJW8%2B5wZkqDd%2F6O7t9qGZMjkYGtqxOcy%2FxWwGKC85guqOep4zp%2BJlAkWQpPBstZhMIKvKv3HdXQ2O4QoXfnvor3J7QBOkJBdSG8qXg","system":true}]
  √  [INFO] Request body: {
  "productId": 1,
  "price": 125
}
  √  [INFO] Response headers: [{"key":"Date","value":"Fri, 28 Jun 2024 13:49:11 GMT"},{"key":"Content-Length","value":"0"},{"key":"Connection","value":"keep-alive"}]
  √  [INFO] Response body:
  √  HTTPStatus 200
  √  Session cookies set
```

```
→ 07_Поиск товаров на складе по наименованию
  GET http://arch.homework/warehouse/products/search?term=Mouse [200 OK, 219B, 36ms]
  √  [INFO] Request headers: [{"key":"User-Agent","value":"PostmanRuntime/7.39.0","system":true},{"key":"Accept","value":"*/*","system":true},{"key":"Cache-Control","value":"no-cache","system":true},{"key":"Postman-Token","value":"df72ed4a-3bd2-4a16-b6a0-0250c23c21bc","system":true},{"key":"Host","value":"arch.homework","system":true},{"key":"Accept-Encoding","value":"gzip, deflate, br","system":true},{"key":"Connection","value":"keep-alive","system":true},{"key":"Cookie","value":"sakurlyk.identity.session=CfDJ8FmpluDZL3ZJsCjAFhdHnf5gBzP2f%2FkExw5WsPqIqZZYrVjCmj195wr4PdcfXXJ0wsgrxgnJW8%2B5wZkqDd%2F6O7t9qGZMjkYGtqxOcy%2FxWwGKC85guqOep4zp%2BJlAkWQpPBstZhMIKvKv3HdXQ2O4QoXfnvor3J7QBOkJBdSG8qXg","system":true}]
  √  [INFO] Request body:
  √  [INFO] Response headers: [{"key":"Date","value":"Fri, 28 Jun 2024 13:49:12 GMT"},{"key":"Content-Type","value":"application/json; charset=utf-8"},{"key":"Transfer-Encoding","value":"chunked"},{"key":"Connection","value":"keep-alive"}]
  √  [INFO] Response body: [{"productId":1,"productName":"Mouse","quantity":7,"price":125}]
  √  HTTPStatus 200
  √  Session cookies set
  √  Product is exists
```

```
→ 08_Создать заказ 2x {product_id}, {product_name}
  POST http://arch.homework/orders/create [200 OK, 338B, 298ms]
  √  [INFO] Request headers: [{"key":"X-Request-Id","value":"7d2ad55c-8525-4f2a-8858-634f51c8216f"},{"key":"Content-Type","value":"application/json","system":true},{"key":"User-Agent","value":"PostmanRuntime/7.39.0","system":true},{"key":"Accept","value":"*/*","system":true},{"key":"Cache-Control","value":"no-cache","system":true},{"key":"Postman-Token","value":"6839eb68-a058-4782-ab4d-0d8f9940c05e","system":true},{"key":"Host","value":"arch.homework","system":true},{"key":"Accept-Encoding","value":"gzip, deflate, br","system":true},{"key":"Connection","value":"keep-alive","system":true},{"key":"Content-Length","value":"137","system":true},{"key":"Cookie","value":"sakurlyk.identity.session=CfDJ8FmpluDZL3ZJsCjAFhdHnf5gBzP2f%2FkExw5WsPqIqZZYrVjCmj195wr4PdcfXXJ0wsgrxgnJW8%2B5wZkqDd%2F6O7t9qGZMjkYGtqxOcy%2FxWwGKC85guqOep4zp%2BJlAkWQpPBstZhMIKvKv3HdXQ2O4QoXfnvor3J7QBOkJBdSG8qXg","system":true}]
  √  [INFO] Request body: {
  "positions": [
    {
      "productId": 1,
      "productName": "Mouse",
      "quantity": 2,
      "price": 125
    }
  ]
}
  √  [INFO] Response headers: [{"key":"Date","value":"Fri, 28 Jun 2024 13:49:14 GMT"},{"key":"Content-Type","value":"application/json; charset=utf-8"},{"key":"Transfer-Encoding","value":"chunked"},{"key":"Connection","value":"keep-alive"}]
  √  [INFO] Response body: {"id":1,"status":"Created","totalPrice":250,"positions":[{"productId":1,"productName":"Mouse","quantity":2,"price":125}],"reason":null,"createdAt":"2024-06-28T13:49:14.0670902+00:00"}
  √  HTTPStatus 200
  √  Session cookies set
  √  Order is created
```

```
→ 09_Получение списка заказов (заказ отклонён-недостаточно средств)
  GET http://arch.homework/orders [200 OK, 392B, 37ms]
  √  [INFO] Request headers: [{"key":"User-Agent","value":"PostmanRuntime/7.39.0","system":true},{"key":"Accept","value":"*/*","system":true},{"key":"Cache-Control","value":"no-cache","system":true},{"key":"Postman-Token","value":"74af6d8f-e9c9-4b51-8bde-39f9a65dfa70","system":true},{"key":"Host","value":"arch.homework","system":true},{"key":"Accept-Encoding","value":"gzip, deflate, br","system":true},{"key":"Connection","value":"keep-alive","system":true},{"key":"Cookie","value":"sakurlyk.identity.session=CfDJ8FmpluDZL3ZJsCjAFhdHnf5gBzP2f%2FkExw5WsPqIqZZYrVjCmj195wr4PdcfXXJ0wsgrxgnJW8%2B5wZkqDd%2F6O7t9qGZMjkYGtqxOcy%2FxWwGKC85guqOep4zp%2BJlAkWQpPBstZhMIKvKv3HdXQ2O4QoXfnvor3J7QBOkJBdSG8qXg","system":true}]
  √  [INFO] Request body: undefined
  √  [INFO] Response headers: [{"key":"Date","value":"Fri, 28 Jun 2024 13:49:15 GMT"},{"key":"Content-Type","value":"application/json; charset=utf-8"},{"key":"Transfer-Encoding","value":"chunked"},{"key":"Connection","value":"keep-alive"}]
  √  [INFO] Response body: [{"id":1,"status":"Declined","totalPrice":250,"positions":[{"productId":1,"productName":"Mouse","quantity":2,"price":125}],"reason":"Недостаточно средств на счете","createdAt":"2024-06-28T13:49:14.06709+00:00"}]
  √  HTTPStatus 200
  √  Session cookies set
  √  Order is declined
```

```
→ 10_Получение баланса (=0)
  GET http://arch.homework/billing/balance [200 OK, 168B, 18ms]
  √  [INFO] Request headers: [{"key":"User-Agent","value":"PostmanRuntime/7.39.0","system":true},{"key":"Accept","value":"*/*","system":true},{"key":"Cache-Control","value":"no-cache","system":true},{"key":"Postman-Token","value":"3e544431-f38b-4bc1-b8a4-290c54fcda22","system":true},{"key":"Host","value":"arch.homework","system":true},{"key":"Accept-Encoding","value":"gzip, deflate, br","system":true},{"key":"Connection","value":"keep-alive","system":true},{"key":"Cookie","value":"sakurlyk.identity.session=CfDJ8FmpluDZL3ZJsCjAFhdHnf5gBzP2f%2FkExw5WsPqIqZZYrVjCmj195wr4PdcfXXJ0wsgrxgnJW8%2B5wZkqDd%2F6O7t9qGZMjkYGtqxOcy%2FxWwGKC85guqOep4zp%2BJlAkWQpPBstZhMIKvKv3HdXQ2O4QoXfnvor3J7QBOkJBdSG8qXg","system":true}]
  √  [INFO] Request body: undefined
  √  [INFO] Response headers: [{"key":"Date","value":"Fri, 28 Jun 2024 13:49:16 GMT"},{"key":"Content-Type","value":"application/json; charset=utf-8"},{"key":"Transfer-Encoding","value":"chunked"},{"key":"Connection","value":"keep-alive"}]
  √  [INFO] Response body: {"balance":0}
  √  HTTPStatus 200
  √  Session cookies set
  √  Balance is 0
```

```
→ 11_Пополнить баланс на 400
  POST http://arch.homework/billing/balance/deposit [200 OK, 99B, 20ms]
  √  [INFO] Request headers: [{"key":"Content-Type","value":"application/json","system":true},{"key":"User-Agent","value":"PostmanRuntime/7.39.0","system":true},{"key":"Accept","value":"*/*","system":true},{"key":"Cache-Control","value":"no-cache","system":true},{"key":"Postman-Token","value":"ff2740c2-d7ec-481b-bbc4-bfbcde89e2ef","system":true},{"key":"Host","value":"arch.homework","system":true},{"key":"Accept-Encoding","value":"gzip, deflate, br","system":true},{"key":"Connection","value":"keep-alive","system":true},{"key":"Content-Length","value":"25","system":true},{"key":"Cookie","value":"sakurlyk.identity.session=CfDJ8FmpluDZL3ZJsCjAFhdHnf5gBzP2f%2FkExw5WsPqIqZZYrVjCmj195wr4PdcfXXJ0wsgrxgnJW8%2B5wZkqDd%2F6O7t9qGZMjkYGtqxOcy%2FxWwGKC85guqOep4zp%2BJlAkWQpPBstZhMIKvKv3HdXQ2O4QoXfnvor3J7QBOkJBdSG8qXg","system":true}]
  √  [INFO] Request body: {
  "amount": "400.0"
}
  √  [INFO] Response headers: [{"key":"Date","value":"Fri, 28 Jun 2024 13:49:17 GMT"},{"key":"Content-Length","value":"0"},{"key":"Connection","value":"keep-alive"}]
  √  [INFO] Response body:
  √  HTTPStatus 200
  √  Session cookies set
```

```
→ 12_Получение баланса (=400)
  GET http://arch.homework/billing/balance [200 OK, 172B, 20ms]
  √  [INFO] Request headers: [{"key":"User-Agent","value":"PostmanRuntime/7.39.0","system":true},{"key":"Accept","value":"*/*","system":true},{"key":"Cache-Control","value":"no-cache","system":true},{"key":"Postman-Token","value":"cdc47af1-a79f-4074-9494-b05dba139502","system":true},{"key":"Host","value":"arch.homework","system":true},{"key":"Accept-Encoding","value":"gzip, deflate, br","system":true},{"key":"Connection","value":"keep-alive","system":true},{"key":"Cookie","value":"sakurlyk.identity.session=CfDJ8FmpluDZL3ZJsCjAFhdHnf5gBzP2f%2FkExw5WsPqIqZZYrVjCmj195wr4PdcfXXJ0wsgrxgnJW8%2B5wZkqDd%2F6O7t9qGZMjkYGtqxOcy%2FxWwGKC85guqOep4zp%2BJlAkWQpPBstZhMIKvKv3HdXQ2O4QoXfnvor3J7QBOkJBdSG8qXg","system":true}]
  √  [INFO] Request body: undefined
  √  [INFO] Response headers: [{"key":"Date","value":"Fri, 28 Jun 2024 13:49:18 GMT"},{"key":"Content-Type","value":"application/json; charset=utf-8"},{"key":"Transfer-Encoding","value":"chunked"},{"key":"Connection","value":"keep-alive"}]
  √  [INFO] Response body: {"balance":400.0}
  √  HTTPStatus 200
  √  Session cookies set
  √  Balance is 400
```

```
→ 13_Создать заказ 2x {product_id}, {product_name}
  POST http://arch.homework/orders/create [200 OK, 338B, 30ms]
  √  [INFO] Request headers: [{"key":"X-Request-Id","value":"f33d97ed-7a93-471e-be31-8f77d5c0649a"},{"key":"Content-Type","value":"application/json","system":true},{"key":"User-Agent","value":"PostmanRuntime/7.39.0","system":true},{"key":"Accept","value":"*/*","system":true},{"key":"Cache-Control","value":"no-cache","system":true},{"key":"Postman-Token","value":"36a2e2dd-7b0f-4f2e-b728-fc9047d52100","system":true},{"key":"Host","value":"arch.homework","system":true},{"key":"Accept-Encoding","value":"gzip, deflate, br","system":true},{"key":"Connection","value":"keep-alive","system":true},{"key":"Content-Length","value":"137","system":true},{"key":"Cookie","value":"sakurlyk.identity.session=CfDJ8FmpluDZL3ZJsCjAFhdHnf5gBzP2f%2FkExw5WsPqIqZZYrVjCmj195wr4PdcfXXJ0wsgrxgnJW8%2B5wZkqDd%2F6O7t9qGZMjkYGtqxOcy%2FxWwGKC85guqOep4zp%2BJlAkWQpPBstZhMIKvKv3HdXQ2O4QoXfnvor3J7QBOkJBdSG8qXg","system":true}]
  √  [INFO] Request body: {
  "positions": [
    {
      "productId": 1,
      "productName": "Mouse",
      "quantity": 2,
      "price": 125
    }
  ]
}
  √  [INFO] Response headers: [{"key":"Date","value":"Fri, 28 Jun 2024 13:49:20 GMT"},{"key":"Content-Type","value":"application/json; charset=utf-8"},{"key":"Transfer-Encoding","value":"chunked"},{"key":"Connection","value":"keep-alive"}]
  √  [INFO] Response body: {"id":2,"status":"Created","totalPrice":250,"positions":[{"productId":1,"productName":"Mouse","quantity":2,"price":125}],"reason":null,"createdAt":"2024-06-28T13:49:20.0033172+00:00"}
  √  HTTPStatus 200
  √  Session cookies set
  √  Order is created
```

```
→ 14_Получение баланса
  GET http://arch.homework/billing/balance [200 OK, 172B, 8ms]
  √  [INFO] Request headers: [{"key":"User-Agent","value":"PostmanRuntime/7.39.0","system":true},{"key":"Accept","value":"*/*","system":true},{"key":"Cache-Control","value":"no-cache","system":true},{"key":"Postman-Token","value":"d16bf21c-e068-4da9-87f5-38946eab5bd6","system":true},{"key":"Host","value":"arch.homework","system":true},{"key":"Accept-Encoding","value":"gzip, deflate, br","system":true},{"key":"Connection","value":"keep-alive","system":true},{"key":"Cookie","value":"sakurlyk.identity.session=CfDJ8FmpluDZL3ZJsCjAFhdHnf5gBzP2f%2FkExw5WsPqIqZZYrVjCmj195wr4PdcfXXJ0wsgrxgnJW8%2B5wZkqDd%2F6O7t9qGZMjkYGtqxOcy%2FxWwGKC85guqOep4zp%2BJlAkWQpPBstZhMIKvKv3HdXQ2O4QoXfnvor3J7QBOkJBdSG8qXg","system":true}]
  √  [INFO] Request body: undefined
  √  [INFO] Response headers: [{"key":"Date","value":"Fri, 28 Jun 2024 13:49:21 GMT"},{"key":"Content-Type","value":"application/json; charset=utf-8"},{"key":"Transfer-Encoding","value":"chunked"},{"key":"Connection","value":"keep-alive"}]
  √  [INFO] Response body: {"balance":150.0}
  √  HTTPStatus 200
  √  Session cookies set
  √  Balance decreased
```

```
→ 15_Получение списка заказов (заказ подтверждён)
  GET http://arch.homework/orders [200 OK, 577B, 7ms]
  √  [INFO] Request headers: [{"key":"User-Agent","value":"PostmanRuntime/7.39.0","system":true},{"key":"Accept","value":"*/*","system":true},{"key":"Cache-Control","value":"no-cache","system":true},{"key":"Postman-Token","value":"5fce36a1-ba85-48b9-b46b-0d42070c356b","system":true},{"key":"Host","value":"arch.homework","system":true},{"key":"Accept-Encoding","value":"gzip, deflate, br","system":true},{"key":"Connection","value":"keep-alive","system":true},{"key":"Cookie","value":"sakurlyk.identity.session=CfDJ8FmpluDZL3ZJsCjAFhdHnf5gBzP2f%2FkExw5WsPqIqZZYrVjCmj195wr4PdcfXXJ0wsgrxgnJW8%2B5wZkqDd%2F6O7t9qGZMjkYGtqxOcy%2FxWwGKC85guqOep4zp%2BJlAkWQpPBstZhMIKvKv3HdXQ2O4QoXfnvor3J7QBOkJBdSG8qXg","system":true}]
  √  [INFO] Request body: undefined
  √  [INFO] Response headers: [{"key":"Date","value":"Fri, 28 Jun 2024 13:49:22 GMT"},{"key":"Content-Type","value":"application/json; charset=utf-8"},{"key":"Transfer-Encoding","value":"chunked"},{"key":"Connection","value":"keep-alive"}]
  √  [INFO] Response body: [{"id":2,"status":"Confirmed","totalPrice":250,"positions":[{"productId":1,"productName":"Mouse","quantity":2,"price":125}],"reason":null,"createdAt":"2024-06-28T13:49:20.003317+00:00"},{"id":1,"status":"Declined","totalPrice":250,"positions":[{"productId":1,"productName":"Mouse","quantity":2,"price":125}],"reason":"Недостаточно средств на счете","createdAt":"2024-06-28T13:49:14.06709+00:00"}]
  √  HTTPStatus 200
  √  Session cookies set
  √  Order is confirmed
```

```
→ 16_Получение статусов доставки (зарезервирована)
  GET http://arch.homework/delivery [200 OK, 244B, 33ms]
  √  [INFO] Request headers: [{"key":"User-Agent","value":"PostmanRuntime/7.39.0","system":true},{"key":"Accept","value":"*/*","system":true},{"key":"Cache-Control","value":"no-cache","system":true},{"key":"Postman-Token","value":"556bf329-dcea-4995-a08a-61fa7a3df858","system":true},{"key":"Host","value":"arch.homework","system":true},{"key":"Accept-Encoding","value":"gzip, deflate, br","system":true},{"key":"Connection","value":"keep-alive","system":true},{"key":"Cookie","value":"sakurlyk.identity.session=CfDJ8FmpluDZL3ZJsCjAFhdHnf5gBzP2f%2FkExw5WsPqIqZZYrVjCmj195wr4PdcfXXJ0wsgrxgnJW8%2B5wZkqDd%2F6O7t9qGZMjkYGtqxOcy%2FxWwGKC85guqOep4zp%2BJlAkWQpPBstZhMIKvKv3HdXQ2O4QoXfnvor3J7QBOkJBdSG8qXg","system":true}]
  √  [INFO] Request body: undefined
  √  [INFO] Response headers: [{"key":"Date","value":"Fri, 28 Jun 2024 13:49:23 GMT"},{"key":"Content-Type","value":"application/json; charset=utf-8"},{"key":"Transfer-Encoding","value":"chunked"},{"key":"Connection","value":"keep-alive"}]
  √  [INFO] Response body: [{"id":1,"orderId":2,"status":"Reserved","createdAt":"2024-06-28T13:49:20.568276+00:00"}]
  √  HTTPStatus 200
  √  Session cookies set
  √  Delivery is reserved
```

```
→ 17_Получение списка отправленных нотификаций (поздравление)
  GET http://arch.homework/notify/emails [200 OK, 467B, 28ms]
  √  [INFO] Request headers: [{"key":"User-Agent","value":"PostmanRuntime/7.39.0","system":true},{"key":"Accept","value":"*/*","system":true},{"key":"Cache-Control","value":"no-cache","system":true},{"key":"Postman-Token","value":"5f107449-160d-4e72-8c38-72dfcf01e915","system":true},{"key":"Host","value":"arch.homework","system":true},{"key":"Accept-Encoding","value":"gzip, deflate, br","system":true},{"key":"Connection","value":"keep-alive","system":true},{"key":"Cookie","value":"sakurlyk.identity.session=CfDJ8FmpluDZL3ZJsCjAFhdHnf5gBzP2f%2FkExw5WsPqIqZZYrVjCmj195wr4PdcfXXJ0wsgrxgnJW8%2B5wZkqDd%2F6O7t9qGZMjkYGtqxOcy%2FxWwGKC85guqOep4zp%2BJlAkWQpPBstZhMIKvKv3HdXQ2O4QoXfnvor3J7QBOkJBdSG8qXg","system":true}]
  √  [INFO] Request body: undefined
  √  [INFO] Response headers: [{"key":"Date","value":"Fri, 28 Jun 2024 13:49:24 GMT"},{"key":"Content-Type","value":"application/json; charset=utf-8"},{"key":"Transfer-Encoding","value":"chunked"},{"key":"Connection","value":"keep-alive"}]
  √  [INFO] Response body: [{"userId":1,"message":"Congratulations! Order '2' is confirmed for user '1'","createdAt":"2024-06-28T13:49:20.782143+00:00"},{"userId":1,"message":"Sorry. Order '1' is not confirmed for user '1'. Reason: 'Недостаточно средств на счете'","createdAt":"2024-06-28T13:49:14.887439+00:00"}]
  √  HTTPStatus 200
  √  Session cookies set
  √  Last notification with Congratulations
```

```
→ 18_Получение резервов склада (товары зарезервированы)
  GET http://arch.homework/warehouse/reservations [200 OK, 271B, 15ms]
  √  [INFO] Request headers: [{"key":"User-Agent","value":"PostmanRuntime/7.39.0","system":true},{"key":"Accept","value":"*/*","system":true},{"key":"Cache-Control","value":"no-cache","system":true},{"key":"Postman-Token","value":"5c58c7a7-f5d4-4d05-8de4-d5a79f950b41","system":true},{"key":"Host","value":"arch.homework","system":true},{"key":"Accept-Encoding","value":"gzip, deflate, br","system":true},{"key":"Connection","value":"keep-alive","system":true},{"key":"Cookie","value":"sakurlyk.identity.session=CfDJ8FmpluDZL3ZJsCjAFhdHnf5gBzP2f%2FkExw5WsPqIqZZYrVjCmj195wr4PdcfXXJ0wsgrxgnJW8%2B5wZkqDd%2F6O7t9qGZMjkYGtqxOcy%2FxWwGKC85guqOep4zp%2BJlAkWQpPBstZhMIKvKv3HdXQ2O4QoXfnvor3J7QBOkJBdSG8qXg","system":true}]
  √  [INFO] Request body:
  √  [INFO] Response headers: [{"key":"Date","value":"Fri, 28 Jun 2024 13:49:25 GMT"},{"key":"Content-Type","value":"application/json; charset=utf-8"},{"key":"Transfer-Encoding","value":"chunked"},{"key":"Connection","value":"keep-alive"}]
  √  [INFO] Response body: [{"id":1,"orderId":2,"productId":1,"quantity":2,"status":"Reserved","createdAt":"2024-06-28T13:49:20.218865+00:00"}]
  √  HTTPStatus 200
  √  Session cookies set
  ┌
  │ `Using "_" is deprecated. Use "require('lodash')" instead.`
  └
  √  Warehouse products is reserved
```

```
→ 19_Заказ {id} по резерву отгрузить
  POST http://arch.homework/warehouse/reservations/handover [200 OK, 99B, 21ms]
  √  [INFO] Request headers: [{"key":"Content-Type","value":"application/json","system":true},{"key":"User-Agent","value":"PostmanRuntime/7.39.0","system":true},{"key":"Accept","value":"*/*","system":true},{"key":"Cache-Control","value":"no-cache","system":true},{"key":"Postman-Token","value":"97dd62ad-9d16-4d91-9d93-a7175d3e8e2f","system":true},{"key":"Host","value":"arch.homework","system":true},{"key":"Accept-Encoding","value":"gzip, deflate, br","system":true},{"key":"Connection","value":"keep-alive","system":true},{"key":"Content-Length","value":"20","system":true},{"key":"Cookie","value":"sakurlyk.identity.session=CfDJ8FmpluDZL3ZJsCjAFhdHnf5gBzP2f%2FkExw5WsPqIqZZYrVjCmj195wr4PdcfXXJ0wsgrxgnJW8%2B5wZkqDd%2F6O7t9qGZMjkYGtqxOcy%2FxWwGKC85guqOep4zp%2BJlAkWQpPBstZhMIKvKv3HdXQ2O4QoXfnvor3J7QBOkJBdSG8qXg","system":true}]
  √  [INFO] Request body: {
  "orderId": 2
}
  √  [INFO] Response headers: [{"key":"Date","value":"Fri, 28 Jun 2024 13:49:26 GMT"},{"key":"Content-Length","value":"0"},{"key":"Connection","value":"keep-alive"}]
  √  [INFO] Response body:
  √  HTTPStatus 200
  √  Session cookies set
```

```
→ 20_Получение резервов склада (товары отгружены)
  GET http://arch.homework/warehouse/reservations [200 OK, 273B, 7ms]
  √  [INFO] Request headers: [{"key":"User-Agent","value":"PostmanRuntime/7.39.0","system":true},{"key":"Accept","value":"*/*","system":true},{"key":"Cache-Control","value":"no-cache","system":true},{"key":"Postman-Token","value":"c52d7aa5-4b1d-47c7-bdfd-d4b7b2d4c438","system":true},{"key":"Host","value":"arch.homework","system":true},{"key":"Accept-Encoding","value":"gzip, deflate, br","system":true},{"key":"Connection","value":"keep-alive","system":true},{"key":"Cookie","value":"sakurlyk.identity.session=CfDJ8FmpluDZL3ZJsCjAFhdHnf5gBzP2f%2FkExw5WsPqIqZZYrVjCmj195wr4PdcfXXJ0wsgrxgnJW8%2B5wZkqDd%2F6O7t9qGZMjkYGtqxOcy%2FxWwGKC85guqOep4zp%2BJlAkWQpPBstZhMIKvKv3HdXQ2O4QoXfnvor3J7QBOkJBdSG8qXg","system":true}]
  √  [INFO] Request body:
  √  [INFO] Response headers: [{"key":"Date","value":"Fri, 28 Jun 2024 13:49:27 GMT"},{"key":"Content-Type","value":"application/json; charset=utf-8"},{"key":"Transfer-Encoding","value":"chunked"},{"key":"Connection","value":"keep-alive"}]
  √  [INFO] Response body: [{"id":1,"orderId":2,"productId":1,"quantity":2,"status":"Handovered","createdAt":"2024-06-28T13:49:20.218865+00:00"}]
  √  HTTPStatus 200
  √  Session cookies set
  ┌
  │ `Using "_" is deprecated. Use "require('lodash')" instead.`
  └
  √  Warehouse products is handovered
```

```
→ 21_Подтвердить, что служба доставки доставила заказ {id}
  POST http://arch.homework/delivery/set-deliveried [200 OK, 99B, 44ms]
  √  [INFO] Request headers: [{"key":"Content-Type","value":"application/json","system":true},{"key":"User-Agent","value":"PostmanRuntime/7.39.0","system":true},{"key":"Accept","value":"*/*","system":true},{"key":"Cache-Control","value":"no-cache","system":true},{"key":"Postman-Token","value":"12dba6df-aa71-466b-b6f6-f690d77a1a84","system":true},{"key":"Host","value":"arch.homework","system":true},{"key":"Accept-Encoding","value":"gzip, deflate, br","system":true},{"key":"Connection","value":"keep-alive","system":true},{"key":"Content-Length","value":"20","system":true},{"key":"Cookie","value":"sakurlyk.identity.session=CfDJ8FmpluDZL3ZJsCjAFhdHnf5gBzP2f%2FkExw5WsPqIqZZYrVjCmj195wr4PdcfXXJ0wsgrxgnJW8%2B5wZkqDd%2F6O7t9qGZMjkYGtqxOcy%2FxWwGKC85guqOep4zp%2BJlAkWQpPBstZhMIKvKv3HdXQ2O4QoXfnvor3J7QBOkJBdSG8qXg","system":true}]
  √  [INFO] Request body: {
  "orderId": 2
}
  √  [INFO] Response headers: [{"key":"Date","value":"Fri, 28 Jun 2024 13:49:29 GMT"},{"key":"Content-Length","value":"0"},{"key":"Connection","value":"keep-alive"}]
  √  [INFO] Response body:
  √  HTTPStatus 200
  √  Session cookies set
```

```
→ 22_Получение статусов доставки (доставлено)
  GET http://arch.homework/delivery [200 OK, 246B, 17ms]
  √  [INFO] Request headers: [{"key":"User-Agent","value":"PostmanRuntime/7.39.0","system":true},{"key":"Accept","value":"*/*","system":true},{"key":"Cache-Control","value":"no-cache","system":true},{"key":"Postman-Token","value":"9238d135-0c61-4109-a61f-4c27a0cff419","system":true},{"key":"Host","value":"arch.homework","system":true},{"key":"Accept-Encoding","value":"gzip, deflate, br","system":true},{"key":"Connection","value":"keep-alive","system":true},{"key":"Cookie","value":"sakurlyk.identity.session=CfDJ8FmpluDZL3ZJsCjAFhdHnf5gBzP2f%2FkExw5WsPqIqZZYrVjCmj195wr4PdcfXXJ0wsgrxgnJW8%2B5wZkqDd%2F6O7t9qGZMjkYGtqxOcy%2FxWwGKC85guqOep4zp%2BJlAkWQpPBstZhMIKvKv3HdXQ2O4QoXfnvor3J7QBOkJBdSG8qXg","system":true}]
  √  [INFO] Request body: undefined
  √  [INFO] Response headers: [{"key":"Date","value":"Fri, 28 Jun 2024 13:49:30 GMT"},{"key":"Content-Type","value":"application/json; charset=utf-8"},{"key":"Transfer-Encoding","value":"chunked"},{"key":"Connection","value":"keep-alive"}]
  √  [INFO] Response body: [{"id":1,"orderId":2,"status":"Deliveried","createdAt":"2024-06-28T13:49:20.568276+00:00"}]
  √  HTTPStatus 200
  √  Session cookies set
  √  Delivery is deliveried
```

```
→ 23_Получение списка заказов (заказ завершен)
  GET http://arch.homework/orders [200 OK, 577B, 15ms]
  √  [INFO] Request headers: [{"key":"User-Agent","value":"PostmanRuntime/7.39.0","system":true},{"key":"Accept","value":"*/*","system":true},{"key":"Cache-Control","value":"no-cache","system":true},{"key":"Postman-Token","value":"2fa68826-4e34-484f-b03e-6cbf881e935c","system":true},{"key":"Host","value":"arch.homework","system":true},{"key":"Accept-Encoding","value":"gzip, deflate, br","system":true},{"key":"Connection","value":"keep-alive","system":true},{"key":"Cookie","value":"sakurlyk.identity.session=CfDJ8FmpluDZL3ZJsCjAFhdHnf5gBzP2f%2FkExw5WsPqIqZZYrVjCmj195wr4PdcfXXJ0wsgrxgnJW8%2B5wZkqDd%2F6O7t9qGZMjkYGtqxOcy%2FxWwGKC85guqOep4zp%2BJlAkWQpPBstZhMIKvKv3HdXQ2O4QoXfnvor3J7QBOkJBdSG8qXg","system":true}]
  √  [INFO] Request body: undefined
  √  [INFO] Response headers: [{"key":"Date","value":"Fri, 28 Jun 2024 13:49:31 GMT"},{"key":"Content-Type","value":"application/json; charset=utf-8"},{"key":"Transfer-Encoding","value":"chunked"},{"key":"Connection","value":"keep-alive"}]
  √  [INFO] Response body: [{"id":2,"status":"Completed","totalPrice":250,"positions":[{"productId":1,"productName":"Mouse","quantity":2,"price":125}],"reason":null,"createdAt":"2024-06-28T13:49:20.003317+00:00"},{"id":1,"status":"Declined","totalPrice":250,"positions":[{"productId":1,"productName":"Mouse","quantity":2,"price":125}],"reason":"Недостаточно средств на счете","createdAt":"2024-06-28T13:49:14.06709+00:00"}]
  √  HTTPStatus 200
  √  Session cookies set
  √  Order is completed
```

```
→ 24_Создать заказ 2x {product_id}, {product_name} (несуществующий товар)
  POST http://arch.homework/orders/create [200 OK, 414B, 26ms]
  √  [INFO] Request headers: [{"key":"X-Request-Id","value":"a0c65bd8-f42f-4209-ba0d-c5a5376b8ad3"},{"key":"Content-Type","value":"application/json","system":true},{"key":"User-Agent","value":"PostmanRuntime/7.39.0","system":true},{"key":"Accept","value":"*/*","system":true},{"key":"Cache-Control","value":"no-cache","system":true},{"key":"Postman-Token","value":"d2328ad4-bc59-41fc-8478-b6cb501200ad","system":true},{"key":"Host","value":"arch.homework","system":true},{"key":"Accept-Encoding","value":"gzip, deflate, br","system":true},{"key":"Connection","value":"keep-alive","system":true},{"key":"Content-Length","value":"266","system":true},{"key":"Cookie","value":"sakurlyk.identity.session=CfDJ8FmpluDZL3ZJsCjAFhdHnf5gBzP2f%2FkExw5WsPqIqZZYrVjCmj195wr4PdcfXXJ0wsgrxgnJW8%2B5wZkqDd%2F6O7t9qGZMjkYGtqxOcy%2FxWwGKC85guqOep4zp%2BJlAkWQpPBstZhMIKvKv3HdXQ2O4QoXfnvor3J7QBOkJBdSG8qXg","system":true}]
  √  [INFO] Request body: {
  "positions": [
    {
      "productId": 0,
      "productName": "some_product",
      "quantity": 2,
      "price": 1.0
    },
    {
      "productId": 0,
      "productName": "some_product",
      "quantity": 1,
      "price": 1.0
    }
  ]
}
  √  [INFO] Response headers: [{"key":"Date","value":"Fri, 28 Jun 2024 13:49:32 GMT"},{"key":"Content-Type","value":"application/json; charset=utf-8"},{"key":"Transfer-Encoding","value":"chunked"},{"key":"Connection","value":"keep-alive"}]
  √  [INFO] Response body: {"id":3,"status":"Created","totalPrice":3.0,"positions":[{"productId":0,"productName":"some_product","quantity":2,"price":1.0},{"productId":0,"productName":"some_product","quantity":1,"price":1.0}],"reason":null,"createdAt":"2024-06-28T13:49:32.400863+00:00"}
  √  HTTPStatus 200
  √  Session cookies set
  √  Order is created
```

```
→ 24_Создать заказ 2x {product_id}, {product_name} Dublicate X-Request-Id
  POST http://arch.homework/orders/create [409 Conflict, 288B, 8ms]
  √  [INFO] Request headers: [{"key":"X-Request-Id","value":"a0c65bd8-f42f-4209-ba0d-c5a5376b8ad3"},{"key":"Content-Type","value":"application/json","system":true},{"key":"User-Agent","value":"PostmanRuntime/7.39.0","system":true},{"key":"Accept","value":"*/*","system":true},{"key":"Cache-Control","value":"no-cache","system":true},{"key":"Postman-Token","value":"c3316e3e-7bc7-46d2-9763-cdbed2a8a818","system":true},{"key":"Host","value":"arch.homework","system":true},{"key":"Accept-Encoding","value":"gzip, deflate, br","system":true},{"key":"Connection","value":"keep-alive","system":true},{"key":"Content-Length","value":"266","system":true},{"key":"Cookie","value":"sakurlyk.identity.session=CfDJ8FmpluDZL3ZJsCjAFhdHnf5gBzP2f%2FkExw5WsPqIqZZYrVjCmj195wr4PdcfXXJ0wsgrxgnJW8%2B5wZkqDd%2F6O7t9qGZMjkYGtqxOcy%2FxWwGKC85guqOep4zp%2BJlAkWQpPBstZhMIKvKv3HdXQ2O4QoXfnvor3J7QBOkJBdSG8qXg","system":true}]
  √  [INFO] Request body: {
  "positions": [
    {
      "productId": 0,
      "productName": "some_product",
      "quantity": 2,
      "price": 1.0
    },
    {
      "productId": 0,
      "productName": "some_product",
      "quantity": 1,
      "price": 1.0
    }
  ]
}
  √  [INFO] Response headers: [{"key":"Date","value":"Fri, 28 Jun 2024 13:49:33 GMT"},{"key":"Content-Type","value":"application/json; charset=utf-8"},{"key":"Transfer-Encoding","value":"chunked"},{"key":"Connection","value":"keep-alive"}]
  √  [INFO] Response body: {"message":"Запрос с id 'a0c65bd8-f42f-4209-ba0d-c5a5376b8ad3' уже существует","details":"IdempotentError"}
  √  HTTPStatus 409
  √  Session cookies set
```

```
→ 25_Получение списка заказов (смотрим reason = несуществующий)
  GET http://arch.homework/orders [200 OK, 924B, 6ms]
  √  [INFO] Request headers: [{"key":"User-Agent","value":"PostmanRuntime/7.39.0","system":true},{"key":"Accept","value":"*/*","system":true},{"key":"Cache-Control","value":"no-cache","system":true},{"key":"Postman-Token","value":"bf7c45b5-0ebd-4a86-8710-6087b50272fb","system":true},{"key":"Host","value":"arch.homework","system":true},{"key":"Accept-Encoding","value":"gzip, deflate, br","system":true},{"key":"Connection","value":"keep-alive","system":true},{"key":"Cookie","value":"sakurlyk.identity.session=CfDJ8FmpluDZL3ZJsCjAFhdHnf5gBzP2f%2FkExw5WsPqIqZZYrVjCmj195wr4PdcfXXJ0wsgrxgnJW8%2B5wZkqDd%2F6O7t9qGZMjkYGtqxOcy%2FxWwGKC85guqOep4zp%2BJlAkWQpPBstZhMIKvKv3HdXQ2O4QoXfnvor3J7QBOkJBdSG8qXg","system":true}]
  √  [INFO] Request body: undefined
  √  [INFO] Response headers: [{"key":"Date","value":"Fri, 28 Jun 2024 13:49:34 GMT"},{"key":"Content-Type","value":"application/json; charset=utf-8"},{"key":"Transfer-Encoding","value":"chunked"},{"key":"Connection","value":"keep-alive"}]
  √  [INFO] Response body: [{"id":3,"status":"Declined","totalPrice":3.0,"positions":[{"productId":0,"productName":"some_product","quantity":2,"price":1.0},{"productId":0,"productName":"some_product","quantity":1,"price":1.0}],"reason":"Один из товар отсутствует в номенклатуре склада","createdAt":"2024-06-28T13:49:32.400863+00:00"},{"id":2,"status":"Completed","totalPrice":250,"positions":[{"productId":1,"productName":"Mouse","quantity":2,"price":125}],"reason":null,"createdAt":"2024-06-28T13:49:20.003317+00:00"},{"id":1,"status":"Declined","totalPrice":250,"positions":[{"productId":1,"productName":"Mouse","quantity":2,"price":125}],"reason":"Недостаточно средств на счете","createdAt":"2024-06-28T13:49:14.06709+00:00"}]
  √  HTTPStatus 200
  √  Session cookies set
  √  Order is declined
```

```
→ 26_Получение списка отправленных нотификаций (сожаление)
  GET http://arch.homework/notify/emails [200 OK, 686B, 8ms]
  √  [INFO] Request headers: [{"key":"User-Agent","value":"PostmanRuntime/7.39.0","system":true},{"key":"Accept","value":"*/*","system":true},{"key":"Cache-Control","value":"no-cache","system":true},{"key":"Postman-Token","value":"e0128ea1-5d31-4e2d-8f47-a56248436f4b","system":true},{"key":"Host","value":"arch.homework","system":true},{"key":"Accept-Encoding","value":"gzip, deflate, br","system":true},{"key":"Connection","value":"keep-alive","system":true},{"key":"Cookie","value":"sakurlyk.identity.session=CfDJ8FmpluDZL3ZJsCjAFhdHnf5gBzP2f%2FkExw5WsPqIqZZYrVjCmj195wr4PdcfXXJ0wsgrxgnJW8%2B5wZkqDd%2F6O7t9qGZMjkYGtqxOcy%2FxWwGKC85guqOep4zp%2BJlAkWQpPBstZhMIKvKv3HdXQ2O4QoXfnvor3J7QBOkJBdSG8qXg","system":true}]
  √  [INFO] Request body: undefined
  √  [INFO] Response headers: [{"key":"Date","value":"Fri, 28 Jun 2024 13:49:35 GMT"},{"key":"Content-Type","value":"application/json; charset=utf-8"},{"key":"Transfer-Encoding","value":"chunked"},{"key":"Connection","value":"keep-alive"}]
  √  [INFO] Response body: [{"userId":1,"message":"Sorry. Order '3' is not confirmed for user '1'. Reason: 'Один из товар отсутствует в номенклатуре склада'","createdAt":"2024-06-28T13:49:32.490511+00:00"},{"userId":1,"message":"Congratulations! Order '2' is confirmed for user '1'","createdAt":"2024-06-28T13:49:20.782143+00:00"},{"userId":1,"message":"Sorry. Order '1' is not confirmed for user '1'. Reason: 'Недостаточно средств на счете'","createdAt":"2024-06-28T13:49:14.887439+00:00"}]
  √  HTTPStatus 200
  √  Session cookies set
  √  Last notification with Sorry
```

```
→ 27_Создать заказ 2x {product_id}, {product_name} (несуществующий товар) Copy
  POST http://arch.homework/orders/create [200 OK, 336B, 13ms]
  √  [INFO] Request headers: [{"key":"X-Request-Id","value":"0b0e5d32-785b-4fa5-8213-1aabeb91249f"},{"key":"Content-Type","value":"application/json","system":true},{"key":"User-Agent","value":"PostmanRuntime/7.39.0","system":true},{"key":"Accept","value":"*/*","system":true},{"key":"Cache-Control","value":"no-cache","system":true},{"key":"Postman-Token","value":"42122961-5f2a-4a31-bdd3-b38af5442fe1","system":true},{"key":"Host","value":"arch.homework","system":true},{"key":"Accept-Encoding","value":"gzip, deflate, br","system":true},{"key":"Connection","value":"keep-alive","system":true},{"key":"Content-Length","value":"136","system":true},{"key":"Cookie","value":"sakurlyk.identity.session=CfDJ8FmpluDZL3ZJsCjAFhdHnf5gBzP2f%2FkExw5WsPqIqZZYrVjCmj195wr4PdcfXXJ0wsgrxgnJW8%2B5wZkqDd%2F6O7t9qGZMjkYGtqxOcy%2FxWwGKC85guqOep4zp%2BJlAkWQpPBstZhMIKvKv3HdXQ2O4QoXfnvor3J7QBOkJBdSG8qXg","system":true}]
  √  [INFO] Request body: {
  "positions": [
    {
      "productId": 1,
      "productName": "Mouse",
      "quantity": 20,
      "price": 1
    }
  ]
}
  √  [INFO] Response headers: [{"key":"Date","value":"Fri, 28 Jun 2024 13:49:36 GMT"},{"key":"Content-Type","value":"application/json; charset=utf-8"},{"key":"Transfer-Encoding","value":"chunked"},{"key":"Connection","value":"keep-alive"}]
  √  [INFO] Response body: {"id":4,"status":"Created","totalPrice":20,"positions":[{"productId":1,"productName":"Mouse","quantity":20,"price":1}],"reason":null,"createdAt":"2024-06-28T13:49:36.8839138+00:00"}
  √  HTTPStatus 200
  √  Session cookies set
  √  Order is created
```

```
→ 28_Получение списка заказов (смотрим reason = нет такого кол-ва на складе)
  GET http://arch.homework/orders [200 OK, 1.21kB, 7ms]
  √  [INFO] Request headers: [{"key":"User-Agent","value":"PostmanRuntime/7.39.0","system":true},{"key":"Accept","value":"*/*","system":true},{"key":"Cache-Control","value":"no-cache","system":true},{"key":"Postman-Token","value":"665c279b-0c63-41db-b588-81ea0d9cea5d","system":true},{"key":"Host","value":"arch.homework","system":true},{"key":"Accept-Encoding","value":"gzip, deflate, br","system":true},{"key":"Connection","value":"keep-alive","system":true},{"key":"Cookie","value":"sakurlyk.identity.session=CfDJ8FmpluDZL3ZJsCjAFhdHnf5gBzP2f%2FkExw5WsPqIqZZYrVjCmj195wr4PdcfXXJ0wsgrxgnJW8%2B5wZkqDd%2F6O7t9qGZMjkYGtqxOcy%2FxWwGKC85guqOep4zp%2BJlAkWQpPBstZhMIKvKv3HdXQ2O4QoXfnvor3J7QBOkJBdSG8qXg","system":true}]
  √  [INFO] Request body: undefined
  √  [INFO] Response headers: [{"key":"Date","value":"Fri, 28 Jun 2024 13:49:38 GMT"},{"key":"Content-Type","value":"application/json; charset=utf-8"},{"key":"Transfer-Encoding","value":"chunked"},{"key":"Connection","value":"keep-alive"}]
  √  [INFO] Response body: [{"id":4,"status":"Declined","totalPrice":20,"positions":[{"productId":1,"productName":"Mouse","quantity":20,"price":1}],"reason":"Недостаточное количество товара 'Mouse' на складе. Id товара '1'","createdAt":"2024-06-28T13:49:36.883913+00:00"},{"id":3,"status":"Declined","totalPrice":3.0,"positions":[{"productId":0,"productName":"some_product","quantity":2,"price":1.0},{"productId":0,"productName":"some_product","quantity":1,"price":1.0}],"reason":"Один из товар отсутствует в номенклатуре склада","createdAt":"2024-06-28T13:49:32.400863+00:00"},{"id":2,"status":"Completed","totalPrice":250,"positions":[{"productId":1,"productName":"Mouse","quantity":2,"price":125}],"reason":null,"createdAt":"2024-06-28T13:49:20.003317+00:00"},{"id":1,"status":"Declined","totalPrice":250,"positions":[{"productId":1,"productName":"Mouse","quantity":2,"price":125}],"reason":"Недостаточно средств на счете","createdAt":"2024-06-28T13:49:14.06709+00:00"}]
  √  HTTPStatus 200
  √  Session cookies set
  √  Order is declined
```

```
→ 29_Выход пользователя
  POST http://arch.homework/identity/users/logout [200 OK, 99B, 5ms]
  √  [INFO] Request headers: [{"key":"User-Agent","value":"PostmanRuntime/7.39.0","system":true},{"key":"Accept","value":"*/*","system":true},{"key":"Cache-Control","value":"no-cache","system":true},{"key":"Postman-Token","value":"89cce50a-1544-4b63-afb5-d7703ab789db","system":true},{"key":"Host","value":"arch.homework","system":true},{"key":"Accept-Encoding","value":"gzip, deflate, br","system":true},{"key":"Connection","value":"keep-alive","system":true},{"key":"Cookie","value":"sakurlyk.identity.session=CfDJ8FmpluDZL3ZJsCjAFhdHnf5gBzP2f%2FkExw5WsPqIqZZYrVjCmj195wr4PdcfXXJ0wsgrxgnJW8%2B5wZkqDd%2F6O7t9qGZMjkYGtqxOcy%2FxWwGKC85guqOep4zp%2BJlAkWQpPBstZhMIKvKv3HdXQ2O4QoXfnvor3J7QBOkJBdSG8qXg","system":true},{"key":"Content-Length","value":"0","system":true}]
  √  [INFO] Request body:
  √  [INFO] Response headers: [{"key":"Date","value":"Fri, 28 Jun 2024 13:49:39 GMT"},{"key":"Content-Length","value":"0"},{"key":"Connection","value":"keep-alive"}]
  √  [INFO] Response body:
  √  HTTPStatus 200
  √  Session cookies set
```

```
→ 30_Создать заказ 2x {product_id}, {product_name} (HTTP 401)
  POST http://arch.homework/orders/create [401 Unauthorized, 308B, 5ms]
  √  [INFO] Request headers: [{"key":"X-Request-Id","value":"ac38ec8d-3176-4b5b-9f40-fb6f3ccf3412"},{"key":"Content-Type","value":"application/json","system":true},{"key":"User-Agent","value":"PostmanRuntime/7.39.0","system":true},{"key":"Accept","value":"*/*","system":true},{"key":"Cache-Control","value":"no-cache","system":true},{"key":"Postman-Token","value":"3a99a7e2-3f4f-4dd5-b873-e602ed4d6fc6","system":true},{"key":"Host","value":"arch.homework","system":true},{"key":"Accept-Encoding","value":"gzip, deflate, br","system":true},{"key":"Connection","value":"keep-alive","system":true},{"key":"Content-Length","value":"270","system":true},{"key":"Cookie","value":"sakurlyk.identity.session=CfDJ8FmpluDZL3ZJsCjAFhdHnf5gBzP2f%2FkExw5WsPqIqZZYrVjCmj195wr4PdcfXXJ0wsgrxgnJW8%2B5wZkqDd%2F6O7t9qGZMjkYGtqxOcy%2FxWwGKC85guqOep4zp%2BJlAkWQpPBstZhMIKvKv3HdXQ2O4QoXfnvor3J7QBOkJBdSG8qXg","system":true}]
  √  [INFO] Request body: {
  "positions": [
    {
      "productId": 0,
      "productName": "some_product",
      "quantity": 2,
      "price": 100.0
    },
    {
      "productId": 0,
      "productName": "some_product",
      "quantity": 1,
      "price": 100.0
    }
  ]
}
  √  [INFO] Response headers: [{"key":"Date","value":"Fri, 28 Jun 2024 13:49:40 GMT"},{"key":"Content-Type","value":"text/html"},{"key":"Content-Length","value":"172"},{"key":"Connection","value":"keep-alive"}]
  √  [INFO] Response body: <html>
<head><title>401 Authorization Required</title></head>
<body>
<center><h1>401 Authorization Required</h1></center>
<hr><center>nginx</center>
</body>
</html>

  √  HTTPStatus 401
  √  Session cookies set
```

```
┌─────────────────────────┬───────────────────┬──────────────────┐
│                         │          executed │           failed │
├─────────────────────────┼───────────────────┼──────────────────┤
│              iterations │                 1 │                0 │
├─────────────────────────┼───────────────────┼──────────────────┤
│                requests │                31 │                0 │
├─────────────────────────┼───────────────────┼──────────────────┤
│            test-scripts │                62 │                0 │
├─────────────────────────┼───────────────────┼──────────────────┤
│      prerequest-scripts │                61 │                0 │
├─────────────────────────┼───────────────────┼──────────────────┤
│              assertions │               206 │                0 │
├─────────────────────────┴───────────────────┴──────────────────┤
│ total run duration: 35.6s                                      │
├────────────────────────────────────────────────────────────────┤
│ total data received: 5.58kB (approx)                           │
├────────────────────────────────────────────────────────────────┤
│ average response time: 39ms [min: 5ms, max: 298ms, s.d.: 67ms] │
└────────────────────────────────────────────────────────────────┘
```
