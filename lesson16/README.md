## Добавление bitnami репозитория

```
helm repo add bitnami https://charts.bitnami.com/bitnami
helm repo update bitnami
```

## Установка postgres

С прокидыванием пароля pgpass

```
helm install postgres oci://registry-1.docker.io/bitnamicharts/postgresql --set auth.postgresPassword=pgpass
```

## Если что-то пошло не так, удаляем

```
helm uninstall postgres
kubectl get pvc
kubectl delete pvc data-postgres-postgresql-0 
```

## Форвардинг портов с локальной машины в k8s

### Для возможности подключения клиента для просмотра БД

kubectl port-forward --namespace default svc/postgres-postgresql 5432:5432 & PGPASSWORD="pgpass" psql --host 127.0.0.1 -U postgres -d postgres -p 5432

### Для возможности обращения к ingress

kubectl port-forward --namespace=m service/nginx-ingress-nginx-controller 80:80

## Запуск k8s

В папке Manifests выполняем
  
```
kubectl apply -f .
```
  
Если что-то пошло не так, то удаляем через 
  
```
kubectl delete -f .
```

