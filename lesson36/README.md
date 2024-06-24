# LESSON-36 Идемпотентость и коммутативность API в HTTP и очередях

## Описание реализованного решения
  
Клиент API при формировании запроса на создание заказа должен генерировать идентификатор запроса в формате `UUIDv4` и передавать его в заголовке запроса `X-Request-Id`.  
Для хранения данных заказов сервис использует таблицу `Orders`.  
Для хранения идентификаторов запроса сервис использует отдельную таблицу `ClientRequests`.  
Сервис, получая данные от клиента, сверяет полученный идентификатор запроса с данными в таблице `ClientRequests`.  
Если в таблице уже имеется такой же идентификатор, то сервис будет отправлять клиенту ответ с ошибкой `HTTP 409`.  
Если же такой идентификатор отсутствует, то сервис помещает данные заказа в таблицу Orders и данные идентификатора в таблицу `ClientRequests`.  

## Развертывание 
  
Дальнейшие действия подразумевают, что Nginx Ingress Controller уже установлен.  
  
Создаем и делаем дефолтным неймспейс sakurlyk-lesson36
```
kubectl create namespace sakurlyk-lesson36
kubectl config set-context --current --namespace=sakurlyk-lesson36
```

В папке Helm выполняем команду установки сервиса  
```
helm -n sakurlyk-lesson36 install orders-chart .\orders-chart
```
  
## Ingress

Для возможности обращения к ingress.  
В качестве namespace указываем тот, в котором установлен и работает ingress контроллер.  
В примере запуска ниже указан --namespace=m  
  
```
kubectl port-forward --namespace=m service/nginx-ingress-nginx-controller 80:80
```
  
## Тестирование
  
После установки запускаем тесты из папки 'Postman' с помощью newman и проверяем, что все корректно запустилось.  
  
```
newman run "lesson_36.postman_collection.json"
```
  
Результаты:

```
→ 01_Создать заказ с RequestId #1 (успешно)
  POST http://arch.homework/orders/create [200 OK, 292B, 285ms]
  √  [INFO] Request headers: [{"key":"X-Request-Id","value":"c9f5f04c-1004-466c-8b3a-1690847d32ff"},{"key":"Content-Type","value":"application/json","system":true},{"key":"User-Agent","value":"PostmanRuntime/7.39.0","system":true},{"key":"Accept","value":"*/*","system":true},{"key":"Cache-Control","value":"no-cache","system":true},{"key":"Postman-Token","value":"b6de69bb-d4d6-4a61-9aed-ff5b94f24ba1","system":true},{"key":"Host","value":"arch.homework","system":true},{"key":"Accept-Encoding","value":"gzip, deflate, br","system":true},{"key":"Connection","value":"keep-alive","system":true},{"key":"Content-Length","value":"59","system":true}]
  √  [INFO] Request body: {
  "product": "some_product",
  "totalPrice": "100.0"
}
  √  [INFO] Response headers: [{"key":"Date","value":"Mon, 24 Jun 2024 19:25:31 GMT"},{"key":"Content-Type","value":"application/json; charset=utf-8"},{"key":"Transfer-Encoding","value":"chunked"},{"key":"Connection","value":"keep-alive"}]
  √  [INFO] Response body: {"id":"8346027f-e462-46b4-9478-ab1e11b593a6","product":"some_product","totalPrice":100.0,"createdAt":"2024-06-24T19:25:31.4754466+00:00"}
  √  HTTPStatus 200
  √  Order created
```

```
→ 02_Создался новый заказ
  GET http://arch.homework/orders [200 OK, 293B, 18ms]
  √  [INFO] Request headers: [{"key":"User-Agent","value":"PostmanRuntime/7.39.0","system":true},{"key":"Accept","value":"*/*","system":true},{"key":"Cache-Control","value":"no-cache","system":true},{"key":"Postman-Token","value":"d14413be-1e73-4cd5-a72f-51118690a2ec","system":true},{"key":"Host","value":"arch.homework","system":true},{"key":"Accept-Encoding","value":"gzip, deflate, br","system":true},{"key":"Connection","value":"keep-alive","system":true}]
  √  [INFO] Request body: undefined
  √  [INFO] Response headers: [{"key":"Date","value":"Mon, 24 Jun 2024 19:25:31 GMT"},{"key":"Content-Type","value":"application/json; charset=utf-8"},{"key":"Transfer-Encoding","value":"chunked"},{"key":"Connection","value":"keep-alive"}]
  √  [INFO] Response body: [{"id":"8346027f-e462-46b4-9478-ab1e11b593a6","product":"some_product","totalPrice":100.0,"createdAt":"2024-06-24T19:25:31.475446+00:00"}]
  √  HTTPStatus 200
  √  Order created
```

```
→ 03_Создать повторно заказ с RequestId #1 с тем же payload (=ошибка 409)
  POST http://arch.homework/orders/create [409 Conflict, 288B, 15ms]
  √  [INFO] Request headers: [{"key":"X-Request-Id","value":"c9f5f04c-1004-466c-8b3a-1690847d32ff"},{"key":"Content-Type","value":"application/json","system":true},{"key":"User-Agent","value":"PostmanRuntime/7.39.0","system":true},{"key":"Accept","value":"*/*","system":true},{"key":"Cache-Control","value":"no-cache","system":true},{"key":"Postman-Token","value":"336fa5b3-9ad4-47fd-bc7c-07972134747f","system":true},{"key":"Host","value":"arch.homework","system":true},{"key":"Accept-Encoding","value":"gzip, deflate, br","system":true},{"key":"Connection","value":"keep-alive","system":true},{"key":"Content-Length","value":"59","system":true}]
  √  [INFO] Request body: {
  "product": "some_product",
  "totalPrice": "100.0"
}
  √  [INFO] Response headers: [{"key":"Date","value":"Mon, 24 Jun 2024 19:25:31 GMT"},{"key":"Content-Type","value":"application/json; charset=utf-8"},{"key":"Transfer-Encoding","value":"chunked"},{"key":"Connection","value":"keep-alive"}]
  √  [INFO] Response body: {"message":"Запрос с id 'c9f5f04c-1004-466c-8b3a-1690847d32ff' уже существует","details":"IdempotentError"}
  √  HTTPStatus 409
  √  Order not created
```

```
→ 04_Создать повторно заказ с RequestId #1 с другим payload (=ошибка 409)
  POST http://arch.homework/orders/create [409 Conflict, 433B, 9ms]
  √  [INFO] Request headers: [{"key":"X-Request-Id","value":"c9f5f04c-1004-466c-8b3a-1690847d32ff"},{"key":"Content-Type","value":"application/json","system":true},{"key":"User-Agent","value":"PostmanRuntime/7.39.0","system":true},{"key":"Accept","value":"*/*","system":true},{"key":"Cache-Control","value":"no-cache","system":true},{"key":"Postman-Token","value":"d3fe0e94-be28-408a-b767-236ecc3cc8d2","system":true},{"key":"Host","value":"arch.homework","system":true},{"key":"Accept-Encoding","value":"gzip, deflate, br","system":true},{"key":"Connection","value":"keep-alive","system":true},{"key":"Content-Length","value":"60","system":true}]
  √  [INFO] Request body: {
  "product": "other_product",
  "totalPrice": "200.0"
}
  √  [INFO] Response headers: [{"key":"Date","value":"Mon, 24 Jun 2024 19:25:31 GMT"},{"key":"Content-Type","value":"application/json; charset=utf-8"},{"key":"Transfer-Encoding","value":"chunked"},{"key":"Connection","value":"keep-alive"}]
  √  [INFO] Response body: {"message":"Запрос с id 'c9f5f04c-1004-466c-8b3a-1690847d32ff' уже существует и данные в очередном запросе не совпадают с уже обрабатываемым запросом","details":"IdempotentParameterMismatch"}
  √  HTTPStatus 409
  √  Order not created
```

```
→ 05_Еще один заказ не добавился и прошлый не поменялся
  GET http://arch.homework/orders [200 OK, 293B, 7ms]
  √  [INFO] Request headers: [{"key":"User-Agent","value":"PostmanRuntime/7.39.0","system":true},{"key":"Accept","value":"*/*","system":true},{"key":"Cache-Control","value":"no-cache","system":true},{"key":"Postman-Token","value":"e4d42147-9144-44cf-b9e1-2491c9fb019f","system":true},{"key":"Host","value":"arch.homework","system":true},{"key":"Accept-Encoding","value":"gzip, deflate, br","system":true},{"key":"Connection","value":"keep-alive","system":true}]
  √  [INFO] Request body: undefined
  √  [INFO] Response headers: [{"key":"Date","value":"Mon, 24 Jun 2024 19:25:31 GMT"},{"key":"Content-Type","value":"application/json; charset=utf-8"},{"key":"Transfer-Encoding","value":"chunked"},{"key":"Connection","value":"keep-alive"}]
  √  [INFO] Response body: [{"id":"8346027f-e462-46b4-9478-ab1e11b593a6","product":"some_product","totalPrice":100.0,"createdAt":"2024-06-24T19:25:31.475446+00:00"}]
  √  HTTPStatus 200
  √  Order not changed
```

```
→ 06_Создать новый заказ с RequestId #2 (успешно)
  POST http://arch.homework/orders/create [200 OK, 293B, 15ms]
  √  [INFO] Request headers: [{"key":"X-Request-Id","value":"b2cc1720-4b4e-4727-a0a1-ae7fd71ee74c"},{"key":"Content-Type","value":"application/json","system":true},{"key":"User-Agent","value":"PostmanRuntime/7.39.0","system":true},{"key":"Accept","value":"*/*","system":true},{"key":"Cache-Control","value":"no-cache","system":true},{"key":"Postman-Token","value":"d0836c43-92f0-493f-8bce-20bdc65877c8","system":true},{"key":"Host","value":"arch.homework","system":true},{"key":"Accept-Encoding","value":"gzip, deflate, br","system":true},{"key":"Connection","value":"keep-alive","system":true},{"key":"Content-Length","value":"60","system":true}]
  √  [INFO] Request body: {
  "product": "other_product",
  "totalPrice": "300.0"
}
  √  [INFO] Response headers: [{"key":"Date","value":"Mon, 24 Jun 2024 19:25:32 GMT"},{"key":"Content-Type","value":"application/json; charset=utf-8"},{"key":"Transfer-Encoding","value":"chunked"},{"key":"Connection","value":"keep-alive"}]
  √  [INFO] Response body: {"id":"2ee4ba9f-5fe3-461f-8853-40f168755db8","product":"other_product","totalPrice":300.0,"createdAt":"2024-06-24T19:25:32.0697704+00:00"}
  √  HTTPStatus 200
  √  Order created
```

```
→ 07_Создался новый заказ
  GET http://arch.homework/orders [200 OK, 430B, 6ms]
  √  [INFO] Request headers: [{"key":"User-Agent","value":"PostmanRuntime/7.39.0","system":true},{"key":"Accept","value":"*/*","system":true},{"key":"Cache-Control","value":"no-cache","system":true},{"key":"Postman-Token","value":"1e4e4bae-c79d-4f3b-ab24-1710c56a8672","system":true},{"key":"Host","value":"arch.homework","system":true},{"key":"Accept-Encoding","value":"gzip, deflate, br","system":true},{"key":"Connection","value":"keep-alive","system":true}]
  √  [INFO] Request body: undefined
  √  [INFO] Response headers: [{"key":"Date","value":"Mon, 24 Jun 2024 19:25:32 GMT"},{"key":"Content-Type","value":"application/json; charset=utf-8"},{"key":"Transfer-Encoding","value":"chunked"},{"key":"Connection","value":"keep-alive"}]
  √  [INFO] Response body: [{"id":"2ee4ba9f-5fe3-461f-8853-40f168755db8","product":"other_product","totalPrice":300.0,"createdAt":"2024-06-24T19:25:32.06977+00:00"},{"id":"8346027f-e462-46b4-9478-ab1e11b593a6","product":"some_product","totalPrice":100.0,"createdAt":"2024-06-24T19:25:31.475446+00:00"}]
  √  HTTPStatus 200
  √  Order created
```

```
┌─────────────────────────┬───────────────────┬──────────────────┐
│                         │          executed │           failed │
├─────────────────────────┼───────────────────┼──────────────────┤
│              iterations │                 1 │                0 │
├─────────────────────────┼───────────────────┼──────────────────┤
│                requests │                 7 │                0 │
├─────────────────────────┼───────────────────┼──────────────────┤
│            test-scripts │                14 │                0 │
├─────────────────────────┼───────────────────┼──────────────────┤
│      prerequest-scripts │                10 │                0 │
├─────────────────────────┼───────────────────┼──────────────────┤
│              assertions │                42 │                0 │
├─────────────────────────┴───────────────────┴──────────────────┤
│ total run duration: 943ms                                      │
├────────────────────────────────────────────────────────────────┤
│ total data received: 1.23kB (approx)                           │
├────────────────────────────────────────────────────────────────┤
│ average response time: 50ms [min: 6ms, max: 285ms, s.d.: 95ms] │
└────────────────────────────────────────────────────────────────┘
```